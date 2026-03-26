using BuildingBlocks.Contracts.ClientContracts;
using BuildingBlocks.Contracts.DutyContracts;
using System.Net.Http.Json;

namespace TaskManager.Web.Services;

public class ClientsService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClientsService(HttpClient httpClient , IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<ClientDto>> GetClients()
    {
        var response = await _httpClient.GetAsync("clients");

        if(!response.IsSuccessStatusCode)
        {
            return new List<ClientDto>();
        }
        
        var result = await response.Content.ReadFromJsonAsync<GetClientsResponse>();
        return result?.Clients.ToList() ?? new List<ClientDto>();
    }

    public async Task AddClient(AddClientRequest request)
    {
        // 1. Token'ı çerezden al ve HttpClient'a ekle (API yetki istiyorsa bu ŞARTTIR)
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["JwtToken"];
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        // 2. İsteği gönder
        var response = await _httpClient.PostAsJsonAsync("clients", request);

        // 3. Eğer API reddederse (hata verirse) konsola yazdır ki ne olduğunu bilelim
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Failed to add client. Status: {response.StatusCode}, Error: {errorMessage}");
        }
    }

    public async Task DeleteClient(int id)
    {
        await _httpClient.DeleteAsync($"clients/{id}");
    }
    public async Task<List<DutyDto>> GetDutiesByClientId(int clientId)
    {
        var result = await _httpClient.GetFromJsonAsync<GetDutiesByClientResponse>($"duties/client/{clientId}");
        return result?.Duties.ToList() ?? new List<DutyDto>();
    }
}