using System;

namespace Facilis.Core.Abstractions
{
    public interface IEntityWithCreateStamps
    {
        string CreatedBy { get; set; }
        DateTime CreatedAtUtc { get; set; }
    }
}