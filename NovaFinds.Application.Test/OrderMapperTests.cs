namespace NovaFinds.Application.Test
{
    using CORE.Domain;
    using DTOs;
    using Mappers;
    using OrderStatusType=CORE.Enums.OrderStatusType;

    [TestFixture]
    public class OrderMapperTests
    {
        [Test]
        public void ToDomain_ConvertsOrderToOrderDtoCorrectly()
        {
            var order = new Order { Id = 1, Date = DateTime.Today, Status = OrderStatusType.Delivered, UserId = 1 };
            var orderDto = OrderMapper.ToDomain(order);

            Assert.Multiple(() => {
                Assert.That(orderDto!.Id, Is.EqualTo(order.Id));
                Assert.That(orderDto.Date, Is.EqualTo(order.Date));
                Assert.That((OrderStatusType)orderDto.Status, Is.EqualTo(order.Status));
                Assert.That(orderDto.UserId, Is.EqualTo(order.UserId));
            });
        }

        [Test]
        public void ToListDomain_ConvertsListOfOrdersToListOfOrderDtosCorrectly()
        {
            var orders = new List<Order>
            {
                new() { Id = 1, Date = DateTime.Today, Status = OrderStatusType.Delivered, UserId = 1 },
                new() { Id = 2, Date = DateTime.Today, Status = OrderStatusType.Pending, UserId = 2 }
            };

            var orderDtos = OrderMapper.ToListDomain(orders).ToList();

            Assert.Multiple(() => {
                Assert.That(orderDtos, Has.Count.EqualTo(2));
                Assert.That(orderDtos[0].Id, Is.EqualTo(orders[0].Id));
                Assert.That((OrderStatusType)orderDtos[1].Status, Is.EqualTo(orders[1].Status));
            });
        }

        [Test]
        public void ToDb_ConvertsOrderDtoToOrderCorrectlyWithDefaultDate()
        {
            var orderDto = new OrderDto { Id = 1, Date = DateTime.Today, Status = DTOs.OrderStatusType.Delivered, UserId = 1 };
            var order = OrderMapper.ToDb(orderDto);

            Assert.Multiple(() => {
                Assert.That(order!.Date.Date, Is.EqualTo(DateTime.Now.Date));
                Assert.That(order.Status, Is.EqualTo((OrderStatusType)orderDto.Status));
            });
        }

        [Test]
        public void ToDb_InvalidOrderStatus_SetsStatusToPending()
        {
            var orderDto = new OrderDto { Status = (DTOs.OrderStatusType)999 };
            var order = OrderMapper.ToDb(orderDto);

            Assert.That(order!.Status, Is.EqualTo(OrderStatusType.Pending));
        }

        [Test]
        public void ToListDb_ConvertsListOfOrderDtosToListOfOrdersWithCorrectStatuses()
        {
            var orderDtos = new List<OrderDto>
            {
                new() { Status = DTOs.OrderStatusType.Delivered },
                new() { Status = DTOs.OrderStatusType.Pending }
            };

            var orders = OrderMapper.ToListDb(orderDtos).ToList();

            Assert.Multiple(() => {
                Assert.That(orders, Has.Count.EqualTo(2));
                Assert.That(orders[0].Status, Is.EqualTo(OrderStatusType.Delivered));
                Assert.That(orders[1].Status, Is.EqualTo(OrderStatusType.Pending));
            });
        }
    }
}