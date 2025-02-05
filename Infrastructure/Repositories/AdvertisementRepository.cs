
using Common.Enums;
using Core.Entities;
using Core.Repositiories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Infrastructure.Repositories
{
    public class AdvertisementRepository : GenericRepository<Advertisement>, IAdvertisementRepository
    {
        private readonly ILogger<AdvertisementRepository> _logger;

        public AdvertisementRepository(ApplicationContext context, ILogger<AdvertisementRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<List<Advertisement>> Search(
            string? query = null, 
            List<Guid>? categories = null,
            AdvertisementSorting sorting = AdvertisementSorting.DateAsc, 
            Dictionary<Guid, List<JsonElement>> parameterEqualsCriteria = null!, 
            Dictionary<Guid, (JsonElement Min, JsonElement Max)> parameterRangeCriteria = null!,
            int costMin = 0,
            int costMax = int.MaxValue,
            bool onlyWithImages = false,
            int skip = 0, 
            int take = 5
            )
        {
            query ??= "";
            query = query.ToUpper();

            var response = Set.Where(x => x.Title.ToUpper().Contains(query));

            if(categories != null && categories.Count > 0)
                response = response.Where(x => categories.Contains(x.Category.Id));

            response = response.Where(x => x.Cost <= costMax && x.Cost >= costMin);

            if(onlyWithImages)
                response = response.Where(x => _context.AdvertisementImages.Any(i => i.Advertisement == x));

            if (parameterEqualsCriteria.Any())
            {
                foreach (var (paramId, values) in parameterEqualsCriteria)
                {
                    var enumIntValues = values.Where(x => x.ValueKind == JsonValueKind.Number).Select(x => x.GetInt32());
                    var floatValues = values.Where(x => x.ValueKind == JsonValueKind.Number).Select(x => x.GetSingle());
                    var stringValues = values.Where(x => x.ValueKind == JsonValueKind.String).Select(x => x.GetString());
                    var boolValues = values.Where(x => x.ValueKind == JsonValueKind.True 
                    || x.ValueKind == JsonValueKind.False).Select(x => x.GetBoolean());

                    response = response.Where(
                        ad => _context.AdvertismentParameterValues.Any(p => 
                        p.Advertisment == ad &&
                        p.CategoryParameter.Id == paramId && (
                            p.CategoryParameter.DataType == ParameterDataType.Integer && enumIntValues.Any(x => x == p.IntegerValue!) ||
                            p.CategoryParameter.DataType == ParameterDataType.Float && floatValues.Any(x => x == p.FloatValue!) ||
                            p.CategoryParameter.DataType == ParameterDataType.String && stringValues.Any(x => x == p.StringValue!) ||
                            p.CategoryParameter.DataType == ParameterDataType.Enum && enumIntValues.Any(x => x == p.EnumValue!) ||
                            p.CategoryParameter.DataType == ParameterDataType.Boolean && boolValues.Any(x => x == p.BooleanValue!)
                            )
                        )
                    );
                }
            }

            if (parameterRangeCriteria.Any())
            {
                foreach (var (paramId, range) in parameterRangeCriteria)
                {
                    var rangeMin = range.Min.ValueKind == JsonValueKind.Number ? range.Min.GetSingle() : float.MinValue;
                    var rangeMax = range.Max.ValueKind == JsonValueKind.Number ? range.Max.GetSingle() : float.MaxValue;

                    _logger.LogInformation($"Using range [{rangeMin}, {rangeMax}] on {paramId}");

                    response = response.Where(
                        ad => _context.AdvertismentParameterValues.Any(p =>
                        p.Advertisment == ad &&
                        p.CategoryParameter.Id == paramId && (
                            p.CategoryParameter.DataType == ParameterDataType.Integer && p.IntegerValue >= rangeMin && p.IntegerValue <= rangeMax ||
                            p.CategoryParameter.DataType == ParameterDataType.Float && p.FloatValue >= rangeMin && p.IntegerValue <= rangeMax
                        )
                        )
                    );
                }
            }

            switch (sorting)
            {
                case AdvertisementSorting.DateAsc:
                    response = response.OrderBy(x => x.CreatedAt);
                    break;
                case AdvertisementSorting.DateDesc:
                    response = response.OrderByDescending(x => x.CreatedAt);
                    break;
                case AdvertisementSorting.CostAsc:
                    response = response.OrderBy(x => x.Cost);
                    break;
                case AdvertisementSorting.CostDesc:
                    response = response.OrderByDescending(x => x.Cost);
                    break;
                case AdvertisementSorting.TitleAsc:
                    response = response.OrderBy(x => x.Title);
                    break;
                case AdvertisementSorting.TitleDesc:
                    response = response.OrderByDescending(x => x.Title);
                    break;
            }

            return await response.Skip(skip).Take(take).ToListAsync();
        }
    }
}
