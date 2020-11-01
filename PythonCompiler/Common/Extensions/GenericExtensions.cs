using System;
using System.IO;
using System.Text;

namespace PythonCompiler.Common.Extensions
{
	public static class GenericExtensions
	{
		public static void WriteToASMFile(string text, string path)
		{
			try
			{
				File.WriteAllText(path, text);

			} catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

		}

		public static string ReadFromPythonFile(string path)
		{
			try
			{
				string textFromFile;
				using (var stream = File.OpenRead(path))
				{
					var array = new byte[stream.Length];
					stream.Read(array, 0, array.Length);
					textFromFile = Encoding.Default.GetString(array);
				}

				if (textFromFile.Length == 0)
				{
					Console.WriteLine("File is empty");
				}

				return textFromFile;

			} catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				return null;
			}

		}

    }
}