using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBeautyHubAPI.Auth;
using TheBeautyHubAPI.Controllers;
using TheBeautyHubAPI.Models;
using TheBeautyHubCore.Constants;

namespace TheBeautyHub.Tests.Controllers;

public class TokenControllerTests
{
    [Fact]
    public async Task Validate_returns_valid_payload()
    {
        var tokens = new Mock<IAccessTokenService>();
        tokens.Setup(t => t.ReadTokenFromRequest(It.IsAny<HttpRequest>())).Returns("tok");
        tokens.Setup(t => t.ValidateAsync("tok", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AccessTokenValidationResult
            {
                IsValid = true,
                UserId = TestIds.User,
                AccountId = TestIds.Account,
                Email = "a@b.com",
                Roles = new List<string> { "Owner" }
            });

        var controller = new TokenController(tokens.Object)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
        };

        var result = Assert.IsType<OkObjectResult>(await controller.Validate());
        var body = Assert.IsType<ApiStatusResponse<TokenValidateDataResponse>>(result.Value);
        Assert.True(body.Status);
        Assert.True(body.Data!.IsValid);
        Assert.Equal(TestIds.Account, body.Data.AccountId);
        Assert.Equal(ApiMessages.AuthTokenValid, body.Message);
    }

    [Fact]
    public async Task Validate_returns_invalid_when_token_fails()
    {
        var tokens = new Mock<IAccessTokenService>();
        tokens.Setup(t => t.ReadTokenFromRequest(It.IsAny<HttpRequest>())).Returns((string?)null);
        tokens.Setup(t => t.ValidateAsync(null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AccessTokenValidationResult { IsValid = false, Message = "gone" });

        var controller = new TokenController(tokens.Object)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
        };

        var result = Assert.IsType<OkObjectResult>(await controller.Validate());
        var body = Assert.IsType<ApiStatusResponse<TokenValidateDataResponse>>(result.Value);
        Assert.False(body.Status);
        Assert.Equal("gone", body.Message);
    }
}
