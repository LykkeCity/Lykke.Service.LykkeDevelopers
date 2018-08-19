using Lykke.Service.LykkeDevelopers.Client.Api;
using Lykke.Service.LykkeDevelopers.Contract.Models;
using Microsoft.Extensions.PlatformAbstractions;
using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Lykke.Service.LykkeDevelopers.Client
{
    /// <summary>
    /// LykkeDevelopers API aggregating interface.
    /// </summary>
    public class LykkeDevelopersClient : ILykkeDevelopersClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHomeApi _homeApi;
        private readonly ApiRunner _runner;

        public LykkeDevelopersClient(LykkeDevelopersServiceClientSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            if (String.IsNullOrEmpty(settings.ServiceUrl))
                throw new ArgumentException("Service URL required");

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(settings.ServiceUrl),
                DefaultRequestHeaders =
                {
                    {
                        "User-Agent",
                        $"{PlatformServices.Default.Application.ApplicationName}/{PlatformServices.Default.Application.ApplicationVersion}"
                    }
                }
            };

            _homeApi = RestService.For<IHomeApi>(_httpClient);
            _runner = new ApiRunner();
        }

        public async Task<List<DeveloperModel>> GetDevelopersAsync()
        {
            return await _runner.RunAsync(() => _homeApi.GetAllDevs());
        }

        public async Task<List<DeveloperModel>> CreateDeveloperAsync(DeveloperModel model)
        {
            return await _runner.RunAsync(() => _homeApi.CreateAsync(model));
        }

        public async Task<string> Test(string id)
        {
            return await _runner.RunAsync(() => _homeApi.Test(id));
        }
    }
}
