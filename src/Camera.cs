using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace TeamSpectate;

internal class Camera : ModPlayer
{
	public static bool SpectatingBoss { get; private set; }
	public static int? Target { get; private set; }

	public override void ModifyScreenPosition()
	{
		// don't move camera if there is no Target, the Target value is invalid or the Target doesn't exist
		if (Target is null or -1) {
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

	public static void SetTarget(int targetID, bool isBoss)
	{
		Target = targetID;
		SpectatingBoss = isBoss;
	}
	
	public static void Untarget()
	{
		Target = null;
		SpectatingBoss = false;
	}

	public override void OnRespawn()
	{
		base.OnRespawn();
		if (ModContent.GetInstance<Config>().RespawnSpectateOffToggle == false) {
			return;
		}
		Untarget();
	}

	public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
	{
		base.Kill(damage, hitDirection, pvp, damageSource);
		if (ModContent.GetInstance<Config>().SpectateOnDeath == false) {
			return;
		}
		
		Player? closest = Main.player.Where(x => x != Main.LocalPlayer).MinBy(x => x.position.Distance(Main.LocalPlayer.position));
		if (closest is null) return;
		
		SetTarget(closest.whoAmI, false);
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
			selectedTarget--;
			SetTarget(selectedTarget, false);
		}
		if (HotkeyLoader.nextPlayer.JustPressed && selectedTarget < Main.player.Count(p => p?.active == true)) {
			selectedTarget++;
			SetTarget(selectedTarget, false);
		}
		if (HotkeyLoader.stopSpectating.JustPressed) {
			Untarget();
		}
	}
}
