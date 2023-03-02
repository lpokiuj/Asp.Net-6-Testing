using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using testt.Config;

namespace testt.Common
{
    public class CommonFunction
    {
        private CommonFunction() { }

        // The Singleton's instance is stored in a static field. There there are
        // multiple ways to initialize this field, all of them have various pros
        // and cons. In this example we'll show the simplest of these ways,
        // which, however, doesn't work really well in multithreaded program.
        private static CommonFunction _instance;

        // This is the static method that controls the access to the singleton
        // instance. On the first run, it creates a singleton object and places
        // it into the static field. On subsequent runs, it returns the client
        // existing object stored in the static field.
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
