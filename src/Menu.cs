using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace TeamSpectate.src
{
    public class Menu : UIPanel
    {
        private PlayerHeadButton[] buttons = new PlayerHeadButton[Main.maxPlayers];
        private UIGrid buttonGrid;
        UIScrollbar scrollbar;
        public Menu(float x, float y, float width, float height)
        {
            MarginLeft = x;
            MarginTop = y;
            Width.Set(width, 0f);
            Height.Set(height, 0f);
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
            if (Main.ActivePlayersCount > 42)
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
            for (int i = 0; i < Main.ActivePlayersCount; i++)
            {
                var button = new PlayerHeadButton(Main.player[i])
                {
                    Id = i.ToString()
                };
                button.OnClick += Button_OnClick;
                buttonGrid.Add(button);
                buttons[i] = button;
            }
        }
        private void Button_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i] != null && listeningElement.Id == buttons[i].Id) // gets the button you pressed if it exists
                {
                    if (i == Main.myPlayer) // if you click the button for your own player, the camera gets reset
                    {
                        Camera.Locked = false;
                        Camera.Target = null; // reset target
                    }
                    else
                    {
                        Camera.Locked = !Camera.Locked;
                        Camera.Target = Camera.Target == null ? i : (int?)null; // set target
                    }
                }
            }
        }

        int oldActivePlayersCount;
        public override void Update(GameTime gameTime)
        {
            // Check if player Count has changed and update buttons if it has
            if (Main.ActivePlayersCount != oldActivePlayersCount)
            {
                if (scrollbar != null)
                    scrollbar.Remove();
                buttonGrid.Clear();
                AppendButtons();
            }

            // Update oldActivePlayersCount
            oldActivePlayersCount = Main.ActivePlayersCount;
        }
    }
}
