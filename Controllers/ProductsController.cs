using GithubCliTest.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace GithubCliTest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private static readonly List<Product> Products = [];
    private static int _nextId = 1;

    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAll()
    {
        return Ok(Products);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Product> GetById(int id)
    {
        var product = Products.FirstOrDefault(p => p.Id == id);
        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpPost]
    public ActionResult<Product> Create(Product product)
    {
        var createdProduct = new Product
        {
            Id = _nextId++,
            Name = product.Name,
            Price = product.Price
        };

        Products.Add(createdProduct);
        return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, Product product)
    {
        var existing = Products.FirstOrDefault(p => p.Id == id);
        if (existing is null)
        {
            return NotFound();
        }

        existing.Name = product.Name;
        existing.Price = product.Price;
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var existing = Products.FirstOrDefault(p => p.Id == id);
        if (existing is null)
        {
            return NotFound();
        }

        Products.Remove(existing);
        return NoContent();
    }

    public static void ResetStoreForTests()
    {
        Products.Clear();
        _nextId = 1;
    }
}
