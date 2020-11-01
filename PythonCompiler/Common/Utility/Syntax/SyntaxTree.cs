using System;
using System.Collections.Generic;
using System.Linq;
using PythonCompiler.Models;

namespace PythonCompiler.Common.Utility.Syntax
{
	public class SyntaxTree
	{
		private SyntaxTree Parent;
		private readonly Token Token;
		private readonly List<SyntaxTree> Children;

		public SyntaxTree(Token token)
		{
			this.Token = token;
			this.Children = new List<SyntaxTree>();
		}

		public Token GetToken()
		{
			return Token;
		}

		public List<SyntaxTree> GetChildren()
		{
			return Children;
		}

		public void SetParent(SyntaxTree parent)
		{
			this.Parent = parent;
		}

		public void AddChild(SyntaxTree child)
		{
			Children.Add(child);
		}

		public override string ToString()
		{
			return "AST2{" +
			       "Parent=" + Parent +
			       ", current=" + Token +
			       ", Children=" + Children +
			       '}';
		}

		public void print()
		{
			var indent = "==> ";

			var info = Token.Value + " (" + Token.Type + ")";
			Console.WriteLine(info);

			var abstractSyntaxTree = Children.FirstOrDefault();
			if (abstractSyntaxTree != null)
			{
				info = indent + abstractSyntaxTree.Token.Value + " (" + abstractSyntaxTree.Token.Type + ")";
				Console.WriteLine(info);

				abstractSyntaxTree = abstractSyntaxTree.Children.FirstOrDefault();
				if (abstractSyntaxTree != null)
				{
					info = indent + abstractSyntaxTree.Token.Value + " (" + abstractSyntaxTree.Token.Type + ")";
					Console.WriteLine("\t" + info);

					abstractSyntaxTree = abstractSyntaxTree.Children.FirstOrDefault();
					if (abstractSyntaxTree != null)
					{
						info = indent + abstractSyntaxTree.Token.Value + " (" + abstractSyntaxTree.Token.Type + ")";
						Console.WriteLine("\t\t" + info);
					}
				}
			}

		}

    }
}