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
            Button.MarginLeft = 1695;
            Button.MarginTop = 570;
            Button.OnClick += (elm, evt) =>
            {
                if (menu == null)
                {
                    menu = new Menu(1390, 570, 300, 250);
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
            Button.MarginTop = Main.mapStyle == 0 || Main.mapStyle == 2 ? 305 : (float)570;

            if (menu != null)
            {
                menu.MarginTop = Main.mapStyle == 0 || Main.mapStyle == 2 ? 305 : (float)570;
            }
        }
    }
}
