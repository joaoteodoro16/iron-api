using Iron.Api.Common;
using Iron.Aplication.DTOs;
using Iron.Aplication.DTOs.Auth.Request;
using Iron.Aplication.Usecases.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iron.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(CreateUserUsecase createUserUsecase, AuthUserUsecase authUserUsecase, RefreshTokenUsecase refreshTokenUsecase) : ControllerBase
{
    private readonly CreateUserUsecase _createUserUsecase = createUserUsecase;
    private readonly AuthUserUsecase _authUserUsecase = authUserUsecase;

    private readonly RefreshTokenUsecase _refreshTokenUsecase = refreshTokenUsecase;

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
    {
        var result = await _createUserUsecase.ExecuteAsync(request);

        return result.ToActionResult();
    }

    [AllowAnonymous]
    [HttpPost("auth")]
    public async Task<IActionResult> Auth([FromBody] AuthUserRequest request)
    {
        var result = await _authUserUsecase.ExecuteAsync(request);
        return result.ToActionResult();
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await _refreshTokenUsecase.ExecuteAsync(request.RefreshToken);
        return result.ToActionResult();
    }
}
