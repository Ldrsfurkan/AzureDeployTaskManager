using BuildingBlocks.Exceptions;

namespace Duty.API.Exceptions
{
    public class DutyNotFoundException : NotFoundException
    {
        public DutyNotFoundException(int Id) : base("Duty", Id)
        {
        }
    }
}

