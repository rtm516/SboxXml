using System;

namespace DZetko.Xml
{
    public class XmlParserException : Exception
    {
        public XmlParserException() : base() {
        }

        public XmlParserException(string message) : base(message) { 
        }
    }
}
