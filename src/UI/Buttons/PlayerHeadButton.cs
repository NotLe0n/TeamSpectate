using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using Terraria;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI;

namespace TeamSpectate.UI.Buttons;

internal sealed class PlayerHeadButton : GridButton
{
	private Player PlayerReference { get; }

	/// <param name="player">Player instance to use as a button reference.</param>
	public PlayerHeadButton(Player player)
	{
		PlayerReference = player;
	}

	/// <summary>
	/// Returns true whenever the player specified fulfills the class requirements.
	/// </summary>
	private bool IsPlayerAccessible =>
		PlayerReference is { dead: false, active: true } && (PlayerReference.team == Main.LocalPlayer.team || PlayerReference.team == 0);
	
	protected override string GetTooltip()
	{
		if (!IsPlayerAccessible) {
			if (!PlayerInput.GetPressedKeys().Contains(Keys.LeftAlt)) {
				if (PlayerReference.dead) {
					return Language.GetTextValue("Mods.TeamSpectate.PlayerDead");
				}

				if (PlayerReference.team != Main.LocalPlayer.team) {
					return Language.GetTextValue("Mods.TeamSpectate.PlayerDifferentTeam");
				}

				return Language.GetTextValue("Mods.TeamSpectate.PlayerNotFound");
			}
			else // alt key hold tooltip
			{
				return PlayerReference.name;
			}
		}
		else {
			return PlayerReference.whoAmI == Main.myPlayer
				? $"{PlayerReference.name} ({Language.GetTextValue("Mods.TeamSpectate.YouTooltip")})"
				: PlayerReference.name;
		}
	}

	public override void LeftClick(UIMouseEvent evt)
	{
		base.LeftClick(evt);

		if (IsPlayerAccessible && PlayerReference.whoAmI != Main.myPlayer) {
			if (Camera.Target != PlayerReference.whoAmI) {
				Camera.SetTarget(PlayerReference.whoAmI, false);
				SpectateMenu.IsUpdateRequired = true;
				return;
			}
		}

		if ((Camera.Target == PlayerReference.whoAmI || PlayerReference.whoAmI == Main.myPlayer) &&
		    Camera.Target != null) {
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

	private void DrawPlayerLayer(Asset<Texture2D> layerAsset, Color color, SpriteBatch spriteBatch)
	{
		// i believe it documented somewhere
		Rectangle bounds = new(0, 0, 40, 56);

		spriteBatch.Draw(layerAsset.Value,
			GetCenter(UIAssets.FrameAsset) - new Vector2(3f, 3f), bounds,
			!IsPlayerAccessible ? Color.Gray : color, 0f,
			Vector2.Zero, 1f, SpriteEffects.None, 0);
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);

		// draw an outline upon button focused
		if (Camera.Target == PlayerReference.whoAmI && Camera.SpectatingBoss == false) {
			spriteBatch.Draw(UIAssets.FrameOutlineAsset.Value,
				GetCenter(UIAssets.FrameOutlineAsset), null, Color.LightGreen, 0f,
				Vector2.Zero, 1f, SpriteEffects.None, 0);
		}

		// draw outline around local player if nothing is focused yet
		else if (Camera.Target == null && PlayerReference.whoAmI == Main.myPlayer) {
			spriteBatch.Draw(UIAssets.FrameOutlineAsset.Value,
				GetCenter(UIAssets.FrameOutlineAsset), null, Color.Gray, 0f,
				Vector2.Zero, 1f, SpriteEffects.None, 0);
		}

		Vector2 headPositionOffset = GetOrigin(UIAssets.FrameAsset) - new Vector2(3f, 3f);

		// draw player face
		DrawPlayerLayer(PlayerLayer.Head, PlayerReference.skinColor, spriteBatch);
		DrawPlayerLayer(PlayerLayer.Eyes, PlayerReference.eyeColor, spriteBatch);
		DrawPlayerLayer(PlayerLayer.Sclera, Color.White, spriteBatch);
		DrawPlayerLayer(PlayerLayer.Hair(PlayerReference), PlayerReference.hairColor, spriteBatch);

		// draw star icon over local player button
		if (PlayerReference == Main.LocalPlayer) {
			spriteBatch.Draw(UIAssets.StarFrameAsset.Value,
				GetCenter(UIAssets.StarFrameAsset), null, Color.White, 0f,
				Vector2.Zero, 1f, SpriteEffects.None, 0);
		}
	}
}
