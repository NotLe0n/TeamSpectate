using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;

namespace TeamSpectate.UI.Buttons;

/// <summary>
/// General button class for spectate menu.
/// </summary>
public abstract class GridButton : UIImageButton
{
	protected GridButton() : base(UIAssets.FrameAsset)
	{
		Width.Set(UIGrid.GridItemWidth, 0);
		Height.Set(UIGrid.GridItemHeight, 0);
	}

	/// <summary>
	/// Returns a position to draw asset aligned to a button center in `Draw()` call.
	/// </summary>
	/// <param name="asset">Texture asset reference.</param>
	protected Vector2 GetCenter(Asset<Texture2D> asset) => new() {
		X = Parent.GetDimensions().X + Left.Pixels
			+ .5f * (Width.Pixels - asset.Width()),

		Y = Parent.GetDimensions().Y + Top.Pixels
			+ .5f * (Height.Pixels - asset.Height())
	};

	/// <summary>
	/// Returns center of the asset.
	/// </summary>
	/// <param name="asset">Texture asset reference.</param>
	protected Vector2 GetOrigin(Asset<Texture2D> asset) => new() {
		X = .5f * asset.Width(),
		Y = .5f * asset.Height()
	};
	
	/// <summary>
	/// Text to be shown upon cursor hovers the button.
	/// </summary>
	protected abstract string GetTooltip();

	public override void Draw(SpriteBatch spriteBatch)
	{
		var frameAsset = UIAssets.FrameAsset;
		
		if (ContainsPoint(Main.MouseScreen)) {
			// so you can't use items while clicking the button
			Main.LocalPlayer.mouseInterface = true;
		}
		
		// update hover tooltip text
		if (IsMouseHovering) {
			Main.hoverItemName = GetTooltip();
		}

		spriteBatch.Draw(frameAsset.Value, GetCenter(frameAsset),
			new Rectangle(0, 0, frameAsset.Width(), frameAsset.Height()),
			Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
	}
}