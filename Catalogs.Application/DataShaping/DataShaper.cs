using System.Reflection;

namespace Catalogs.Application.DataShaping
{
    public class DataShaper<T> : IDataShaper<T> where T : class
    {
        public PropertyInfo[] Properties { get; set; }

        public DataShaper()
        {
            Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        public IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string fieldsString)
        {
            var properties = GetRequiredProperties(fieldsString);

            return FetchData(entities, properties);
        }

        public ShapedEntity ShapeData(T entity, string fieldsString)
        {
            var properties = GetRequiredProperties(fieldsString);

            return FetchDataForEntity(entity, properties);
        }

        private List<PropertyInfo> GetRequiredProperties(string fieldsString)
        {
            var properties = new List<PropertyInfo>();

            if (!string.IsNullOrEmpty(fieldsString))
            {
                var fields = fieldsString.Split(',', StringSplitOptions.RemoveEmptyEntries);

                AddPropertiesToList(fields, properties);
            }
            else
            {
                properties = [.. Properties];
            }

            return properties;
        }

        private void AddPropertiesToList(string[] fields, List<PropertyInfo> properties)
        {
            foreach (var field in fields)
            {
                var property = Properties
                                    .FirstOrDefault(p => p.Name.Equals(field.Trim(), StringComparison.InvariantCultureIgnoreCase));

                if (property != null)
                {
                    properties.Add(property);
                }
            }
        }

        private static List<ShapedEntity> FetchData(IEnumerable<T> entities, IEnumerable<PropertyInfo> properties)
        {
            var shapedData = new List<ShapedEntity>();

            foreach (var entity in entities)
            {
                var shapedObject = FetchDataForEntity(entity, properties);
                shapedData.Add(shapedObject);
            }

            return shapedData;
        }

        private static ShapedEntity FetchDataForEntity(T entity, IEnumerable<PropertyInfo> properties)
        {
            var shapedObject = new ShapedEntity();

            foreach (var property in properties)
            {
                var objectPropertyValue = property.GetValue(entity);
                shapedObject.Entity.TryAdd(property.Name.ToLower(), objectPropertyValue);
            }

            var objectProperty = entity.GetType().GetProperty("Id");
            shapedObject.Id = Convert.ToInt32(objectProperty.GetValue(entity));

            return shapedObject;
        }
    }
}
