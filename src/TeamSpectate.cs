using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace TeamSpectate.src
{
    public class TeamSpectate : Mod
    {
        public override void Unload()
        {
            Camera.Target = null;
            Camera.Locked = false;
        }

        /// <summary>
        /// for world section fix
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="whoAmI"></param>
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            switch (reader.ReadByte())
            {
                case 0:
                    RemoteClient.CheckSection(whoAmI, reader.ReadVector2());
                    break;
            }
        }
    }
}