using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace TeamSpectate.src
{
    class PlayerHeadButton : UIImageButton
    {
        public Player Player;
        private string hovertext;
        public int index => Main.player.ToList().FindIndex(x => x == Player);

        public PlayerHeadButton(Player player) : base(ModContent.Request<Texture2D>("TeamSpectate/Assets/empty", ReLogic.Content.AssetRequestMode.ImmediateLoad))
        {
            Player = player;
            hovertext = $"[{index}] {Player.name}";
        }

        public override void Click(UIMouseEvent evt)
        {
            if (Player == Main.LocalPlayer) // if you click the button for your own player, the camera gets reset
            {
                Camera.Locked = false;
                Camera.Target = null; // reset target
            }
            else
            {
                Camera.Locked = !Camera.Locked;
                Camera.Target = Camera.Target == null ? index : (int?)null; // set target
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (IsMouseHovering)
                Main.hoverItemName = hovertext;

            if (ContainsPoint(Main.MouseScreen)) //so you can't use items while clicking the button
                Main.LocalPlayer.mouseInterface = true;

            Rectangle headBounds = new Rectangle(0, 0, 40, 56);
            Vector2 drawpos = new Vector2(Parent.GetDimensions().X + Left.Pixels - 3, Parent.GetDimensions().Y + Top.Pixels - 3);
            bool invalid = (Player == null || !Player.active || Player.team != Main.LocalPlayer.team && Player.team != 0);

            // head
            spriteBatch.Draw(TextureAssets.Players[0, 0].Value, drawpos, headBounds, invalid ? Color.Gray : Player.skinColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            // eyes
            spriteBatch.Draw(TextureAssets.Players[0, 2].Value, drawpos, headBounds, invalid ? Color.Gray : Player.eyeColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            // sclera
            spriteBatch.Draw(TextureAssets.Players[0, 1].Value, drawpos, headBounds, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            // hair
            spriteBatch.Draw(TextureAssets.PlayerHair[Player.hair].Value, drawpos, headBounds, invalid ? Color.Gray : Player.hairColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

            hovertext = invalid ? $"[{index}] {Player.name}" + "\nThis player is in a different team than you or doesn't exist" : $"[{Main.player.ToList().FindIndex(x => x == Player)}] {Player.name}";
        }
    }
}
