using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI;

namespace TeamSpectate.UI.Buttons;

internal sealed class BossHeadButton : GridButton
{
	private NPC NpcReference { get; }
	private int Index => Main.npc.ToList().FindIndex(x => x == NpcReference);
	private int HeadIndex => NpcReference.GetBossHeadTextureIndex();
	private bool IsNpcAccessible => HeadIndex != -1 && NpcReference is { active: true, dontCountMe: false };
	
	protected override string GetTooltip()
	{
		if (!IsNpcAccessible && !PlayerInput.GetPressedKeys().Contains(Keys.LeftAlt)) {
			return $"{Language.GetTextValue("Mods.TeamSpectate.BossHeadIndexNotFound")}";
		}

		return $"{NpcReference.FullName} ({Language.GetTextValue("Mods.TeamSpectate.BossTooltip")})";
	}

	/// <param name="npc">NPC instance to use as a button reference.</param>
	public BossHeadButton(NPC npc)
	{
		NpcReference = npc;
	}

	public override void LeftClick(UIMouseEvent evt)
	{
		base.LeftClick(evt);

		if (IsNpcAccessible) {
			if (Camera.Target != NpcReference.whoAmI) {
				Camera.SetTarget(NpcReference.whoAmI, true);
				SpectateMenu.IsUpdateRequired = true;
				return;
			}
		}

		if (Camera.Target == NpcReference.whoAmI) {
			// reset the camera
			Camera.Untarget();
			SpectateMenu.IsUpdateRequired = true;
		}
	}

	public override void RightClick(UIMouseEvent evt)
	{
		base.RightClick(evt);

		// reset the camera
		Camera.Untarget();
		SpectateMenu.IsUpdateRequired = true;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);

		// get boss head texture index
		if (!IsNpcAccessible) {
			// draw eye outline if boss is hidden or dead
			spriteBatch.Draw(UIAssets.EyeFrameAsset.Value,
				GetCenter(UIAssets.EyeFrameAsset), null, Color.PeachPuff, 0f,
				Vector2.Zero, 1f, SpriteEffects.None, 0);

			return;
		}

		var headTexture = TextureAssets.NpcHeadBoss[HeadIndex];

		Vector2 headPosition = new() // custom center finder
		{
			X = Parent.GetDimensions().X + Left.Pixels
				+ .5f * (Width.Pixels - headTexture.Width()),

			Y = Parent.GetDimensions().Y + Top.Pixels
				+ .5f * (Height.Pixels - headTexture.Height())
		};

		spriteBatch.Draw(headTexture.Value, headPosition,
			new Rectangle(0, 0, headTexture.Width(), headTexture.Height()),
			Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);

		// draw an outline upon button focused
		if (Camera.Target == Index && Camera.SpectatingBoss) {
			spriteBatch.Draw(UIAssets.FrameOutlineAsset.Value,
				GetCenter(UIAssets.FrameOutlineAsset), null, Color.Coral, 0f,
				Vector2.Zero, 1f, SpriteEffects.None, 0);

			//spriteBatch.Draw(UIAssets.FrameOutlineAsset.Value, GetDimensions().Position(), Color.Coral);
		}
	}
}