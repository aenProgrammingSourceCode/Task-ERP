using AenEnterprise.ServiceImplementations.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Messaging
{
    public class GetAllCategoryResponse
    {
        public IEnumerable<CategoryView> Categories  { get; set; }
    }
}
