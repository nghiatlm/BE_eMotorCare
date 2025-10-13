
using eMotoCare.Domain.Common;
using eMotoCare.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.Domain.Entities
{
    [Index(nameof(CitizenId), IsUnique = true)]
    public class Customer : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Address { get; set; }
        public string CitizenId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string? Avatar { get; set; }
    }
}