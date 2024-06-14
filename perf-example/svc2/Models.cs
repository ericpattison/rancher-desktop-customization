using System.ComponentModel.DataAnnotations.Schema;

namespace classicmodels;

[Table("productlines")]
public record ProductLines(string ProductLine, string Description);

[Table("products")]
public record Products(string ProductCode, string ProductName, string ProductLine, string ProductScale, string ProductVendor, string QuantityInStock, string BuyPrice, string MSRP);

[Table("orders")]
public record Orders(int? OrderNumber, DateTime? OrderDate, DateTime? RequiredDate, DateTime? ShippedDate, string? Status, string? comments, int? CustomerNumber);

[Table("orderdetails")]
public record OrderDetails(string OrderNumber, string ProductCode, string QuantityOrdered, string PriceEach, string OrderLineNumber);

[Table("customers")]
public record Customers(string CustomerNumber, string CustomerName, string ContactLastName, string ContactFirstName, string Phone, string AddressLine1, string AddressLine2, string City, string State, string PostalCode, string Country, string SalesRepEmployeeNumber, string CreditLimit);

[Table("employees")]
public record Employees(string EmployeeNumber, string LastName, string FirstName, string Extension, string Email, string OfficeCode, string ReportsTo, string JobTitle);

[Table("offices")]
public record Offices(string OfficeCode, string City, string Phone, string AddressLine1, string AddressLine2, string State, string Country, string PostalCode, string Territory);