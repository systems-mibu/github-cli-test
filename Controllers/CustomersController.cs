using Microsoft.AspNetCore.Mvc;

namespace GithubCliTest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private static readonly List<Customer> Customers = [];
    private static int _nextId = 1;

    [HttpGet]
    public ActionResult<IEnumerable<Customer>> GetAll()
    {
        return Ok(Customers);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Customer> GetById(int id)
    {
        var customer = Customers.FirstOrDefault(c => c.Id == id);
        return customer is null ? NotFound() : Ok(customer);
    }

    [HttpPost]
    public ActionResult<Customer> Create(Customer customer)
    {
        var created = new Customer
        {
            Id = _nextId++,
            Name = customer.Name
        };

        Customers.Add(created);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, Customer customer)
    {
        var existing = Customers.FirstOrDefault(c => c.Id == id);
        if (existing is null)
        {
            return NotFound();
        }

        existing.Name = customer.Name;
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var existing = Customers.FirstOrDefault(c => c.Id == id);
        if (existing is null)
        {
            return NotFound();
        }

        Customers.Remove(existing);
        return NoContent();
    }

    public static void ResetStorageForTests()
    {
        Customers.Clear();
        _nextId = 1;
    }
}
