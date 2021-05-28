using System;
using Sandbox;

namespace DZetko.Xml
{
	public class XmlParser
	{
		private string _input = String.Empty;
		private XmlDocument _xmlDocument;
		private InputType _inputType;

		public enum InputType
		{
			Text,
			File
		};

		public XmlParser( InputType inputType, string input )
		{
			_inputType = inputType;
			_input = input;
		}

		public XmlDocument Parse()
		{
			switch ( _inputType )
			{
				case InputType.File:
					{
						_input = FileSystem.Mounted.ReadAllText( _input );
						break;
					}
				case InputType.Text:
					{
						break;
					}
				default:
					{
						throw new XmlParserException( "No input type selected" );
					}
			}

			TextSteamReader rdr = new TextSteamReader( _input );
			_xmlDocument = new XmlDocument();
			char character = rdr.Read();
			if ( character == '<' && rdr.Peek() == '?' )
			{
				rdr.ReadLine();
			}

			char currentRead;
			XmlElement currentParent = null;
			while ( (currentRead = rdr.Read()) > 0 )
			{
				if ( !char.IsWhiteSpace( currentRead ) && (currentRead == '<' || char.IsLetterOrDigit( currentRead )) )
				{
					if ( char.IsLetterOrDigit( currentRead ) )
					{
						string elementContent = currentRead.ToString();
						while ( rdr.Peek() != '<' )
						{
							elementContent += rdr.Read();
						}

						currentParent.Content = elementContent;
					}
					else
					{
						if ( rdr.Peek() == '/' )
						{
							rdr.Read();
							//closing tag
							string innerNode = ReadAlphanumericalName( rdr );

							if ( innerNode != currentParent.Name )
							{
								throw new XmlParserException( "Tag not matching" );
							}

							currentParent = currentParent.Parent;
						}
						else if ( rdr.Peek() == '!' )
						{
							bool hasCommentEnded = false;
							int dashCount = 0;
							while ( !hasCommentEnded )
							{
								while ( rdr.Peek() != '>' )
								{
									if ( rdr.Read() == '-' ) dashCount++;
									else dashCount = 0;
								}

								if ( dashCount >= 2 )
								{
									hasCommentEnded = true;
								}

								rdr.Read();
							}
						}
						else
						{
							string innerNode = ReadAlphanumericalName( rdr );
							XmlElement newElement = new XmlElement( innerNode, currentParent );
							bool isClosingTag = false;

							char nextChar = rdr.Peek();
							while ( (nextChar != '/') && (nextChar != '>') )
							{
								if ( !char.IsWhiteSpace( nextChar ) )
								{
									XmlAttribute attribute = ReadAttribute( rdr, newElement );
									newElement.AddAttribute( attribute );
								}
								else
								{
									rdr.Read();
								}

								nextChar = rdr.Peek();
							}

							if ( rdr.Peek() == '/' )
							{
								isClosingTag = true;
							}

							rdr.Read();

							if ( currentParent == null )
							{
								_xmlDocument.RootNode = newElement;
							}
							else
							{
								currentParent.AddChild( newElement );
							}

							if ( !isClosingTag )
							{
								currentParent = newElement;
							}
						}
					}
				}
			}

			return _xmlDocument;
		}

		public string ReadAlphanumericalName( TextSteamReader reader )
		{
			string name = String.Empty;
			while ( char.IsLetterOrDigit( reader.Peek() ) || reader.Peek() == '_' )
			{
				char currentChar = reader.Read();
				name += currentChar;
			}

			return name;
		}

		public string ReadName( TextSteamReader reader, char stopSign )
		{
			string name = String.Empty;
			while ( reader.Peek() != stopSign )
			{
				char currentChar = reader.Read();
				name += currentChar;
			}

			return name;
		}

		public XmlAttribute ReadAttribute( TextSteamReader reader, XmlElement element )
		{
			string attributeName = ReadAlphanumericalName( reader );
			if ( reader.Peek() != '=' )
			{
				throw new XmlParserException( "Invalid XML attribute syntax." );
			}

			reader.Read();
			if ( reader.Peek() != '"' )
			{
				throw new XmlParserException( "Invalid XML attribute syntax." );
			}

			reader.Read();
			string attributeContent = ReadName( reader, '"' );
			reader.Read();

			XmlAttribute attribute = new XmlAttribute( attributeName, attributeContent, element );
			return attribute;
		}
	}
}
