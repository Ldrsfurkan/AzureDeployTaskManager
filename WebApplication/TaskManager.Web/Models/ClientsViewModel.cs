using BuildingBlocks.Contracts.DutyContracts;
using BuildingBlocks.Contracts.ClientContracts;

namespace TaskManager.Web.Models;

public class ClientsViewModel
{
    public List<ClientDto> Clients { get; set; } = new();

    public List<DutyDto> Duties { get; set; } = new();

    public int? SelectedClientId { get; set; }
}
