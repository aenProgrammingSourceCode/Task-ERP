using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.UserDomain
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string RefreshToken { get; set; } = string.Empty; 
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
        public Role Role { get; set; }
        public int RoleId { get; set; }
        public string Password { get; set; }
    }
}
