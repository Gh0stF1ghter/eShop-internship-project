using Catalogs.Application.DataTransferObjects;
using Catalogs.Domain.Entities.LinkModels;
using Catalogs.Domain.Entities.Models;
using Catalogs.Domain.Interfaces;
using Microsoft.Net.Http.Headers;
using System.Dynamic;

namespace Catalogs.API.Utility
{
    public class ItemLinks(LinkGenerator linkGenerator, IDataShaper<ItemDto> dataShaper) : IItemLinks<ItemDto>
    {
        private readonly LinkGenerator _linkGenerator = linkGenerator;
        private readonly IDataShaper<ItemDto> _dataShaper = dataShaper;

        public LinkResponse TryGenerateLinks(IEnumerable<ItemDto> itemsDtos, string fields, int typeId, HttpContext httpContext)
        {
            var shapedItems = ShapeData(itemsDtos, fields);

            if (ShouldGenerateLinks(httpContext))
            {
                return ReturnLinkedItems(itemsDtos, fields, typeId, httpContext, shapedItems);
            }

            return ReturnShapedItems(shapedItems);
        }

        private List<ExpandoObject> ShapeData(IEnumerable<ItemDto> itemsDto, string fields) =>
            _dataShaper.ShapeData(itemsDto, fields)
                .Select(e => e.Entity)
                .ToList();

        private static bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];

            return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
        }

        private static LinkResponse ReturnShapedItems(List<ExpandoObject> shapedItems) =>
            new() { ShapedEntities = shapedItems };

        private LinkResponse ReturnLinkedItems(IEnumerable<ItemDto> itemDtos, string fields, int typeId, HttpContext httpContext, List<ExpandoObject> shapedItems)
        {
            var itemDtoList = itemDtos.ToList();

            for (var i = 0; i < itemDtoList.Count; i++)
            {
                var itemLinks = CreateLinksForItem(httpContext, typeId, itemDtoList[i].Id, fields);
                shapedItems[i].TryAdd("Links", itemLinks);
            }

            var itemCollection = new LinkCollectionWrapper<ExpandoObject>(shapedItems);
            var linkedItems = CreateLinksForItems(httpContext, itemCollection, fields);

            return new LinkResponse { HasLinks = true, LinkedEntities = linkedItems };
        }

#pragma warning disable CS8604
        private List<Link> CreateLinksForItem(HttpContext httpContext, int typeId, int id, string fields = "")
        {
            var links = new List<Link>
            {
                new(_linkGenerator.GetUriByAction(httpContext,
                                        "GetItemOfType",
                                        values: new {typeId, id, fields }),
                                        "self",
                                        "GET"),
                new(_linkGenerator.GetUriByAction(httpContext,
                                        "DeleteItemOfType",
                                        values: new {typeId, id }),
                                        "delete_item",
                                        "DELETE"),
                new(_linkGenerator.GetUriByAction(httpContext,
                                        "UpdateItemOfType",
                                        values: new {typeId, id }),
                                        "update_item",
                                        "PUT")
            };

            return links;
        }

        private LinkCollectionWrapper<ExpandoObject> CreateLinksForItems(HttpContext httpContext, LinkCollectionWrapper<ExpandoObject> itemsWrapper, string fields)
        {
            itemsWrapper.Links.Add(new(_linkGenerator.GetUriByAction(httpContext,
                                                            "GetItemsOfType",
                                                            values: new { fields }),
                                                            "self",
                                                            "GET"));

            return itemsWrapper;
        }
#pragma warning restore
    }
}
