using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task_test.Data;
using Task_test.Models;

namespace Task_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditorTarefasController : ControllerBase
    {
        private readonly APIDbContext _context;

        public EditorTarefasController(APIDbContext context)
        {
            _context = context;
        }

        // GET: api/EditorTarefas
        [HttpGet("Todas_Tasks")]
        public async Task<ActionResult<IEnumerable<EditorTarefas>>> GetEditorTarefas()
        {
            if (_context.Tarefas == null)
            {
                return NotFound();
            }

            return await _context.Tarefas.ToListAsync();
        }

        // GET: api/EditorTarefas/5
        [HttpGet("Encontra_Task/{id}")]
        public async Task<ActionResult<EditorTarefas>> GetEditorTarefas(int id)
        {
            if (_context.Tarefas == null)
            {
                return NotFound();
            }

            var editorTarefas = await _context.Tarefas.FindAsync(id);

            if (editorTarefas == null)
            {
                return NotFound();
            }

            return editorTarefas;
        }
                
        [HttpPut("Atualiza_Task/{id}")]
        public async Task<IActionResult> PutEditorTarefas(int id, EditorTarefas editorTarefas)
        {
            if (id != editorTarefas.Id)
            {
                editorTarefas.Id = id;
                //return BadRequest("O ID fornecido no corpo não corresponde ao ID na URL.");
            }

            var existingTarefa = await _context.Tarefas.FindAsync(id);

            if (existingTarefa == null)
            {
                return NotFound("Tarefa não encontrada.");
            }

            if (editorTarefas.Completed_at != null)
            {
                editorTarefas.Completed_at = null;
            }

            if (editorTarefas.Title == "string")
            {
                editorTarefas.Title = existingTarefa.Title;
            }

            if (editorTarefas.Description == "string")
            {
                editorTarefas.Description = existingTarefa.Description;
            }

            editorTarefas.Created_at = existingTarefa.Created_at;

            editorTarefas.Updated_at = DateTime.Now;

            _context.Entry(existingTarefa).CurrentValues.SetValues(editorTarefas);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EditorTarefasExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/EditorTarefas
        [HttpPost("Cria_Task")]
        public async Task<ActionResult<EditorTarefas>> PostEditorTarefas(EditorTarefas editorTarefas)
        {
            if (_context.Tarefas == null)
            {
                return Problem("Entity set 'APIDbContext.Tarefas' is null.");
            }
            if (editorTarefas.Title == "string")
            {
                editorTarefas.Title = null;
            }

            if (editorTarefas.Description == "string")
            {
                editorTarefas.Description = null;
            }

            editorTarefas.Completed_at = null;
            editorTarefas.Created_at = DateTime.Now;
            editorTarefas.Updated_at = DateTime.Now;

            _context.Tarefas.Add(editorTarefas);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEditorTarefas", new { id = editorTarefas.Id }, editorTarefas);
        }

        [HttpPatch("Complete_Task/{id}")]
        public async Task<IActionResult> CompleteTarefa(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);

            if (tarefa == null)
            {
                return NotFound();
            }
            if (tarefa.Completed_at == null)
            {
                tarefa.Completed_at = DateTime.Now;
            }
            else{
                return BadRequest("Esta tarefa já foi concluída!");
            }
            

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/EditorTarefas/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteEditorTarefas(int id)
        {
            if (_context.Tarefas == null)
            {
                return NotFound();
            }

            var editorTarefas = await _context.Tarefas.FindAsync(id);
            if (editorTarefas == null)
            {
                return NotFound();
            }

            _context.Tarefas.Remove(editorTarefas);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EditorTarefasExists(int id)
        {
            return (_context.Tarefas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
