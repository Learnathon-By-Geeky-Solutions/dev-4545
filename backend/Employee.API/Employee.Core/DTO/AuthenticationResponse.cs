using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.Core.DTO
{
    public class AuthenticationResponse
    {
        public Guid Id { get; set; }
        public string ?JwToken { get; set; }
        public string ?Role { get; set; }
        public string ?RefreshToken { get; set; }
    }
}