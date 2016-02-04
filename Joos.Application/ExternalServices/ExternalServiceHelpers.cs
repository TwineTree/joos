using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Joos.ExternalServices
{
    public class ExternalServiceHelper<T> where T : class
    {
        public async Task<T> Get(string url)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                var response = (HttpWebResponse) (await request.GetResponseAsync());
                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var data = await reader.ReadToEndAsync();
                        return JsonConvert.DeserializeObject<T>(data);
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    using (var stream = resp.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var data = await reader.ReadToEndAsync();
                            return JsonConvert.DeserializeObject<T>(data);
                        }
                    }
                }
            }
            catch
            {
            }

            return null;
        }
    }
}
