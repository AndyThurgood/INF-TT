using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Model.Fhrs;
using Model.Fhrs.Reports;
using Model.Fhrs.Responses;
using Services.Api.Interface;
using Services.Cache.Interface;
using System.Linq;
using Microsoft.Extensions.Options;
using Model.Configuration;
using Model.Fhrs.Extensions;

namespace Server.Controllers.Fhrs
{
    /// <summary>
    /// Establishment Controller, orchestrates calls to the FHRS API (http://api.ratings.food.gov.uk) and does some aggrgation to provide the
    /// consumers receives simple json data.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class EstablishmentController : BaseController
    {
        private readonly IHttpClientServiceFactory _httpClientServiceFactory;

        /// <summary>
        /// Creates a new instance of the FhrsController.
        /// We should be catching exceptions globally and returning an appropriate API status code, simple implementation here.
        /// </summary>
        /// <param name="httpClientServiceFactory"></param>
        /// <param name="cacheService"></param>
        /// <param name="configurationOptions"></param>
        public EstablishmentController(IHttpClientServiceFactory httpClientServiceFactory, ICacheService cacheService, IOptions<ApiConfigurationOption> configurationOptions) : base(cacheService, configurationOptions)
        {
            _httpClientServiceFactory = httpClientServiceFactory ?? throw new ArgumentNullException(nameof(httpClientServiceFactory));
        }


        /// <summary>
        /// Get a report that summaries Establishments by authority.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="schemeType"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("EstablishmentReport")]
        public IActionResult GetEstablishmentReport(int id, int schemeType)
        {
            try
            {
                var report =  GetOrAddToCache(GenerateCacheKey(nameof(AuthorityReport), id), () => GetAuthorityReport(id, schemeType), ConfigurationOptions.Value.CacheKey);               
                return Json(report);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Get an AuthorityReport by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="schemeType"></param>
        /// <returns></returns>
        private AuthorityReport GetAuthorityReport(int id, int schemeType)
        {
            var establishments = GetOrAddToCache(GenerateCacheKey(nameof(Establishment), id), () => GetEstablishments(id), ConfigurationOptions.Value.CacheKey);
            return establishments.GetAuthorityReport(id, GetRatingTypes(schemeType));
        }

        /// <summary>
        /// Get a list of establishments by authority Id.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Establishment> GetEstablishments(int id)
        {
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("localauthorityid", id.ToString()));
            parameters.Add(new KeyValuePair<string, string>("pagesize", "0"));


            using (IHttpClientService httpClientService = _httpClientServiceFactory.Create(ConfigurationOptions.Value.ServiceAddress, ConfigurationOptions.Value.EstablishmentEndpoint, GetApiHeaders()))
            {
                var httpResponse = httpClientService.GetAsync<EstablishmentResponse>(parameters).Result;

                if (!httpResponse.IsSuccessful)
                {
                    throw new InvalidOperationException(httpResponse.ErrorMessage);
                }

                return httpResponse.ResponseContent.Establishments;
            }
        }

        /// <summary>
        /// Get a list of expecting rating types (except they don't match up to the rating keys used...)
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Rating> GetRatingTypes(int schemeType)
        {
            var ratings = new List<Rating>
            {
                new Rating {Name = "5-Star", RatingKey="fhrs_5_en-gb", SchemeType=1 },
                new Rating {Name = "4-Star", RatingKey="fhrs_4_en-gb", SchemeType=1 },
                new Rating {Name = "3-Star", RatingKey="fhrs_3_en-gb", SchemeType=1 },
                new Rating {Name = "2-Star", RatingKey="fhrs_2_en-gb", SchemeType=1 },
                new Rating {Name = "1-Star", RatingKey="fhrs_1_en-gb", SchemeType=1 },
                new Rating {Name = "0-Star", RatingKey="fhrs_0_en-gb", SchemeType=1 },
                new Rating {Name = "Awaiting Inspection", RatingKey="fhrs_awaitinginspection_en-gb", SchemeType=1 },
                new Rating {Name = "Awaiting Publication", RatingKey="fhrs_awaitingpublication_en-gb", SchemeType=1 },
                new Rating {Name = "Exempt", RatingKey="fhrs_exempt_en-gb", SchemeType=1 },
                new Rating {Name = "Pass", RatingKey="fhis_pass_en-gb", SchemeType=2 },
                new Rating {Name = "Pass and eat safe", RatingKey="fhis_pass_and_eat_safe_en-gb", SchemeType=2 },
                new Rating {Name = "Improvement Required", RatingKey="fhis_improvement_required_en-gb", SchemeType=2 },
                new Rating {Name = "Awaiting Inspection", RatingKey="fhis_awaiting_inspection_en-gb", SchemeType=2 },
                new Rating {Name = "Awaiting Publication", RatingKey="fhis_awaiting_publication_en-gb", SchemeType=2 },
                new Rating {Name = "Exempt", RatingKey="fhis_exempt_en-gb", SchemeType=2 },
            };

            return ratings.Where(x => x.SchemeType == schemeType);
        }
    }
}