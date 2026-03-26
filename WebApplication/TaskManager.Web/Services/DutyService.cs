using BuildingBlocks.Contracts.AuditContracts;
using BuildingBlocks.Contracts.ClientContracts;
using BuildingBlocks.Contracts.DutyContracts;
using BuildingBlocks.Contracts.EmployeeContracts;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace TaskManager.Web.Services;

public class DutyService
{
    private readonly HttpClient _client;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DutyService(HttpClient client, IHttpContextAccessor httpContextAccessor)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public async Task<GetDutiesResponse?> GetDuties(GetDutiesRequest request)
    {
        var queryString = new Dictionary<string, string?>();

        if (request.PageNumber.HasValue)
            queryString.Add("PageNumber", request.PageNumber.Value.ToString());

        if (request.PageSize.HasValue)
            queryString.Add("PageSize", request.PageSize.Value.ToString());

        var url = QueryHelpers.AddQueryString("duties", queryString);

        var response = await _client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<GetDutiesResponse>(new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
    public async Task<int?> CreateDutyAsync(CreateDutyRequest request)
    {
        // Post the request object as JSON to the "/duties" endpoint
        var response = await _client.PostAsJsonAsync("duties", request);

        if (!response.IsSuccessStatusCode)
            return null; // Or throw a custom exception / return an error model

        // Deserialize the response to get the newly created Id
        var result = await response.Content.ReadFromJsonAsync<CreateDutyResponse>(new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return result?.Id;
    }
    public async Task<bool> DeleteDutyAsync(int id)
    {
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["JwtToken"];

        if (!string.IsNullOrEmpty(token))
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await _client.DeleteAsync($"duties/{id}");

        return response.IsSuccessStatusCode;
    }
    public async Task<DutyDto?> GetDutyByIdAsync(int id)
    {
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["JwtToken"];
        if (!string.IsNullOrEmpty(token))
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await _client.GetAsync($"duties/{id}");

        if (!response.IsSuccessStatusCode)
            return null;

        // We deserialize into the wrapper response because the API returns { "duty": { ... } }
        var result = await response.Content.ReadFromJsonAsync<GetDutyByIdResponse>(new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return result?.Duty;
    }

    public async Task<bool> UpdateDutyAsync(UpdateDutyRequest request)
    {
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["JwtToken"];
        if (!string.IsNullOrEmpty(token))
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Sends the PUT request with the updated data
        var response = await _client.PutAsJsonAsync("duties", request);

        return response.IsSuccessStatusCode;
    }
    public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync()
    {
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["JwtToken"];
        if (!string.IsNullOrEmpty(token))
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await _client.GetAsync("employees");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<GetEmployeesResponse>();
            return result?.Employees ?? new List<EmployeeDto>();
        }
        return new List<EmployeeDto>();
    }

    public async Task<IEnumerable<ClientDto>> GetClientsAsync()
    {
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["JwtToken"];
        if (!string.IsNullOrEmpty(token))
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await _client.GetAsync("clients");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<GetClientsResponse>();
            return result?.Clients ?? new List<ClientDto>();
        }
        return new List<ClientDto>();
    }
    public async Task<IEnumerable<DutyDto>> GetDutiesByEmployeeIdAsync(int employeeId)
    {
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["JwtToken"];
        if (!string.IsNullOrEmpty(token))
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await _client.GetAsync($"duties/employee/{employeeId}");

        if (response.IsSuccessStatusCode)
        {
            // Adjust this if your API response record name is different
            var result = await response.Content.ReadFromJsonAsync<GetDutiesResponse>();
            return result?.Duties ?? new List<DutyDto>();
        }

        return new List<DutyDto>();
    }
    public async Task<bool> CreateEmployeeAsync(CreateEmployeeRequest request)
    {
        // (If your Duty.API requires token, add it here like we did for Clients)
        var response = await _client.PostAsJsonAsync("employees", request);
        return response.IsSuccessStatusCode;
    }
    public async Task<IEnumerable<AuditLogDto>> GetRecentAuditLogsAsync()
    {
        // Make sure your API endpoint is correct
        var response = await _client.GetAsync("audit-logs");

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<GetAuditLogsResponse>();
            return result?.Logs ?? new List<AuditLogDto>();
        }

        return new List<AuditLogDto>();
    }
}

