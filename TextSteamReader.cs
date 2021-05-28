using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZetko.Xml
{
	public class TextSteamReader
	{
		private string Input { get; set; }
		private int CurrentIndex { get; set; }

		public TextSteamReader( string input )
		{
			Input = input;
			CurrentIndex = 0;
		}

		public char Read()
		{
			if ( CurrentIndex >= Input.Length ) return '\0';

			char currentChar = Input[CurrentIndex];
			CurrentIndex++;
			return currentChar;
		}

		public char Peek()
		{
			return Input[CurrentIndex];
		}

		public string ReadLine()
		{
			string line = "";
			char lastCharacter;
			do
			{
				lastCharacter = Read();
				line += lastCharacter;
			} while ( lastCharacter != '\r' && lastCharacter != '\n' );

			if ( Peek() == '\n' )
			{
				line += Read();
			}

			return line;
		}
	}
}
