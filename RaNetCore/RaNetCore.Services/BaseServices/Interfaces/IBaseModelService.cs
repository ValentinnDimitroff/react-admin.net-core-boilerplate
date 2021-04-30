using System.Linq;
using System.Threading.Tasks;

namespace RaNetCore.Services.BaseServices.Interfaces
{
    public interface IBaseModelService<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll(string filter = "");

        IQueryable<TEntity> GetById(int id);

        Task<TEntity> Create(TEntity entity);

        Task<TEntity> Update(int id, TEntity entity, bool creatorOnlyAllowed = false);

        Task Delete(int id, bool creatorOnlyAllowed = false);
    }
}
