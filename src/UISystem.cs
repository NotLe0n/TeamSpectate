using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace TeamSpectate;

[Autoload(Side = ModSide.Client)]
internal class UISystem : ModSystem
{
    public static readonly Asset<Texture2D> EmptyButtonAsset = ModContent.Request<Texture2D>("TeamSpectate/Assets/empty", AssetRequestMode.ImmediateLoad);
    public static readonly Asset<Texture2D> CameraButtonAsset = ModContent.Request<Texture2D>("TeamSpectate/Assets/cameraButton", AssetRequestMode.ImmediateLoad);
    public static readonly Asset<Texture2D> SelectFrameAsset = ModContent.Request<Texture2D>("TeamSpectate/Assets/selectedFrame", AssetRequestMode.ImmediateLoad);

	private UserInterface? userInterface, deadUserInterface;

	public override void Load()
	{
		if (!Main.dedServ) {
			var ui = new TeamSpectateUI();
			ui.Activate();
			userInterface = new UserInterface();
			userInterface.SetState(ui);

			var deadUI = new TeamSpectateDeadUI();
			deadUI.Activate();
			deadUserInterface = new UserInterface();
			deadUserInterface.SetState(deadUI);
		}
	}

	private GameTime? lastUpdateUiGameTime;
	public override void UpdateUI(GameTime gameTime)
	{
		lastUpdateUiGameTime = gameTime;
		if (Main.netMode == NetmodeID.MultiplayerClient) {
			if (Main.playerInventory) {
				userInterface?.Update(gameTime);
			}

			if (Main.LocalPlayer.dead) {
				deadUserInterface?.Update(gameTime);
			}
		}
	}

    private bool DrawUI()
    {
        if (Main.netMode == NetmodeID.MultiplayerClient) {
            if (Main.playerInventory) {
                userInterface?.Draw(Main.spriteBatch, lastUpdateUiGameTime);
            }

            if (Main.LocalPlayer.dead) {
                deadUserInterface?.Draw(Main.spriteBatch, lastUpdateUiGameTime);
            }
        }
        return true;
    }

	public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
	{
		int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
		if (mouseTextIndex != -1) {
			layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("Team Spectate: UI", DrawUI, InterfaceScaleType.UI));
		}
	}
}
