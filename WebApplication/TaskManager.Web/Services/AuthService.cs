using System.Net.Http;
using TaskManager.Web.Models.Auth;
using BuildingBlocks.Contracts.EmployeeContracts;

namespace TaskManager.Web.Services;

public class AuthService
{
    private readonly HttpClient _client;

    public AuthService(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<LoginResponse?> Login(LoginRequest request)
    {
        var response = await _client.PostAsJsonAsync("login", request);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<LoginResponse>();
    }

    public async Task<RegisterResponse?> Register(RegisterRequest request)
    {
        var response = await _client.PostAsJsonAsync("register", request);
        if (!response.IsSuccessStatusCode)
            return null;
        return await response.Content.ReadFromJsonAsync<RegisterResponse>();
    }
    public async Task<List<UserDto>> GetUsersAsync()
    {
        var response = await _client.GetAsync("users");
        if (!response.IsSuccessStatusCode) return new List<UserDto>();

        var result = await response.Content.ReadFromJsonAsync<GetUsersResult>();
        return result?.Users.ToList() ?? new List<UserDto>();
    }

    public async Task<bool> UpdateUserRoleAsync(UpdateUserRoleRequest request)
    {
        var response = await _client.PutAsJsonAsync("users/role", request);
        return response.IsSuccessStatusCode;
    }
    public async Task<bool> DeleteUserAsync(int userId)
    {
        var response = await _client.DeleteAsync($"users/{userId}");
        return response.IsSuccessStatusCode;
    }
}
