namespace Catalogs.Domain.Entities.LinkModels
{
    public class LinkCollectionWrapper<T> : LinkResourceBase
    {
        public List<T>? Values { get; set; }

        public LinkCollectionWrapper() { }

        public LinkCollectionWrapper(List<T> values)
        {
            Values = values;
        }
    }
}
