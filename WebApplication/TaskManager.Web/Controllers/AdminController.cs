using BuildingBlocks.Contracts.ClientContracts;
using BuildingBlocks.Contracts.EmployeeContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Web.Services;

namespace TaskManager.Web.Controllers;

[Authorize(Roles = "Admin")]
[Route("admin")]
public class AdminController : Controller
{
    private readonly DutyService _dutyService;
    private readonly ClientsService _clientsService;
    private readonly AuthService _authService; 

    public AdminController(DutyService dutyService, ClientsService clientsService, AuthService authService)
    {
        _dutyService = dutyService;
        _clientsService = clientsService;
        _authService = authService;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        // ViewBag ile iki farklı listeyi aynı sayfaya gönderiyoruz
        ViewBag.Employees = await _dutyService.GetEmployeesAsync();
        ViewBag.Clients = await _clientsService.GetClients();
        ViewBag.Users = await _authService.GetUsersAsync();

        return View();
    }

    // --- EMPLOYEE SECTION ---
    [HttpGet("employee-tasks/{employeeId}")]
    public async Task<IActionResult> GetEmployeeTasks(int employeeId)
    {
        var duties = await _dutyService.GetDutiesByEmployeeIdAsync(employeeId);
        return PartialView("_EmployeeTasksPartial", duties);
    }

    // --- CLIENT SECTION ---
    [HttpGet("client-tasks/{clientId}")]
    public async Task<IActionResult> GetClientTasks(int clientId)
    {
        var duties = await _clientsService.GetDutiesByClientId(clientId);
        return PartialView("_ClientTasksPartial", duties);
    }

    [HttpPost("add-client")]
    public async Task<IActionResult> AddClient(AddClientRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            await _clientsService.AddClient(request);
            TempData["SuccessMessage"] = "Client added successfully.";
        }
        return RedirectToAction(nameof(Dashboard));
    }

    [HttpPost("delete-client")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        await _clientsService.DeleteClient(id);
        TempData["SuccessMessage"] = "Client deleted successfully.";
        return RedirectToAction(nameof(Dashboard));
    }

    [HttpPost("update-user-role")]
    public async Task<IActionResult> UpdateUserRole(int userId, string username, string role)
    {
        var request = new UpdateUserRoleRequest(userId, role);
        var isSuccess = await _authService.UpdateUserRoleAsync(request);

        if (isSuccess)
        {
            // Eğer yetki verildiyse Duty.API'ye (EmployeeDB'ye) ekle!
            if (role == "Employee" || role == "Admin")
            {
                var createEmployeeRequest = new CreateEmployeeRequest(userId, username, role);
                await _dutyService.CreateEmployeeAsync(createEmployeeRequest);
            }
            // Eğer rol "None" yapıldıysa Duty.API'den silme işlemi de yazılabilir..

            TempData["SuccessMessage"] = "User role updated and synchronized successfully.";
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to update user role.";
        }

        return RedirectToAction(nameof(Dashboard));
    }
    [HttpPost("delete-user")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        var isSuccess = await _authService.DeleteUserAsync(userId);

        if (isSuccess)
        {
            TempData["SuccessMessage"] = "User deleted successfully.";
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to delete user.";
        }

        return RedirectToAction(nameof(Dashboard));
    }
    [HttpGet("recent-logs")]
    public async Task<IActionResult> GetRecentLogs()
    {
        var logs = await _dutyService.GetRecentAuditLogsAsync();

        // We return JSON so JavaScript can easily build the notification list
        return Json(logs);
    }
}
