using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace ClimaLatamPipeline
{
    internal class WorldBankApiClient
    {
        private readonly HttpClient _httpClient;



        public WorldBankApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<string> ExtraerDatosAmbientalesAsync(string indicadorId)
        {
            string url = $"https://api.worldbank.org/v2/country/ARG;BRA;CHL;COL;MEX/indicator/{indicadorId}?format=json&date=2000:2023&per_page=1000";

            Console.WriteLine($"[Extracción] Consultando API para el indicador {indicadorId}...");

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }



    }
}
