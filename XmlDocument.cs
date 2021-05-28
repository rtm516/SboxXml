
namespace DZetko.Xml
{
    public class XmlDocument
    {
        private XmlElement _rootNode;

        public XmlElement RootNode
        {
            get
            {
                return _rootNode;
            }

            set
            {
                _rootNode = value;
            }
        }
    }
}
