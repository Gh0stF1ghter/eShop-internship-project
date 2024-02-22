namespace Catalogs.Domain.RequestFeatures
{
    public class ItemParameters : RequestParameters
    {
        public double MinPrice { get; set; } = 0;
        public double MaxPrice { get; set; } = int.MaxValue;

        public uint Stock { get; set; } = 0;

        public string? SearchTerm { get; set; }
    }
}
