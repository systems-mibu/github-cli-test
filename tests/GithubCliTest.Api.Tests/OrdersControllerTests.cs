using GithubCliTest.Api.Controllers;
using GithubCliTest.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace GithubCliTest.Api.Tests;

public class OrdersControllerTests
{
    public OrdersControllerTests()
    {
        OrdersController.ResetStorageForTests();
    }

    [Fact]
    public void GetAll_ReturnsOkWithOrders()
    {
        var controller = new OrdersController();
        controller.Create(new Order { CustomerName = "Alice", Total = 25m });

        var result = controller.GetAll().Result as OkObjectResult;

        Assert.NotNull(result);
        var orders = Assert.IsAssignableFrom<IEnumerable<Order>>(result!.Value);
        Assert.Single(orders);
    }

    [Fact]
    public void GetById_ReturnsOrder_WhenFound()
    {
        var controller = new OrdersController();
        var created = (controller.Create(new Order { CustomerName = "Bob", Total = 10m }).Result as CreatedAtActionResult)!;
        var createdOrder = Assert.IsType<Order>(created.Value);

        var result = controller.GetById(createdOrder.Id).Result as OkObjectResult;

        Assert.NotNull(result);
        var order = Assert.IsType<Order>(result!.Value);
        Assert.Equal("Bob", order.CustomerName);
    }

    [Fact]
    public void GetById_ReturnsNotFound_WhenMissing()
    {
        var controller = new OrdersController();

        var result = controller.GetById(404).Result;

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Create_ReturnsCreatedOrder()
    {
        var controller = new OrdersController();

        var result = controller.Create(new Order { CustomerName = "Cara", Total = 15m }).Result as CreatedAtActionResult;

        Assert.NotNull(result);
        var createdOrder = Assert.IsType<Order>(result!.Value);
        Assert.Equal(1, createdOrder.Id);
        Assert.Equal("Cara", createdOrder.CustomerName);
    }

    [Fact]
    public void Update_ReturnsNoContent_WhenFound()
    {
        var controller = new OrdersController();
        var created = (controller.Create(new Order { CustomerName = "Dan", Total = 20m }).Result as CreatedAtActionResult)!;
        var order = Assert.IsType<Order>(created.Value);

        var result = controller.Update(order.Id, new Order { CustomerName = "Dan Updated", Total = 21m });

        Assert.IsType<NoContentResult>(result);
        var updated = (controller.GetById(order.Id).Result as OkObjectResult)!;
        var updatedOrder = Assert.IsType<Order>(updated.Value);
        Assert.Equal("Dan Updated", updatedOrder.CustomerName);
    }

    [Fact]
    public void Update_ReturnsNotFound_WhenMissing()
    {
        var controller = new OrdersController();

        var result = controller.Update(404, new Order { CustomerName = "Missing", Total = 1m });

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Delete_ReturnsNoContent_WhenFound()
    {
        var controller = new OrdersController();
        var created = (controller.Create(new Order { CustomerName = "Eli", Total = 30m }).Result as CreatedAtActionResult)!;
        var order = Assert.IsType<Order>(created.Value);

        var result = controller.Delete(order.Id);

        Assert.IsType<NoContentResult>(result);
        Assert.IsType<NotFoundResult>(controller.GetById(order.Id).Result);
    }

    [Fact]
    public void Delete_ReturnsNotFound_WhenMissing()
    {
        var controller = new OrdersController();

        var result = controller.Delete(404);

        Assert.IsType<NotFoundResult>(result);
    }
}
