using PythonCompiler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PythonCompiler.Common.Utility
{
	public class VocabularyParsing
	{
		public List<Token> VocabularyAnalysis(string inputString)
		{
			var result = new List<Token>();
			var vocabulary = GetVocabulary();

			var indexes = new HashSet<int>();
			foreach (var pair in vocabulary)
			{
				var i = 0;

				while (i < inputString.Length)
				{
					if (i + 4 < inputString.Length && inputString.Substring(i, 4) == "    ")
					{
						var myToken = new Token(TokenType.Tabulation, "\\t", i);
						myToken.Position = i;

						if (!indexes.Contains(i))
						{
							result.Add(myToken);
						}
						indexes.Add(i);
						i += 4;
					}

					if (inputString.Substring(i, 1) == " ")
					{
						i++;
						continue;
					}

					var token = FindToken(inputString.Substring(i), vocabulary, i);

					token.Position = i;

					if (!indexes.Contains(i))
					{
						result.Add(token);
					}

					indexes.Add(i);

					i += token.Value.Length;
				}
			}

			return result;
		}

		private Token FindToken(string str, Dictionary<TokenType, string> vocabulary, int beginIndex)
		{
			var currentIndex = str.Length;

			foreach (var pair in vocabulary)
			{
				var index = str.IndexOf(pair.Value.ToLower(), StringComparison.Ordinal);

				if (index == 0)
				{
					return pair.Key == TokenType.NewLine ? new Token(pair.Key, "\\n", beginIndex) : new Token(pair.Key, str.Substring(0, pair.Value.Length), beginIndex);
				}

				if (index != -1 && index < currentIndex)
				{
					currentIndex = index;
				}

			}

			var indexOfSpace = str.IndexOf(" ", StringComparison.Ordinal);
			if (indexOfSpace != -1 && indexOfSpace < currentIndex)
			{
				currentIndex = indexOfSpace;
			}

			var undefinedPart = str.Substring(0, currentIndex);

			if (IsInt(undefinedPart))
			{
				return new Token(TokenType.Int, undefinedPart, beginIndex);
			}
			else if (IsStrConstant(undefinedPart))
			{
				return new Token(TokenType.Str, undefinedPart, beginIndex);
			}
			else if (IsHex(undefinedPart))
			{
				return new Token(TokenType.Hex, undefinedPart, beginIndex);
			}
			else if (IsBin(undefinedPart))
			{
				return new Token(TokenType.Bin, undefinedPart, beginIndex);
			}
			else if (IsOct(undefinedPart))
			{
				return new Token(TokenType.Oct, undefinedPart, beginIndex);
			}
			else if (IsVarName(undefinedPart))
			{
				return new Token(TokenType.FunctionName, undefinedPart, beginIndex);
			}

			return new Token(TokenType.Error, undefinedPart, beginIndex);
		}

		private bool IsInt(string str)
		{
			var regex = new Regex("\\d+");
			return regex.Matches(str).Any();
		}

		private bool IsStrConstant(string str)
		{
			var r1 = new Regex("\"[a-zA-Z0-9]*\"");
			var r2 = new Regex("'[a-zA-Z0-9]*'");
			return r1.Matches(str).Any() || r2.Matches(str).Any();
		}

		private bool IsHex(string str)
		{
			var regex = new Regex("^0x[A-F0-9]*");
			return regex.Matches(str).Any();
		}

		private bool IsBin(string str)
		{
			var regex = new Regex("^0b[0-1]*");
			return regex.Matches(str).Any();
		}

		private bool IsOct(string str)
		{
			var regex = new Regex("^0o[0-7]*");
			return regex.Matches(str).Any();
		}

		private bool IsVarName(string str)
		{
			var regex = new Regex("[A-Za-z]*");
			return regex.Matches(str).Any();
		}

		public static Dictionary<TokenType, string> GetVocabulary()
		{
			return new Dictionary<TokenType, string>
			{
				{TokenType.LeftBracket, "("},
				{TokenType.RightBracket, ")"},
				{TokenType.Semicolon, ";"},
				{TokenType.Colon, ":"},
				{TokenType.NewLine, Environment.NewLine},
				{TokenType.Tabulation, "\t"},
				{TokenType.Function, "def"},
				{TokenType.Return, "return"}
			};
		}

	}
}