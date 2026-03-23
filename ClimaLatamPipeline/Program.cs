using System;
using System.Threading.Tasks;
using ClimaLatamPipeline;
using System.Collections.Generic;
using ClimaLatamPipeline.Modelos;
using ClimaLatamPipeline.Servicios;
using System.Text.RegularExpressions;

Console.WriteLine("Iniciando Pipiline de Datos ambientales de latam.");

var apiClient = new WorldBankApiClient();
var Transformador = new Transformador();



try
{
	string indicadorActual = "EG.FEC.RNEW.ZS";

    string jsonCrudo = await apiClient.ExtraerDatosAmbientalesAsync(indicadorActual);


	List<RegistroDatoApi> datosLimpios = Transformador.LimpiarYConvertir(jsonCrudo);

	Console.WriteLine("Exito - Datos extraídos correctamente.");
	Console.WriteLine($"Se extrajeron y limpiaron {datosLimpios.Count} registros.");

	if(datosLimpios.Count > 0)
	{
		var primerDato = datosLimpios[0];
		Console.WriteLine("\nMuestra del primer registro limpio.");
		Console.WriteLine($"Indicador - {primerDato.Indicador?.Id}");
		Console.WriteLine($"País - {primerDato.CodigoPais}");
		Console.WriteLine($"Año - {primerDato.Anio}");
		Console.WriteLine($"Valor - {primerDato.Valor}");
	}

}
catch (Exception ex)
{
	Console.WriteLine($"Error:{ ex.Message}");
	throw;
}

Console.WriteLine("Fin del Proceso");
Console.ReadKey();


