using Auth.API.Entities;
using BuildingBlocks.CQRS; // ICommand/ICommandHandler interface’in
using Marten;

namespace Auth.API.Features.UpdateUser;

public record UpdateUserRoleCommand(int UserId, string Role) : ICommand<UpdateUserRoleResult>;
public record UpdateUserRoleResult(bool IsSuccess);

public class UpdateUserRoleHandler
{
    private readonly IDocumentSession _session;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public UpdateUserRoleHandler(IDocumentSession session, HttpClient httpClient, IConfiguration configuration)
    {
        _session = session;
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<UpdateUserRoleResult> HandleAsync(UpdateUserRoleCommand command)
    {
        var user = await _session.LoadAsync<User>(command.UserId);

        if (user == null)
            return new UpdateUserRoleResult(false);

        user.Role = command.Role;

        _session.Update(user);
        await _session.SaveChangesAsync();

        if (command.Role.Equals("Employee", StringComparison.OrdinalIgnoreCase))
        {
            try
            {
                var dutyApiBaseUrl = _configuration["ApiSettings:DutyApiBaseUrl"];
                if (string.IsNullOrEmpty(dutyApiBaseUrl))
                    throw new Exception("DutyApiBaseUrl config bulunamadı");

                var requestUrl = dutyApiBaseUrl.TrimEnd('/') + "/employees";

                var response = await _httpClient.PostAsJsonAsync(requestUrl, new
                {
                    userId = user.Id,
                    name = user.Username
                });

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Employee creation failed: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Employee oluşturulamadı: {ex.Message}");
                return new UpdateUserRoleResult(false);
            }
        }

        return new UpdateUserRoleResult(true);
    }
}