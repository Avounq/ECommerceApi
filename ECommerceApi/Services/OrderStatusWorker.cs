using ECommerceApi.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Services
{
    public class OrderStatusWorker : BackgroundService
    {
        private static readonly Dictionary<string, string> NextStatuses = new()
        {
            [OrderStatuses.Preparing] = OrderStatuses.Shipped,
            [OrderStatuses.Shipped] = OrderStatuses.OutForDelivery,
            [OrderStatuses.OutForDelivery] = OrderStatuses.Delivered
        };

        private static readonly string[] TrackableStatuses =
        [
            OrderStatuses.Preparing,
            OrderStatuses.Shipped,
            OrderStatuses.OutForDelivery
        ];

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<OrderStatusWorker> _logger;
        private readonly TimeSpan _checkInterval;
        private readonly TimeSpan _statusChangeDelay;

        public OrderStatusWorker(
            IServiceScopeFactory scopeFactory,
            ILogger<OrderStatusWorker> logger,
            IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _checkInterval = TimeSpan.FromSeconds(
                configuration.GetValue("OrderStatusWorker:CheckIntervalSeconds", 60));
            _statusChangeDelay = TimeSpan.FromSeconds(
                configuration.GetValue("OrderStatusWorker:StatusChangeDelaySeconds", 120));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "OrderStatusWorker started. Check interval: {CheckIntervalSeconds} seconds, status change delay: {StatusChangeDelaySeconds} seconds",
                _checkInterval.TotalSeconds,
                _statusChangeDelay.TotalSeconds);

            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateOrderStatusesAsync(stoppingToken);
                await Task.Delay(_checkInterval, stoppingToken);
            }
        }

        private async Task UpdateOrderStatusesAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var now = DateTime.UtcNow;
                var threshold = now.Subtract(_statusChangeDelay);

                var orders = await dbContext.Orders
                    .Where(order =>
                        TrackableStatuses.Contains(order.Status) &&
                        order.StatusUpdatedAt <= threshold)
                    .ToListAsync(cancellationToken);

                if (orders.Count == 0)
                {
                    return;
                }

                foreach (var order in orders)
                {
                    var oldStatus = order.Status;
                    order.Status = NextStatuses[order.Status];
                    order.StatusUpdatedAt = now;

                    _logger.LogInformation(
                        "Order #{OrderId} status updated: {OldStatus} -> {NewStatus}",
                        order.Id,
                        oldStatus,
                        order.Status);
                }

                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "OrderStatusWorker failed.");
            }
        }
    }
}
