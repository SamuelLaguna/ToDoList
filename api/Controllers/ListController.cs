using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListController : ControllerBase
    {
        private readonly AppDbContext _context;
       public ListController(AppDbContext context)
       {
        _context = context;
       }

        [HttpGet]

        public async Task<IEnumerable<ToDoList>> getList()
        {
            var lists = await _context.ToDoLists.AsNoTracking().ToListAsync();
            return lists;
        }


        [HttpPost]
        
        public async Task<IActionResult> Create(ToDoList list)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.AddAsync(list);
            var result = await _context.SaveChangesAsync();

            if(result > 0)
            {
                return Ok();
            }
            return BadRequest();
        }


        // [HttpDelete("{id:int}")]
        // public async Task<IActionResult> Delete(int id)
        // {
        //    var list = await _context.ToDoLists.FindAsync(id);
        //     if(list == null)
        //     {
        //         return NotFound();
        //     }
        //     _context.Remove(list);

        //     var result = await _context.SaveChangesAsync();

        //     if(result > 0)
        //     {
        //         return Ok("List was Deleted");
        //     }

        //     return BadRequest("Unable to delte list");
        // }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var list = await _context.ToDoLists.FindAsync(id);
            if(list == null)
            {
                return NotFound();
            }

            _context.Remove(list);

            var result = await _context.SaveChangesAsync();

            if(result > 0)
            {
                return Ok("List was Deleted");
            }

            return BadRequest("Unable to delete list");
        }


        [HttpGet("{id:int}")]
     public async Task<ActionResult<ToDoList>> GetList(int id)
     {
        var list = await _context.ToDoLists.FindAsync(id);
        if(list == null)
        {
            return NotFound("Sorry student not found");
        }
        return Ok(list);
     }

     [HttpPut("{id:int}")]
     public async Task<IActionResult> EditList(int id, ToDoList ToDolist)
     {
        var listFromDb = await _context.ToDoLists.FindAsync(id);

        if(listFromDb == null)
        {
            return NotFound("Sorry student not found");
        }
       listFromDb.Tasks = ToDolist.Tasks;
       listFromDb.Description = ToDolist.Description;
       

       var result = await _context.SaveChangesAsync();

       if(result > 0)
       {
        return Ok("Student was edited");
       }
       return BadRequest("Unable to update data");
     }




    }
}