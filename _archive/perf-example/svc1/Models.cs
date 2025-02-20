namespace Models;

public record Order(int? OrderNumber, DateTime? OrderDate, DateTime? RequiredDate, DateTime? ShippedDate, string? Status, string? comments, int? CustomerNumber);