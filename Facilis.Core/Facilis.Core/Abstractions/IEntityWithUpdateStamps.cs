using System;

namespace Facilis.Core.Abstractions
{
    public interface IEntityWithUpdateStamps
    {
        string UpdatedBy { get; set; }
        DateTime UpdatedAtUtc { get; set; }
    }
}