using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Utils
{
    public class JsonUtil
    {
        public static async Task<TValue> FromJson<TValue>(string json)
        {
            return await Task.Run(() => JsonConvert.DeserializeObject<TValue>(json));
        }
    }
}