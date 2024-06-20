using AenEnterprise.DomainModel.SalesManagement;
using AenEnterprise.ServiceImplementations.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Messaging.InventoryManagement
{
    public class GetAllUnitResponse
    {
        public IEnumerable<UnitView> Units { get; set; }
    }
}
