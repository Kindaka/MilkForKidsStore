using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.BackgroundServices.Interfaces
{
    public interface IOrderBackgroundService
    {
        Task RejectExpiredOrder();
    }
}
