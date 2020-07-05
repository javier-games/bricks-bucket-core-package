using System.IO;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using UnityEngine;

namespace BricksBucket.Core
{
	/// <!-- StringExtensions -->
	///
	/// <summary>
	/// Collection of extension methods for the
	/// <see href="https://docs.microsoft.com/en-us/dotnet/api/system.string">
	/// System.String</see> class.
	/// </summary>
	///
	/// <seealso href=
	/// "https://docs.microsoft.com/en-us/dotnet/api/system.string">
	/// System.String</seealso>
	/// 
	/// <!-- By Javier García | @jvrgms | 2020 -->
	public static class StringExtensions
	{
		#region Unity Rich Text

		/// <summary> Color Format. </summary>
		private const string ColorTag = "<color={1}>{0}</color>";

		/// <summary> Size Format. </summary>
		private const string SizeTag = "<size={1}>{0}</size>";

		/// <summary> Bold Format. </summary>
		private const string BoldTag = "<b>{0}</b>";

		/// <summary> Italic Format. </summary>
		private const string ItalicTag = "<i>{0}</i>";

		/// <summary>
		/// Add RichText Color Tags to the message.
		/// </summary>
		/// <param name="message">Message to format.</param>
		/// <param name="color">Color to use.</param>
		/// <returns>Formatted RichText string.</returns>
		public static string RichTextColor (this string message, Color color) =>
			string.Format (ColorTag, message, color.Hex ());

		/// <summary>
		/// Add RichText Size Tags to the message.
		/// </summary>
		/// <param name="message">Message to format.</param>
		/// <param name="size">Size to use.</param>
		/// <returns>Formatted RichText string.</returns>
		public static string RichTextSize (this string message, int size) =>
			string.Format (SizeTag, message, size);

		/// <summary>
		/// Add RichText Bold Tags to the message.
		/// </summary>
		/// <param name="message">Message to format.</param>
		/// <returns>Formatted RichText string.</returns>
		public static string RichTextBold (this string message) =>
			string.Format (BoldTag, message);

		/// <summary>
		/// Add RichText Italics Tags to the message.
		/// </summary>
		/// <param name="message">Message to format.</param>
		/// <returns>Formatted RichText string.</returns>
		public static string RichTextItalics (this string message) =>
			string.Format (ItalicTag, message);

		#endregion


		#region String Validation

		/// <summary>
		/// Pattern to identify special characters.
		/// Format this string to add tolerance for special characters.
		/// </summary>
		private const string SpecialCharactersRegex = "[^0-9a-zA-Z{0}]+";

		/// <summary>
		/// Validates whether the text has special characters.
		/// </summary>
		/// <param name="text">Text to validate.</param>
		/// <returns><value>True</value> if has special characters.</returns>
		public static bool HasSpecialCharacters (this string text) =>
			Regex.IsMatch (
				text,
				string.Format (SpecialCharactersRegex, string.Empty)
			);

		/// <summary>
		/// Removes Special Characters from the string maintaining only
		/// characters from A - Z, Digits, and exceptions.
		/// </summary>
		/// <param name="text">Any string text.</param>
		/// <param name="exceptions">Any special character to maintain.</param>
		/// <returns>Text with out special characters.</returns>
		public static string RemoveSpecialCharacters (
			this string text, params char[] exceptions
		)
		{
			var stringBuilder = new StringBuilder ();
			for (int i = 0; i < exceptions.Length; i++)
				stringBuilder.Append (exceptions[i]);

			var pattern = string.Format (
				SpecialCharactersRegex,
				stringBuilder
			);

			return Regex.Replace (
				input: text,
				pattern,
				replacement: string.Empty
			);
		}

		/// <summary>
		/// Removes Districts for a regular text.
		/// Based on the following stack overflow thread.
		/// https://stackoverflow.com/questions/249087
		/// </summary>
		/// <param name="text">Any string text.</param>
		/// <returns>Text with out diacritics.</returns>
		public static string RemoveDiacritics (this string text)
		{
			var normalizedText = text.Normalize (NormalizationForm.FormD);
			var stringBuilder = new StringBuilder ();

			for (int i = 0; i < normalizedText.Length; i++)
			{
				var category = CharUnicodeInfo.GetUnicodeCategory (
					normalizedText[i]
				);

				if (category != UnicodeCategory.NonSpacingMark)
					stringBuilder.Append (normalizedText[i]);
			}

			return stringBuilder.ToString ().
				Normalize (NormalizationForm.FormC);
		}

		/// <summary>
		/// Validates an string as a mail.
		/// </summary>
		/// <param name="email">String to validate.</param>
		/// <returns>Whether the string is valid.</returns>
		public static bool HasEmailFormat (this string email)
		{
			try
			{
				var mail = new MailAddress (email);
				return mail.Address == email;
			}
			catch
			{
				return false;
			}
		}

		#endregion


		#region System.IO

		/// <summary>
		/// Generates an stream from string.
		/// </summary>
		/// <returns>The stream from string.</returns>
		/// <param name="toConvert">String to convert.</param>
		public static Stream ToStream (this string toConvert)
		{
			var stream = new MemoryStream ();
			var writer = new StreamWriter (stream);
			writer.Write (toConvert);
			writer.Flush ();
			stream.Position = 0;
			return stream;
		}

		#endregion
	}
}