using System;
using System.Threading.Tasks;
using ClimaLatamPipeline;
using System.Collections.Generic;
using ClimaLatamPipeline.Modelos;
using ClimaLatamPipeline.Servicios;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Iniciando Pipiline de Datos ambientales de latam.");



IConfiguration configuration = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.Build();

string? cadenaConexion = configuration.GetConnectionString("climaLatamDB");

var apiClient = new WorldBankApiClient();
var transformador = new Transformador();
var cargador = new Cargador(cadenaConexion);


Console.WriteLine("Obteniendo lista de indicadores a procesar...");
List<String> indicadores = cargador.ObtenerIndicadores();
Console.WriteLine($"Se encontraron {indicadores.Count} indicadores para procesar.");

foreach (string indicador in indicadores)
{
	
	Console.WriteLine($"Procesando indicador: {indicador}");
	
	
	try
	{
		string jsonCrudo = await apiClient.ExtraerDatosAmbientalesAsync(indicador);


		List<RegistroDatoApi> datosLimpios = transformador.LimpiarYConvertir(jsonCrudo);

		Console.WriteLine("Exito - Datos extraídos correctamente.");
		Console.WriteLine($"Se extrajeron y limpiaron {datosLimpios.Count} registros.");

		if(datosLimpios.Count > 0)
		{
			cargador.CargarDatos(datosLimpios);
			Console.WriteLine("Exito - Datos cargados correctamente a la base de datos.");
		}
		else
		{
			Console.WriteLine("No se encontraron datos para cargar en la base de datos.");
		}

	}
	catch (Exception ex)
	{
		Console.WriteLine($"Error:{ ex.Message}");
		throw;
	}
	
	
}




Console.WriteLine("Fin del Proceso");
Console.ReadKey();


