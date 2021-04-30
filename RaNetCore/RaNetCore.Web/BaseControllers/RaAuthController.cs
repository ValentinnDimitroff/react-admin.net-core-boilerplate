using AutoMapper;

using Microsoft.AspNetCore.Authorization;

using RaNetCore.Common.Entities.Interfaces;
using RaNetCore.Database.Interfaces;
using RaNetCore.Services.BaseServices.Interfaces;

namespace RaNetCore.Web.BaseControllers
{
    [Authorize]
    public class RaAuthController<TBase, TDetails> : RaController<TBase, TDetails>
        where TBase : class, IIdentifiable, new()
        where TDetails : class, IIdentifiable, new()
    {
        public RaAuthController(IRaNetCoreDbContext dbContext, IBaseModelService<TBase> modelService, IMapper mapper) 
            : base(dbContext, modelService, mapper)
        {
        }
    }
}
