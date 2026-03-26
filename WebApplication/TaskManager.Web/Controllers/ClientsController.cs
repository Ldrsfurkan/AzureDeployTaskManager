using BuildingBlocks.Contracts.ClientContracts;
using BuildingBlocks.Contracts.DutyContracts;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Web.Models;
using TaskManager.Web.Services;

public class ClientsController : Controller
{
    private readonly ClientsService _clientsService;

    public ClientsController(ClientsService clientsService)
    {
        _clientsService = clientsService;
    }
    public async Task<IActionResult> Index(int? clientId)
    {
        var clients = await _clientsService.GetClients();

        List<DutyDto> duties = new();

        if (clientId.HasValue)
        {
            duties = await _clientsService.GetDutiesByClientId(clientId.Value);
        }

        var model = new ClientsViewModel
        {
            Clients = clients,
            Duties = duties,
            SelectedClientId = clientId
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddClientRequest request)
    {
        await _clientsService.AddClient(request);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        await _clientsService.DeleteClient(id);
        return RedirectToAction(nameof(Index));
    }
}