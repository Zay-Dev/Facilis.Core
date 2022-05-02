using System;

namespace Facilis.Core.Abstractions
{
    public interface IEntityWithExpiration
    {
        DateTime ExpiredAtUtc { get; }
    }
}