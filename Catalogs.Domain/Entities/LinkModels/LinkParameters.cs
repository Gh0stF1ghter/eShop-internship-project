using Catalogs.Domain.RequestFeatures;
using Microsoft.AspNetCore.Http;

namespace Catalogs.Domain.Entities.LinkModels
{
    public record LinkParameters(ItemParameters ItemParameters, HttpContext HttpContext);
}
