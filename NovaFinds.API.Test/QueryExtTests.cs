namespace NovaFinds.API.Test
{
    using Filters;

    [TestFixture]
    public class QueryExtTests
    {
        private IQueryable<TestEntity> _testData;

        [SetUp]
        public void Setup()
        {
            _testData = new List<TestEntity>
            {
                new() { Id = 1 },
                new() { Id = 2 },
                new() { Id = 3 },
                new() { Id = 4 },
                new() { Id = 5 }
            }.AsQueryable();
        }

        [Test]
        public void GetPaged_WithValidPageAndSize_ReturnsCorrectItems()
        {
            var result = _testData.GetPaged(1, 2).ToList();
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.Multiple(() => {
                Assert.That(result[0].Id, Is.EqualTo(1));
                Assert.That(result[1].Id, Is.EqualTo(2));
            });
        }

        [Test]
        public void GetPaged_WithSecondPage_ReturnsNextItems()
        {
            var result = _testData.GetPaged(2, 2).ToList();
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.Multiple(() => {
                Assert.That(result[0].Id, Is.EqualTo(3));
                Assert.That(result[1].Id, Is.EqualTo(4));
            });
        }

        [Test]
        public void GetPaged_WithSizeGreaterThanCollection_ReturnsAllItems()
        {
            var result = _testData.GetPaged(1, 10).ToList();
            Assert.That(result, Has.Count.EqualTo(5));
        }

        [Test]
        public void GetPaged_WithZeroPage_ReturnsFirstPage()
        {
            var result = _testData.GetPaged(0, 2).ToList();
            Assert.That(result, Has.Count.EqualTo(2));
        }

        [Test]
        public void GetPaged_WithPageBeyondRange_ReturnsEmpty()
        {
            var result = _testData.GetPaged(10, 2).ToList();
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetPaged_WithValidPageAndExactSize_ReturnsCorrectItems()
        {
            var result = _testData.GetPaged(2, 5).ToList();
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetPaged_WithSingleItemPerPage_ReturnsCorrectItemForEachPage()
        {
            var firstPageItem = _testData.GetPaged(1, 1).Single();
            var secondPageItem = _testData.GetPaged(2, 1).Single();

            Assert.Multiple(() => {
                Assert.That(firstPageItem.Id, Is.EqualTo(1));
                Assert.That(secondPageItem.Id, Is.EqualTo(2));
            });
        }

        private class TestEntity
        {
            public int Id { get; init; }
        }
    }
}