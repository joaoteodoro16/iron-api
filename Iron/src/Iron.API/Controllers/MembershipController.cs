using Iron.Api.Common;
using Iron.Aplication.Usecases.Membership;
using Microsoft.AspNetCore.Mvc;

namespace Iron.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembershipController : ControllerBase
{
    private readonly GetUserMembershipsUsecase _getUserMembershipUsecase;

    public MembershipController(GetUserMembershipsUsecase getUserMembershipUsecase)
    {
        _getUserMembershipUsecase = getUserMembershipUsecase;
    }

    [HttpGet("me")]
    public async Task<IActionResult> ListMyMemberships()
    {
        var userId = User.GetRequiredUserId();

        var result = await _getUserMembershipUsecase.ExecuteAsync(userId);
        return result.ToActionResult();
    }
}
