using RaNetCore.Common.Entities.Interfaces;

namespace RaNetCore.Common.Entities
{
    public class IdentifiableModel : IIdentifiable
    {
        public int Id { get; set; }
    }
}
