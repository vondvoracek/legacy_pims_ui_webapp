using MI.PIMS.BL.Common;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace MI.PIMS.UI
{
    public class RedisCacheProvider : ICacheProvider
    {
        private readonly Common.Helper _helper;
        private readonly ILoggerService _logger;

        public RedisCacheProvider(  Common.Helper helper, ILoggerService logger)
        {
            _helper = helper;
            _logger = logger;
        }

        public T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                return default(T);
            try
            {
                var newKey = _helper.ApplicationName + "." + _helper.EnvironmentFirstChar + "." + _helper.MS_ID + "." + key;
                //_logger.Info($"T Get<{typeof(T)}> call with key {newKey}");
                var redis = ConnectionMultiplexer.Connect(_helper.AzureRedisCacheConnectionString);
                var cache = redis.GetDatabase();

                var redisValue = GetStringInRedis(newKey, cache);

                var sRedisValue = redisValue;
                if (typeof(T) == typeof(bool))
                {
                    var tempRedisValue = (string)redisValue;
                    if (tempRedisValue.ToLower() == "true")
                        sRedisValue = RedisValue.Unbox(1);
                    else
                        sRedisValue = RedisValue.Unbox(0);
                }
                //_logger.Info($"Finish T Get<{typeof(T)}> call with key {newKey}");

                
                if (redis != null && redis.IsConnected)
                    redis.Close();

                if (redisValue.HasValue)
                {
                    //_logger.Info($"key {newKey} value:{sRedisValue}");
                    var obj = JsonConvert.DeserializeObject<T>(redisValue);
                    return obj;
                }
                return default(T);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());                
            }  
            return default(T);
        }
        public T GetGlobal<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                return default(T);

            try
            {
                var newKey = _helper.ApplicationName + "." + _helper.EnvironmentFirstChar + "." + key;
                //_logger.Info($"T GetGlobal<{typeof(T)}> call with key {newKey}");
                var redis = ConnectionMultiplexer.Connect(_helper.AzureRedisCacheConnectionString);
                var cache = redis.GetDatabase();

                var redisValue = GetStringInRedis(newKey, cache);

                var sRedisValue = redisValue;
                if (typeof(T) == typeof(bool))
                {
                    var tempRedisValue = (string)redisValue;
                    if (tempRedisValue.ToLower() == "true")
                        sRedisValue = RedisValue.Unbox(1);
                    else
                        sRedisValue = RedisValue.Unbox(0);
                }

                if (redis != null && redis.IsConnected)
                    redis.Close();

                //_logger.Info($"Finish T GetGlobal<{typeof(T)}> with key {newKey}");

                if (redisValue.HasValue)
                {
                    //_logger.Info($"key {newKey} value:{sRedisValue}");
                    var obj = JsonConvert.DeserializeObject<T>(redisValue);                    
                    return obj;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                
            }
            return default(T);
        }

        public IEnumerable<T> GetGlobalList<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                return default(IEnumerable<T>);

            try
            {
                var newKey = _helper.ApplicationName + "." + _helper.EnvironmentFirstChar + "." + key;
                //_logger.Info($"T GetGlobal<{typeof(T)}> call with key {newKey}");
                var redis = ConnectionMultiplexer.Connect(_helper.AzureRedisCacheConnectionString);
                var cache = redis.GetDatabase();

                var redisValue = GetStringInRedis(newKey, cache);

                var sRedisValue = redisValue;
                if (typeof(T) == typeof(bool))
                {
                    var tempRedisValue = (string)redisValue;
                    if (tempRedisValue.ToLower() == "true")
                        sRedisValue = RedisValue.Unbox(1);
                    else
                        sRedisValue = RedisValue.Unbox(0);
                }

                if (redis != null && redis.IsConnected)
                    redis.Close();

                //_logger.Info($"Finish T GetGlobal<{typeof(T)}> with key {newKey}");

                if (redisValue.HasValue)
                {
                    //_logger.Info($"key {newKey} value:{sRedisValue}");
                    var obj = JsonConvert.DeserializeObject<IEnumerable<T>>(redisValue);
                    return obj;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());

            }
            return default(IEnumerable<T>);
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
