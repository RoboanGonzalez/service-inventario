using inventario.Context;
using inventario.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace inventario.Service
{
    public class PiezaService
    {
        private readonly AppDbContext _context;

        public PiezaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> InsertarPieza(string operacion, int? id, string codigo, string nombre, string descripcion,
                                      string marca, string modelo, int? anio, string motor, decimal? precio,
                                      int? stock, int? proveedorId)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync("EXEC sp_piezas @i_operacion, @i_Id, @i_Codigo, @i_Nombre, @i_Descripcion, @i_Marca, @i_Modelo, @i_Anio, @i_Motor, @i_Precio, @i_Stock, @i_ProveedorId",
                    new SqlParameter("@i_operacion", operacion),
                    new SqlParameter("@i_Id", id ?? (object)DBNull.Value),
                    new SqlParameter("@i_Codigo", codigo ?? (object)DBNull.Value),
                    new SqlParameter("@i_Nombre", nombre ?? (object)DBNull.Value),
                    new SqlParameter("@i_Descripcion", descripcion ?? (object)DBNull.Value),
                    new SqlParameter("@i_Marca", marca ?? (object)DBNull.Value),
                    new SqlParameter("@i_Modelo", modelo ?? (object)DBNull.Value),
                    new SqlParameter("@i_Anio", anio ?? (object)DBNull.Value),
                    new SqlParameter("@i_Motor", motor ?? (object)DBNull.Value),
                    new SqlParameter("@i_Precio", precio ?? (object)DBNull.Value),
                    new SqlParameter("@i_Stock", stock ?? (object)DBNull.Value),
                    new SqlParameter("@i_ProveedorId", proveedorId ?? (object)DBNull.Value)
                );

                return true; // Indica que la inserción fue exitosa
            }
            catch (Exception ex)
            {
                // Puedes registrar el error si tienes un sistema de logs
                Console.WriteLine($"Error al insertar pieza: {ex.Message}");
                return false; // Indica que hubo un error
            }
        }
        public async Task<List<PiezaResultado>> ConsultarPiezas(string codigo, string marca, string modelo, int? anio, string nombre)
        {
            try
            {
                var sql = "EXEC sp_piezas @i_operacion = 'Q', @i_Codigo = @i_Codigo, @i_Marca = @i_Marca, @i_Modelo = @i_Modelo, @i_Anio = @i_Anio, @i_Nombre = @i_Nombre";

                var resultado = await _context.PiezasResultado
                    .FromSqlRaw(sql,
                        new SqlParameter("@i_operacion", "Q"), // 'Q' para consulta
                        new SqlParameter("@i_Codigo", string.IsNullOrEmpty(codigo) ? (object)DBNull.Value : (object)codigo),
                        new SqlParameter("@i_Marca", string.IsNullOrEmpty(marca) ? (object)DBNull.Value : (object)marca),
                        new SqlParameter("@i_Modelo", string.IsNullOrEmpty(modelo) ? (object)DBNull.Value : (object)modelo),
                        new SqlParameter("@i_Anio", anio.HasValue ? (object)anio.Value : DBNull.Value), // Si anio es nulo, pasar DBNull
                        new SqlParameter("@i_Nombre", string.IsNullOrEmpty(nombre) ? (object)DBNull.Value : (object)nombre)
                    )
                    .ToListAsync();
                Console.WriteLine($"Consulta SQL: {sql}");
                return resultado;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al consultar piezas: {ex.Message}");
                return new List<PiezaResultado>(); // Devuelve una lista vacía en caso de error
            }
        }

        public async Task<string> UpdatePiezaAsync(PiezaUpdateDto piezaUpdate)
        {
            try
            {
                // Llamada a tu procedimiento almacenado
                var parameters = new[]
                {
                    new SqlParameter("@i_operacion", "U"),
                    new SqlParameter("@i_Id", piezaUpdate.Id),
                    new SqlParameter("@i_Codigo", piezaUpdate.Codigo ?? (object)DBNull.Value),
                    new SqlParameter("@i_Nombre", piezaUpdate.Nombre ?? (object)DBNull.Value),
                    new SqlParameter("@i_Descripcion", piezaUpdate.Descripcion ?? (object)DBNull.Value),
                    new SqlParameter("@i_Marca", piezaUpdate.Marca ?? (object)DBNull.Value),
                    new SqlParameter("@i_Modelo", piezaUpdate.Modelo ?? (object)DBNull.Value),
                    new SqlParameter("@i_Anio", piezaUpdate.Anio),
                    new SqlParameter("@i_Motor", piezaUpdate.Motor ?? (object)DBNull.Value),
                    new SqlParameter("@i_Precio", piezaUpdate.Precio ?? (object)DBNull.Value),
                    new SqlParameter("@i_Stock", piezaUpdate.Stock),
                    new SqlParameter("@i_IdProveedor", piezaUpdate.ProveedorId)
                };

                await _context.Database.ExecuteSqlRawAsync("EXEC sp_piezas @i_operacion, @i_Id, @i_Codigo, @i_Nombre, @i_Descripcion, @i_Marca, @i_Modelo, @i_Anio, @i_Motor, @i_Precio, @i_Stock, @i_IdProveedor", parameters);

                return "Pieza actualizada correctamente.";
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return $"Error al actualizar la pieza: {ex.Message}";
            }
        }

        // Método para eliminar una pieza
        public async Task<string> DeletePiezaAsync(PiezaDeleteDto piezaDelete)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@i_operacion", "D"),
                    new SqlParameter("@i_Id", piezaDelete.Id)
                };

                await _context.Database.ExecuteSqlRawAsync("EXEC sp_piezas @i_operacion, @i_Id", parameters);

                return "Pieza eliminada correctamente.";
            }
            catch (Exception ex)
            {
                return $"Error al eliminar la pieza: {ex.Message}";
            }
        }
    }
}

