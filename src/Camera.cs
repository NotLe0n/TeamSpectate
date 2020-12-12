using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

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
                if (Main.player[(int)Target].dead || Main.player[(int)Target] == Main.player[Main.myPlayer] || (Main.player[(int)Target].team != Main.LocalPlayer.team && Main.player[(int)Target].team != 0))
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
        public override void PostUpdate()
        {
            if (player.whoAmI == Main.myPlayer && Main.netMode == NetmodeID.MultiplayerClient && Main.GameUpdateCount % 10 == 0)
            {
                ModPacket modPacket = mod.GetPacket();

                modPacket.Write((byte)0); //MessageId
                modPacket.WriteVector2(Main.screenPosition);

                modPacket.Send();
            }
        }
        int selectedTarget = 0;
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (TeamSpectate.prevPlayer.JustPressed && selectedTarget > 0)
            {
                selectedTarget--;
                Locked = true;
                Target = selectedTarget;
            }
            if (TeamSpectate.nextPlayer.JustPressed && selectedTarget < Main.player.Where(p => p?.active == true).Count())
            {
                selectedTarget++;
                Locked = true;
                Target = selectedTarget;
            }
        }
    }
}