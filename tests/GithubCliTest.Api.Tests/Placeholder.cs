using GithubCliTest.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Xunit;

namespace GithubCliTest.Api.Tests;

public class CustomersControllerTests
{
    public CustomersControllerTests()
    {
        CustomersController.ResetStorageForTests();
    }

    [Fact]
    public void GetAll_ReturnsOkWithCustomers()
    {
        var controller = new CustomersController();
        controller.Create(new Customer { Name = "Alice" });

        var result = controller.GetAll().Result as OkObjectResult;

        Assert.NotNull(result);
        var customers = Assert.IsAssignableFrom<IEnumerable<Customer>>(result.Value);
        Assert.Single(customers);
    }

    [Fact]
    public void GetById_ReturnsNotFound_WhenMissing()
    {
        var controller = new CustomersController();

        var result = controller.GetById(999).Result;

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Create_ReturnsCreatedCustomer()
    {
        var controller = new CustomersController();

        var result = controller.Create(new Customer { Name = "Bob" }).Result as CreatedAtActionResult;

        Assert.NotNull(result);
        var created = Assert.IsType<Customer>(result.Value);
        Assert.Equal(1, created.Id);
        Assert.Equal("Bob", created.Name);
    }

    [Fact]
    public void Update_ReturnsNoContent_WhenCustomerExists()
    {
        var controller = new CustomersController();
        controller.Create(new Customer { Name = "Carol" });

        var result = controller.Update(1, new Customer { Name = "Caroline" });
        var updated = (controller.GetById(1).Result as OkObjectResult)?.Value as Customer;

        Assert.IsType<NoContentResult>(result);
        Assert.NotNull(updated);
        Assert.Equal("Caroline", updated.Name);
    }

    [Fact]
    public void Update_ReturnsNotFound_WhenMissing()
    {
        var controller = new CustomersController();

        var result = controller.Update(123, new Customer { Name = "Nobody" });

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Delete_ReturnsNoContent_WhenCustomerExists()
    {
        var controller = new CustomersController();
        controller.Create(new Customer { Name = "Dave" });

        var result = controller.Delete(1);
        var getAfterDelete = controller.GetById(1).Result;

        Assert.IsType<NoContentResult>(result);
        Assert.IsType<NotFoundResult>(getAfterDelete);
    }

    [Fact]
    public void Delete_ReturnsNotFound_WhenMissing()
    {
        var controller = new CustomersController();

        var result = controller.Delete(123);

        Assert.IsType<NotFoundResult>(result);
    }
}
