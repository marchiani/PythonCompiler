using PythonCompiler.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PythonCompiler.Common.Utility.Syntax
{
	public class ASTParser
	{
		protected SyntaxTree MainSyntaxTree;
		protected SyntaxTree CurrentSyntaxTree;
		protected Token CurrentToken;
		protected List<Token> TokenList;
		protected List<TokenType> SignatureFunctionTypes = new List<TokenType> {
					TokenType.LeftBracket,
					TokenType.RightBracket,
					TokenType.Colon,
					TokenType.NewLine,
					TokenType.Tabulation
			};

		public ASTParser(List<Token> tokenList)
		{
			this.TokenList = tokenList;
		}

		public SyntaxTree ParseToAST()
		{

			var currentTypes = TokenList.Select(token => token.Type).ToList();

			IsNotWrongBodyFunction(currentTypes);
			IsNotWrongAST(TokenList);
			return MainSyntaxTree;
		}

		public void IsNotWrongBodyFunction(List<TokenType> ourTypes)
		{
			var bodyTokens = new List<TokenType> {
					TokenType.Function,
					TokenType.FunctionName,
					TokenType.LeftBracket,
					TokenType.RightBracket,
					TokenType.Colon,
					TokenType.Return
			};

			if (!bodyTokens.Any(ourTypes.Contains))
			{
				Console.WriteLine("Body of your function is wrong!");
				Environment.Exit(0);
			}
			if (ourTypes.Contains(TokenType.NewLine) && !ourTypes.Contains(TokenType.Tabulation))
			{
				Console.WriteLine("Error in body of your function! Firstly, new line separator, after tabulation!");
				Environment.Exit(0);
			}

		}

		public void IsNotWrongAST(List<Token> tokens)
		{
			foreach (var token in tokens)
			{
				CurrentToken = token;

				if (CurrentToken.Type == TokenType.Function)
				{
					MainSyntaxTree = new SyntaxTree(CurrentToken);
					CurrentSyntaxTree = MainSyntaxTree;
					continue;
				}
				if (CurrentToken.Type == TokenType.FunctionName)
				{
					AddToAST();
					continue;
				}
				if (CurrentToken.Type == TokenType.Return)
				{
					AddToAST();
					continue;
				}
				if (SignatureFunctionTypes.Count != 0)
				{
					IsNotWrongType(CurrentToken);
					continue;
				}
				ParseReturnValue(CurrentToken);
				break;
			}
		}

		public void AddToAST()
		{
			var abstractSyntaxTree = new SyntaxTree(CurrentToken);
			abstractSyntaxTree.SetParent(CurrentSyntaxTree);

			CurrentSyntaxTree.AddChild(abstractSyntaxTree);
			CurrentSyntaxTree = abstractSyntaxTree;
		}

		private void ParseReturnValue(Token token)
		{
			switch (token.Type.ToString())
			{
				case "Int":
				case "Bin":
				case "Str":
					AddToAST();
					break;
				default:
					Console.WriteLine("Error in return type value! You can return only Integer, or String, or Binary!");
					Environment.Exit(0);
					break;
			}
		}

		public void IsNotWrongType(Token token)
		{
			if (SignatureFunctionTypes.Contains(token.Type))
			{
				SignatureFunctionTypes.Remove(token.Type);
			}
			else
			{
				Console.WriteLine("Error! Token with value '" + token.Value + "' has type " + token.Type + " !");
			}

		}
	}
}