using Microsoft.Xna.Framework.Input;
using Terraria.ModLoader;

namespace TeamSpectate;

internal class HotkeyLoader : ModSystem
{
	public static ModKeybind? prevPlayer, nextPlayer, stopSpectating;

	public override void Load()
	{
		prevPlayer = KeybindLoader.RegisterKeybind(Mod, "PreviousPlayer", Keys.None);
		nextPlayer = KeybindLoader.RegisterKeybind(Mod, "NextPlayer", Keys.None);
		stopSpectating = KeybindLoader.RegisterKeybind(Mod, "Stop", Keys.None);
	}

	public override void Unload()
	{
		nextPlayer = null;
		prevPlayer = null;
	}
}
