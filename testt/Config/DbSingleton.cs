namespace testt.Config
{
    public sealed class DbSingleton
    {
        private readonly IConfiguration _configuration;

        private DbSingleton(IConfiguration configuration) { 
            this._configuration = configuration;
        }   
        private static DbSingleton _instance = null;
        public static DbSingleton Instance
        {
            get { 
                if(_instance == null)
                {
                    /*_instance = new DbSingleton();*/
                }
                return _instance;
            }
        }
    }
}
