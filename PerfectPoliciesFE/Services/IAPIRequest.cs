using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// interface that defines methods to be implemented in the child class
/// </summary>
namespace PerfectPoliciesFE.Services
{
   public interface IAPIRequest<T>
    {
        List<T> getAll(string controllerName);
        List<T> getAll(string controllerName, string endpoint = "");
        T getSingle(string controllerName, int id);

        T Create(string controllerName, T entity);

        T Edit(string controllerName, int id, T entity);

        T Delete(string controller, int id);

        List<T> GetChildrenforParentID(string controllerName, string Endpoint, int id);

        List<T> GetChilrenforParentName(string controllerName, string EndPoint, string name);
    }
}
