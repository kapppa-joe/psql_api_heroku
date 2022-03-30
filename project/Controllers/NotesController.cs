#nullable disable
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.Data;
using project.Models;

namespace project.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotesController : ControllerBase
{
    private readonly PsqlDbContext _context;

    public NotesController(PsqlDbContext context)
    {
        _context = context;
        
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
    {
        var notes = await _context.Notes.ToListAsync();
        return Ok(notes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Note>> GetNote(int id)
    {
        var note = await _context.Notes.FindAsync(id);
        if (note == null)
        {
            return NotFound();
        }

        return Ok(note);
    }

    [HttpPost]
    public async Task<ActionResult<Note>> PostNote(Note note)
    {
        _context.Notes.Add(note);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetNote", new {id = note.id}, note);
    }

    // [HttpDelete("{id}")]
    // public async Task<IActionResult> DeleteNote(int id)
    // {
    //     var note = await _context.Notes.FindAsync(id);
    //     if (note == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     _context.Notes.Remove(note);
    //     await _context.SaveChangesAsync();
    //
    //     return NoContent();
    // }
    //
    // [httpPut("{id}")]
    // public async Task<IActionResult PutNote(int id, Note note)
    // {
    //     if (id != note.id)
    //     {
    //         return BadRequest();
    //     }
    //
    //     _context.Entry(note).State = EntityState.Modified;
    //
    //     try
    //     {
    //         await _context.SaveChangesAsync();
    //     }
    //     catch (DbUpdateConcurrencyException)
    //     {
    //         if (!NoteExists(id))
    //         {
    //             return NotFound();
    //         }
    //         else
    //         {
    //             throw;
    //         }
    //     }
    //
    //     return NoContent();
    // }
    //
    // private bool NoteExists(int id)
    // {
    //     return _context.Notes.Any(e => e.id == id);
    // }
    
}