using RaNetCore.Common.Entities.Interfaces;

namespace RaNetCore.Web.ViewModels.Interfaces
{
    public interface IRaImage : IIdentifiable
    {
        string Title { get; set; }

        string Url { get; set; }

        string Base64String { get; set; }
    }
}
