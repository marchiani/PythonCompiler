using System;
using System.Linq;
using PythonCompiler.Common.Utility.Syntax;

namespace PythonCompiler.Common.Utility
{
	public class AssemblerCompiler
	{
        public string textStart =
            ".386\n" +
            ".model flat, stdcall\n" +
            "\n" +
            "option casemap: none;\n" +
            "include C:\\\\masm32\\include\\masm32rt.inc\n" +
            "\n" +
            "includelib C:\\\\masm32\\lib\\user32.lib\n" +
            "includelib C:\\\\masm32\\lib\\kernel32.lib\n" +
            "\n" +
            ".data\n" +
            ".code\n" +
            "\n";

        public string textNumber =
                "{0} proc\n" +
                "    mov eax, {1}\n" +
                "    fn MessageBoxA, 0, str$(eax), \" Done by Dmytro Boychenko 1\", MB_OK\n" +
                "    ret\n" +
                "{0} endp\n";

        public string textString =
                        "{0} proc\n" +
                        "    fn MessageBoxA, 0, {0}, \"LABA 1\",MB_OK\n" +
                        "    ret\n" +
                        "{0} endp\n";

        public string textEnd =
                "\n" +
                "start:\n" +
                "    invoke %s\n" +
                "    invoke ExitProcess, 0\n" +
                "end start";

        private SyntaxTree abstractSyntaxTree;

        public AssemblerCompiler(SyntaxTree abstractSyntaxTree)
        {
            this.abstractSyntaxTree = abstractSyntaxTree;
        }

        public string GenerateAsmText()
        {

            var abstractSyntaxTreeDefName = abstractSyntaxTree.GetChildren().First();
            var abstractSyntaxTreeReturnValue = abstractSyntaxTree.GetChildren().First().GetChildren().First();

            var variableName = abstractSyntaxTreeDefName.GetToken().Value;
            var returnTypeToken = abstractSyntaxTreeReturnValue.GetToken().Type;
            var returnValue = abstractSyntaxTreeReturnValue.GetToken().Value;

            var result = textStart;

            if (returnTypeToken == TokenType.Str)
            {
                result += generateForString(variableName, returnValue);
            }
            else
            {
                result += generateForNumber(variableName, returnTypeToken, returnValue);
            }

            result += string.Format(textEnd, variableName);

            return result;

        }

        public string generateForNumber(string variableName, TokenType valueType, string value)
        {

            if (valueType == TokenType.Hex)
            {
                value = value.Substring(2) + "h";
            }
            else if (valueType == TokenType.Oct)
            {
                value = value.Substring(2) + "o";
            }

            return string.Format(textNumber, variableName, value, variableName);

        }

        public string generateForString(string variableName, string value)
        {

            return string.Format(textString, variableName, value, variableName);

        }
    }
}