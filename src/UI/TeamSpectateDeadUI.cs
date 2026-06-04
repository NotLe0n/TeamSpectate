using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace TeamSpectate.UI;

internal class TeamSpectateDeadUI : UIState
{
	/// <summary>
	/// Spectate menu instance.
	/// </summary>
	public SpectateMenu MenuInstance { get; } = new();

	public TeamSpectateDeadUI()
	{
		Append(MenuInstance);
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);
		MenuInstance.Top.Set(Main.mapStyle == 0 || Main.mapStyle == 2 ? 100 : Main.miniMapY + Main.miniMapHeight + 50,
			0);
	}
}