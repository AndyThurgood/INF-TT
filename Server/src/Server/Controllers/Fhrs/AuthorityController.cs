using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model.Configuration;
using Model.Fhrs;
using Model.Fhrs.Responses;
using Services.Api.Interface;
using Services.Cache.Interface;

namespace Server.Controllers.Fhrs
{
    /// <summary>
    /// Authority Controller, orchestrates calls to the FHRS API (http://api.ratings.food.gov.uk) and does some aggrgation to ensure the
    /// consumers receives simple json data.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AuthorityController : BaseController
    {
        private readonly IHttpClientServiceFactory _httpClientServiceFactory;

        /// <summary>
        /// Creates a new instance of the FhrsController.
        /// We should be catching exceptions globally and returning an appropriate API status code, simple implementation here.
        /// </summary>
        /// <param name="httpClientServiceFactory"></param>
        /// <param name="cacheService"></param>
        /// <param name="configurationOptions"></param>
        public AuthorityController(IHttpClientServiceFactory httpClientServiceFactory, ICacheService cacheService, IOptions<ApiConfigurationOption> configurationOptions) : base(cacheService, configurationOptions)
        {
            _httpClientServiceFactory = httpClientServiceFactory ?? throw new ArgumentNullException(nameof(httpClientServiceFactory));
        }

        /// <summary>
        /// Get reference data that is the key for further data rrequests.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Authorities")]
        public IActionResult GetAuthorities()
        {
            try
            {
                IEnumerable<Authority> localAuthorities = GetOrAddToCache("LocalAuthorities", GetLocalAuthorities, ConfigurationOptions.Value.CacheKey);
                return Json(localAuthorities);
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }       

        /// <summary>
        /// Get a list of Local Authrities from the FHRS API.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Authority> GetLocalAuthorities()
        {
            using (IHttpClientService httpClientService = _httpClientServiceFactory.Create(ConfigurationOptions.Value.ServiceAddress, ConfigurationOptions.Value.AuthorityEndpoint, GetApiHeaders()))
            {
                var httpResponse = httpClientService.GetSingleAsync<AuthorityResponse>().Result;

                if (!httpResponse.IsSuccessful)
                {
                    throw new InvalidOperationException(httpResponse.ErrorMessage);
                }

                return httpResponse.ResponseContent.Authorities;
            }
        }


    }
}