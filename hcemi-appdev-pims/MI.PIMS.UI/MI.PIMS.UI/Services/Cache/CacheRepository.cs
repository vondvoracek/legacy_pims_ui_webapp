using Enyim.Caching;
using MI.PIMS.UI.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MI.PIMS.UI
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IMemcachedClient _memcachedClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Helper _helper;

        public CacheRepository(IMemcachedClient memcachedClient, IHttpContextAccessor httpContextAccessor, Helper helper)
        {
            _memcachedClient = memcachedClient;
            _httpContextAccessor = httpContextAccessor;
            _helper = helper;
        }
        
        public void Set<T>(string key, T value, int? sec = null)
        {
            _memcachedClient.Set(_helper.ApplicationName + "." + _helper.EnvironmentFirstChar + "." + _helper.MS_ID + "." + key, value, sec == null? (int)Duration.Day: (int)sec);
        }
        
        public void SetGlobal<T>(string key, T value, int? sec = null)
        {
            _memcachedClient.Set(_helper.ApplicationName + "." + _helper.EnvironmentFirstChar + "." + key, value, sec == null ? (int)Duration.Day : (int)sec);
        }

        public void Remove(string key)
        {
            _memcachedClient.Remove(_helper.ApplicationName + "." + _helper.EnvironmentFirstChar + "." + _helper.MS_ID + "." + key);
        }

        public void Remove(string key, string ms_id)
        {
            _memcachedClient.Remove(_helper.ApplicationName + "." + _helper.EnvironmentFirstChar + "." + ms_id + "." + key);
        }

        public void RemoveGlobal(string key)
        {
            _memcachedClient.Remove(_helper.ApplicationName + "." + _helper.EnvironmentFirstChar + "." + key);
        }

        public void FlushAll()
        {
            _memcachedClient.FlushAll();
        }

        public void SetGlobal<T>(string key, IEnumerable<T> value, int? sec = null)
        {
            _memcachedClient.Set(_helper.ApplicationName + "." + _helper.EnvironmentFirstChar + "." + key, value, sec == null ? (int)Duration.Day : (int)sec);
        }
    }
}
