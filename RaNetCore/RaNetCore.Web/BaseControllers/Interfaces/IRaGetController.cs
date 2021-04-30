using System.Collections.Generic;
using System.Threading.Tasks;

namespace RaNetCore.Web.BaseControllers.Interfaces
{
    public interface IRaGetController<TDetails>
    {
        Task<IEnumerable<TDetails>> Get(string filter = "", string range = "", string sort = "");

        Task<TDetails> Get(int id);
    }
}
