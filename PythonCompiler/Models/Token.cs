using PythonCompiler.Common.Utility;

namespace PythonCompiler.Models
{
	public class Token
	{
		public string Value { get; set; }
		public TokenType Type { get; set; }
		public int Position { get; set; }


		public Token(TokenType type, string value, int position)
		{
			Type = type;
			Value = value;
			Position = position;
		}

		public override string ToString()
		{
			return "Token{" +
				   "type=" + Type +
				   ", value='" + Value + '\'' +
				   ", startPosition=" + Position +
				   '}';
		}

	}
}