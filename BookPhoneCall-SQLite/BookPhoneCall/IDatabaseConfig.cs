using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPhoneCall
{
    internal interface IDatabaseConfig
    {
        string GetConnectionString();
    }
}
