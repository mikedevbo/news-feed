using System.Xml.Serialization;

namespace NewsFeed.Client
{
    public static partial class Extensions
    {
        public static T DeserializeXml<T>(this string @this)
        {
            var x = new XmlSerializer(typeof(T), new XmlRootAttribute("Root"));
            var r = new StringReader(@this);

            return (T)x.Deserialize(r)!;
        }
    }
}
