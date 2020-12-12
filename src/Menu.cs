using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;

namespace TeamSpectate.src
{
    public class Menu : UIPanel
    {
        private UIGrid buttonGrid;
        private UIScrollbar scrollbar;
        private int realPlayerCount => Main.player.Where(p => p?.active == true).Count();

        public Menu(float width, float height)
        {
            MaxWidth.Set(width, 0f);
            MaxHeight.Set(height, 0f);
            SetPadding(7);

            buttonGrid = new UIGrid(7);
            buttonGrid.Width.Set(-20, 1);
            buttonGrid.Height.Set(0, 1);
            buttonGrid.ListPadding = 7f;
            Append(buttonGrid);

            AppendButtons();
        }

        public void AppendButtons()
        {
            if (realPlayerCount > 42)
            {
                scrollbar = new UIScrollbar();
                scrollbar.Left.Set(-20f, 1f);
                scrollbar.Top.Set(10, 0f);
                scrollbar.Height.Set(-20, 1f);
                scrollbar.Width.Set(20f, 0f);
                scrollbar.SetView(100f, 1000f);

                buttonGrid.SetScrollbar(scrollbar);
                Append(scrollbar);
            }
            for (int i = 0; i < Main.maxPlayers; i++)
                if (Main.player[i].active)
                    buttonGrid.Add(new PlayerHeadButton(Main.player[i]));
                
            
        }

        int oldActivePlayersCount;
        public override void Update(GameTime gameTime)
        {
            // Check if player Count has changed and update buttons if it has
            if (realPlayerCount != oldActivePlayersCount)
            {
                if (scrollbar != null)
                    scrollbar.Remove();
                buttonGrid.Clear();
                AppendButtons();
                buttonGrid.UpdateOrder();
            }

            // Update oldActivePlayersCount
            oldActivePlayersCount = realPlayerCount;

            // Dynamic width/height
            Height.Set(20 + buttonGrid.GetTotalHeight(), 0);
            Width.Set(20 + buttonGrid.GetRowWidth(), 0);

            Left.Set(Main.screenWidth / 1.3812949640287769784172661870504f + MaxWidth.Pixels - Width.Pixels - 7, 0);
        }
    }
}
