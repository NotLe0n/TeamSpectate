using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Terraria.ModLoader;

namespace TeamSpectate.src
{
    class HotkeyLoader : ModSystem
    {
        public static ModKeybind prevPlayer, nextPlayer;

        public override void Load()
        {
            prevPlayer = KeybindLoader.RegisterKeybind(Mod, "Spectate Previous Player", Keys.None);
            nextPlayer = KeybindLoader.RegisterKeybind(Mod, "Spectate Next Player", Keys.None);
        }

        public override void Unload()
        {
            nextPlayer = prevPlayer = null;
        }
    }
}
