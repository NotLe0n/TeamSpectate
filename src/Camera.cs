using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace TeamSpectate;

internal class Camera : ModPlayer
{
	public static bool SpectatingBoss { get; set; }
	public static int? Target { get; set; }
	public static bool Locked { get; set; }

	public override void ModifyScreenPosition()
	{
		// don't move camera if the screen is locked, there is no Target, the Target value is invalid or the Target doesn't exist
		if (Locked == false || Target is null or -1) {
			return;
		}

		if (SpectatingBoss) {
			bool isBossInactive = !Main.npc[Target.Value].active;
			if (isBossInactive) {
				Untarget();
				return;
			}

			// specate target boss
			Main.screenPosition = Main.npc[Target.Value].position - (new Vector2(Main.screenWidth, Main.screenHeight) / 2);
		}
		else {
			if (Target >= Main.player.Length || !Main.player[Target.Value].active) {
				Untarget();
				return;
			}

			// true if player is dead, myself, or in another team (except no team)
			bool isPlayerInvalid = Main.player[Target.Value].dead || Main.player[Target.Value] == Main.LocalPlayer || (Main.player[Target.Value].team != Main.LocalPlayer.team && Main.player[Target.Value].team != 0);
			if (isPlayerInvalid) {
				Untarget();
				return;
			}

			// spectate target player
			Main.screenPosition = Main.player[Target.Value].position - (new Vector2(Main.screenWidth, Main.screenHeight) / 2);
		}
	}

	private static void Untarget()
	{
		Target = null;
		Locked = false;
	}

	public override void PostUpdate()
	{
		if (Player.whoAmI == Main.myPlayer && Main.netMode == NetmodeID.MultiplayerClient && Main.GameUpdateCount % 10 == 0) {
			ModPacket modPacket = Mod.GetPacket();

			modPacket.Write((byte)0); //MessageId
			modPacket.WriteVector2(Main.screenPosition);

			modPacket.Send();
		}
	}

	private int selectedTarget = 0;
	public override void ProcessTriggers(TriggersSet triggersSet)
	{
		if (HotkeyLoader.nextPlayer is null || HotkeyLoader.prevPlayer is null || HotkeyLoader.stopSpectating is null) {
			return;
		}

		if (HotkeyLoader.prevPlayer.JustPressed && selectedTarget > 0) {
			SpectatingBoss = false;
			selectedTarget--;
			Locked = true;
			Target = selectedTarget;
		}
		if (HotkeyLoader.nextPlayer.JustPressed && selectedTarget < Main.player.Count(p => p?.active == true)) {
			SpectatingBoss = false;
			selectedTarget++;
			Locked = true;
			Target = selectedTarget;
		}
		if (HotkeyLoader.stopSpectating.JustPressed) {
			Untarget();
			SpectatingBoss = false;
		}
	}
}
