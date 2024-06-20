using AenEnterprise.DomainModel;
using AenEnterprise.DomainModel.SalesManagement;
using AenEnterprise.ServiceImplementations.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Mapping
{
    public static class UnitExtensionMethods
    {
        public static UnitView ConvertToUnitView(this Unit unit)
        {
            var unitView = new UnitView
            {
                Id = unit.Id,
                Name = unit.Name

            };

            return unitView;


        }
        public static IList<UnitView> ConvertToUnitViews(this IEnumerable<Unit> units)
        {
            IList<UnitView> unitViews = new List<UnitView>();

            foreach (Unit unit in units)
            {
                
                unitViews.Add(ConvertToUnitView(unit));
            }

            return unitViews;
        }
    }
}
