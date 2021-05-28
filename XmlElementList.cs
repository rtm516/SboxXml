using System.Collections.Generic;

namespace DZetko.Xml
{
    public class XmlElementList : List<XmlElement>
    {
        public XmlElement this[string name]
        {
            get
            {
                foreach (XmlElement element in this)
                {
                    if (element.Name == name) return element;
                }

                throw new XmlParserException("No element found with this index: " + name);
            }
        }
    }
}
