using Microsoft.EntityFrameworkCore;
using AutoMapper;
using UtsRestfullAPI.Data;
using UtsRestfullAPI.DTO;
using UtsRestfullAPI.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProduct, ProductEF>();
builder.Services.AddScoped<ICategory, CategoryEF>();
builder.Services.AddScoped<ICustomer, CustomerEF>();
builder.Services.AddScoped<ISaless, SalessEF>();
builder.Services.AddScoped<ISalesItem, SaleItemsEF>();
builder.Services.AddScoped<IEmployee, EmployeeEF>();
builder.Services.AddScoped<IMember, MemberEF>();
// builder.Services.AddScoped<IViewProductWithCategory, ViewProductWithCategoryEF>();
// builder.Services.AddScoped<IViewSalesProductCustomer, ViewSalesProductCustomerEF>();
// builder.Services.AddScoped<IViewSalesProductCustomerOB, ViewSalesProductCustomerOBEF>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(UtsRestfullAPI.Helpers.ApiSettings.GenerateSecretByte()),
        ValidateIssuer = false, // Set to true if you want to validate the issuer
        ValidateAudience = false, // Set to true if you want to validate the audience
        ClockSkew = TimeSpan.Zero // Disable clock skew for testing purposes
    };
});

builder.Services.AddAuthorization(option =>
{
    option.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser().Build();
});


