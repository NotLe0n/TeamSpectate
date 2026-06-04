using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace TeamSpectate.UI;

/// <summary>
/// Contains various assets used in the mod.
/// </summary>
[Autoload(Side = ModSide.Client)]
internal static class UIAssets
{
	public static readonly Asset<Texture2D> FrameAsset =
		ModContent.Request<Texture2D>("TeamSpectate/Assets/frame", AssetRequestMode.ImmediateLoad);

	public static readonly Asset<Texture2D> FrameOutlineAsset =
		ModContent.Request<Texture2D>("TeamSpectate/Assets/frameOutline", AssetRequestMode.ImmediateLoad);

	public static readonly Asset<Texture2D> EyeFrameAsset =
		ModContent.Request<Texture2D>("TeamSpectate/Assets/eyeFrame", AssetRequestMode.ImmediateLoad);

	public static readonly Asset<Texture2D> StarFrameAsset =
		ModContent.Request<Texture2D>("TeamSpectate/Assets/starFrame", AssetRequestMode.ImmediateLoad);

	public static readonly Asset<Texture2D> CameraButtonTransparentAsset =
		ModContent.Request<Texture2D>("TeamSpectate/Assets/cameraButtonTransparent", AssetRequestMode.ImmediateLoad);

	public static readonly Asset<Texture2D> CameraButtonOutlineAsset =
		ModContent.Request<Texture2D>("TeamSpectate/Assets/cameraButtonOutline", AssetRequestMode.ImmediateLoad);

	public static readonly Asset<Texture2D> FilterAsset =
		ModContent.Request<Texture2D>("TeamSpectate/Assets/filter", AssetRequestMode.ImmediateLoad);
}