namespace MyChy.Frame.Core.Redis
{
    internal class RedisConfig
    {
        public bool IsCache { get; set; }

        public string Name { get; set; }

        public double CacheSeconds { get; set; }

        public string Connect { get; set; }

        public int DefaultDatabase { get; set; }

        public bool IsWebCache { get; set; }

        public bool IsRemoveAll { get; set; }


    }
}
