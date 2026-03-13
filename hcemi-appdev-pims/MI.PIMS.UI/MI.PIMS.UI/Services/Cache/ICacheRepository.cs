using System.Collections.Generic;

namespace MI.PIMS.UI
{
    public interface ICacheRepository
    {
        /// <summary>
        /// Set the case based on Application and MS ID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set<T>(string key, T value, int? sec = null);
        /// <summary>
        /// Set the cache for entire application
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetGlobal<T>(string key, T value, int? sec = null);
        /// <summary>
        /// Set the cache for entire application with list of objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="sec"></param>
        void SetGlobal<T>(string key, IEnumerable<T> value, int? sec = null);
        /// <summary>
        /// Remove item from cache by Key
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
        /// <summary>
        /// Remove item from cache by Key and MS ID
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ms_id"></param>
        void Remove(string key, string ms_id);
        public void RemoveGlobal(string key);
        void FlushAll();
    }
}