using GithubCliTest.Api.Controllers;
using GithubCliTest.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace GithubCliTest.Api.Tests;

public class ProductsControllerTests
{
    public ProductsControllerTests()
    {
        ProductsController.ResetStoreForTests();
    }

    [Fact]
    public void GetAll_ReturnsEmptyList_Initially()
    {
        var controller = new ProductsController();

        var result = controller.GetAll().Result as OkObjectResult;

        var products = Assert.IsAssignableFrom<IEnumerable<Product>>(result!.Value);
        Assert.Empty(products);
    }

    [Fact]
    public void GetById_ReturnsNotFound_WhenMissing()
    {
        var controller = new ProductsController();

        var result = controller.GetById(999).Result;

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Create_AssignsId_AndReturnsCreated()
    {
        var controller = new ProductsController();

        var result = controller.Create(new Product { Name = "Keyboard", Price = 49.99m }).Result as CreatedAtActionResult;

        Assert.NotNull(result);
        var created = Assert.IsType<Product>(result.Value);
        Assert.Equal(1, created.Id);
        Assert.Equal("Keyboard", created.Name);
        Assert.Equal(49.99m, created.Price);
    }

    [Fact]
    public void Update_ReturnsNoContent_WhenProductExists()
    {
        var controller = new ProductsController();
        var created = (controller.Create(new Product { Name = "Mouse", Price = 19.99m }).Result as CreatedAtActionResult)!.Value as Product;

        var updateResult = controller.Update(created!.Id, new Product { Name = "Gaming Mouse", Price = 29.99m });
        var getResult = controller.GetById(created.Id).Result as OkObjectResult;

        Assert.IsType<NoContentResult>(updateResult);
        var updated = Assert.IsType<Product>(getResult!.Value);
        Assert.Equal("Gaming Mouse", updated.Name);
        Assert.Equal(29.99m, updated.Price);
    }

    [Fact]
    public void Update_ReturnsNotFound_WhenMissing()
    {
        var controller = new ProductsController();

        var result = controller.Update(999, new Product { Name = "Missing", Price = 1m });

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Delete_RemovesProduct_WhenExists()
    {
        var controller = new ProductsController();
        var created = (controller.Create(new Product { Name = "Monitor", Price = 199.99m }).Result as CreatedAtActionResult)!.Value as Product;

        var deleteResult = controller.Delete(created!.Id);
        var getResult = controller.GetById(created.Id).Result;

        Assert.IsType<NoContentResult>(deleteResult);
        Assert.IsType<NotFoundResult>(getResult);
    }

    [Fact]
    public void Delete_ReturnsNotFound_WhenMissing()
    {
        var controller = new ProductsController();

        var result = controller.Delete(999);

        Assert.IsType<NotFoundResult>(result);
    }
}
