using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using ClimaLatamPipeline.Modelos;

namespace ClimaLatamPipeline.Servicios;

internal class Cargador
{
    private readonly string _connectionString;
    
    public Cargador(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void CargarDatos(List<RegistroDatoApi> datos)
    {
        Console.WriteLine("Iniciando carga de datos a la base de datos...");
        int registrosCargados = 0;
        
        string query = @"
            INSERT INTO fact_metrica (IdPais, IdIndicador, Anio, Valor)
            SELECT @IdPais, @IdIndicador, @Anio, @Valor
            WHERE NOT EXISTS (
                SELECT 1 FROM fact_metrica 
                WHERE IdPais = @IdPais AND IdIndicador = @IdIndicador AND Anio = @Anio
            )";


        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                Console.WriteLine("Conexión a la base de datos establecida.");

                foreach (var dato in datos)
                {
                    if (dato.CodigoPais != null && dato.Indicador?.Id != null && dato.Anio != null)
                    {
                        Console.WriteLine(
                            $"Procesando registro: País={dato.CodigoPais}, Indicador={dato.Indicador.Id}, Año={dato.Anio}, Valor={dato.Valor}");


                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@IdPais", dato.CodigoPais);
                            command.Parameters.AddWithValue("@IdIndicador", dato.Indicador.Id);
                            command.Parameters.AddWithValue("@Anio", int.Parse(dato.Anio));
                            command.Parameters.AddWithValue("@Valor", dato.Valor ?? (object)DBNull.Value);

                            int rowsAffected = command.ExecuteNonQuery();
                            registrosCargados += rowsAffected;
                        }
                    }
                }
            }
            {
                Console.WriteLine($"Carga de datos completada. Total de registros cargados: {registrosCargados}");
            }
            
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar datos: {ex.Message}");
            throw;
        }
    }


    public List<String> ObtenerIndicadores()
    {
        Console.WriteLine("Obteniendo lista de indicadores desde la base de datos...");
        List<string> indicadores = new List<string>();
        
        string query = "SELECT IdIndicador FROM dim_indicador";


        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                Console.WriteLine("Conexión a la base de datos establecida.");

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            indicadores.Add(reader.GetString(0));
                        }
                    }
                }
            }    
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error al obtener indicadores: {e.Message}");
            throw;
        }
        return indicadores;
        
    }

}