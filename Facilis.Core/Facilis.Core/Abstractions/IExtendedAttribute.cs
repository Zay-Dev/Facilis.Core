using Facilis.Core.Enums;
using System;

namespace Facilis.Core.Abstractions
{
    public interface IExtendedAttribute :
        IEntityWithId,
        IEntityWithStatus,
        IEntityWithCreateStamps,
        IEntityWithUpdateStamps,
        IEntityWithScope,
        IEntityWithKeyValuePair
    {
        T CastValue<T>();
    }

    public class ExtendedAttribute : IExtendedAttribute
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public StatusTypes Status { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public string UpdatedBy { get; set; }
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

        public string Scope { get; set; }
        public string ScopedId { get; set; }

        public string Key { get; set; }
        public string Value { get; set; }

        public T CastValue<T>()
        {
            return (T)Convert.ChangeType(this.Value, typeof(T));
        }
    }
}