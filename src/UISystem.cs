using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace TeamSpectate.src
{
    class UISystem : ModSystem
    {
        internal UserInterface UserInterface, deadUserInterface;
        private UIState UI, deadUI;

        public override void Load()
        {
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
        }

        public override void Unload()
        {
            UI = deadUI = null;
            UserInterface = deadUserInterface = null;
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
    }
}
