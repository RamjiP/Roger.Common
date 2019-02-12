using Newtonsoft.Json;

namespace Roger.Json.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object obj)
        {
            if (obj == null)
            {
                return null;
            }

            return JsonConvert.SerializeObject(obj);
        }
    }
}