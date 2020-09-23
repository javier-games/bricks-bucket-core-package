using UnityEngine;
using NUnit.Framework;

namespace BricksBucket.Core.Tests
{
    /// <!-- BucketTestSuit -->
    ///
    /// <summary>
    /// Test suit for the Bucket methods.
    /// </summary>
    /// 
    /// <!-- By Javier García | @jvrgms | 2020 -->
    public class BucketTestSuit
    {
        
        #region Tests

        /// <summary>
        /// Tests the swap method.
        /// </summary>
        [Test]
        public void SwapTest()
        {
            // Structure Swap Test.
            var integerA = 0;
            var integerB = 1;
            var integerC = integerB;
            
            Bucket.Swap (ref integerA, ref integerB);
            Assert.IsTrue (
                integerA == integerC,
                "Wrong Swap method for Structures"
            );
            
            Bucket.Swap (ref integerA, ref integerB);
            Assert.IsTrue (
                integerB == integerC,
                "Wrong Swap method for Structures"
            );
            
            // Classes Swap Test.
            var textureA = new Texture2D (10, 10);
            var textureB = new Texture2D (20, 20);
            var textureC = textureB;
            
            Bucket.Swap (ref textureA, ref textureB);
            Assert.IsTrue (
                textureA == textureC,
                "Wrong Swap method for Classes"
            );
            
            Bucket.Swap (ref textureA, ref textureB);
            Assert.IsTrue (
                textureB == textureC,
                "Wrong Swap method for Classes"
            );
        }
        
        #endregion
    }
}
