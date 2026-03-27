using Auth.API.Entities;
using BuildingBlocks.CQRS; // ICommand/ICommandHandler interface’in
using Marten;

namespace Auth.API.Features.UpdateUser;

public record UpdateUserRoleCommand(int UserId, string Role) : ICommand<UpdateUserRoleResult>;
public record UpdateUserRoleResult(bool IsSuccess);

public class UpdateUserRoleHandler(IDocumentSession session, HttpClient httpClient, IConfiguration configuration)
{
    public async Task<UpdateUserRoleResult> HandleAsync(UpdateUserRoleCommand command)
    {
        var user = await session.LoadAsync<User>(command.UserId);

        if (user == null)
            return new UpdateUserRoleResult(false);

        user.Role = command.Role;

        session.Update(user);
        await session.SaveChangesAsync();

        if (command.Role == "Employee")
        {
            try
            {
                var dutyApiBaseUrl = configuration["ApiSettings:DutyApiBaseUrl"];
                var requestUrl = $"{dutyApiBaseUrl?.TrimEnd('/')}/employees";

                var response = await httpClient.PostAsJsonAsync(
                    requestUrl,
                    new
                    {
                        userId = user.Id,
                        name = user.Username
                    });

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Employee creation failed");
                }
            }
            catch (Exception ex)
            {
                // Updated to English log message
                Console.WriteLine($"Failed to create employee: {ex.Message}");
                return new UpdateUserRoleResult(false);
            }
        }

        return new UpdateUserRoleResult(true);
    }
}