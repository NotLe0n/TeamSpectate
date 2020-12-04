using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;

namespace TeamSpectate.src
{
    class PlayerHeadButton : UIImageButton
    {
        public Player Player;
        public PlayerHeadButton(Player player) : base(ModContent.GetTexture("TeamSpectate/Assets/empty"))
        {
            Player = player;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (IsMouseHovering)
                Main.hoverItemName = Player.name;

            if (ContainsPoint(Main.MouseScreen)) //so you can't use items while clicking the button
                Main.LocalPlayer.mouseInterface = true;

            Rectangle headBounds = new Rectangle(0, 0, 40, 56);
            Vector2 drawpos = new Vector2(Parent.GetDimensions().X + Left.Pixels - 3, Parent.GetDimensions().Y + Top.Pixels - 3);

            // head
            spriteBatch.Draw(Main.playerTextures[0, 0], drawpos, headBounds, Player.skinColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            // eyes
            spriteBatch.Draw(Main.playerTextures[0, 2], drawpos, headBounds, Player.eyeColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            // sclera
            spriteBatch.Draw(Main.playerTextures[0, 1], drawpos, headBounds, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            // hair
            spriteBatch.Draw(Main.playerHairTexture[Player.hair], drawpos, headBounds, Player.hairColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }
    }
}
