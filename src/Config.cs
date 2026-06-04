using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Terraria.ModLoader.Config;
// ReSharper disable UnassignedField.Global
// ReSharper disable InconsistentNaming // renaming would reset the values on update

namespace TeamSpectate;

[SuppressMessage("ReSharper", "UnassignedField.Global")]
public class Config : ModConfig
{
	public override ConfigScope Mode => ConfigScope.ClientSide;

	[Header("QoL")]
	[DefaultValue(true)]
	public bool spectateOnDeath;

	[DefaultValue(true)]
	public bool respawnSpectateOffToggle;


	[Header("GridProperties")]
	[Slider]
	[Range(1, 16)]
	[DefaultValue(6)]
	public int gridMaxColumns;

	[Slider]
	[Range(2, 16)]
	[DefaultValue(6)]
	public int gridPadding;

	[Slider]
	[Range(0, 16)]
	[DefaultValue(6)]
	public int gridItemGap;
	
	[Slider]
	[Range(0.2f, 1f)]
	[DefaultValue(.33f)]
	public float animationVelocity;
}
