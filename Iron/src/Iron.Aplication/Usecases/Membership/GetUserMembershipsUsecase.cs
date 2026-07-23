using Iron.Aplication.DTOs.Membership.Response;
using Iron.Domain.Common;
using Iron.Domain.Repositories;

namespace Iron.Aplication.Usecases.Membership;

public class GetUserMembershipsUsecase(IMembershipRepository membershipRepository)
{
    private readonly IMembershipRepository _membershipRepository = membershipRepository;
    public async Task<Result<List<UserMembershipResponse>>> ExecuteAsync(long userId)
    {
        var memberships = await _membershipRepository.GetByUserId(userId);

        var dtos = memberships
            .Select(m => new UserMembershipResponse(m.TenantId, m.Gym.TradeName, m.Role.Name))
            .ToList();

        return Result.Ok(dtos);
    }
}
