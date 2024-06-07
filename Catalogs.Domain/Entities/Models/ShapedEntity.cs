using System.Dynamic;

namespace Catalogs.Domain.Entities.Models
{
    public class ShapedEntity
    {
        public ShapedEntity()
        {
            Entity = new ExpandoObject();
        }

        public int Id { get; set; }
        public ExpandoObject Entity { get; set; }
    }
}
