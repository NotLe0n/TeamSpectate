using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace TeamSpectate;

public class Config : ModConfig
{
	public override ConfigScope Mode => ConfigScope.ClientSide;

	[DefaultValue(false)]
	public bool RespawnSpectateOffToggle;
}