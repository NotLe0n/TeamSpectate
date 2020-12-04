using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TeamSpectate.src
{
    class Camera : ModPlayer
    {
        public static int? Target { get; set; }
        public static bool Locked { get; set; }
        public override void ModifyScreenPosition()
        {
            if (Locked && Target != null && Main.player[(int)Target].active)
            {
                if (Main.player[(int)Target].dead)
                {
                    Target = null;
                    Locked = false;
                }
                else
                {
                    Main.screenPosition = Main.player[(int)Target].position - new Vector2(Main.screenWidth, Main.screenHeight) / 2;
                }
            }
        }
    }
}
