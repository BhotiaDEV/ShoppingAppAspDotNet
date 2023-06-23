using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Users.Data;
using Users.Models;

namespace Users.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly UserDbcontext context;

        public ProductController(UserDbcontext _context)
        {
            this.context = _context;
        }

        [Route("getproducts")]
        [HttpGet]
        public IActionResult GetProducts([FromQuery] int limit = 50, [FromQuery] int skip = 0 )
        {
                var products = context.Products.ToList().Skip(skip).Take(limit);
                var response = new { products = products };
                return Ok(response);   
        }

        [Route("getproduct/{id:guid}")]
        [HttpGet]
        public IActionResult GetProduct(Guid id)
        {
            //if(User.Identity.IsAuthenticated)
            //{
                var product = context.Products.FirstOrDefault(p => p.Id == id);
                if(product == null) 
                {
                    return NotFound();
                }
                return Ok(product);
            //}
            //return BadRequest("User is not Authenticated!");
        }

       // [Authorize(Roles ="Admin")]
        [Route("addproduct")]
        [HttpPost]
        public IActionResult AddProducts(AddProduct request)
        {
            if (ModelState.IsValid)
            {
                var existing_product = context.Products.FirstOrDefault(p => p.title == request.title);

                if(existing_product != null)
                {
                    return BadRequest("Product already exists");
                }
                var Product = new Product()
                {
                    Id = Guid.NewGuid(),
                    title = request.title,
                    description = request.description,
                    price = request.price,
                    discountPercentage = request.discountPercentage,
                    rating = request.rating,
                    stock = request.stock,
                    brand = request.brand,
                    category = request.category,
                    thumbnail = request.thumbnail,
                    images = request.images,
                };

                context.Products.Add(Product);
                context.SaveChanges();
                return Ok(Product);
            }

            else { return BadRequest("Bad Request"); }
        }

        [Authorize(Roles = "Admin")]
        [Route("updateproduct/{id:guid}")]
        [HttpPut]
        public IActionResult UpdateProductdetails([FromRoute] Guid id, UpdateProduct request)
        {
            var existing_product = context.Products.FirstOrDefault(p => p.Id == id);

            if (existing_product == null) 
            {
                return NotFound("Product does not exist!");
            }

            context.Entry(existing_product).CurrentValues.SetValues(request);
            context.SaveChanges();
            return Ok(existing_product);    
             
        }

        [Route("deleteproduct/{id:guid}")]
        [HttpDelete]
        public IActionResult DeleteProduct(Guid id)
        {
            var product = context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return BadRequest("Product not found!");
            }
            context.Products.Remove(product);
            context.SaveChanges();
            return Ok("Product Deleted.");
        }

        [Route("search")]
        [HttpGet]
        public IActionResult Search([FromQuery] string q , [FromQuery] int limit = 5)
        {
            var results = context.Products.Where(p => p.title.Contains(q) || p.category.Contains(q)).ToList().Take(limit);
            var response = new { products = results };
            return Ok(response);
        }

        [Route("category")]
        [HttpPost]
        public IActionResult category(AddCategory request)
        {
           if(ModelState.IsValid)
            {
                var Category = new Category()
                {
                    Id = Guid.NewGuid(),
                    category = request.category,
                };
                context.Categories.Add(Category);
                context.SaveChanges();
                return Ok(Category);
            }
            return BadRequest("Bad request");
        }

        [Route("categories")]
        [HttpGet]
        public IActionResult getCategories()
        {
            var categories = context.Categories.Select(c => c.category).ToList();
            return Ok(categories);
        }

        [Route("category/{search}")]
        [HttpGet]
        public IActionResult category([FromRoute] string search) 
        {
            var products = context.Products.Where(p => 
            p.category.Contains(search) || p.title.Contains(search))
                .ToList().Take(5);
            var response = new { products = products };
            return Ok(response);

        }
    }
}