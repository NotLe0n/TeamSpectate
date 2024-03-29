﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace TeamSpectate;

internal class PlayerHeadButton : UIImageButton
{
	private readonly Player player;
	private string hovertext;

	public PlayerHeadButton(Player player) : base(ModContent.Request<Texture2D>("TeamSpectate/Assets/empty", AssetRequestMode.ImmediateLoad))
	{
		this.player = player;
		hovertext = $"[{player.whoAmI}] {player.name}";
	}

	public override void LeftClick(UIMouseEvent evt)
	{
		if (player == Main.LocalPlayer) {
			// if you click the button for your own player, the camera gets reset
			Camera.Untarget();
		}
		else {
			// toggle target
			if (Camera.Target == null) {
				Camera.SetTarget(player.whoAmI, false);
			}
			else {
				Camera.Untarget();
			}
		}
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);

		if (IsMouseHovering) {
			Main.hoverItemName = hovertext;
		}

		if (ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true; // so you can't use items while clicking the button
		}

		// draw player head texture
		Rectangle headBounds = new(0, 0, 40, 56);
		Vector2 drawpos = new(Parent.GetDimensions().X + Left.Pixels - 3, Parent.GetDimensions().Y + Top.Pixels - 3);

		bool invalid = player.whoAmI != Main.myPlayer && (player.dead || !player.active || player.team != Main.LocalPlayer.team && player.team != 0);

		// draw head
		spriteBatch.Draw(TextureAssets.Players[0, 0].Value, drawpos, headBounds, invalid ? Color.Gray : player.skinColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
		// draw eyes
		spriteBatch.Draw(TextureAssets.Players[0, 2].Value, drawpos, headBounds, invalid ? Color.Gray : player.eyeColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
		// draw sclera
		spriteBatch.Draw(TextureAssets.Players[0, 1].Value, drawpos, headBounds, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
		// draw hair
		spriteBatch.Draw(TextureAssets.PlayerHair[player.hair].Value, drawpos, headBounds, invalid ? Color.Gray : player.hairColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

		// update hover text
		if (invalid) {
			if (player.dead) {
				hovertext = Language.GetTextValue("Mods.TeamSpectate.PlayerDead");
			} else if (player.team != Main.LocalPlayer.team) {
				hovertext = Language.GetTextValue("Mods.TeamSpectate.PlayerDifferentTeam");
			}
			else {
				hovertext = $"[{player.whoAmI}] {player.name}\n{Language.GetTextValue("Mods.TeamSpectate.PlayerNotFound")}";
			}
		}
		else {
			hovertext = $"[{player.whoAmI}] {player.name}";
		}

		if (Camera.Target == player.whoAmI && Camera.SpectatingBoss == false) {
			// draw frame
			spriteBatch.Draw(ModContent.Request<Texture2D>("TeamSpectate/Assets/selectedFrame", AssetRequestMode.ImmediateLoad).Value,
				GetDimensions().Position(),
				Color.White
			);
		}
	}
}
