using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
// ReSharper disable UnassignedField.Global
// ReSharper disable InconsistentNaming // renaming would reset the values on update

namespace TeamSpectate;

public class Config : ModConfig
{
	public override ConfigScope Mode => ConfigScope.ClientSide;

	[DefaultValue(true)]
	public bool RespawnSpectateOffToggle;
	
	[DefaultValue(true)]
	public bool SpectateOnDeath;
}