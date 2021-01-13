using UnityEngine;

namespace Monogum.BricksBucket.Core
{
    /// <!-- TextureExtensions -->
    ///
    /// <summary>
    /// Collection of extension methods for the
    /// <see href="https://docs.unity3d.com/ScriptReference/Texture.html">
    /// UnityEngine.Texture</see> related classes.
    /// </summary>
    ///
    /// <seealso href="https://docs.unity3d.com/ScriptReference/Texture.html">
    /// UnityEngine.Texture</seealso>
    /// <seealso href="https://docs.unity3d.com/ScriptReference/Texture2D.html">
    /// UnityEngine.Texture2D</seealso>
    /// <seealso href="https://docs.unity3d.com/ScriptReference/Sprite.html">
    /// UnityEngine.Sprite</seealso>
    /// 
    /// <!-- By Javier García | @jvrgms | 2020 -->
    public static class TextureExtensions
    {
        /// <summary>Generates a new sample texture resized.</summary>
        /// <param name="source">Texture base.</param>
        /// <param name="targetWidth">Target width.</param>
        /// <param name="targetHeight">Target Height.</param>
        /// <returns>New texture resized.</returns>
        public static Texture2D
            GetSample (this Texture2D source, int targetWidth, int targetHeight)
        {
            var width = source.width;
            var height = source.height;
            var aspect = (float) width / height;
            var targetAspect = (float) targetWidth / targetHeight;

            //  Determines the factor and offset by aspect ratio.
            float factor;
            var offset = Vector2.zero;
            if (aspect > targetAspect)
            {
                // Crop width.
                factor = (float) targetHeight / height;
                offset.x = (int) ((width - height * targetAspect) * 0.5f);
            }
            else
            {
                // Crop height.
                factor = (float) targetWidth / width;
                offset.y = (int) ((height - width / targetAspect) * 0.5f);
            }

            //  Conversion.
            var data = source.GetPixels32 ();
            var targetData = new Color32[targetWidth * targetHeight];
            for (int y = 0; y < targetHeight; y++)
            {
                for (int x = 0; x < targetWidth; x++)
                {
                    var p = new Vector2 (
                        x: Mathf.Clamp (offset.x + x / factor, 0, width - 1),
                        y: Mathf.Clamp (offset.y + y / factor, 0, height - 1)
                    );

                    var floorToInt = new Vector2Int (
                        x: Mathf.FloorToInt (p.x),
                        y: Mathf.FloorToInt (p.y)
                    );
                    var ceilToInt = new Vector2Int (
                        x: Mathf.CeilToInt (p.x),
                        y: Mathf.CeilToInt (p.y)
                    );
                    // Bi-linear filtering.
                    var c11 = data[floorToInt.x + width * floorToInt.y];
                    var c12 = data[floorToInt.x + width * ceilToInt.y];
                    var c21 = data[ceilToInt.x + width * floorToInt.y];
                    var c22 = data[ceilToInt.x + width * ceilToInt.y];

                    targetData[x + y * targetWidth] = Color.Lerp (
                        a: Color.Lerp (c11, c12, p.y),
                        b: Color.Lerp (c21, c22, p.y),
                        t: p.x
                    );
                }
            }

            //  Configure the new texture.
            var texture = new Texture2D (targetWidth, targetHeight);
            texture.SetPixels32 (targetData);
            texture.Apply (true);
            return texture;
        }

        /// <summary>Crops the current texture.</summary>
        /// <param name="source">Texture base.</param>
        /// <param name="offset">Offset to apply.</param>
        /// <returns>Crop the current text.</returns>
        public static Texture2D
            Crop (this Texture2D source, RectOffset offset)
        {
            var x = offset.left + offset.right;
            var y = offset.top + offset.bottom;
            var diffWidth = source.width - x;
            var diffHeight = source.height - y;

            var pixels = source.GetPixels (
                x: offset.left,
                y: offset.bottom,
                blockWidth: diffWidth,
                blockHeight: diffHeight
            );

            var result = new Texture2D (
                width: diffWidth,
                height: diffHeight,
                textureFormat: TextureFormat.RGB24,
                mipChain: false
            );
            result.SetPixels (pixels);
            result.Apply ();

            return result;
        }

        /// <summary> Creates a new sprite form texture. </summary>
        /// <param name="texture"></param>
        /// <returns> New Sprite. </returns>
        public static Sprite ToSprite (this Texture2D texture)
        {
            var rect = new Rect (0, 0, texture.width, texture.height);
            var pivot = Vector2.one * 0.5f;
            return Sprite.Create (texture, rect, pivot);
        }
    }
}