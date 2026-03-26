using BuildingBlocks.Contracts.DutyContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Web.Services;

namespace TaskManager.Web.Controllers;

public class DutyController : Controller
{
    private readonly DutyService _dutyService;

    public DutyController(DutyService dutyService)
    {
        _dutyService = dutyService ?? throw new ArgumentNullException(nameof(dutyService));
    }
    [Authorize(Roles = "Admin, Employee")]
    [HttpGet("duties")]
    public async Task<IActionResult> GetDuties(int? pageNumber, int? pageSize)
    {
        var request = new GetDutiesRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _dutyService.GetDuties(request);

        ViewBag.Employees = await _dutyService.GetEmployeesAsync();
        ViewBag.Clients = await _dutyService.GetClientsAsync();

        if (result == null || result.Duties == null)
        {
            ViewBag.Error = "Duty(/ler) bulunamadı";
            return View(new List<DutyDto>());
        }
        return View(result.Duties);
    }
    [HttpPost]
    public async Task<IActionResult> CreateDuty(CreateDutyRequest request)
    {
        // Check if the submitted form data is valid
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Please fill in all required fields correctly.";
            return RedirectToAction(nameof(GetDuties));
        }

        var newDutyId = await _dutyService.CreateDutyAsync(request);

        if (newDutyId.HasValue)
        {
            // If successful, redirect back to the duty list
            TempData["SuccessMessage"] = "Duty successfully created!";
            return RedirectToAction(nameof(GetDuties));
        }

        ViewBag.ErrorMessage = "An error occurred while creating the duty. Please try again.";
        return RedirectToAction(nameof(GetDuties));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> DeleteDuty(int id)
    {
        var isSuccess = await _dutyService.DeleteDutyAsync(id);

        if (isSuccess)
        {
            TempData["SuccessMessage"] = "Duty deleted successfully.";
        }
        else
        {
            // 401 Unauthorized or 403 Forbidden might return false here
            TempData["ErrorMessage"] = "Failed to delete duty. You might not have permission.";
        }

        return RedirectToAction(nameof(GetDuties));
    }

    [Authorize(Roles = "Admin, Employee")]
    [HttpPost]
    public async Task<IActionResult> EditDuty(UpdateDutyRequest request)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Please fill in all required fields correctly.";
            return RedirectToAction(nameof(GetDuties));
        }

        var isSuccess = await _dutyService.UpdateDutyAsync(request);

        if (isSuccess)
        {
            TempData["SuccessMessage"] = "Duty updated successfully.";
            return RedirectToAction(nameof(GetDuties));
        }

        ViewBag.ErrorMessage = "An error occurred while updating the duty.";
        return RedirectToAction(nameof(GetDuties));
    }
    // Inside DutyController.cs

    [Authorize(Roles = "Employee, Admin")] 
    [HttpGet("my-tasks")]
    public async Task<IActionResult> MyTasks()
    {
        // 1. Read the EmployeeId from the JWT Claims
        var employeeIdClaim = User.Claims.FirstOrDefault(c => c.Type == "EmployeeId")?.Value;

        // 2. Validate the claim
        if (string.IsNullOrEmpty(employeeIdClaim) || !int.TryParse(employeeIdClaim, out int employeeId))
        {
            TempData["ErrorMessage"] = "Could not find your Employee profile. Please login again.";
            return RedirectToAction("Index", "Home");
        }

        // 3. Fetch ONLY the duties assigned to this specific employee
        var duties = await _dutyService.GetDutiesByEmployeeIdAsync(employeeId);

        return View(duties);
    }

    [Authorize(Roles = "Employee, Admin")]
    [HttpPost("update-task-status")]
    public async Task<IActionResult> UpdateTaskStatus(UpdateDutyRequest request)
    {
        var isSuccess = await _dutyService.UpdateDutyAsync(request);

        if (isSuccess)
        {
            TempData["SuccessMessage"] = "Task status updated successfully.";
        }
        else
        {
            TempData["ErrorMessage"] = "An error occurred while updating the status.";
        }

        // Redirect back to the personal dashboard
        return RedirectToAction(nameof(MyTasks));
    }
}