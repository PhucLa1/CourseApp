using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public static class HandleString
    {
        public static string GenerateRandomCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}
