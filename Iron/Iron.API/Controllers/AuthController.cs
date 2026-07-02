using Iron.Api.Common;
using Iron.Aplication.DTOs;
using Iron.Aplication.Usecases.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iron.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController(CreateUserUsecase createUserUsecase) : ControllerBase
{
    private readonly CreateUserUsecase _createUserUsecase = createUserUsecase;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
    {
        var result = await _createUserUsecase.ExecuteAsync(request);

        if (result.IsFailure)
            return BadRequest(result.ToApiResponse());

        return Ok(result.ToApiResponse("Usuário criado com sucesso."));
    }
}
