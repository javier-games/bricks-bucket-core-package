using UnityEngine;

namespace Monogum.BricksBucket.Core
{
	/// <!-- ColorExtensions -->
	///
	/// <summary>
	/// Collection of extension methods for the
	/// <see href="https://docs.unity3d.com/ScriptReference/Color.html">
	/// UnityEngine.Color</see> structure.
	/// </summary>
	///
	/// <seealso href="https://docs.unity3d.com/ScriptReference/Color.html">
	/// UnityEngine.Color</seealso>
	/// 
	/// <!-- By Javier García | @jvrgms | 2020 -->
	public static class ColorExtensions
	{
		/// <summary> Add Brightness to color. </summary>
		/// <param name="color">Color to manage brightness.</param>
		/// <param name="brightness">Brightness to add.</param>
		/// <returns> Brightness color. </returns>
		public static Color Brightness (this Color color, float brightness)
		{
			return new Color (
				color.r + brightness,
				color.g + brightness,
				color.b + brightness,
				color.a
			);
		}

		/// <summary> Sets the alpha of a color. </summary>
		/// <param name="color">Color to manage brightness.</param>
		/// <param name="alpha">New alpha to add.</param>
		/// <returns> Brightness color. </returns>
		public static Color SetAlpha (this Color color, float alpha) =>
			new Color (color.r, color.g, color.b, alpha);

		/// <summary>Convert Color to HEX string color.</summary>
		/// <param name="color">Color to convert to HEX.</param>
		/// <returns>HEX code of a color.</returns>
		public static string Hex (this Color color)
		{
			var colorString = color.a >= 1f
				? ColorUtility.ToHtmlStringRGBA (color)
				: ColorUtility.ToHtmlStringRGB (color);

			return string.Concat ("#", colorString);
		}

		/// <summary>Convert string HEX color to Color.</summary>
		/// <param name="text">String to Convert.</param>
		/// <returns>Color from string.</returns>
		public static Color ToColor (this string text)
		{
			ColorUtility.TryParseHtmlString (text, out var color);
			return color;
		}
	}
}