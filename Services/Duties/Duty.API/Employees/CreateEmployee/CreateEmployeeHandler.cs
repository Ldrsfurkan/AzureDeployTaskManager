namespace Duty.API.Employees.CreateEmployee;

public record CreateEmployeeCommand(int UserId, string Name, string UserRole) : ICommand<CreateEmployeeResult>;
public record CreateEmployeeResult(int Id);

internal class CreateEmployeeCommandHandler(IDocumentSession session) : ICommandHandler<CreateEmployeeCommand, CreateEmployeeResult>
{
    public async Task<CreateEmployeeResult> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
    {
        // Prevent duplicate employees if admin clicks update multiple times
        var existingEmployee = await session.Query<Employee>()
            .FirstOrDefaultAsync(e => e.UserId == command.UserId, cancellationToken);

        if (existingEmployee != null)
        {
            // If exists, just update the role
            existingEmployee.UserRole = command.UserRole;
            session.Update(existingEmployee);
            await session.SaveChangesAsync(cancellationToken);
            return new CreateEmployeeResult(existingEmployee.Id);
        }

        // Create new employee
        var employee = new Employee
        {
            UserId = command.UserId,
            Name = command.Name,
            UserRole = command.UserRole
        };

        session.Store(employee);
        await session.SaveChangesAsync(cancellationToken);

        return new CreateEmployeeResult(employee.Id);
    }
}
