using Enyim.Caching;
using MI.PIMS.UI.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MI.PIMS.UI
{
    public class CacheProvider : ICacheProvider
    {
        private readonly IMemcachedClient _memcachedClient;
        private readonly Helper _helper;
        public CacheProvider(IMemcachedClient memcachedClient, Helper helper)
        {
            _memcachedClient = memcachedClient;
            _helper = helper;
        }

        public T Get<T>(string key)
        {
            return _memcachedClient.Get<T>(_helper.ApplicationName + "." + _helper.EnvironmentFirstChar + "." + _helper.MS_ID + "." + key);
        }
        public T GetGlobal<T>(string key)
        {
            return _memcachedClient.Get<T>(_helper.ApplicationName + "." + _helper.EnvironmentFirstChar + "." + key);
        }

        public IEnumerable<T> GetGlobalList<T>(string key)
        {
            return _memcachedClient.Get<IEnumerable<T>>(_helper.ApplicationName + "." + _helper.EnvironmentFirstChar + "." + key);
        }
    }
}
