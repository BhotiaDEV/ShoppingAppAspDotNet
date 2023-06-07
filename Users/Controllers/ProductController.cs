using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetProduct()
        {

            if (User.Identity.IsAuthenticated)
            {
                return Ok(context.Products.ToList());
            }
            else
            {
                return BadRequest("User is unauthorized");
            }
        }

        [Route("addproduct")]
        [HttpPost]
        public IActionResult AddProducts(AddProduct request)
        {
            if (ModelState.IsValid)
            {
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

        [Route("updateproduct/{id:guid}")]
        [HttpPut]
        public IActionResult UpdateProductdetails([FromRoute] Guid id , UpdateProduct request)
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
    }
}
