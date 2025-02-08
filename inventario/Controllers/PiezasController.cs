using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using inventario.Context;
using inventario.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Data;
using System.Data.SqlClient;
using System.Data;
using inventario.Service;


namespace inventario.Controllers
{

    [ApiController]
    [Route("api/piezas")]
    public class PiezasController : ControllerBase
    {
        private readonly PiezaService _piezaService;

        public PiezasController(PiezaService piezaService)
        {
            _piezaService = piezaService;
        }

        [HttpPost("insertar")]
        public async Task<IActionResult> InsertarPieza([FromBody] Piezas pieza)
        {
            bool resultado = await _piezaService.InsertarPieza("I", null, pieza.Codigo, pieza.Nombre, pieza.Descripcion,
                                                               pieza.Marca, pieza.Modelo, pieza.Anio, pieza.Motor,
                                                               pieza.Precio, pieza.Stock, pieza.ProveedorId);

            if (resultado)
            {
                return Ok(new { mensaje = "Pieza insertada correctamente" });
            }
            else
            {
                return StatusCode(500, new { mensaje = "Hubo un error al momento de insertar el registro" });
            }
        }

        [HttpPost("consultar")]
        public async Task<IActionResult> ConsultarPiezas([FromBody] PiezaFiltroDTO filtro)
        {


            var piezas = await _piezaService.ConsultarPiezas(filtro.Codigo, filtro.Marca, filtro.Modelo, filtro.Anio, filtro.Nombre);

            if (piezas.Any())
            {
                return Ok(piezas);
            }
            else
            {
                return NotFound(new { mensaje = "No se encontraron piezas con los criterios proporcionados" });
            }
        }

        // Método HttpPost para actualizar una pieza
        [HttpPost("update")]
        public async Task<IActionResult> UpdatePieza([FromBody] PiezaUpdateDto piezaUpdate)
        {
            if (piezaUpdate == null)
            {
                return BadRequest("Los datos proporcionados no son válidos.");
            }

            // Llamar al servicio para actualizar la pieza
            var result = await _piezaService.UpdatePiezaAsync(piezaUpdate);

            // Si la actualización es exitosa
            if (result == "Pieza actualizada exitosamente.")
            {
                return Ok(result);
            }

            // Si hubo un error
            return StatusCode(500, result);
        }

        // Método POST para eliminar una pieza
        [HttpPost("delete")]
        public async Task<IActionResult> DeletePieza([FromBody] PiezaDeleteDto piezaDelete)
        {
            if (piezaDelete == null || piezaDelete.Id <= 0)
            {
                return BadRequest("El id de la pieza es inválido.");
            }

            var result = await _piezaService.DeletePiezaAsync(piezaDelete);

            if (result.Contains("Error"))
            {
                return BadRequest(result); // Si ocurre un error, respondemos con BadRequest.
            }

            return Ok(result); // Si la eliminación fue exitosa, respondemos con Ok.
        }

    }
}
