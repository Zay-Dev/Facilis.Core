﻿using Facilis.Core.Abstractions;
using Facilis.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Facilis.Core.EntityFrameworkCore.Models
{
    internal class BindingProfileAttributes<T>
        where T : class, IExtendedAttribute, new()
    {
        public string EntityId { get; set; }
        public StatusTypes EntityStatus { get; set; }

        public string Scope { get; set; }
        public T[] Attributes { get; set; }

        public IOperators Operators { get; set; }

        public List<string> ImmutableKeys { get; } = new();
        public Dictionary<string, string> ValuesGroupedInKeys { get; } = new();

        public T[] GetNewAttributes()
        {
            return this.ValuesGroupedInKeys.Keys
                .Where(key => this.ImmutableKeys.Contains(key) ||
                    !this.Attributes.Any(attribute => attribute.Key == key)
                )
                .Select(key =>
                {
                    var value = this.ValuesGroupedInKeys[key];
                    return new T()
                    {
                        Status = this.EntityStatus,
                        CreatedBy = this.Operators.GetSystemOperatorName(),
                        UpdatedBy = this.Operators.GetSystemOperatorName(),

                        Scope = this.Scope,
                        ScopedId = this.EntityId,

                        Key = key,
                        Value = value
                    };
                })
                .ToArray();
        }

        public T[] GetUpdatingAttributes()
        {
            return this.ValuesGroupedInKeys.Keys
                .Where(key => this.Attributes
                    .Any(attribute => attribute.Key == key)
                )
                .Select(key =>
                {
                    var attribute = Array.Find(
                        this.Attributes,
                        attribute => attribute.Key == key
                    );
                    var value = this.ValuesGroupedInKeys[key];

                    var immutable = this.ImmutableKeys.Contains(key);
                    var valueChanged = value != attribute.Value;
                    var statusChanged = this.EntityStatus != attribute.Status;

                    if (!valueChanged && !statusChanged) return null;
                    if (immutable && !statusChanged) return null;

                    attribute.Status = this.EntityStatus;
                    attribute.UpdatedBy = this.Operators.GetSystemOperatorName();
                    attribute.UpdatedAtUtc = DateTime.UtcNow;

                    if (!immutable) attribute.Value = value;

                    return attribute;
                })
                .Where(attribute => attribute != null)
                .ToArray();
        }
    }
}