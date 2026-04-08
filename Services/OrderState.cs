using Bookstore.Models;

namespace Bookstore.Services;

public class OrderState
{
    public Order Order { get; private set; } = new();

    public event Action? OnChange;

    public OrderState()
    {
        Order.OrderItems = new List<OrderItem>();
    }

    public IReadOnlyList<OrderItem> Items => Order.OrderItems;

    public int TotalQuantity => Order.OrderItems.Sum(item => item.Quantity);

    public decimal TotalPrice => Order.OrderItems.Sum(item => item.GetTotal());

    public void AddBook(Book book, int quantity = 1)
    {
        var existingItem = Order.OrderItems.FirstOrDefault(item => item.Book?.Id == book.Id);

        if (existingItem is null)
        {
            Order.OrderItems.Add(new OrderItem
            {
                Book = book,
                Quantity = quantity
            });
        }
        else
        {
            existingItem.Quantity += quantity;
        }

        NotifyStateChanged();
    }

    public void AddItem(OrderItem item)
    {
        AddBook(item.Book, item.Quantity);
    }

    public void RemoveItem(int bookId)
    {
        var item = Order.OrderItems.FirstOrDefault(i => i.Book?.Id == bookId);

        if (item is not null)
        {
            Order.OrderItems.Remove(item);
            NotifyStateChanged();
        }
    }

    public void IncreaseQuantity(int bookId)
    {
        var item = Order.OrderItems.FirstOrDefault(i => i.Book?.Id == bookId);

        if (item is not null)
        {
            item.Quantity++;
            NotifyStateChanged();
        }
    }

    public void DecreaseQuantity(int bookId)
    {
        var item = Order.OrderItems.FirstOrDefault(i => i.Book?.Id == bookId);

        if (item is null)
        {
            return;
        }

        item.Quantity--;

        if (item.Quantity <= 0)
        {
            Order.OrderItems.Remove(item);
        }

        NotifyStateChanged();
    }

    public void ResetOrder()
    {
        Order = new Order
        {
            OrderItems = new List<OrderItem>()
        };
        NotifyStateChanged();
    }

    private void NotifyStateChanged()
    {
        OnChange?.Invoke();
    }
}
