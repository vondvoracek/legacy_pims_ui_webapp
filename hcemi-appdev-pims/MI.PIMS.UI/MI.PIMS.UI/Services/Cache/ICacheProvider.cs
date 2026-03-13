using System.Collections.Generic;

namespace MI.PIMS.UI
{
    public interface ICacheProvider
    {
        T Get<T>(string key);
        T GetGlobal<T>(string key);
        IEnumerable<T> GetGlobalList<T>(string key);
    }
}