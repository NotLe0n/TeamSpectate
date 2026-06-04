using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace TeamSpectate;

public class TeamSpectate : Mod
{
	/// <summary>
	/// Filter value for a SpectateMenu instances.
	/// </summary>
	public static SpectateFilter SpectateFilter { get; set; } = SpectateFilter.Everything;

	/// <summary>
	/// Emulates Vector2.Lerp(), but for a `float` type.
	/// </summary>
	/// <param name="from">Float reference value.</param>
	/// <param name="to">Target value.</param>
	/// <param name="weight">Lerp velocity.</param>
	public static float Lerp(float from, float to, float weight)
	{
		return Vector2.Lerp(new Vector2(from, 0f), new Vector2(to, 0f), weight).X;
	}

	public override void Load()
	{
		ConstantSeedFix.Fix();
	}

	public override void Unload()
	{
		Camera.Untarget();
	}

	/// <summary>
	/// for world section fix
	/// </summary>
	/// <param name="reader"></param>
	/// <param name="whoAmI"></param>
	public override void HandlePacket(BinaryReader reader, int whoAmI)
	{
		switch (reader.ReadByte()) {
			case 0:
				RemoteClient.CheckSection(whoAmI, reader.ReadVector2());
				break;
		}
	}
}