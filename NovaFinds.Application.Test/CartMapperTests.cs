namespace NovaFinds.Application.Test
{
    using CORE.Domain;
    using DTOs;
    using Mappers;

    [TestFixture]
    public class CartMapperTests
    {
        [Test]
        public void ToDomain_ConvertsCartToCartDto_Correctly()
        {
            var cart = new Cart { Id = 1, UserName = "TestUser", Date = DateTime.Now };

            var result = CartMapper.ToDomain(cart);

            Assert.Multiple(() => {
                Assert.That(result, Is.Not.Null);
                Assert.That(result!.Id, Is.EqualTo(cart.Id));
                Assert.That(result.UserName, Is.EqualTo(cart.UserName));
                Assert.That(result.Date, Is.EqualTo(cart.Date));
            });
        }

        [Test]
        public void ToListDomain_ConvertsListOfCartsToListOfCartDtos_Correctly()
        {
            var carts = new List<Cart>
            {
                new() { Id = 1, UserName = "User1", Date = DateTime.Now },
                new() { Id = 2, UserName = "User2", Date = DateTime.Now }
            };

            var result = CartMapper.ToListDomain(carts);

            Assert.Multiple(() => {
                Assert.That(result.Count(), Is.EqualTo(carts.Count));
                Assert.That(result.First().UserName, Is.EqualTo(carts.First().UserName));
            });
        }

        [Test]
        public void ToDb_ConvertsCartDtoToCart_Correctly()
        {
            var cartDto = new CartDto { UserId = 1, UserName = "TestUser", Date = DateTime.Now };

            var result = CartMapper.ToDb(cartDto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.UserId, Is.EqualTo(cartDto.UserId));
        }

        [Test]
        public void ToListDb_ConvertsListOfCartDtosToListOfCarts_Correctly()
        {
            var cartDtos = new List<CartDto>
            {
                new() { UserId = 1, UserName = "User1", Date = DateTime.Now },
                new() { UserId = 2, UserName = "User2", Date = DateTime.Now }
            };

            var result = CartMapper.ToListDb(cartDtos);

            Assert.Multiple(() => {
                Assert.That(result.Count(), Is.EqualTo(cartDtos.Count));
                Assert.That(result.First().UserId, Is.EqualTo(cartDtos.First().UserId));
            });
        }
    }
}