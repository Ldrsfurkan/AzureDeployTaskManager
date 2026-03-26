using Auth.API.Entities;
using Marten;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using BuildingBlocks.Contracts.EmployeeContracts;

namespace Auth.API.Features.Login;

public record LoginUserCommand(string Username, string Password);
public record LoginUserResult(string Token);

public class LoginUserHandler(IDocumentSession documentSession, IConfiguration configuration)
{
    public async Task<LoginUserResult?> HandleAsync(LoginUserCommand command)
    {
        var user = await documentSession.Query<User>()
            .FirstOrDefaultAsync(u => u.Username == command.Username);

        if (user is null)
            return null;

        var hasher = new PasswordHasher<User>();
        var verificationResult = hasher.VerifyHashedPassword(user, user.PasswordHash, command.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
            return null;

        // --- NEW: Fetch EmployeeId from Duty.API before generating the token ---
        int? employeeId = null;

        if (user.Role == "Employee" || user.Role == "Admin")
        {
            try
            {
                using var httpClient = new HttpClient();

                var dutyApiBaseUrl = configuration["ApiSettings:DutyApiBaseUrl"] ?? "http://localhost:5000";

                var requestUrl = $"{dutyApiBaseUrl.TrimEnd('/')}/employees/by-user/{user.Id}";

                // 3. Make the HTTP GET request
                var response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Case-insensitive deserialization ensures it maps properly even if API returns camelCase
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var result = await response.Content.ReadFromJsonAsync<GetEmployeeByUserIdResponse>(options);

                    employeeId = result?.Employee?.Id;
                }
            }
            catch (Exception ex)
            {
                // We log it, but do NOT stop the login process.
                Console.WriteLine($"Could not fetch EmployeeId from Duty.API: {ex.Message}");
            }
        }

        var token = GenerateJwtToken(user, employeeId);

        return new LoginUserResult(token);
    }

    private string GenerateJwtToken(User user, int? employeeId = null)
    {
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Role, user.Role)
    };
        if (employeeId.HasValue)
        {
            claims.Add(new Claim("EmployeeId", employeeId.Value.ToString()));
        }

        var tokenKey = configuration.GetValue<string>("Appsettings:Token") ?? throw new InvalidOperationException("Token key is missing");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: configuration.GetValue<string>("Appsettings:Issuer"),
            audience: configuration.GetValue<string>("Appsettings:Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddHours(10),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}
