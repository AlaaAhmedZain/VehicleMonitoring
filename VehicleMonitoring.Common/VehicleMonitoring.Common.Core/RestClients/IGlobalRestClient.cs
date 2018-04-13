using System;
using System.Collections.Generic;
using System.Text;

namespace VehicleMonitoring.Common.Core.RestClients
{
    public interface IGlobalRestClient<T> where T : class
    {
        string Add(T obj, string APIMethodURL);
        string Update(T obj, string APIMethodURL);
        string Delete(string APIMethodURL);
        T FindById(int Id, string APIMethodURL);
        IEnumerable<T> GetAll(string APIMethodURL);
        IEnumerable<T> GetWithFilter(Dictionary<String, object> Parametrs, string APIMethodURL);
        IEnumerable<T> Post(Dictionary<String, object> Parametrs, string APIMethodURL);

    }
}
