using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace TeamSpectate;

internal class TeamSpectateUI : UIState
{
	private Menu? menu;
	private readonly UIImageButton button;

	public TeamSpectateUI()
	{
		button = new UIImageButton(ModContent.Request<Texture2D>("TeamSpectate/Assets/cameraButton", ReLogic.Content.AssetRequestMode.ImmediateLoad));
		button.Left.Set(-225, 1);
		button.OnLeftClick += (_, _) =>
		{
			if (menu == null) {
				menu = new Menu(300, 250);
				Append(menu);
			}
			else {
				menu.Remove();
				menu = null;
			}
		};
		Append(button);
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);

		const int SUPER_IMPORTANT_MAGIC_NUMBER = 304;
		int mH = 256;
		if (mH + Main.instance.RecommendedEquipmentAreaPushUp > Main.screenHeight) {
			mH = Main.screenHeight - Main.instance.RecommendedEquipmentAreaPushUp;
		}

		button.Top.Set(Main.mapStyle == 0 || Main.mapStyle == 2 ? Main.instance.invBottom + 50 : mH + SUPER_IMPORTANT_MAGIC_NUMBER, 0);
		menu?.Top.Set(Main.mapStyle == 0 || Main.mapStyle == 2 ? Main.instance.invBottom + 50 : mH + SUPER_IMPORTANT_MAGIC_NUMBER, 0);
	}
}

internal class TeamSpectateDeadUI : UIState
{
	private readonly Menu deadMenu;
	public TeamSpectateDeadUI()
	{
		deadMenu = new Menu(300, 250);
		Append(deadMenu);
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);
		deadMenu.Top.Set(Main.mapStyle == 0 || Main.mapStyle == 2 ? 100 : Main.miniMapY + Main.miniMapHeight + 50, 0);
	}
}
