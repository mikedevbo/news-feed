using System.Net.Http.Json;
using System.Xml.Serialization;
using static System.Net.WebRequestMethods;

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

        public static async Task<string> CallApi<TRequest>(HttpClient http, TRequest request)
        {
            var response = await http.PostAsJsonAsync(request!.GetType().Name, request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
