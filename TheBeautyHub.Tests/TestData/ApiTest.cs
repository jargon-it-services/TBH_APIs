using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using TheBeautyHubAPI;
using TheBeautyHubAPI.Auth;
using TheBeautyHubAPI.Helpers;
using TheBeautyHubAPI.Models;
using TheBeautyHubCore.Services.Interfaces;

namespace TheBeautyHub.Tests.TestData;

public static class ApiTest
{
    public static IMapper Mapper()
        => new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

    public static ICurrentUser User()
    {
        var mock = new Mock<ICurrentUser>();
        mock.SetupGet(u => u.IsAuthenticated).Returns(true);
        mock.SetupGet(u => u.AccountId).Returns(TestIds.Account);
        mock.SetupGet(u => u.UserId).Returns(TestIds.User);
        mock.SetupGet(u => u.Email).Returns("owner@example.com");
        mock.SetupGet(u => u.Roles).Returns(new[] { "Owner" });
        mock.SetupGet(u => u.Permissions).Returns(Array.Empty<string>());
        return mock.Object;
    }

    public static IExceptionLogService Logs()
    {
        var mock = new Mock<IExceptionLogService>();
        mock.Setup(l => l.LogExceptionAsync(It.IsAny<Exception>(), It.IsAny<Guid?>(), It.IsAny<string?>()))
            .ReturnsAsync(1L);
        return mock.Object;
    }

    public static IWebHostEnvironment Environment()
    {
        var mock = new Mock<IWebHostEnvironment>();
        var root = Path.Combine(Path.GetTempPath(), "tbh-tests");
        mock.SetupGet(e => e.ContentRootPath).Returns(root);
        mock.SetupGet(e => e.WebRootPath).Returns(Path.Combine(root, "wwwroot"));
        return mock.Object;
    }

    public static BranchLogoStorage BranchLogos() => new(Environment());
    public static ServicePhotoStorage ServicePhotos() => new(Environment());
    public static StaffFileStorage StaffFiles() => new(Environment());

    public static T Controller<T>(T controller) where T : ControllerBase
    {
        controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
        controller.MetadataProvider = new EmptyModelMetadataProvider();
        controller.ObjectValidator = new NoOpObjectValidator();
        return controller;
    }

    private sealed class NoOpObjectValidator : IObjectModelValidator
    {
        public void Validate(ActionContext actionContext, ValidationStateDictionary? validationState, string prefix, object? model)
        {
        }
    }

    public static void AssertOk<T>(IActionResult result, string message, Action<T>? assertData = null)
    {
        var objectResult = Assert.IsAssignableFrom<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode ?? StatusCodes.Status200OK);
        var body = Assert.IsType<ApiStatusResponse<T>>(objectResult.Value);
        Assert.True(body.Status);
        Assert.Equal(message, body.Message);
        Assert.NotNull(body.Data);
        assertData?.Invoke(body.Data!);
    }

    public static void AssertFail(IActionResult result, int statusCode, string message)
    {
        var objectResult = Assert.IsAssignableFrom<ObjectResult>(result);
        Assert.Equal(statusCode, objectResult.StatusCode);
        var body = Assert.IsType<ApiStatusResponse<object>>(objectResult.Value);
        Assert.False(body.Status);
        Assert.Equal(message, body.Message);
    }

    public static void AssertNotFound(IActionResult result, string message)
        => AssertFail(result, StatusCodes.Status404NotFound, message);

    public static void AssertBadRequest(IActionResult result, string message)
    {
        var objectResult = Assert.IsType<BadRequestObjectResult>(result);
        var body = Assert.IsType<ApiStatusResponse<object>>(objectResult.Value);
        Assert.False(body.Status);
        Assert.Equal(message, body.Message);
    }

    public static void AssertConflict(IActionResult result, string message)
    {
        var objectResult = Assert.IsType<ConflictObjectResult>(result);
        var body = Assert.IsType<ApiStatusResponse<object>>(objectResult.Value);
        Assert.False(body.Status);
        Assert.Equal(message, body.Message);
    }
}
