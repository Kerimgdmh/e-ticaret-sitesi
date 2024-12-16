public class CartItem
{
    public int Id { get; set; } // Bu, veritabanında otomatik olarak artan bir değer olmalıdır.
    public int ProductId { get; set; } // Bu alan, ürün tablosundaki ürünün ID'sini tutacak.
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public int Quantity { get; set; }
    public string UserId { get; set; }
}
