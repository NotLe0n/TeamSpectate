using System;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;
using TeamSpectate.UI.Buttons;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;

namespace TeamSpectate.UI;

internal class SpectateMenu : UIPanel
{
	private UIGrid GridInstance { get; } = new();

	/// <summary>
	/// True whenever SpectateMenu update is required.
	/// Useful to update grid items after filter button was pressed.
	/// </summary>
	public static bool IsUpdateRequired { get; set; }
	
	/// <summary>
	/// The speed of animations
	/// </summary>
	private static float AnimationVelocity => MathF.Pow(ModContent.GetInstance<Config>().animationVelocity, 2);

	/// <summary>
	/// True whenever the menu is shown.
	/// </summary>
	public bool IsMenuShown { get; set; } = false;

	public SpectateMenu()
	{
		MaxWidth.Set(512f, 0f);
		MaxHeight.Set(512f, 0f);

		SetPadding(0);

		Append(GridInstance);
	}

	/// <summary>
	/// Fills GridInstance with players.
	/// </summary>
	/// <param name="selectedOnly">Set this to `true` to load only "selected" player. Useful to mix filters during spectating for someone.</param>
	private void AddPlayers(bool selectedOnly = false)
	{
		if (!selectedOnly) {
			// add current player button first
			GridInstance.AddButton(Main.LocalPlayer);
		}

		// Add other players buttons
		foreach (var player in Main.ActivePlayers) {
			// skip local player
			if (player == Main.LocalPlayer) {
				continue;
			}

			if (!selectedOnly) {
				GridInstance.AddButton(player);
			}
			else if (Camera.Target == player.whoAmI) {
				// add only a player selected by a spectator mode
				GridInstance.AddButton(player);
			}
		}
	}

	/// <summary>
	/// Fills GridInstance with npcs.
	/// </summary>
	/// <param name="selectedOnly">Set this to `true` to load only "selected" npc. Useful to mix filters during spectating for someone.</param>
	private void AddBosses(bool selectedOnly = false)
	{
		// Add boss buttons
		foreach (var npc in Main.npc) {
			if (npc.GetBossHeadTextureIndex() == -1) continue;
			
			npc.CheckActive();
			if (!npc.active) continue;
			
			if (!selectedOnly) {
				if (!npc.dontCountMe) {
					GridInstance.AddButton(npc);
				}
			}
			else if (Camera.Target == npc.whoAmI) {
				// add only a npc selected by a spectator mode
				GridInstance.AddButton(npc);
			}
		}
	}

	/// <summary>
	/// Updates grid items.
	/// </summary>
	private void UpdateButtons()
	{
		// clear old items
		GridInstance.Clear();

		new Task(() => // async to improve performance for `for` loops.
		{
			GridInstance.AddFilterButton();

			switch (TeamSpectate.SpectateFilter) {
				case SpectateFilter.Everything:
					AddPlayers();
					AddBosses();
					break;

				case SpectateFilter.Players:
					AddPlayers();
					AddBosses(true);
					break;

				case SpectateFilter.Bosses:
					AddPlayers(true);
					AddBosses();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}).Start();
	}

	public override void Update(GameTime gameTime)
	{
		// update filter icon rotation
		// it updates outside the class,
		// because button re-generates each tick
		// so it need to be a static pre-calculated value
		FilterButton.IconRotation = TeamSpectate.Lerp(FilterButton.IconRotation, FilterButton.IconRotationTarget, AnimationVelocity);

		// async task during opened menu should be fine
		// this is quite a fix for bosses like
		// Lucille's WOTM stage changes, 
		// where bosses can swap each other
		// being at the background with a zero head index.
		if ((IsMenuShown && !GridInstance.IsMouseHovering) || IsUpdateRequired) {
			UpdateButtons();
			IsUpdateRequired = false;
		}

		if (IsMenuShown) {
			Width.Set(TeamSpectate.Lerp(
				Width.Pixels, GridInstance.GridWidth, AnimationVelocity), 0);

			Height.Set(TeamSpectate.Lerp(
				Height.Pixels, GridInstance.GridHeight, AnimationVelocity), 0);
		}
		else {
			// 16f just for clarify,
			// it just looks great in appear animation
			Width.Set(16f, 0f);
			Height.Set(16f, 0f);
		}

		// move the menu beyond screen, so it's hidden
		// align to a processing position otherwise
		Left.Set(IsMenuShown
			? -1 * (Width.Pixels + 235f)
			: 64f, 1f); /* 64f is offset to be sure menu is beyond the screen */
	}
}
