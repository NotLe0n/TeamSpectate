using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace TeamSpectate;

internal class TeamSpectateUI : UIState
{
	private Menu? menu;
	private readonly UIImage button;

	public TeamSpectateUI()
	{
		button = new UIImage(UISystem.CameraButtonAsset);
		button.Left.Set(-225, 1);
		button.OnMouseOver += (_, _) => SoundEngine.PlaySound(SoundID.MenuTick);
        button.OnLeftClick += ToggleMenu;
		Append(button);
	}

    private void ToggleMenu(UIMouseEvent evt, UIElement e)
    {
        if (menu == null) {
            menu = new Menu(300, 250);
            Append(menu);
        }
        else {
            menu.Remove();
            menu = null;
        }
    }

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);
		
		if (button.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true; // so you can't use items while clicking the button
			spriteBatch.Draw(UISystem.SelectFrameAsset.Value, button.GetDimensions().Position() - Vector2.One, Main.OurFavoriteColor);
		}

		const int SuperImportantMagicNumber = 304;
		int mH = 256;
		if (mH + Main.instance.RecommendedEquipmentAreaPushUp > Main.screenHeight) {
			mH = Main.screenHeight - Main.instance.RecommendedEquipmentAreaPushUp;
		}

		button.Top.Set(Main.mapStyle == 0 || Main.mapStyle == 2 ? Main.instance.invBottom + 50 : mH + SuperImportantMagicNumber, 0);
		menu?.Top.Set(Main.mapStyle == 0 || Main.mapStyle == 2 ? Main.instance.invBottom + 50 : mH + SuperImportantMagicNumber, 0);
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
