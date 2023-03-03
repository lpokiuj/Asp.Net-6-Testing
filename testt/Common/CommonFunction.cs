using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using testt.Config;

namespace testt.Common
{
    public class CommonFunction
    {
        private CommonFunction() { }

        private static CommonFunction _instance;

        public static CommonFunction GetInstance()
        {
            if (_instance == null)
            {
                _instance = new CommonFunction();
            }
            return _instance;
        }

        public JObject RequestToJObject(IFormCollection data)
        {
            var dataDictionary = data.ToDictionary(x => x.Key, x => x.Value.ToString());
            var dataJObject = new JObject();
            foreach(var kv in dataDictionary)
            {
                dataJObject.Add(kv.Key, kv.Value.ToString());
            }

            return dataJObject;
        }

    }
}
