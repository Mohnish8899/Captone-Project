using APPUI.Data;
using APPUI.Entities;
using APPUI.Extensions;
using APPUI.RequestHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace APPUI.Controllers
{
    
    public class MedicinesController : BaseApiController
    {
        private readonly StoreContext _context;

        public MedicinesController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<Product>>> GetMedicines([FromQuery]ProductParams productParams)
        {
           var query =_context.Medicines
                .Sort(productParams.OrderBy)
                .Search(productParams.SearchTerm)
                .Filter(productParams.Brands, productParams.Categories)
                .AsQueryable();

            var medicines = await PagedList<Product>.ToPagedList(query,
                productParams.PageNumber, productParams.PageSize);
            Response.AddPaginationHeader(medicines.MetaData);

            return medicines;
        }

        [HttpGet("{id}")]
        public  async Task<ActionResult<Product>> GetMedicine(int id)
        {
            return await _context.Medicines.FindAsync(id);
        }

        [HttpGet("filters")]
        public async Task<IActionResult> GetFilters()
        {
            var brands = await _context.Medicines.Select(p => p.Brand).Distinct().ToListAsync();
            var categories = await _context.Medicines.Select(p => p.Category).Distinct().ToListAsync();

            return Ok(new { brands, categories});
        }
    }
}
