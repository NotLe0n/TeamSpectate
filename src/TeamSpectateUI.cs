using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace TeamSpectate;

internal class TeamSpectateUI : UIState
{
	private Menu? menu;
	private readonly UIImage button;

	public TeamSpectateUI()
	{
		button = new UIImage(ModContent.Request<Texture2D>("TeamSpectate/Assets/cameraButton", ReLogic.Content.AssetRequestMode.ImmediateLoad));
		button.Left.Set(-225, 1);
		button.OnMouseOver += (_, _) => SoundEngine.PlaySound(SoundID.MenuTick);
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
		
		if (button.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true; // so you can't use items while clicking the button
			spriteBatch.Draw(ModContent.Request<Texture2D>("TeamSpectate/Assets/selectedFrame", AssetRequestMode.ImmediateLoad).Value,
				button.GetDimensions().Position() - Vector2.One,
				Main.OurFavoriteColor
			);
		}

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
