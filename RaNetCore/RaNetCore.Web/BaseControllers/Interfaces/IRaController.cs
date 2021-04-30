using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace RaNetCore.Web.BaseControllers.Interfaces
{
    public interface IRaController<TDetails> : IRaGetController<TDetails>
    {
        // TODO: Remove JObject

        Task<TDetails> Put(JObject entity);

        Task<TDetails> Post(JObject entity); 

        Task<bool> Delete(int id);
    }
}
