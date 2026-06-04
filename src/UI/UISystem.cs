using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace TeamSpectate.UI;

[Autoload(Side = ModSide.Client)]
internal class UISystem : ModSystem
{
	private UserInterface UserInterface { get; } = new();
	private UserInterface DeadUserInterface { get; } = new();

	public static TeamSpectateUI TeamSpectateUIInstance { get; } = new();
	public static TeamSpectateDeadUI TeamSpectateDeadUIInstance { get; } = new();

	public override void Load()
	{
		if (!Main.dedServ) {
			TeamSpectateUIInstance.Activate();
			UserInterface.SetState(TeamSpectateUIInstance);

			TeamSpectateDeadUIInstance.Activate();
			DeadUserInterface.SetState(TeamSpectateDeadUIInstance);
		}
	}

	private GameTime LastUpdateUiGameTime { get; set; } = new();

	public override void UpdateUI(GameTime gameTime)
	{
		LastUpdateUiGameTime = gameTime;

		if (Main.netMode == NetmodeID.MultiplayerClient) {
			if (Main.playerInventory) {
				UserInterface.Update(gameTime);
			}

			if (Main.LocalPlayer.dead) {
				DeadUserInterface.Update(gameTime);
				// update visibility of the menu instance in dead ui
				TeamSpectateDeadUIInstance.MenuInstance.IsMenuShown = true;
			}
			else {
				// update visibility of the menu instance in dead ui
				TeamSpectateDeadUIInstance.MenuInstance.IsMenuShown = false;
			}
		}
	}

	private bool DrawUI()
	{
		if (Main.netMode == NetmodeID.MultiplayerClient) {
			if (Main.playerInventory) {
				UserInterface.Draw(Main.spriteBatch, LastUpdateUiGameTime);
			}

			if (Main.LocalPlayer.dead) {
				DeadUserInterface.Draw(Main.spriteBatch, LastUpdateUiGameTime);
			}
		}

		return true;
	}

	public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
	{
		int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
		if (mouseTextIndex != -1) {
			layers.Insert(mouseTextIndex,
				new LegacyGameInterfaceLayer("Team Spectate: UI", DrawUI, InterfaceScaleType.UI));
		}
	}
}