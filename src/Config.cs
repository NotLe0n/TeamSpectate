using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace TeamSpectate.src
{
	public class Config : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Label("Exit spectate mode upon respawn")]
		[DefaultValue(false)]
		public bool RespawnSpectateOffToggle;
	}
}