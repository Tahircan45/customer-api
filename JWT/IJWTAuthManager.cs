using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace customer_api.JWT
{
    public interface IJWTAuthManager
    {
        string Auth(string username, string password); 
    }
}
