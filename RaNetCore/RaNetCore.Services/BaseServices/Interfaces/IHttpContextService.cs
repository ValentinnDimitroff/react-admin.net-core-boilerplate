using RaNetCore.Services.BaseServices.Models;

namespace RaNetCore.Services.BaseServices.Interfaces
{
    public interface IHttpContextService
    {
        UserHttpContext User { get; }
    }
}
