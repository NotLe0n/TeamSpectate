using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace TeamSpectate.src;
internal class BossHeadButton : UIImageButton
{
	public readonly NPC boss;
	private int Index => Main.npc.ToList().FindIndex(x => x == boss);

	public BossHeadButton(NPC boss) : base(ModContent.Request<Texture2D>("TeamSpectate/Assets/empty", AssetRequestMode.ImmediateLoad))
	{
		this.boss = boss;
	}

	public override void Click(UIMouseEvent evt)
	{
		base.Click(evt);

		if (Camera.SpectatingBoss && Camera.Target == Index) {
			Camera.Target = null;
			Camera.SpectatingBoss = false;
			Camera.Locked = false;
		}
		else {
			Camera.Target = Index;
			Camera.SpectatingBoss = true;
			Camera.Locked = true;
		}
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);

		if (IsMouseHovering) {
			Main.hoverItemName = $"Spectate Boss: {boss.FullName}";
		}

		if (ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true; // so you can't use items while clicking the button
		}

		// draw boss head texture
		var index = boss.GetBossHeadTextureIndex();
		if (index == -1) {
			return;
		}

		var headTexture = TextureAssets.NpcHeadBoss[index];
		var drawpos = new Vector2(Parent.GetDimensions().X + Left.Pixels, Parent.GetDimensions().Y + Top.Pixels);
		spriteBatch.Draw(headTexture.Value, drawpos, new Rectangle(0, 0, headTexture.Width(), headTexture.Height()), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

		if (Camera.Target == Index && Camera.SpectatingBoss) {
			// draw frame
			spriteBatch.Draw(ModContent.Request<Texture2D>("TeamSpectate/Assets/selectedFrame", AssetRequestMode.ImmediateLoad).Value,
				GetDimensions().Position(),
				Color.White
			);
		}
	}
}
