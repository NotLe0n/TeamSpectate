using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace TeamSpectate.src
{
    public class TeamSpectateUI : UIState
    {
        Menu menu;
        UIImageButton Button;
        public override void OnInitialize()
        {
            Button = new UIImageButton(ModContent.GetTexture("TeamSpectate/Assets/cameraButton"));
            Button.HAlign = 0.896f;
            Button.VAlign = 0.57f;
            Button.OnClick += (elm, evt) =>
            {
                if (menu == null)
                {
                    menu = new Menu(300, 250);
                    Append(menu);
                }
                else
                {
                    menu.Remove();
                    menu = null;
                }
            };
            Append(Button);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Button.VAlign = Main.mapStyle == 0 || Main.mapStyle == 2 ? 0.32f : 0.57f;

            if (menu != null)
            {
                menu.VAlign = Main.mapStyle == 0 || Main.mapStyle == 2 ? 0.33f : 0.58f;
            }
        }
    }
    public class TeamSpectateDeadUI : UIState
    {
        Menu deadMenu;
        public override void OnInitialize()
        {
            deadMenu = new Menu(300, 250);
            Append(deadMenu);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            deadMenu.VAlign = Main.mapStyle == 0 || Main.mapStyle == 2 ? 0.2f : 0.4f;
        }
    }
}
