using System;

namespace Facilis.Core.Abstractions
{
    public interface IScopeBuilder
    {
        string GetScopeOf(object entity);

        string GetScopeOf(Type type);
    }

    public class ScopeBuilder : IScopeBuilder
    {
        public string GetScopeOf(object entity)
        {
            return GetScopeOf(entity.GetType());
        }

        public string GetScopeOf(Type type)
        {
            return $"{type.Namespace}.{type.Name}";
        }
    }
}