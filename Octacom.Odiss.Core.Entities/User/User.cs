using System;

namespace Octacom.Odiss.Core.Entities.User
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte? Type { get; set; }
        public string PhoneOffice { get; set; }
        public string PhoneMobile { get; set; }
        public string Company { get; set; }
        public bool? Active { get; set; }
        public DateTime? Expire { get; set; }
        public bool DocumentsChanged { get; set; }
        public UserPermission Permissions { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime? LockAccessUntil { get; set; }
        public int? WrongAccessAttempts { get; set; }
    }
}
