using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace TeamSpectate.src
{
    public class TeamSpectate : Mod
    {
        // UI
        internal UserInterface UserInterface;
        private UIState UI;

        public override void Load()
        {
            // UI
            if (!Main.dedServ)
            {
                UI = new TeamSpectateUI();
                UI.Activate();
                UserInterface = new UserInterface();
                UserInterface.SetState(UI);
            }
        }
        public override void Unload()
        {
            // UI
            UI = null;
            UserInterface = null;
        }
        private GameTime _lastUpdateUiGameTime;
        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUiGameTime = gameTime;
            if (Main.playerInventory && Main.netMode == NetmodeID.MultiplayerClient)
                UserInterface.Update(gameTime);
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
                        if (Main.playerInventory && Main.netMode == NetmodeID.MultiplayerClient)
                            UserInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        return true;
                    }, InterfaceScaleType.UI));
            }
        }
    }
}