builder.Services.AddAutoMapper(typeof(UtsRestfullAPI.Mapping.MappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

//product
app.MapGet("api/uts/products",(IProduct productData,IMapper mapper)=>
{
    var products = productData.GetProducts();
    var productDTOs = mapper.Map<List<ProductDTO>>(products);
    return Results.Ok(productDTOs);
}).RequireAuthorization();

app.MapGet("api/uts/products/{id}", (IProduct productData, int id, IMapper mapper) =>
{
    var product = productData.GetProductById(id);
    if (product == null)
    {
        return Results.NotFound($"Product with ID {id} not found.");
    }
    var productDTO = mapper.Map<ProductDTO>(product);
    return Results.Ok(productDTO);
}).RequireAuthorization();

app.MapPost("api/uts/products", (IProduct productData, ProductAddDTO productAddDTO, IMapper mapper) =>
{
    try
    {
        var product = mapper.Map<Product>(productAddDTO);
        var newProduct = productData.AddProduct(product);
        var insertedProduct = productData.GetProductById(newProduct.ProductId);

        var result = mapper.Map<ProductDTO>(insertedProduct);
        return Results.Created($"/api/uts/products/{result.ProductId}", result);
    }
    catch (System.Exception ex)
    {
        return Results.BadRequest($"Error adding product: {ex.Message}");
    }
}).RequireAuthorization();

app.MapPut("api/uts/products", (IProduct productData, ProductEditDTO productEditDTO, IMapper mapper) =>
{
    var existingProduct = productData.GetProductById(productEditDTO.ProductId);
    if (existingProduct == null)
    {
        return Results.NotFound($"Product with ID {productEditDTO.ProductId} not found.");
    }
    existingProduct.ProductName = productEditDTO.ProductName;
    existingProduct.CategoryId = productEditDTO.CategoryId;
    existingProduct.Price = productEditDTO.Price;
    existingProduct.StockQuantity = productEditDTO.StockQuantity;
    existingProduct.Description = productEditDTO.Description;

    productData.UpdateProduct(existingProduct);

    // Ambil ulang produk dari database agar properti Category terisi
    var updatedProduct = productData.GetProductById(productEditDTO.ProductId);

    var updatedProductDTO = mapper.Map<ProductDTO>(updatedProduct);
    return Results.Ok(updatedProductDTO);
}).RequireAuthorization();

app.MapDelete("api/uts/products/{id}", (IProduct productData, int id) =>
{
    productData.DeleteProduct(id);
    return Results.NoContent();
}).RequireAuthorization();

//category
app.MapGet("api/uts/categories", (ICategory categoryData, IMapper mapper) =>
{
    var categories = categoryData.GetCategories();
    var categoryDTOs = mapper.Map<List<CategoryDTO>>(categories);
    return categoryDTOs;
}).RequireAuthorization();

app.MapGet("api/uts/categories/{id}", (ICategory categoryData, int id, IMapper mapper) =>
{
    var category = categoryData.GetCategoryById(id);
    if (category == null)
    {
        return Results.NotFound($"Category with ID {id} not found.");
    }
    var categoryDTO = mapper.Map<CategoryDTO>(category);
    return Results.Ok(categoryDTO);
}).RequireAuthorization();

app.MapPost("api/uts/categories", (ICategory categoryData, CategoryAddDTO categoryAddDTO, IMapper mapper) =>
{
    try
    {
        var category = mapper.Map<Category>(categoryAddDTO);
        var newCategory = categoryData.AddCategory(category);
        var insertedCategory = categoryData.GetCategoryById(newCategory.CategoryId);

        var result = mapper.Map<CategoryDTO>(insertedCategory);
        return Results.Created($"/api/uts/categories/{result.CategoryId}", result);
    }
    catch (System.Exception ex)
    {
        return Results.BadRequest($"Error adding category: {ex.Message}");
    }
}).RequireAuthorization();

app.MapPut("api/uts/categories", (ICategory categoryData, CategoryDTO categoryDTO, IMapper mapper) =>
{
    var existingCategory = categoryData.GetCategoryById(categoryDTO.CategoryId);
    if (existingCategory == null)
    {
        return Results.NotFound($"Category with ID {categoryDTO.CategoryId} not found.");
    }
    existingCategory.CategoryName = categoryDTO.CategoryName;

    var updatedCategory = categoryData.UpdateCategory(existingCategory);
    var updatedCategoryDTO = mapper.Map<CategoryDTO>(updatedCategory);
    return Results.Ok(updatedCategoryDTO);
}).RequireAuthorization();

app.MapDelete("api/uts/categories/{id}", (ICategory categoryData, int id) =>
{
    categoryData.DeleteCategory(id);
    return Results.NoContent();
}).RequireAuthorization();

//customer
app.MapGet("api/uts/customers", (ICustomer customerData, IMapper mapper) =>
{
    var customers = customerData.GetCustomers();
    var customerDTOs = mapper.Map<List<CustomerDTO>>(customers);
    return Results.Ok(customerDTOs);
}).RequireAuthorization();

app.MapGet("api/uts/customers/{id}", (ICustomer customerData, int id, IMapper mapper) =>
{
    var customer = customerData.GetCustomerById(id);
    if (customer == null)
    {
        return Results.NotFound($"Customer with ID {id} not found.");
    }
    var customerDTO = mapper.Map<CustomerDTO>(customer);
    return Results.Ok(customerDTO);
}).RequireAuthorization();

app.MapPost("api/uts/customers", (ICustomer customerData, CustomerAddDTO customerAddDTO, IMapper mapper) =>
{
    try
    {
        var customer = mapper.Map<Customer>(customerAddDTO);
        var newCustomer = customerData.AddCustomer(customer);
        var insertedCustomer = customerData.GetCustomerById(newCustomer.CustomerId);

        var result = mapper.Map<CustomerDTO>(insertedCustomer);
        return Results.Created($"/api/uts/customers/{result.CustomerId}", result);
    }
    catch (System.Exception ex)
    {
        return Results.BadRequest($"Error adding customer: {ex.Message}");
    }
}).RequireAuthorization();


app.MapPut("api/uts/customers", (ICustomer customerData, CustomerDTO customerDTO, IMapper mapper) =>
{
    var existingCustomer = customerData.GetCustomerById(customerDTO.CustomerId);
    if (existingCustomer == null)
    {
        return Results.NotFound($"Customer with ID {customerDTO.CustomerId} not found.");
    }
    existingCustomer.CustomerName = customerDTO.CustomerName;
    existingCustomer.Email = customerDTO.Email;
    existingCustomer.ContactNumber = customerDTO.ContactNumber;
    existingCustomer.Address = customerDTO.Address;

    var updatedCustomer = customerData.UpdateCustomer(existingCustomer);
    var updatedCustomerDTO = mapper.Map<CustomerDTO>(updatedCustomer);
    return Results.Ok(updatedCustomerDTO);
}).RequireAuthorization();

app.MapDelete("api/uts/customers/{id}", (ICustomer customerData, int id) =>
{
    customerData.DeleteCustomer(id);
    return Results.NoContent();
}).RequireAuthorization();


//sales
app.MapGet("api/uts/sales", (ISaless salesData, IMapper mapper) =>
   {
       var sales = salesData.GetSales(); // Ambil data penjualan dari repository
       var salesDTOs = mapper.Map<List<SalessGetDTO>>(sales); // Konversi ke DTO
       return Results.Ok(salesDTOs); // Kembalikan hasil sebagai respons
   }).RequireAuthorization();
   

app.MapGet("api/uts/sales/{id}", (ISaless salesData, int id, IMapper mapper) =>
{
    var sale = salesData.GetSalesById(id);
    if (sale == null)
    {
        return Results.NotFound($"Sale with ID {id} not found.");
    }
    var saleDTO = mapper.Map<SalessGetDTO>(sale);
    return Results.Ok(saleDTO);
}).RequireAuthorization();

app.MapPost("api/uts/sales", (ISaless salesData, SalessAddDTO salesAddDTO, IMapper mapper) =>
{
    try
    {
        var sale = mapper.Map<Saless>(salesAddDTO);
        var newSale = salesData.AddSales(sale);
        var insertedSale = salesData.GetSalesById(newSale.SaleId);

        var result = mapper.Map<SalessDTO>(insertedSale);
        return Results.Created($"/api/uts/sales/{result.SaleId}", new {
            result.SaleId,
            result.CustomerId,
            result.SaleDate,
            result.TotalAmount,
            result.Customer
        });
    }
    catch (System.Exception ex)
    {
        return Results.BadRequest($"Error adding sale: {ex.Message}");
    }
}).RequireAuthorization();

app.MapPut("api/uts/sales", (ISaless salesData, SalessGetDTO salessGetDTO, IMapper mapper) =>
{
    var existingSale = salesData.GetSalesById(salessGetDTO.SaleId);
    if (existingSale == null)
    {
        return Results.NotFound($"Sale with ID {salessGetDTO.SaleId} not found.");
    }
    existingSale.CustomerId = salessGetDTO.CustomerId;
    existingSale.SaleDate = salessGetDTO.SaleDate;
    existingSale.TotalAmount = salessGetDTO.TotalAmount;

    var updatedSale = salesData.UpdateSales(existingSale);
    var updatedSaleDTO = mapper.Map<SalessGetDTO>(updatedSale);
    return Results.Ok(updatedSaleDTO);
}).RequireAuthorization();

app.MapDelete("api/uts/sales/{id}", (ISaless salesData, int id) =>
{
    salesData.DeleteSales(id);
    return Results.NoContent();
}).RequireAuthorization();


//SaleItem
app.MapGet("api/uts/saleItems", (ISalesItem saleItemData, IMapper mapper) =>
{
    var saleItems = saleItemData.GetSaleItems();
    var saleItemDTOs = mapper.Map<List<SaleItemGetDTO>>(saleItems);
    return Results.Ok(saleItemDTOs);
}).RequireAuthorization();

app.MapGet("api/uts/saleItems/{id}", (ISalesItem saleItemData, int id, IMapper mapper) =>
{
    var saleItem = saleItemData.GetSaleItemById(id);
    if (saleItem == null)
    {
        return Results.NotFound($"SaleItem with ID {id} not found.");
    }
    var saleItemDTO = mapper.Map<SaleItemGetDTO>(saleItem);
    return Results.Ok(saleItemDTO);
}).RequireAuthorization();

app.MapPost("api/uts/saleItems", (ISalesItem saleItemData, SaleItemAddDTO saleItemAddDTO, IMapper mapper) =>
{
    try
    {
        var saleItem = mapper.Map<SaleItem>(saleItemAddDTO);
        var newSaleItem = saleItemData.AddSaleItem(saleItem);
        var insertedSaleItem = saleItemData.GetSaleItemById(newSaleItem.SaleItemId);

        var result = mapper.Map<SaleItemGetDTO>(insertedSaleItem);
        return Results.Created($"/api/uts/saleItems/{result.SaleItemId}", result);
    }
    catch (System.Exception ex)
    {
        return Results.BadRequest($"Error adding SaleItem: {ex.InnerException?.Message ?? ex.Message}");
    }
}).RequireAuthorization();

app.MapPut("api/uts/saleItems", (ISalesItem saleItemData, SaleItemDTO saleItemDTO, IMapper mapper) =>
{
    var existingSaleItem = saleItemData.GetSaleItemById(saleItemDTO.SaleItemId);
    if (existingSaleItem == null)
    {
        return Results.NotFound($"SaleItem with ID {saleItemDTO.SaleItemId} not found.");
    }
    existingSaleItem.SaleId = saleItemDTO.SaleId;
    existingSaleItem.ProductId = saleItemDTO.ProductId;
    existingSaleItem.Quantity = saleItemDTO.Quantity;
    existingSaleItem.Price = saleItemDTO.Price;

    var updatedSaleItem = saleItemData.UpdateSaleItem(existingSaleItem);
    var updatedSaleItemDTO = mapper.Map<SaleItemGetDTO>(updatedSaleItem);
    return Results.Ok(updatedSaleItemDTO);
}).RequireAuthorization();

app.MapDelete("api/uts/saleItems/{id}", (ISalesItem saleItemData, int id) =>
{
    saleItemData.DeleteSaleItem(id);
    return Results.NoContent();
}).RequireAuthorization();


//employee
app.MapGet("api/uts/employees", (IEmployee employeeData, IMapper mapper) =>
{
    var employees = employeeData.GetEmployees();
    var employeeDTOs = mapper.Map<List<EmployeeDTO>>(employees);
    return Results.Ok(employeeDTOs);
}).RequireAuthorization();

app.MapGet("api/uts/employees/{id}", (IEmployee employeeData, int id, IMapper mapper) =>
{
    var employee = employeeData.GetEmployeeById(id);
    if (employee == null)
    {
        return Results.NotFound($"Employee with ID {id} not found.");
    }
    var employeeDTO = mapper.Map<EmployeeDTO>(employee);
    return Results.Ok(employeeDTO);
}).RequireAuthorization();

app.MapPost("api/uts/employees", (IEmployee employeeData, EmployeeAddDTO employeeAddDTO, IMapper mapper) =>
{
    try
    {
        var employee = mapper.Map<Employee>(employeeAddDTO);
        var newEmployee = employeeData.AddEmployee(employee);
        var insertedEmployee = employeeData.GetEmployeeById(newEmployee.EmployeeId);

        var result = mapper.Map<EmployeeDTO>(insertedEmployee);
        return Results.Created($"/api/uts/employees/{result.EmployeeId}", result);
    }
    catch (System.Exception ex)
    {
        return Results.BadRequest($"Error adding employee: {ex.Message}");
    }
}).RequireAuthorization();

app.MapPut("api/uts/employees", (IEmployee employeeData, EmployeeDTO employeeDTO, IMapper mapper) =>
{
    var existingEmployee = employeeData.GetEmployeeById(employeeDTO.EmployeeId);
    if (existingEmployee == null)
    {
        return Results.NotFound($"Employee with ID {employeeDTO.EmployeeId} not found.");
    }

    existingEmployee.EmployeeName = employeeDTO.EmployeeName;
    existingEmployee.ContactNumber = employeeDTO.ContactNumber;
    existingEmployee.Email = employeeDTO.Email;
    existingEmployee.Position = employeeDTO.Position;


    var updatedEmployee = employeeData.UpdateEmployee(existingEmployee);
    var updatedEmployeeDTO = mapper.Map<EmployeeDTO>(updatedEmployee);
    return Results.Ok(updatedEmployeeDTO);
}).RequireAuthorization();

app.MapDelete("api/uts/employees/{id}", (IEmployee employeeData, int id) =>
{
    employeeData.DeleteEmployee(id);
    return Results.NoContent();
}).RequireAuthorization();


//view 
//viewproductwithcategory
// app.MapGet("api/uts/viewProductWithCategories", (IViewProductWithCategory viewProductWithCategoryData) =>
// {
//     var viewProductWithCategories = viewProductWithCategoryData.GetViewProductWithCategories();
//     return viewProductWithCategories;
// });


// app.MapGet("api/uts/viewProductWithCategories/{id}", (IViewProductWithCategory viewProductWithCategoryData, int id) =>
// {
//     var viewProductWithCategory = viewProductWithCategoryData.GetViewProductWithCategoryById(id);
//     return viewProductWithCategory;
// });


// // view sales product customer
// app.MapGet("api/uts/viewSalesProductCustomers", (IViewSalesProductCustomer viewSalesProductCustomerData) =>
// {
//     var viewSalesProductCustomers = viewSalesProductCustomerData.GetViewSalesProductCustomers();
//     return viewSalesProductCustomers;

// });


// app.MapGet("api/uts/ViewSalesProductCustomers/{id}", (IViewSalesProductCustomer viewSalesProductCustomerData, int id) =>
// {
//     var viewSalesProductCustomer = viewSalesProductCustomerData.GetViewSalesProductCustomerById(id);
//     return viewSalesProductCustomer;
// });

// // view sales product customer order by saleid vieww
// app.MapGet("api/uts/viewSalesProductCustomersOB", (IViewSalesProductCustomerOB viewSalesProductCustomerOBData) =>
// {
//     var viewSalesProductCustomersOB = viewSalesProductCustomerOBData.GetViewSalesProductCustomers();
//     return viewSalesProductCustomersOB;

// });


// get all sales
app.MapGet("api/uts/sales/details", (ISaless salesData, IMapper mapper) =>
{
    var sales = salesData.GetAllSalesWithDetails(); // Ambil data penjualan dari repository
    var salesDTOs = mapper.Map<IEnumerable<SalessDTO>>(sales); // Konversi ke DTO
    return Results.Ok(salesDTOs); // Kembalikan hasil sebagai respons
}).RequireAuthorization();


// detail sale
app.MapGet("api/uts/sales/details/{saleId}", (ISaless salesData, IMapper mapper, int saleId) =>
{
    var sale = salesData.GetSaleDetail(saleId);
    if (sale == null)
    {
        return Results.NotFound($"Sale with ID {saleId} not found.");
    }
    var saleDTO = mapper.Map<SalessDTO>(sale);
    return Results.Ok(saleDTO);
}).RequireAuthorization();



//member 
app.MapGet("api/uts/members", (IMember memberData, IMapper mapper) =>
{
    var members = memberData.GetMembers();
    var memberDTOs = mapper.Map<List<MemberDTO>>(members);
    return Results.Ok(memberDTOs);
}).RequireAuthorization();

app.MapGet("api/uts/members/{username}", (IMember memberData, string username, IMapper mapper) =>
{
    var member = memberData.GetMemberByUsername(username);
    if (member == null)
    {
        return Results.NotFound($"Member with username {username} not found.");
    }
    var memberDTO = mapper.Map<MemberDTO>(member);
    return Results.Ok(memberDTO);
}).RequireAuthorization();

app.MapPost("api/uts/members", (IMember memberData, RegisterDTO registerDTO, IMapper mapper) =>
{
    try
    {
        var newMember = memberData.RegisterMember(registerDTO);
        var insertedMember = memberData.GetMemberByUsername(newMember.Username);

        var result = mapper.Map<MemberDTO>(insertedMember);
        return Results.Created($"/api/uts/members/{result.Username}", result);
    }
    catch (System.Exception ex)
    {
        return Results.BadRequest($"Error adding member: {ex.Message}");
    }
});

app.MapPut("api/uts/members", (IMember memberData, MemberDTO memberDTO, IMapper mapper) =>
{
    var existingMember = memberData.GetMemberByUsername(memberDTO.Username);
    if (existingMember == null)
    {
        return Results.NotFound($"Member with username {memberDTO.Username} not found.");
    }
    existingMember.Email = memberDTO.Email;
    existingMember.Password = memberDTO.Password; // Assuming you handle password hashing in the EF implementation

    var updatedMember = memberData.UpdateMember(existingMember);
    var updatedMemberDTO = mapper.Map<MemberDTO>(updatedMember);
    return Results.Ok(updatedMemberDTO);
}).RequireAuthorization();

app.MapDelete("api/uts/members/{username}", (IMember memberData, string username) =>
{
    memberData.DeleteMember(username);
    return Results.NoContent();
}).RequireAuthorization();


// Login endpoint
app.MapPost("api/uts/members/login", (IMember memberData, LoginDTO loginDTO) =>
{
    var isValidUser = memberData.login(loginDTO.Username, loginDTO.Password);
    if (isValidUser)
    {
        // Generate JWT token
        var token = memberData.GenerateToken(loginDTO.Username);
        loginDTO.Password = string.Empty; // Clear password for security
        loginDTO.Token = token;
        return Results.Ok(loginDTO); // Return the login DTO with the token
    }
    return Results.BadRequest("Invalid username or password");
});


//reset password
app.MapPost("api/uts/members/resetPassword", (IMember memberData, ResetPasswordDTO resetPasswordDTO) =>
{
    if (memberData.resetPassword(resetPasswordDTO.Username, resetPasswordDTO.NewPassword))
    {
        return Results.Ok("Password reset successful");
    }
    return Results.NotFound("Member not found");
}).RequireAuthorization();

app.Run();

