using EcommerceWebApi.DTOs;
using EcommerceWebApi.Interfaces;
using EcommerceWebApi.Models;
using EcommerceWebApi.Enums;

namespace EcommerceWebApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<User> _userRepository;
        private static readonly Dictionary<OrderStatus, OrderStatus[]> _allowedTransitions = new()
        {
            { OrderStatus.Pending,    new[] { OrderStatus.Processing } },
            { OrderStatus.Processing, new[] { OrderStatus.Shipped } },
            { OrderStatus.Shipped,    new[] { OrderStatus.Delivered } },
            { OrderStatus.Delivered,  Array.Empty<OrderStatus>() }
        };

        public OrderService(IGenericRepository<Order> orderRepo, IGenericRepository<Product> productRepo, IGenericRepository<User> userRepo)
        {
            _orderRepository = orderRepo;
            _productRepository = productRepo;
            _userRepository = userRepo;
        }
        public async Task<Order> CreateAsync(OrderCreateDto order)
        {
            if (order.UserId<=0)
            {
                throw new ArgumentException("Invalid user id.");
            }
            var user = await _userRepository.GetByIdAsync(order.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with id {order.UserId} not found.");
            }

            var newOrder = new Order
            {
                TotalAmount = 0,
                UserId = order.UserId,
                ShippingAddress = user.Address,
                OrderItems = new List<OrderItem>()
            };

            foreach (var item in order.Items)
            {
                if(item.Quantity <= 0)
                {
                    throw new ArgumentException("Invalid order item quantity.");
                }
                if(item.ProductId <= 0)
                {
                    throw new ArgumentException("Invalid order item product id.");
                }

                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if(product == null)
                {
                    throw new KeyNotFoundException($"Product with id {item.ProductId} not found.");
                }
                if (item.Quantity > product.Stock)
                {
                    throw new ArgumentException("Order quantity exceeds available stock.");
                }

                newOrder.TotalAmount += item.Quantity * product.Price;
                newOrder.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                });

                product.Stock -= item.Quantity;
                await _productRepository.UpdateAsync(product);
            }
            await _orderRepository.AddAsync(newOrder);
            await _orderRepository.SaveAsync();
            return newOrder;
        }
        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if(order == null) 
            {
                throw new KeyNotFoundException($"Order with id {id} not found.");
            }
            return order;
        }

        public async Task UpdateStatusAsync(int id, UpdateOrderStatusDto status)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            { 
                throw new KeyNotFoundException($"Order with id {id} not found.");
            }
            if (status.Status == OrderStatus.Cancelled) 
            {
                throw new ArgumentException($"Cancelling an order is not supported through this endpoint.");
            }
            if (!_allowedTransitions.TryGetValue(order.Status, out var allowed) || !allowed.Contains(status.Status))
            {
                throw new ArgumentException(
                    $"Invalid status transition from {order.Status} to {status.Status}. " +
                    $"Valid transitions from {order.Status}: {string.Join(", ", allowed ?? Array.Empty<OrderStatus>())}");
            }

            order.Status = status.Status;
            await _orderRepository.UpdateAsync(order);
            await _orderRepository.SaveAsync();
        }
    }
}
