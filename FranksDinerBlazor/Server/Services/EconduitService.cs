
using FranksDinerBlazor.Server.Interfaces;
using FranksDinerBlazor.Shared.Models.Econduit;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FranksDinerBlazor.Server.Services
{
    public class EconduitService : IEconduitService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public EconduitService(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public async Task<bool> RunTransaction(RunTransaction parameters)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json");

            var httpResponseMessage = await httpClient.PostAsync(
                _config.GetValue<string>("Settings:Econduit:Url") + "/runtransaction", content);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var response =
                    await httpResponseMessage.Content.ReadAsStringAsync();

                var data = JObject.Parse(response);

                if (data["ResultCode"]?.Value<string>() == "Approved")
                {
                    return true;
                }                
            }
            return false;
        }
    }
}
