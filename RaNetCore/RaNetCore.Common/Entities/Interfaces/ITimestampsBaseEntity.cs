using System;

namespace RaNetCore.Common.Entities.Interfaces
{
    public interface ITimestampsBaseEntity
    {
        DateTime? CreatedDate { get; set; }

        DateTime? ModifiedDate { get; set; }

        int CreatedBy { get; set; }

        int ModifiedBy { get; set; }
    }
}
