using CatalogService.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        DatabaseContext _db;
        public CatalogController(DatabaseContext db)
        {
            _db = db;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Returing the list of Products", OperationId = "GetProducts")]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _db.Products.ToListAsync();
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Returing a Product based upon {id}", OperationId = "GetProduct")]
        public async Task<Product> GetProduct(int id)
        {
            return await _db.Products.FindAsync(id);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Add new Product", OperationId = "AddProduct")]
        public async Task<IActionResult> AddProduct(Product model)
        {
            try
            {
                _db.Products.Add(model);
                await _db.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Update Product By {id}", OperationId = "UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(int id, Product model)
        {
            try
            {
                if (id != model.ProductId)
                    return BadRequest();

                _db.Products.Update(model);
                await _db.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Delete Product By {id}", OperationId = "DeleteProduct")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            Product product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK);
            }
            else
                return StatusCode(StatusCodes.Status404NotFound);
        }
    }
}
