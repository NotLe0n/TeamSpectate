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
        // UI
        internal UserInterface UserInterface, deadUserInterface;
        private UIState UI, deadUI;
        public static ModHotKey prevPlayer, nextPlayer;

        public override void Load()
        {
            // UI
            if (!Main.dedServ)
            {
                UI = new TeamSpectateUI();
                UI.Activate();
                UserInterface = new UserInterface();
                UserInterface.SetState(UI);

                deadUI = new TeamSpectateDeadUI();
                deadUI.Activate();
                deadUserInterface = new UserInterface();
                deadUserInterface.SetState(deadUI);
            }

            // Hotkeys
            prevPlayer = RegisterHotKey("Spectate Previous Player", "");
            nextPlayer = RegisterHotKey("Spectate Next Player", "");
        }
        public override void Unload()
        {
            // UI
            UI = deadUI = null;
            UserInterface = deadUserInterface = null;

            // Other static Fields
            Camera.Target = null;
            Camera.Locked = false;

            // Hotkeys
            nextPlayer = prevPlayer = null;
        }
        private GameTime _lastUpdateUiGameTime;
        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUiGameTime = gameTime;
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                if (Main.playerInventory)
                    UserInterface.Update(gameTime);
                if (Main.LocalPlayer.dead)
                    deadUserInterface.Update(gameTime);
            }
        }

        /// <summary>
        /// Add UI
        /// </summary>
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "Team Spectate: UI",
                    delegate
                    {
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            if (Main.playerInventory)
                                UserInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);

                            if (Main.LocalPlayer.dead)
                                deadUserInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        }
                        return true;
                    }, InterfaceScaleType.UI));
            }
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