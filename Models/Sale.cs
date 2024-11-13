using examen2doP.Models;

public partial class Sale
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public DateTime Date { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public virtual Product? Product { get; set; }  // Hacerlo opcional
}
