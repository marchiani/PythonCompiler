using PythonCompiler.Common.Extensions;
using PythonCompiler.Common.Utility;
using PythonCompiler.Common.Utility.Syntax;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PythonCompiler
{
	public class Program
	{

		static void Main(string[] args)
		{
			var assemblerFilePath = Directory.GetCurrentDirectory() + @"\Files\Lab1.asm";
			var pythonFilePath = Directory.GetCurrentDirectory() + @"\Files\Lab1.py";

			Console.WriteLine("Lexical analyze");

			var vocabularyParsing = new VocabularyParsing();
			var textFromPythonFile = GenericExtensions.ReadFromPythonFile(pythonFilePath);
			var lexerTokens = vocabularyParsing.VocabularyAnalysis(textFromPythonFile);

			var hasErrors = lexerTokens.Any(i => i.Type == TokenType.Error);
			if (hasErrors)
			{
				foreach (var token in lexerTokens.Where(token => token.Type == TokenType.Error))
				{
					Console.WriteLine("Error: \n " + token.Value + " in position:  " + token.Position);
				}
			}
			else
			{

				foreach (var token in vocabularyParsing.VocabularyAnalysis(textFromPythonFile))
				{
					Console.WriteLine(token.Value + " = " + token.Type);
				}

				Console.WriteLine("\n**** Syntax Tree ****\n");

				var parser = new ASTParser(vocabularyParsing.VocabularyAnalysis(textFromPythonFile));
				var abstractSyntaxTree = parser.ParseToAST();
				abstractSyntaxTree.print();

				var compiler = new AssemblerCompiler(abstractSyntaxTree);
				var output = compiler.GenerateAsmText();

				GenericExtensions.WriteToASMFile(output, assemblerFilePath);
			}

			Console.WriteLine("\nPrint enter to exit!");
			Console.ReadLine();
		}
	}
}
