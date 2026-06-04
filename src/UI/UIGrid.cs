using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using TeamSpectate.UI.Buttons;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace TeamSpectate.UI;

public class UIGrid : UIElement
{
	public delegate bool ElementSearchMethod(UIElement element);

	/// <summary>
	/// Initial width of the item in UIGrid.
	/// </summary>
	public static int GridItemWidth => UIAssets.FrameAsset.Width() + ModContent.GetInstance<Config>().gridItemGap;

	/// <summary>
	/// Initial height of the item in UIGrid.
	/// </summary>
	public static int GridItemHeight => UIAssets.FrameAsset.Height() + ModContent.GetInstance<Config>().gridItemGap;

	/// <summary>
	/// Grid padding in dots.
	/// </summary>
	public static float GridPadding => ModContent.GetInstance<Config>().gridPadding;

	/// <summary>
	/// Grid max columns value.
	/// </summary>
	public static int GridMaxColumns => ModContent.GetInstance<Config>().gridMaxColumns;

	/// <summary>
	/// Current grid height in dots.
	/// </summary>
	public float GridHeight { get; private set; } = 1f;

	/// <summary>
	/// Current grid width in dots.
	/// </summary>
	public float GridWidth { get; private set; } = 1f;

	/// <summary>
	/// Current grid rows value.
	/// </summary>
	public int GridCurrentRows { get; private set; } = 0;

	/// <summary>
	/// Current grid columns value.
	/// </summary>
	public int GridCurrentColumns { get; private set; } = 0;

	/// <summary>
	/// Grid items container.
	/// </summary>
	public UIElement ItemsContainer { get; private set; } = new();

	public UIGrid()
	{
		Width.Set(0f, 1f);
		Height.Set(0f, 1f);

		OverflowHidden = true;

		ItemsContainer.Width.Set(0f, 1f);
		ItemsContainer.Height.Set(0f, 1f);

		ItemsContainer.OverflowHidden = false;

		Append(ItemsContainer);
	}

	/// <summary>
	/// Adds a new `FilterButton` to a grid.
	/// </summary>
	public void AddFilterButton()
	{
		ItemsContainer.Append(new FilterButton());
		ItemsContainer.Recalculate();
	}

	/// <summary>
	/// Adds a new `PlayerHeadButton` to a grid.
	/// </summary>
	/// <param name="player">Player instance reference.</param>
	public void AddButton(Player player)
	{
		ItemsContainer.Append(new PlayerHeadButton(player));
		ItemsContainer.Recalculate();
	}

	/// <summary>
	/// Adds a new `BossHeadButton` to a grid.
	/// </summary>
	/// <param name="npc">NPC instance reference.</param>
	public void AddButton(NPC npc)
	{
		ItemsContainer.Append(new BossHeadButton(npc));
		ItemsContainer.Recalculate();
	}

	/// <summary>
	/// Clears UIGrid items.
	/// </summary>
	public void Clear()
	{
		ItemsContainer.RemoveAllChildren();
	}

	public static int SortMethod(UIElement item1, UIElement item2)
	{
		return item1.CompareTo(item2);
	}

	public override void Recalculate()
	{
		base.Recalculate();
	}

	public override void RecalculateChildren()
	{
		base.RecalculateChildren();

		int currentColumns = 0;
		int currnetRows = 1;

		int rowIndex = 0;
		int colIndex = 0;

		bool isHeightUpdated = true;

		var children = ItemsContainer.Children.ToList();

		for (int i = 0; i < children.Count; i++) {
			children[i].Top.Set(rowIndex * GridItemHeight + GridPadding, 0f);
			children[i].Left.Set(colIndex * GridItemWidth + GridPadding, 0f);

			children[i].Recalculate();

			if (!isHeightUpdated) {
				currnetRows++;
				isHeightUpdated = true;
			}

			if (colIndex < GridMaxColumns - 1) {
				colIndex++;
			}
			else {
				colIndex = 0;
				rowIndex++;
				isHeightUpdated = false;
			}

			if (currentColumns < GridMaxColumns) {
				currentColumns++;
			}
		}

		// update grid size
		GridWidth = currentColumns * GridItemWidth + 2 * GridPadding;
		GridHeight = currnetRows * GridItemHeight + 2 * GridPadding;

		// update public grid info
		GridCurrentColumns = currentColumns;
		GridCurrentRows = currnetRows;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		Recalculate();
	}
}