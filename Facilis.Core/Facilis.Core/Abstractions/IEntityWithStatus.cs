using Facilis.Core.Enums;

namespace Facilis.Core.Abstractions
{
    public interface IEntityWithStatus
    {
        StatusTypes Status { get; set; }
    }
}