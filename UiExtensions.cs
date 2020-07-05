using UnityEngine;
using UnityEngine.UI;

namespace BricksBucket.Core
{
	/// <!-- UiExtensions -->
	///
	/// <summary>
	/// Collection of extension methods for the
	/// <see href="https://docs.unity3d.com/Packages/com.unity.ugui@1.0/api/">
	/// UnityEngine.UI</see> package and related classes.
	/// </summary>
	///
	/// <seealso href=
	/// "https://docs.unity3d.com/Packages/com.unity.ugui@1.0/api/">
	/// UnityEngine.UI</seealso>
	/// 
	/// <!-- By Javier García | @jvrgms | 2020 -->
	public static class UiExtensions
	{
		/// <summary> Updates Unity Text Component Text. </summary>
		/// <param name="label">Updates a label.</param>
		/// <param name="text">Text to add to the labels.</param>
		public static void UpdateText (this Text label, params string[] text) =>
			label.text = string.Concat (text);

		/// <summary> Sets the image alpha. </summary>
		/// <param name="image">Sets the alpha of an Image.</param>
		/// <param name="alpha">Value of the new alpha.</param>
		public static void SetAlpha (this Image image, float alpha) =>
			image.color.SetAlpha (alpha);

		/// <summary> Activate alpha, interactivity and rays. </summary>
		/// <param name="canvas">Canvas to modify.</param>
		/// <param name="active">Whether to active the canvas group.</param>
		public static void ActivateAll (this CanvasGroup canvas, bool active)
		{
			canvas.alpha = active ? 1 : 0;
			canvas.interactable = active;
			canvas.blocksRaycasts = active;
		}
	}
}