using ISTP_LABA_3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Data
{
    public class DBInit
    {
        public static void Initialize(BankAppContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
