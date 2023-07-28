using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace TeamSpectate;

internal class UISystem : ModSystem
{
	private UserInterface? userInterface, deadUserInterface;
	private UIState? ui, deadUI;

	public override void Load()
	{
		if (!Main.dedServ) {
			ui = new TeamSpectateUI();
			ui.Activate();
			userInterface = new UserInterface();
			userInterface.SetState(ui);

			deadUI = new TeamSpectateDeadUI();
			deadUI.Activate();
			deadUserInterface = new UserInterface();
			deadUserInterface.SetState(deadUI);
		}
	}

	public override void Unload()
	{
		ui = deadUI = null;
		userInterface = deadUserInterface = null;
		Camera.Target = null;
		Camera.Locked = false;
	}

	private GameTime? _lastUpdateUiGameTime;
	public override void UpdateUI(GameTime gameTime)
	{
		_lastUpdateUiGameTime = gameTime;
		if (Main.netMode == NetmodeID.MultiplayerClient) {
			if (Main.playerInventory) {
				userInterface?.Update(gameTime);
			}

			if (Main.LocalPlayer.dead) {
				deadUserInterface?.Update(gameTime);
			}
		}
	}

	public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
	{
		int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
		if (mouseTextIndex != -1) {
			layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
				"Team Spectate: UI",
				delegate
				{
					if (Main.netMode == NetmodeID.MultiplayerClient) {
						if (Main.playerInventory) {
							userInterface?.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
						}

						if (Main.LocalPlayer.dead) {
							deadUserInterface?.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
						}
					}
					return true;
				}, InterfaceScaleType.UI));
		}
	}
}
