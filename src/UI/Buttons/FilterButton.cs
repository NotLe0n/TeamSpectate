using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Localization;
using Terraria.UI;

namespace TeamSpectate.UI.Buttons;

internal sealed class FilterButton : GridButton
{
	protected override string GetTooltip()
	{
		string filterText = Language.GetTextValue("Mods.TeamSpectate.Filter");
		return TeamSpectate.SpectateFilter switch {
			SpectateFilter.Everything   => $"{filterText}: {Language.GetTextValue("Mods.TeamSpectate.FilterEverythingTooltip")}",
			SpectateFilter.Players      => $"{filterText}: {Language.GetTextValue("Mods.TeamSpectate.FilterPlayersTooltip")}",
			SpectateFilter.Bosses       => $"{filterText}: {Language.GetTextValue("Mods.TeamSpectate.FilterBossesTooltip")}",
			_ => ""
		};
	}

	/// <summary>
	/// Filter icon rotation value.
	/// </summary>
	public static float IconRotation { get; set; } = 0f;

	/// <summary>
	/// Filter icon rotation target value. Used in `Lerp()` calculations.
	/// </summary>
	public static float IconRotationTarget { get; private set; } = 0f;

	public override void LeftClick(UIMouseEvent evt)
	{
		var maxItem = Enum.GetNames(typeof(SpectateFilter)).Length - 1;
		TeamSpectate.SpectateFilter = (int)TeamSpectate.SpectateFilter == maxItem ? 0 : TeamSpectate.SpectateFilter + 1;

		IconRotationTarget -= 2 * MathF.PI;

		// force update
		SpectateMenu.IsUpdateRequired = true;
	}

	public override void RightClick(UIMouseEvent evt)
	{
		var maxItem = Enum.GetNames(typeof(SpectateFilter)).Length - 1;
		TeamSpectate.SpectateFilter = (int)TeamSpectate.SpectateFilter == 0
			? (SpectateFilter)maxItem
			: TeamSpectate.SpectateFilter - 1;

		IconRotationTarget += 2 * MathF.PI;

		// force update
		SpectateMenu.IsUpdateRequired = true;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);

		var color = TeamSpectate.SpectateFilter switch {
			SpectateFilter.Everything => Color.White,
			SpectateFilter.Players => Color.LightGreen,
			SpectateFilter.Bosses => Color.LightCoral,
			_ => Color.White
		};

		spriteBatch.Draw(UIAssets.FilterAsset.Value,
			GetCenter(UIAssets.FilterAsset) + GetOrigin(UIAssets.FrameAsset), null,
			color, IconRotation, GetOrigin(UIAssets.FrameAsset), 1f, SpriteEffects.None, 0);
	}
}