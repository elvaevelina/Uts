using UtsRestfullAPI.Data;
using UtsRestfullAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IProduct, ProductADO>();
builder.Services.AddSingleton<ICategory, CategoryADO>();
builder.Services.AddSingleton<ICustomer, CustomerADO>();
builder.Services.AddSingleton<ISales, SalesADO>();
builder.Services.AddSingleton<ISalesItem, SalesItemADO>();
builder.Services.AddSingleton<IEmployee, EmployeeADO>();
builder.Services.AddSingleton<IViewProductWithCategory, ViewProductWithCategoryADO>();
builder.Services.AddSingleton<IViewSalesProductCustomer, ViewSalesProductCustomerADO>();
builder.Services.AddSingleton<IViewSalesProductCustomerOB, ViewSalesProductCustomerOBADO>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

//product
app.MapGet("api/uts/products",(IProduct productData)=>
{
    var products = productData.GetProducts();
    return products;
});

app.MapGet("api/uts/products/{id}", (IProduct productData, int id) =>
{
    var product = productData.GetProductById(id);
    return product;
});

app.MapPost("api/uts/products", (IProduct productData, Product product) =>
{
    var newProduct = productData.AddProduct(product);
    return newProduct;
});

app.MapPut("api/uts/products", (IProduct productData, Product product) =>
{
    var updatedProduct = productData.UpdateProduct(product);
    return updatedProduct;
});

app.MapDelete("api/uts/products/{id}", (IProduct productData, int id) =>
{
    productData.DeleteProduct(id);
    return Results.NoContent();
});

//category
app.MapGet("api/uts/categories", (ICategory categoryData) =>
{
    var categories = categoryData.GetCategories();
    return categories;
});
app.MapGet("api/uts/categories/{id}", (ICategory categoryData, int id) =>
{
    var category = categoryData.GetCategoryById(id);
    return category;
});
app.MapPost("api/uts/categories", (ICategory categoryData, Category category) =>
{
    var newCategory = categoryData.AddCategory(category);
    return newCategory;
});
app.MapPut("api/uts/categories", (ICategory categoryData, Category category) =>
{
    var updatedCategory = categoryData.UpdateCategory(category);
    return updatedCategory;
});
app.MapDelete("api/uts/categories/{id}", (ICategory categoryData, int id) =>
{
    categoryData.DeleteCategory(id);
    return Results.NoContent();
});

//customer
app.MapGet("api/uts/customers", (ICustomer customerData) =>
{
    var customers = customerData.GetCustomers();
    return customers;
});
app.MapGet("api/uts/customers/{id}", (ICustomer customerData, int id) =>
{
    var customer = customerData.GetCustomerById(id);
    return customer;
});
app.MapPost("api/uts/customers", (ICustomer customerData, Customer customer) =>
{
    var newCustomer = customerData.AddCustomer(customer);
    return newCustomer;
});
app.MapPut("api/uts/customers", (ICustomer customerData, Customer customer) =>
{
    var updatedCustomer = customerData.UpdateCustomer(customer);
    return updatedCustomer;
});
app.MapDelete("api/uts/customers/{id}", (ICustomer customerData, int id) =>
{
    customerData.DeleteCustomer(id);
    return Results.NoContent();
});

//sales
app.MapGet("api/uts/sales", (ISales salesData) =>
{
    var sales = salesData.GetSales();
    return sales;
});
app.MapGet("api/uts/sales/{id}", (ISales salesData, int id) =>
{
    var sale = salesData.GetSalesById(id);
    return sale;
});
app.MapPost("api/uts/sales", (ISales salesData, Sales sales) =>
{
    var newSale = salesData.AddSales(sales);
    return newSale;
});
app.MapPut("api/uts/sales", (ISales salesData, Sales sales) =>
{
    var updatedSale = salesData.UpdateSales(sales);
    return updatedSale;
});
app.MapDelete("api/uts/sales/{id}", (ISales salesData, int id) =>
{
    salesData.DeleteSales(id);
    return Results.NoContent();
});

//SaleItem
app.MapGet("api/uts/saleItems", (ISalesItem saleItemData) =>
{
    var saleItems = saleItemData.GetSaleItems();
    return saleItems;
});
app.MapGet("api/uts/saleItems/{id}", (ISalesItem saleItemData, int id) =>
{
    var saleItem = saleItemData.GetSaleItemById(id);
    return saleItem;
});
app.MapPost("api/uts/saleItems", (ISalesItem saleItemData, SaleItem saleItem) =>
{
    var newSaleItem = saleItemData.AddSaleItem(saleItem);
    return newSaleItem;
});
app.MapPut("api/uts/saleItems", (ISalesItem saleItemData, SaleItem saleItem) =>
{
    var updatedSaleItem = saleItemData.UpdateSaleItem(saleItem);
    return updatedSaleItem;
});
app.MapDelete("api/uts/saleItems/{id}", (ISalesItem saleItemData, int id) =>
{
    saleItemData.DeleteSaleItem(id);
    return Results.NoContent();
});

//employee
app.MapGet("api/uts/employees", (IEmployee employeeData) =>
{
    var employees = employeeData.GetEmployees();
    return employees;
});
app.MapGet("api/uts/employees/{id}", (IEmployee employeeData, int id) =>
{
    var employees = employeeData.GetEmployeeById(id);
    return employees;
});
app.MapPost("api/uts/employees", (IEmployee employeeData, Employee employee) =>
{
    var newEmployee = employeeData.AddEmployee(employee);
    return newEmployee;
});
app.MapPut("api/uts/employees", (IEmployee employeeData, Employee employee) =>
{
    var updatedEmployee = employeeData.UpdateEmployee(employee);
    return updatedEmployee;
});
app.MapDelete("api/uts/employees/{id}", (IEmployee employeeData, int id) =>
{
    employeeData.DeleteEmployee(id);
    return Results.NoContent();
});

//viewproductwithcategory
app.MapGet("api/uts/viewProductWithCategories", (IViewProductWithCategory viewProductWithCategoryData) =>
{
    var viewProductWithCategories = viewProductWithCategoryData.GetViewProductWithCategories();
    return viewProductWithCategories;
});


app.MapGet("api/uts/viewProductWithCategories/{id}", (IViewProductWithCategory viewProductWithCategoryData, int id) =>
{
    var viewProductWithCategory = viewProductWithCategoryData.GetViewProductWithCategoryById(id);
    return viewProductWithCategory;
});


// view sales product customer
app.MapGet("api/uts/viewSalesProductCustomers", (IViewSalesProductCustomer viewSalesProductCustomerData) =>
{
    var viewSalesProductCustomers = viewSalesProductCustomerData.GetViewSalesProductCustomers();
    return viewSalesProductCustomers;

});


app.MapGet("api/uts/ViewSalesProductCustomers/{id}", (IViewSalesProductCustomer viewSalesProductCustomerData, int id) =>
{
    var viewSalesProductCustomer = viewSalesProductCustomerData.GetViewSalesProductCustomerById(id);
    return viewSalesProductCustomer;
});

// view sales product customer order by saleid
app.MapGet("api/uts/viewSalesProductCustomersOB", (IViewSalesProductCustomerOB viewSalesProductCustomerOBData) =>
{
    var viewSalesProductCustomersOB = viewSalesProductCustomerOBData.GetViewSalesProductCustomers();
    return viewSalesProductCustomersOB;

});
app.Run();

