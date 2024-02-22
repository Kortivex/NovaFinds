namespace NovaFinds.Application.Test
{
    using CORE.Domain;
    using Mappers;

    [TestFixture]
    public class CategoryMapperTests
    {
        [Test]
        public void ToDomain_ConvertsCategoryToCategoryDto()
        {
            var category = new Category { Id = 1, Name = "Electronics" };
            var result = CategoryMapper.ToDomain(category);

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() => {
                Assert.That(result!.Id, Is.EqualTo(category.Id));
                Assert.That(result.Name, Is.EqualTo(category.Name));
            });
        }

        [Test]
        public void ToListDomain_ConvertsListOfCategoryToListOfCategoryDto()
        {
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Books" }
            };

            var results = CategoryMapper.ToListDomain(categories).ToList();

            Assert.That(results.Count, Is.EqualTo(2));
            Assert.That(results[0]!.Name, Is.EqualTo("Electronics"));
            Assert.That(results[1]!.Name, Is.EqualTo("Books"));
        }

        [Test]
        public void ToListDomain_EmptyList_ReturnsEmptyList()
        {
            var results = CategoryMapper.ToListDomain(new List<Category>()).ToList();
            Assert.That(results.Count, Is.EqualTo(0));
        }

        [Test]
        public void ToListDomain_ListContainsCategoryWithEmptyName_NameIsEmptyInDto()
        {
            var categories = new List<Category> { new Category { Id = 1, Name = "" } };
            var results = CategoryMapper.ToListDomain(categories).ToList();
            Assert.That(results[0]!.Name, Is.EqualTo(""));
        }
    }
}