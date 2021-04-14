using System.Collections.Generic;
using IntegrationTest.Enums;

namespace IntegrationTest.Models
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public List<Role> Roles { get; set; }
    }
}
