using Terraria.GameContent;

namespace TeamSpectate;

public static class ConstantSeedFix
{
	/// <summary>
	/// Fixes a bug where if spectating a player while playing on the "constant" seed, you would still die even if there is enough light around you.
	/// </summary>
	public static void Fix()
	{
		On_DontStarveDarknessDamageDealer.Update += (orig, player) =>
		{
			if (Camera.Target != null) {
				DontStarveDarknessDamageDealer.Reset(); // this makes it possible to cheat, because you won't die
				return;
			}
			orig(player);
		};
	}
}