using Microsoft.Xna.Framework.Input;
using Terraria.ModLoader;

namespace TeamSpectate;

class HotkeyLoader : ModSystem
{
	public static ModKeybind? prevPlayer, nextPlayer, stopSpectating;

	public override void Load()
	{
		prevPlayer = KeybindLoader.RegisterKeybind(Mod, "Spectate Previous Player", Keys.None);
		nextPlayer = KeybindLoader.RegisterKeybind(Mod, "Spectate Next Player", Keys.None);
		stopSpectating = KeybindLoader.RegisterKeybind(Mod, "Stop specating", Keys.None);
	}

	public override void Unload()
	{
		nextPlayer = null;
		prevPlayer = null;
	}
}
