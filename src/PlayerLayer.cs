using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;

namespace TeamSpectate;

/// <summary>
/// Stores assets layers related to a player's head.
/// General use is to draw player's head in `PlayerHeadButton` class.
/// </summary>
internal static class PlayerLayer
{
	public static Asset<Texture2D> Head => TextureAssets.Players[0, 0];
	public static Asset<Texture2D> Eyes => TextureAssets.Players[0, 2];
	public static Asset<Texture2D> Sclera => TextureAssets.Players[0, 1];
	public static Asset<Texture2D> Hair(Player player) => TextureAssets.PlayerHair[player.hair];
}