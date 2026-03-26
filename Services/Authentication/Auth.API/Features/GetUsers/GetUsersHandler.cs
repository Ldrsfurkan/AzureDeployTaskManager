using Auth.API.Entities;
using Marten;
using BuildingBlocks.Contracts.EmployeeContracts;

namespace Auth.API.Features.GetUsers;

public class GetUsersHandler(IDocumentSession session)
{
    public async Task<GetUsersResult> HandleAsync()
    {
        var users = await session.Query<User>().ToListAsync();

        var userDtos = users.Select(u => new UserDto(u.Id, u.Username, u.Role ?? "None")).ToList();
        return new GetUsersResult(userDtos);
    }
}