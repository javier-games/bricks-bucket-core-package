using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace BricksBucket.Core.Tests
{
	public class ArrayExtensionsTestSuit
	{
		
		#pragma warning disable 649
		private static readonly byte[] NullArray;
		#pragma warning restore 649
		
		private static readonly string[] Alphabet =
		{
			"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
			"N", "O", "P", "Q", "R", "S", "T", "U", "W", "X", "Y", "Z"
		};

		private static readonly float[] FloatArray = { 0.001f };
		

		[Test]
		public void HasIndexTest ()
		{
			Assert.IsTrue (
				Alphabet.HasIndex (24),
				"Wrong ArrayExtensions.HasIndex method."
			);
			
			Assert.IsFalse (
				Alphabet.HasIndex (25),
				"Wrong ArrayExtensions.HasIndex method."
			);
			
			Assert.IsFalse (
				Alphabet.HasIndex (-1),
				"Wrong ArrayExtensions.HasIndex method."
			);
		}

		[Test]
		public void IsNullOrEmptyTest ()
		{
			Assert.IsFalse (
				Alphabet.IsNullOrEmpty (),
				"Wrong ArrayExtensions.IsNullOrEmpty method."
			);
			
			Assert.IsTrue (
				NullArray.IsNullOrEmpty (),
				"Wrong ArrayExtensions.IsNullOrEmpty method."
			);
		}
		
		[Test]
		public void ContainsTest ()
		{
			Assert.IsFalse (
				Alphabet.Contains ("."),
				"Wrong ArrayExtensions.Contains method."
			);
			
			Assert.IsTrue (
				Alphabet.Contains ("A"),
				"Wrong ArrayExtensions.Contains method."
			);
			
			Assert.IsTrue (
				FloatArray.Contains (0.001f),
				"Wrong ArrayExtensions.Contains method."
			);
			
			Assert.IsFalse (
				FloatArray.Contains (0.0001f),
				"Wrong ArrayExtensions.Contains method."
			);
		}
		
		[Test]
		public void ExistsTest ()
		{
			Assert.IsFalse (
				Alphabet.Exists ( s => s == "."),
				"Wrong ArrayExtensions.Exists method."
			);
			
			Assert.IsTrue (
				Alphabet.Exists ( s => s == "A"),
				"Wrong ArrayExtensions.Exists method."
			);
			
			Assert.IsTrue (
				FloatArray.Exists ( f => f.Equals (0.001f)),
				"Wrong ArrayExtensions.Exists method." 
			);
			
			Assert.IsFalse (
				FloatArray.Exists ( f => f.Equals (0.0001f)),
				"Wrong ArrayExtensions.Exists method." 
			);
		}
		
		[Test]
		public void FindTest ()
		{
			Assert.IsTrue (
				Alphabet.Find (".") == -1,
				"Wrong ArrayExtensions.Find method."
			);
			
			Assert.IsTrue (
				Alphabet.Find ("A") == 0,
				"Wrong ArrayExtensions.Find method."
			);
			
			Assert.IsTrue (
				FloatArray.Find (0.001f) == 0,
				"Wrong ArrayExtensions.Find method."
			);
			
			Assert.IsTrue (
				FloatArray.Find (f => f.Equals (0.001f)) == 0,
				"Wrong ArrayExtensions.Find method."
			);
		}
		
		[Test]
		public void SwapTest ()
		{
			var alphabet = new string[Alphabet.Length];
			Alphabet.CopyTo (alphabet, 0);
			alphabet.Swap (0, 1);

			Assert.IsTrue (
				Alphabet[0] == alphabet[1] && Alphabet[1] == alphabet[0],
				"Wrong ArrayExtensions.Swap method."
			);
		}
		
		[Test]
		public void ShuffleTest ()
		{
			var alphabet = new string[Alphabet.Length];
			Alphabet.CopyTo (alphabet, 0);
			alphabet.Shuffle ();

			var isEqual = true;
			for (int i = 0; i < Alphabet.Length; i++)
				isEqual &= Alphabet[i] == alphabet[i];

			Assert.IsFalse (
				isEqual,
				"Wrong ArrayExtensions.Shuffle method."
			);
		}
		
		[Test]
		public void GetRandomTest ()
		{
			Assert.IsFalse (
				string.IsNullOrEmpty (Alphabet.GetRandom ()),
				"Wrong ArrayExtensions.GetRandom method."
			);
		}
		
		[Test]
		public void GetIndexesSampleTest ()
		{
			var indexesSample = FloatArray.GetIndexesSample (2);
			Assert.IsTrue (
				indexesSample[0] == indexesSample[1],
				"Wrong ArrayExtensions.GetIndexesSample method."
			);
			
			indexesSample = Alphabet.GetIndexesSample (100, random: true);
			Assert.IsTrue (
				indexesSample.Length == 100,
				"Wrong ArrayExtensions.GetIndexesSample method."
			);
		}
		
		[Test]
		public void GetSampleTest ()
		{
			var floatSample = FloatArray.GetSample (2);
			Assert.IsTrue (
				floatSample[0].Equals (floatSample[1]),
				"Wrong ArrayExtensions.GetSample method."
			);
			
			var alphabetSample = Alphabet.GetSample (50, random: true);
			Assert.IsTrue (
				alphabetSample.Length == 50,
				"Wrong ArrayExtensions.GetSample method."
			);

			var hasNullValue = false;
			for (int i = 0; i < alphabetSample.Length; i++)
				hasNullValue |= string.IsNullOrEmpty (alphabetSample[i]);
			Assert.IsFalse (
				hasNullValue,
				"Wrong ArrayExtensions.GetSample method."
			);
		}
		
		[Test]
		public void LoopTest ()
		{
			for (int j = 1; j < 10; j++)
			{
				var index = 0;
				var increment = j;
				var isEqual = true;
				for (int i = index; i < Alphabet.Length; i += increment)
				{
					var value = Alphabet.Loop (ref index, increment);
					isEqual &= value == Alphabet[i];
				}

				Assert.IsTrue(
					isEqual,
					"Wrong ArrayExtensions.Loop method." + " " + j 
				);
			}
		}
	}
}