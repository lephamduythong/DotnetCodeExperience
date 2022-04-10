#nullable disable

namespace ServiceLocatorTest
{
    public class ServiceLocator
    {
        private static readonly IDictionary<Type, object> _serviceCache;
        private static ServiceLocator _instance;

        static ServiceLocator()
        {
            _serviceCache = new Dictionary<Type, object>();
        }

        public static ServiceLocator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ServiceLocator();
                }

                return _instance;
            }
        }

        public void Register<T>(T service)
        {
            var key = typeof(T);
            if (!_serviceCache.ContainsKey(key))
            {
                _serviceCache.Add(key, service);
            }
            else  // overwrite the existing instance.
            {
                _serviceCache[key] = service;
            }
        }

        public T GetService<T>()
        {
            var key = typeof(T);
            if (!_serviceCache.ContainsKey(key))
            {
                throw new ArgumentException(string.Format("Type '{0}' has not been registered.", key.Name));
            }

            return (T)_serviceCache[key];
        }
    }

}