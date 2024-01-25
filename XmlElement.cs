using System;
using System.Collections.Generic;

namespace DZetko.Xml
{
    public class XmlElement
    {
        private string _elementName = String.Empty;
        private XmlElementList _children;
        private XmlElement _parent;
        private List<XmlAttribute> _attributes = new List<XmlAttribute>();
        private string _content;

        public XmlAttribute this[string attributeName]
        {
            get
            {
                foreach (XmlAttribute attribute in Attributes)
                {
                    if (attribute.Name == attributeName) return attribute;
                }

                throw new XmlParserException("No attribute found with index: " + attributeName);
            }
        }

        public XmlElement(string elementName, XmlElement parent)
        {
            _elementName = elementName;
            _parent = parent;
            _children = new XmlElementList();
        }

        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
            }
        }

        public XmlElement Parent
        {
            get
            {
                return _parent;
            }
        }

        public void AddChild(XmlElement element)
        {
            Children.Add(element);
        }

        public void AddAttribute(XmlAttribute attribute)
        {
            Attributes.Add(attribute);
        }

        public XmlElementList Children
        {
            get
            {
                return _children;
            }
        }

        public string Name
        {
            get
            {
                return _elementName;
            }
        }

        public List<XmlAttribute> Attributes
        {
            get
            {
                return _attributes;
            }
        }

		public XmlElement GetChild( string name )
		{
			foreach ( XmlElement child in Children )
			{
				if ( child.Name == name )
				{
					return child;
				}
			}

			return null;
		}

		public List<XmlElement> GetChildren( string name )
		{
			List<XmlElement> foundChildren = new();
			foreach ( XmlElement child in Children )
			{
				if ( child.Name == name )
				{
					foundChildren.Add(child);
				}
			}

			return foundChildren;
		}
    }
}
