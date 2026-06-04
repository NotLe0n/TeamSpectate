using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace TeamSpectate.UI;

internal class TeamSpectateUI : UIState
{
	/// <summary>
	/// Spectate menu instance.
	/// </summary>
	private SpectateMenu MenuInstance { get; } = new();

	/// <summary>
	/// Spectate menu toggle button instance.
	/// </summary>
	private UIImage MenuToggleButton { get; } = new(UIAssets.CameraButtonTransparentAsset);

	public TeamSpectateUI()
	{
		MenuToggleButton.Left.Set(-225, 1);
		MenuToggleButton.OnLeftClick += ToggleMenu;
		Append(MenuToggleButton);
		Append(MenuInstance);
	}

	private void ToggleMenu(UIMouseEvent evt, UIElement e)
	{
		SoundEngine.PlaySound(SoundID.MenuTick);
		MenuInstance.IsMenuShown = !MenuInstance.IsMenuShown;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);

		if (MenuToggleButton.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true; // so you can't use items while clicking the button
			spriteBatch.Draw(UIAssets.CameraButtonOutlineAsset.Value, MenuToggleButton.GetDimensions().Position(),
				Main.OurFavoriteColor);
		}

		const int SuperImportantMagicNumber = 304;
		int mH = 256;
		if (mH + Main.instance.RecommendedEquipmentAreaPushUp > Main.screenHeight) {
			mH = Main.screenHeight - Main.instance.RecommendedEquipmentAreaPushUp;
		}

		MenuToggleButton.Top.Set(
			Main.mapStyle == 0 || Main.mapStyle == 2 ? Main.instance.invBottom + 50 : mH + SuperImportantMagicNumber,
			0);
		MenuInstance.Top.Set(
			Main.mapStyle == 0 || Main.mapStyle == 2 ? Main.instance.invBottom + 50 : mH + SuperImportantMagicNumber,
			0);
	}
}