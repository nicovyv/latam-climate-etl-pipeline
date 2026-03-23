using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace ClimaLatamPipeline.Modelos
{

    public class IndicadorJson
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
    }



    public class RegistroDatoApi
    {
        [JsonPropertyName("indicator")]
        public IndicadorJson? Indicador { get; set; }

        [JsonPropertyName("countryiso3code")]
        public string? CodigoPais { get; set; }

        [JsonPropertyName("date")]
        public string? Anio { get; set; }

        [JsonPropertyName("value")]
        public decimal? Valor { get; set; }

    }



}
