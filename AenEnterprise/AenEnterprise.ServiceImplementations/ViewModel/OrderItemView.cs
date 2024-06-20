using AenEnterprise.DomainModel.SalesManagement;
using AenEnterprise.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace AenEnterprise.ServiceImplementations.ViewModel
{
    public class OrderItemView
    {
        public int Id { get; set; }
        public int SalesOrderId { get; set; }
        public int ProductId { get; set; }
        public int ApprovalId { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public string ApprovalName { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public string QuantityInFormat
        {
            get
            {
                return $"{Quantity:#,#.##}";
            }
        }

        public string InvoiceQuantityInFormat
        {
            get
            {
                return $"{InvoiceQuantity:#,#.##}";
            }
        }
        public string BalanceQuantityInFormat
        {
            get
            {
                return $"{BalanceQuantity:#,#.##}";
            }
        }

        public decimal Amount { get; set; }


        public string AmountInFormat
        {
            get
            {
                return string.Format(new CultureInfo("bn-BD"), "{0:C}", Amount);
            }
        }
            
        
        public decimal BalanceQuantity { get; set; }
        public decimal InvoiceQuantity { get; set; }
        public bool IsFullyApproved { get; set; }

    }
}
