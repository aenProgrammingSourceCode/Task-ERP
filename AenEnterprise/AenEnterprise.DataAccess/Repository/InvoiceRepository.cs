﻿using AenEnterprise.DataAccess.RepositoryInterface;
using AenEnterprise.DomainModel.SalesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DataAccess.Repository
{
    public class InvoiceRepository:GenericRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(AenEnterpriseDbContext context) : base(context)
        {
            
        }
    }
}