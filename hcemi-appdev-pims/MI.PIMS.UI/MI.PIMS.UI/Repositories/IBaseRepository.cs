using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Repositories
{
    public interface IBaseRepository
    {
        Task<T> GetAsync<T>(string apiUrl);
        Task<IEnumerable<T>> GetAsyncList<T>(string apiUrl);
        Task<T> PostAsync<T>(string apiUrl, object obj);
        Task<IEnumerable<T>> PostAsyncList<T>(string apiUrl, object obj);
        Task<T> PutAsync<T>(string apiUrl, object obj);
        Task<IEnumerable<T>> PutAsyncList<T>(string apiUrl, object obj);
        Task<T> DeleteAsync<T>(string apiUrl);
    }
}
