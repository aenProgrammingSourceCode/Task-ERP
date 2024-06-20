using AenEnterprise.DomainModel;
using AenEnterprise.DomainModel.EmployeeManagement;
using AenEnterprise.DomainModel.HRDomain;
using AenEnterprise.DomainModel.Inventory;
using AenEnterprise.DomainModel.InventoryManagement;
using AenEnterprise.DomainModel.PurchaseManagement;
using AenEnterprise.DomainModel.SalesManagement;
using AenEnterprise.DomainModel.SupplierManagement;
using AenEnterprise.DomainModel.UserDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DataAccess
{
    public class AenEnterpriseDbContext : DbContext
    {
        public AenEnterpriseDbContext()
        {

        }
        public AenEnterpriseDbContext(DbContextOptions<AenEnterpriseDbContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=AenDbEnterprise; Integrated Security=True;TrustServerCertificate=True;")
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        //Sales management system
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<DeliveryOrder> DeliveryOrders { get; set; }
        public DbSet<DispatcheOrder> DispatchOrders { get; set; }
        public DbSet<DomainModel.SalesManagement.Unit> Units { get; set; }
        public DbSet<SalesOrderStatus> SalesOrderStatuses { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        //Inventory System
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockItem> StockItems { get; set; }

        //HR Management
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<Salary> Salary { get; set; }
        public DbSet<Increament> Increaments { get; set; }
        public DbSet<Department> Departments { get; set; }
        
        //Company Info
        public DbSet<Company> Companies { get; set; }
        public DbSet<Branch> Branches { get; set; }

        //SupplierManagement
        public DbSet<Supplier> Suppliers { get; set; }


        //Purchase Management
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseItem> PurchaseItems { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Leave>(entity =>
            {
                entity.HasKey(x => x.Id);
                //map relation One SalesOrder with many Invoice
                entity.HasOne(emp => emp.Employee).WithMany(emp => emp.Leaves).HasForeignKey(sa => sa.EmployeeId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id").HasColumnType("int").IsRequired();
                entity.Property(e => e.FirstName).HasColumnName("FirstName").HasColumnType("nvarchar(100)").IsRequired();
                entity.Property(e => e.LastName).HasColumnName("LastName").HasColumnType("nvarchar(100)").IsRequired();
                entity.Property(e => e.EmployeeCode).HasColumnName("EmployeeCode").HasColumnType("nvarchar(20)").IsRequired();
                entity.Property(e => e.Email).HasColumnName("Email").HasColumnType("nvarchar(100)").IsRequired();
                entity.Property(e => e.PhoneNumber).HasColumnName("PhoneNumber").HasColumnType("nvarchar(20)").IsRequired();
                entity.Property(e => e.DateOfBirth).HasColumnName("DateOfBirth").HasColumnType("date").IsRequired();
                entity.Property(e => e.Address).HasColumnName("Address").HasColumnType("nvarchar(255)").IsRequired();
                entity.Property(e => e.Department).HasColumnName("Department").HasColumnType("nvarchar(50)").IsRequired();
                entity.Property(e => e.Salary).HasColumnName("Salary").HasColumnType("decimal(18, 2)").IsRequired();
                entity.Property(e => e.HireDate).HasColumnName("HireDate").HasColumnType("date").IsRequired();
                entity.Property(e => e.Gender).HasColumnName("Gender").HasColumnType("nvarchar(10)").IsRequired();
                entity.Property(e => e.HairColor).HasColumnName("HairColor").HasColumnType("nvarchar(20)").IsRequired();
                entity.Property(e => e.Age).HasColumnName("Age").HasColumnType("nvarchar(10)").IsRequired();
                entity.Property(e => e.Nationality).HasColumnName("Nationality").HasColumnType("nvarchar(50)").IsRequired();
                entity.Property(e => e.MaritalStatus).HasColumnName("MaritalStatus").HasColumnType("nvarchar(20)").IsRequired();
                entity.Property(e => e.Weight).HasColumnName("Weight").HasColumnType("nvarchar(20)").IsRequired();
                entity.Property(e => e.Height).HasColumnName("Height").HasColumnType("nvarchar(20)").IsRequired();
                entity.Property(e => e.IdentificationMark).HasColumnName("IdentificationMark").HasColumnType("nvarchar(255)").IsRequired();
                entity.Property(e => e.Favourite).HasColumnName("Favourite").HasColumnType("nvarchar(100)").IsRequired();
                entity.Property(e => e.TAXIDNo).HasColumnName("TAXIDNo").HasColumnType("nvarchar(20)").IsRequired();
                entity.Property(e => e.DateOfExpiry).HasColumnName("DateOfExpiry").HasColumnType("date").IsRequired();
                
                entity.HasData(
                    new Employee
                    {
                        Id = 1,
                        FirstName = "John",
                        LastName = "Doe",
                        EmployeeCode = "EMP001",
                        Email = "john@example.com",
                        PhoneNumber = "1234567890",
                        DateOfBirth = new DateTime(1990, 5, 15),
                        Address = "123 Main St",
                        Department = "IT",
                        Salary = 50000.00m,
                        HireDate = DateTime.Now.AddYears(-2),
                        Gender = "Male",
                        // Populate other properties accordingly
                        DateOfExpiry = DateTime.Now.AddYears(5)
                    },
                    new Employee
                    {
                        Id = 2,
                        FirstName = "Jane",
                        LastName = "Smith",
                        EmployeeCode = "EMP002",
                        Email = "jane@example.com",
                        PhoneNumber = "9876543210",
                        DateOfBirth = new DateTime(1988, 9, 20),
                        Address = "456 Elm St",
                        Department = "HR",
                        Salary = 60000.00m,
                        HireDate = DateTime.Now.AddYears(-3),
                        Gender = "Female",
                        // Populate other properties accordingly
                        DateOfExpiry = DateTime.Now.AddYears(4)
                    }
                );
            });

            modelBuilder.Entity<Increament>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(emp => emp.Amount).HasColumnType("decimal(18,2)");
                //map relation One SalesOrder with many Invoice
                entity.HasOne(emp => emp.Employee).WithMany(emp => emp.Increaments).HasForeignKey(sa => sa.EmployeeId).OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<Salary>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(emp => emp.BasicSalary).HasColumnType("decimal(18,2)");
                entity.Property(emp => emp.SalaryPerDay).HasColumnType("decimal(18,2)");
                entity.Property(emp => emp.Allowances).HasColumnType("decimal(18,2)");
                entity.Property(emp => emp.Deductions).HasColumnType("decimal(18,2)");
                entity.Property(emp => emp.GrossSalary).HasColumnType("decimal(18,2)");
                entity.Property(emp => emp.NetSalary).HasColumnType("decimal(18,2)");

                //map relation One SalesOrder with many Invoice
                entity.HasOne(sa => sa.Employee).WithMany(sa => sa.Salaries).HasForeignKey(sa => sa.EmployeeId).OnDelete(DeleteBehavior.NoAction);

            });

            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.HasKey(x => x.Id);

                //map relation One SalesOrder with many Invoice
                entity.HasOne(at => at.Employee).WithMany(at => at.Attendances).HasForeignKey(at => at.EmployeeId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<BankAccount>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(so => so.Balance).HasColumnType("decimal(18,2)");
                entity.HasMany(b => b.Transactions).WithOne(t => t.BankAccount).HasForeignKey(t => t.BankAccountId);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(so => so.Amount).HasColumnType("decimal(18,2)");
                entity.Property(so => so.BalanceBefore).HasColumnType("decimal(18,2)");
                entity.Property(so => so.BalanceAfter).HasColumnType("decimal(18,2)");
                //map relation One Bank Account with many Transaction

                entity.HasOne(t => t.BankAccount).WithMany(b => b.Transactions).HasForeignKey(t => t.BankAccountId).OnDelete(DeleteBehavior.NoAction);


            });

            //Mapping Relation



            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasKey(x => x.Id);
                //map relation One role with many User
                entity.HasOne(st => st.PurchaseOrder).WithMany(p => p.Stocks).HasForeignKey(st => st.PurchaseOrderId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(st => st.Warehouse).WithMany(w =>w.Stocks).HasForeignKey(st => st.WareHouseId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<StockItem>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(so => so.CurrentStockQuantity).HasColumnType("decimal(18,2)");
                entity.Property(so => so.NewStockQuantity).HasColumnType("decimal(18,2)");
                //map relation One role with many User
                entity.HasOne(st => st.PurchaseItem).WithMany(w => w.StockItems).HasForeignKey(st => st.PurchaseItemId).OnDelete(DeleteBehavior.NoAction);
            });


            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(u => u.Username).IsRequired();

                //map relation One role with many User
                entity.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId).OnDelete(DeleteBehavior.NoAction);
            });

            //Default data

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(p => p.RoleName).IsRequired();
                entity.HasData(new Role { Id = 1, RoleName = "Admin" },
                                new Role { Id = 2, RoleName = "User" });
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(p => p.Name).IsRequired();
                entity.HasData(new Product { Id = 1, Name = "Plate" },
                    new Product { Id = 2, Name = "Brass" });
            });


            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(c => c.Id); // Set the primary key
                entity.Property(c => c.Name).HasColumnName("Name").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(c => c.RegistrationNumber).HasColumnName("RegistrationNumber").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(c => c.TaxIdentifier).HasColumnName("TaxIdentifier").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(c => c.IndustryType).HasColumnName("IndustryType").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(c => c.CompanyAddress).HasColumnName("CompanyAddress").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(c => c.Country).HasColumnName("Country").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(c => c.CompanyEmail).HasColumnName("CompanyEmail").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(c => c.CompanyPhone).HasColumnName("CompanyPhone").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(c => c.IncorporationDate).HasColumnName("IncorporationDate").HasColumnType("datetime2").IsRequired();
                entity.Property(c => c.FullName).HasColumnName("FullName").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(c => c.InvoiceAddress).HasColumnName("InvoiceAddress").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(c => c.WebPageAddress).HasColumnName("WebPageAddress").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(c => c.VATCode).HasColumnName("VATCode").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(c => c.VATAreaCode).HasColumnName("VATAreaCode").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(c => c.IsPrimary).HasColumnName("IsPrimary").HasColumnType("bit").IsRequired();
                entity.Property(c => c.CreatedDate).HasColumnName("CreatedDate").HasColumnType("datetime2").IsRequired();
                entity.Property(c => c.ExpiryDate).HasColumnName("ExpiryDate").HasColumnType("datetime2").IsRequired();
                entity.Property(c => c.IsDeleted).HasColumnName("IsDeleted").HasColumnType("bit").IsRequired();
                entity.Property(c => c.AnnualRevenue).HasColumnName("AnnualRevenue").HasColumnType("decimal").IsRequired();
                entity.Property(c => c.IsPubliclyListed).HasColumnName("IsPubliclyListed").HasColumnType("bit").IsRequired();
                entity.Property(c => c.IsMultipleWareHouse).HasColumnName("IsMultipleWareHouse").HasColumnType("bit").IsRequired();

                // Sample data for Company entity
                entity.HasData(
                    new Company
                    {
                        Id = 1,
                        Name = "Company B",
                        RegistrationNumber = "123456789",
                        TaxIdentifier = "TAX123",
                        IndustryType = "Sample Industry",
                        CompanyAddress = "123 Sample St",
                        Country = "Sample Country",
                        CompanyEmail = "company@example.com",
                        CompanyPhone = "123-456-7890",
                        IncorporationDate = DateTime.Now.AddYears(-5),
                        FullName = "Full Name",
                        InvoiceAddress = "456 Invoice St",
                        WebPageAddress = "www.company.com",
                        VATCode = "VAT-123",
                        VATAreaCode = "Area-123",
                        IsPrimary = true,
                        CreatedDate = DateTime.Now.AddYears(-5),
                        ExpiryDate = DateTime.Now.AddYears(5),
                        IsDeleted = false,
                        AnnualRevenue = 1000000.00m,
                        IsPubliclyListed = false,
                        IsMultipleWareHouse = true
                    },
                    new Company
                    {
                        Id = 2,
                        Name = "Company A",
                        RegistrationNumber = "123488796",
                        TaxIdentifier = "TAX123",
                        IndustryType = "Sample Industry",
                        CompanyAddress = "123 Sample St",
                        Country = "Sample Country",
                        CompanyEmail = "company@example.com",
                        CompanyPhone = "123-456-7890",
                        IncorporationDate = DateTime.Now.AddYears(-5),
                        FullName = "Full Name",
                        InvoiceAddress = "456 Invoice St",
                        WebPageAddress = "www.company.com",
                        VATCode = "VAT-123",
                        VATAreaCode = "Area-123",
                        IsPrimary = true,
                        CreatedDate = DateTime.Now.AddYears(-5),
                        ExpiryDate = DateTime.Now.AddYears(5),
                        IsDeleted = false,
                        AnnualRevenue = 1000000.00m,
                        IsPubliclyListed = false,
                        IsMultipleWareHouse = true
                    });
         
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(c => c.Id); // Set the primary key
                entity.Property(p => p.Name).HasColumnName("Name").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(p => p.CompanyId).HasColumnName("CompanyId").HasColumnType("int").IsRequired();
                entity.Property(p => p.UnitId).HasColumnName("UnitId").HasColumnType("int").IsRequired();
                entity.Property(p => p.DefaultVatPercent).HasColumnName("DefaultVatPercent").HasColumnType("nvarchar(max)");
                entity.Property(p => p.PurchasePrice).HasColumnName("PurchasePrice").HasColumnType("decimal");
                entity.Property(p => p.CostPrice).HasColumnName("CostPrice").HasColumnType("decimal");
                entity.Property(p => p.WholesalePrice).HasColumnName("WholesalePrice").HasColumnType("decimal");
                entity.Property(p => p.MRP).HasColumnName("MRP").HasColumnType("decimal");
                entity.Property(p => p.TradePrice).HasColumnName("TradePrice").HasColumnType("decimal");
                entity.Property(p => p.InventoryType).HasColumnName("InventoryType").HasColumnType("nvarchar(max)");
                entity.Property(p => p.IsQuantityMeasureable).HasColumnName("IsQuantityMeasureable").HasColumnType("bit");
                entity.Property(p => p.FixedAsset).HasColumnName("FixedAsset").HasColumnType("bit");
                entity.Property(p => p.IsPurchaseable).HasColumnName("IsPurchaseable").HasColumnType("bit");
                entity.Property(p => p.IsSaleable).HasColumnName("IsSaleable").HasColumnType("bit");
                entity.Property(p => p.IsConsumable).HasColumnName("IsConsumable").HasColumnType("bit");
                entity.Property(p => p.RawMaterials).HasColumnName("RawMaterials").HasColumnType("bit");
                entity.Property(p => p.IsInventoryRequired).HasColumnName("IsInventoryRequired").HasColumnType("bit");
                entity.Property(p => p.WarehouseId).HasColumnName("WarehouseId").HasColumnType("int");
                entity.Property(p => p.Description).HasColumnName("Description").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(p => p.CategoryId).HasColumnName("CategoryId").HasColumnType("int").IsRequired();

                //map relation One SalesOrder with many Invoice
                entity.HasOne(p => p.Categories).WithMany(p => p.Products).HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(p => p.Warehouse).WithMany(p => p.Products).HasForeignKey(p => p.WarehouseId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(p => p.Company).WithMany(p => p.Products).HasForeignKey(p => p.CompanyId).OnDelete(DeleteBehavior.NoAction);

             entity.HasData(
            new Product
            {
                Id = 1,
                Name = "6/8 Plate",
                CompanyId = 1,
                UnitId = 1,
                DefaultVatPercent = "10%",
                PurchasePrice = 20.0m,
                CostPrice = 18.0m,
                WholesalePrice = 25.0m,
                MRP = 30.0m,
                TradePrice = 22.0m,
                InventoryType = "Type1",
                IsQuantityMeasureable = true,
                FixedAsset = false,
                IsPurchaseable = true,
                IsSaleable = true,
                IsConsumable = false,
                RawMaterials = false,
                IsInventoryRequired = true,
                WarehouseId = 1,
                Description = "Description1",
                CategoryId = 1
            }
            // Add more products as needed
        );

            });

            modelBuilder.Entity<Warehouse>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(w => w.Name).IsRequired();
                // Property configurations
                entity.Property(w => w.Name).HasColumnName("Name").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(w => w.CostingMethodId).HasColumnName("CostingMethodId").HasColumnType("int").IsRequired();
                entity.Property(w => w.Address1).HasColumnName("Address1").HasColumnType("nvarchar(max)");
                entity.Property(w => w.Address2).HasColumnName("Address2").HasColumnType("nvarchar(max)");
                entity.Property(w => w.PhoneNo).HasColumnName("PhoneNo").HasColumnType("nvarchar(max)");
                entity.Property(w => w.EmployeeId).HasColumnName("EmployeeId").HasColumnType("int");
                entity.Property(w => w.Description).HasColumnName("Description").HasColumnType("nvarchar(max)").IsRequired();

                // Relationship mapping
                entity.HasOne(w => w.Employee)
                       .WithMany(e=>e.Warehouses)
                       .HasForeignKey(w => w.EmployeeId)
                       .OnDelete(DeleteBehavior.NoAction);

                // Data seeding using HasData method
                entity.HasData(
                    new Warehouse
                    {
                        Id = 1,
                        Name = "Warehouse A",
                        CostingMethodId = 1,
                        Address1 = "123 Main St",
                        EmployeeId=1,
                        Description = "Warehouse A description"
                        // Add other properties accordingly
                    },
                    new Warehouse
                    {
                        Id = 2,
                        Name = "Warehouse B",
                        CostingMethodId = 2,
                        Address1 = "456 Elm St",
                        EmployeeId = 2,
                        Description = "Warehouse B description"
                        // Add other properties accordingly
                    }         
                );
            });

            // Supplier Mapping
            modelBuilder.Entity<Supplier>(entity =>
            {
                 entity.HasKey(x => x.Id);
            });

            // Purchase Mapping
            modelBuilder.Entity<PurchaseOrder>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(oi => oi.Supplier).WithMany(sp=>sp.PurchaseOrders).HasForeignKey(po => po.SupplierId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<PurchaseItem>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(so => so.Price).HasColumnType("decimal(18,2)");
                entity.Property(so => so.Quantity).HasColumnType("decimal(18,2)");
                entity.Property(so => so.TotalCost).HasColumnType("decimal(18,2)");
                entity.Property(so => so.DiscountAmount).HasColumnType("decimal(18,2)");
                entity.Property(so => so.DiscountPercent).HasColumnType("decimal(18,2)");
                entity.HasOne(oi => oi.PurchaseOrder).WithMany(sp => sp.PurchaseItems).HasForeignKey(po => po.PurchaseOrderId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(oi => oi.Unit).WithMany(sp => sp.PurchaseItems).HasForeignKey(po => po.UnitId).OnDelete(DeleteBehavior.NoAction);
            });


            modelBuilder.Entity<Status>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(p => p.Name).IsRequired();
                entity.HasData(new Product { Id = 1, Name = "Pending"},
                    new Product { Id = 2, Name = "Approved"}, new Product { Id = 3, Name = "Rejected" });
            });


            modelBuilder.Entity<SalesOrderStatus>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(c => c.Name).IsRequired();
                entity.HasData(
                    new SalesOrderStatus { Id = 1, Name = "CreateSalesOrder" },
                    new SalesOrderStatus { Id = 2, Name = "PendingApprovalOrder" },
                    new SalesOrderStatus { Id = 3, Name = "ApprovedSalesOrder" },
                    new SalesOrderStatus { Id = 4, Name = "CreateInvoice" },
                    new SalesOrderStatus { Id = 5, Name = "ApprovedInvoice" },
                    new SalesOrderStatus { Id = 6, Name = "CreateDO" },
                    new SalesOrderStatus { Id = 7, Name = "ApprovedDO" },
                    new SalesOrderStatus { Id = 8, Name = "CreateDispatcheAndDone" },
                    new SalesOrderStatus { Id = 9, Name = "Reject" }
                 );

                //map relation One role with many User
            });

            modelBuilder.Entity<DomainModel.SalesManagement.Unit>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(c => c.Name).IsRequired();
                entity.HasData(new DomainModel.SalesManagement.Unit { Id = 1, Name = "KG" },
                    new DomainModel.SalesManagement.Unit { Id = 2, Name = "Lot" });
            });


            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(c => c.Name).IsRequired();
                entity.HasData(new Customer { Id = 1, Name = "Alam", Address = "Dhaka", CreatedDate = DateTime.Now, Description = "This is Plate Customer", MobileNo = "01887969696" },
                    new Customer { Id = 2, Name = "Shamim Enterprise", Address = "Dhaka", CreatedDate = DateTime.Now, Description = "This is Brass Customer", MobileNo = "01887969696" },
                    new Customer { Id = 3, Name = "Shahab Uddin", Address = "Chittagong", CreatedDate = DateTime.Now, Description = "This is Plate Customer", MobileNo = "01887969696" });
                //map relation One role with many User
            });

            modelBuilder.Entity<SalesOrder>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasOne(so => so.Customer).WithMany(cust => cust.SalesOrders).HasForeignKey(cust => cust.CustomerId).OnDelete(DeleteBehavior.NoAction);
                //entity.HasMany(so => so.OrderItems).WithOne(t => t.SalesOrder).HasForeignKey(t => t.SalesOrderId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(oi => oi.Price).HasColumnType("decimal(18,2)");
                entity.Property(oi => oi.Quantity).HasColumnType("decimal(18,2)");
                entity.Property(oi => oi.BalanceQuantity).HasColumnType("decimal(18,2)");
                entity.Property(oi => oi.InvoicedQuantity).HasColumnType("decimal(18,2)");
                entity.Property(oi => oi.Amount).HasColumnType("decimal(18,2)");
                entity.Property(oi => oi.DiscountPercent).HasColumnType("decimal(18,2)");
                entity.Property(oi => oi.NetAmount).HasColumnType("decimal(18,2)");
                entity.Property(oi => oi.ProductId).HasColumnType("int");
                entity.Property(oi => oi.InvoicedAmount).HasColumnType("decimal(18,2)");
                //map relation One SalesOrder with many Invoice
                entity.HasOne(oi => oi.SalesOrder).WithMany(so => so.OrderItems).HasForeignKey(oi => oi.SalesOrderId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(oi => oi.Product).WithMany(p => p.OrderItems).HasForeignKey(oi => oi.ProductId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(oi => oi.Unit).WithMany(un => un.OrderItems).HasForeignKey(oi => oi.UnitId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(oi => oi.Status).WithMany(p => p.OrderItems).HasForeignKey(oi => oi.StatusId).OnDelete(DeleteBehavior.NoAction);
                
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(inv => inv.SalesOrder).WithMany(so => so.Invoices).HasForeignKey(so => so.SalesOrderId).OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<InvoiceItem>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(inv => inv.InvoiceQuantity).HasColumnType("decimal(18,2)");
                entity.Property(inv => inv.InvoiceAmount).HasColumnType("decimal(18,2)");
                entity.Property(inv => inv.DeliveryQuantity).HasColumnType("decimal(18,2)");
                entity.Property(inv => inv.DeliveryAmount).HasColumnType("decimal(18,2)");
                entity.Property(inv => inv.BalanceQuantity).HasColumnType("decimal(18,2)");
                entity.Property(inv => inv.BalanceAmount).HasColumnType("decimal(18,2)");
                entity.Property(inv => inv.DeliveryQuantity).HasColumnType("decimal(18,2)");
                entity.HasOne(oi => oi.Status).WithMany(p => p.InvoiceItems).HasForeignKey(oi => oi.StatusId).OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<DeliveryOrder>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(dord => dord.Id).IsRequired();
                entity.HasOne(dord => dord.SalesOrder).WithMany(so => so.DeliveryOrders).HasForeignKey(dord => dord.SalesOrderId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(dord => dord.Invoice).WithMany(inv => inv.DeliveryOrders).HasForeignKey(dord => dord.InvoiceId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<DeliveryOrderItem>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(dord => dord.Id).IsRequired();
                entity.Property(dord => dord.DeliveryQuantity).HasColumnType("decimal(18,2)");
                entity.Property(dord => dord.DeliveryAmount).HasColumnType("decimal(18,2)");
                entity.Property(dord => dord.DispatchQuantity).HasColumnType("decimal(18,2)");
                entity.Property(dord => dord.DispatchAmount).HasColumnType("decimal(18,2)");
                entity.Property(dord => dord.BalanceQuantity).HasColumnType("decimal(18,2)");
                entity.Property(dord => dord.BalanceAmount).HasColumnType("decimal(18,2)");
                
                entity.HasOne(oi => oi.Status).WithMany(p => p.DeliveryOrderItems).HasForeignKey(oi => oi.StatusId).OnDelete(DeleteBehavior.NoAction);
                
                entity.HasOne(oi => oi.OrderItem).WithMany(p => p.DeliveryOrderItems).HasForeignKey(oi => oi.OrderItemId).OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<DispatcheOrder>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(disp => disp.SalesOrder).WithMany(so => so.DispatcheOrders).HasForeignKey(disp => disp.SalesOrderId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(disp => disp.Invoice).WithMany(inv => inv.DispatcheOrders).HasForeignKey(disp => disp.InvoiceId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(disp => disp.DeliveryOrder).WithMany(dord => dord.DispatcheOrders).HasForeignKey(disp => disp.DeliveryOrderId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<DispatchItem>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(disp => disp.Id).IsRequired();
                entity.Property(dord => dord.VehicalEmptyWeight).HasColumnType("decimal(18,2)");
                entity.Property(dord => dord.VehicalLoadWeight).HasColumnType("decimal(18,2)");
                entity.Property(dord => dord.DispatchQuantity).HasColumnType("decimal(18,2)");
                entity.Property(dord => dord.DispatchAmount).HasColumnType("decimal(18,2)");
                entity.HasOne(di => di.OrderItem).WithMany(di => di.DispatchItems).HasForeignKey(di => di.OrderItemId).OnDelete(DeleteBehavior.NoAction);
            });


        }
    }
}

    

     



