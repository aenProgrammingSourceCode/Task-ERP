using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.ViewModel
{
    public class CategoryView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProductView> Products { get; set; }
    }
}
