using System.Dynamic;

namespace Catalogs.Domain.Entities.LinkModels
{
    public class LinkResponse
    {
        public bool HasLinks { get; set; }
        public List<ExpandoObject> ShapedEntities { get; set; } = [];
        public LinkCollectionWrapper<ExpandoObject> LinkedEntities { get; set; } = new LinkCollectionWrapper<ExpandoObject>();
    }
}
