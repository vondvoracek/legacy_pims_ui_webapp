using MI.PIMS.BL.Common;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace MI.PIMS.UI
{
    public class RedisCacheRepository : ICacheRepository
    {
        private readonly Common.Helper _helper;
        private readonly ILoggerService _logger;

        public RedisCacheRepository(Common.Helper helper, ILoggerService logger)
        {
            _helper = helper;
            _logger = logger;
        }

        public void Set<T>(string key, T value, int? sec = null)
        {
            try
            {
                var newKey = _helper.ApplicationName + "." + _helper.EnvironmentFirstChar + "." + _helper.MS_ID + "." + key;

                _logger.Info($"Set<{typeof(T)}> call with key {newKey}");
                var redis = ConnectionMultiplexer.Connect(_helper.AzureRedisCacheConnectionString);
                var cache = redis.GetDatabase();

                if (!cache.KeyExists(newKey))
                {
                    var redisValue = GetStringInRedis(newKey, cache);
                    var json = JsonConvert.SerializeObject(value); ;
                    var timeSpan = TimeSpan.FromSeconds(sec == null ? (int)Duration.Day : (int)sec);
                    cache.StringSet(newKey, json, timeSpan);
                }

                _logger.Info($"finish Set<{typeof(T)}> call with key {newKey}");
                if (redis != null && redis.IsConnected)
                {
                    redis.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }

        }

        public void SetGlobal<T>(string key, T value, int? sec = null)
        {
            try
            {
                var newKey = _helper.ApplicationName + "." + _helper.EnvironmentFirstChar + "." + key;

                //_logger.Info($"SetGlobal<{typeof(T)}> with key {newKey}");
                var redis = ConnectionMultiplexer.Connect(_helper.AzureRedisCacheConnectionString);
                var cache = redis.GetDatabase();
                
                if (!cache.KeyExists(newKey))
                {                    
                    var json = JsonConvert.SerializeObject(value);
                    var timeSpan = TimeSpan.FromSeconds(sec == null ? (int)Duration.Day : (int)sec);
                    cache.StringSet(newKey, json, timeSpan);

                    _logger.Info($"Setting SetGlobal<{typeof(T)}> with key {newKey} and value: {json}");
                }
                _logger.Info($"Finish SetGlobal<{typeof(T)}> with key {newKey}");
                if (redis != null && redis.IsConnected)
                {
                    redis.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
        }

        public void SetGlobal<T>(string key, IEnumerable<T> value, int? sec = null)
        {
            try
            {
                var newKey = _helper.ApplicationName + "." + _helper.EnvironmentFirstChar + "." + key;

                //_logger.Info($"SetGlobal<{typeof(T)}> with key {newKey}");
                var redis = ConnectionMultiplexer.Connect(_helper.AzureRedisCacheConnectionString);
                var cache = redis.GetDatabase();

                if (cache.KeyExists(newKey))
                {
                    RemoveGlobal(newKey);
                }

                var json = JsonConvert.SerializeObject(value);
                var timeSpan = TimeSpan.FromSeconds(sec == null ? (int)Duration.Day : (int)sec);
                cache.StringSet(newKey, json, timeSpan);

                _logger.Info($"Setting SetGlobal<{typeof(T)}> with key {newKey} and value: {json}");
                _logger.Info($"Finish SetGlobal<{typeof(T)}> with key {newKey}");
                if (redis != null && redis.IsConnected)
                {
                    redis.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
        }

        public void Remove(string key)
        {
            var newKey = _helper.ApplicationName + "." + _helper.EnvironmentFirstChar + "." + _helper.MS_ID + "." + key;
            _logger.Info($"Remove() with key {newKey}");

            RemoveKey(newKey);

            _logger.Info($"Finish Remove() with key {newKey}");
        }

        public void Remove(string key, string ms_id)
        {
            var newKey = _helper.ApplicationName + "." + _helper.EnvironmentFirstChar + "." + ms_id + "." + key;
            _logger.Info($"Remove() with key {newKey}");

            RemoveKey(newKey);

            _logger.Info($"Finish Remove() with key {newKey}");
        }

        public void RemoveGlobal(string key)
        {
            var newKey = _helper.ApplicationName + "." + _helper.EnvironmentFirstChar + "." + key;
            _logger.Info($"RemoveGlobal() with key {newKey}");

            RemoveKey(newKey);

            _logger.Info($"Finish RemoveGlobal() with key {newKey}");
        }

        public void FlushAll()
        {
            try
            {
                _logger.Info("FlushAll()");

                var connectionMultiplexer = ConnectionMultiplexer.Connect(_helper.AzureRedisCacheConnectionString);
                var endpoints = connectionMultiplexer.GetEndPoints();
                var server = connectionMultiplexer.GetServer(endpoints[0]); // Use first endpoint

                var cache = connectionMultiplexer.GetDatabase();

                foreach (var key in server.Keys(pattern: "*"))
                {
                    _logger.Info($"Deleting key: {key}");
                    cache.KeyDelete(key);
                }

                _logger.Info("Finished FlushAll()");
            }
            catch (Exception ex)
            {
                _logger.Error($"FlushAll() Exception: {ex}");
            }
        }


        private void RemoveKey(string key)
        {
            try
            {
                var redis = ConnectionMultiplexer.Connect(_helper.AzureRedisCacheConnectionString);
                var cache = redis.GetDatabase();

                if (cache.KeyExists(key))
                    cache.KeyDelete(key);

                if (redis != null && redis.IsConnected)
                {
                    redis.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
        }

        private RedisValue GetStringInRedis(string key, IDatabase db)
        {
            var redisValue = db.StringGet(key);
            if (redisValue.HasValue)
            {
                return redisValue;
            }

            return RedisValue.Null;
        }
    }
}
