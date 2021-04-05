using System;
using System.Linq;
using System.Linq.Expressions;

namespace  HAF.Domain
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(Type entityType, string propertyUsed, object valuesUsed)
            : this(entityType, new[] { propertyUsed }, new[] { valuesUsed })
        {
        }

        public EntityNotFoundException(Type entityType, string[] propertiesUsed, object[] valuesUsed)
        {
            if (entityType == null)
                throw new ArgumentNullException(nameof(entityType));
            EntityType = entityType;
            PropertiesUsed = propertiesUsed ?? throw new ArgumentNullException(nameof(propertiesUsed));
            ValuesUsed = valuesUsed ?? throw new ArgumentNullException(nameof(valuesUsed));
        }

        public Type EntityType { get; set; }
        public string[] PropertiesUsed { get; set; }
        public object[] ValuesUsed { get; set; }
    }

    public class EntityNotFoundException<T> : EntityNotFoundException
    {
        public EntityNotFoundException(Expression<Func<T, object>> propertyUsed, object valueUsed)
            : base(typeof(T), ExpressionsHelper.GetPropertyName(propertyUsed), valueUsed)
        {
        }

        public EntityNotFoundException(Expression<Func<T, object>>[] propertiesUsed, object[] valueUsed)
            : base(typeof(T), propertiesUsed.Select(ExpressionsHelper.GetPropertyName).ToArray(), valueUsed)
        {
        }
    }
}