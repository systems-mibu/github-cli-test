using GithubCliTest.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace GithubCliTest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private static readonly List<Order> Orders = [];
    private static int _nextId = 1;
    private static readonly object Sync = new();

    [HttpGet]
    public ActionResult<IEnumerable<Order>> GetAll()
    {
        lock (Sync)
        {
            return Ok(Orders.Select(o => Copy(o)).ToList());
        }
    }

    [HttpGet("{id:int}")]
    public ActionResult<Order> GetById(int id)
    {
        lock (Sync)
        {
            var order = Orders.FirstOrDefault(o => o.Id == id);
            return order is null ? NotFound() : Ok(Copy(order));
        }
    }

    [HttpPost]
    public ActionResult<Order> Create(Order order)
    {
        lock (Sync)
        {
            var created = new Order
            {
                Id = _nextId++,
                CustomerName = order.CustomerName,
                Total = order.Total
            };
            Orders.Add(created);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, Copy(created));
        }
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, Order order)
    {
        lock (Sync)
        {
            var existing = Orders.FirstOrDefault(o => o.Id == id);
            if (existing is null)
            {
                return NotFound();
            }

            existing.CustomerName = order.CustomerName;
            existing.Total = order.Total;
            return NoContent();
        }
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        lock (Sync)
        {
            var existing = Orders.FirstOrDefault(o => o.Id == id);
            if (existing is null)
            {
                return NotFound();
            }

            Orders.Remove(existing);
            return NoContent();
        }
    }

    public static void ResetStorageForTests()
    {
        lock (Sync)
        {
            Orders.Clear();
            _nextId = 1;
        }
    }

    private static Order Copy(Order order) =>
        new()
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            Total = order.Total
        };
}
