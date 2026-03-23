using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ClimaLatamPipeline.Modelos;

namespace ClimaLatamPipeline.Servicios
{
    internal class Transformador
    {
        public List<RegistroDatoApi> LimpiarYConvertir(string jsonCrudo)
        {
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var listaLimpia = new List<RegistroDatoApi>();


            Console.WriteLine("Iniciando lectura y limpieza del Json...");

            try
            {
                using (JsonDocument doc = JsonDocument.Parse(jsonCrudo))
                {
                JsonElement raiz = doc.RootElement;

                    if (raiz.ValueKind == JsonValueKind.Array && raiz.GetArrayLength() >= 2)
                    {
                        JsonElement arrayDeDatos = raiz[1];

                        listaLimpia = JsonSerializer.Deserialize<List<RegistroDatoApi>>(arrayDeDatos.GetRawText(), opciones);
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al limpiar datos {ex.Message}");

            }
            return listaLimpia ?? new List<RegistroDatoApi>();

        }
    }
}
