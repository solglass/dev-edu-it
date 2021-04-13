using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTest.Models
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public List<Role> Roles { get; set; }
    }
}
