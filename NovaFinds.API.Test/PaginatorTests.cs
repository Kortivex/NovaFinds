namespace NovaFinds.API.Test
{
    using Filters;

    [TestFixture]
    public class PaginatorTests
    {
        [Test]
        public void Paginator_WithTotalItemsLessThanPageSize_ShouldSetTotalPagesToOne()
        {
            var paginator = new Paginator(5, 1, 10);
            Assert.That(paginator.TotalPages, Is.EqualTo(1));
        }

        [Test]
        public void Paginator_WithTotalItemsMoreThanPageSize_ShouldCalculateTotalPagesCorrectly()
        {
            var paginator = new Paginator(15, 1, 10);
            Assert.That(paginator.TotalPages, Is.EqualTo(2));
        }

        [Test]
        public void Paginator_WithCurrentPageLessThanOne_ShouldSetCurrentPageToOne()
        {
            var paginator = new Paginator(15, 0, 10);
            Assert.That(paginator.CurrentPage, Is.EqualTo(1));
        }

        [Test]
        public void Paginator_WithCurrentPageMoreThanTotalPages_ShouldSetCurrentPageToLastPage()
        {
            var paginator = new Paginator(15, 3, 10);
            Assert.That(paginator.CurrentPage, Is.EqualTo(2));
        }

        [Test]
        public void Paginator_WithValidParameters_ShouldCalculateStartAndEndIndexCorrectly()
        {
            var paginator = new Paginator(25, 2, 10);
            Assert.Multiple(() => {
                Assert.That(paginator.StartIndex, Is.EqualTo(10));
                Assert.That(paginator.EndIndex, Is.EqualTo(19));
            });
        }

        [Test]
        public void Paginator_WithMaxPagesLessThanTotalPages_ShouldLimitPagesToMaxPages()
        {
            var paginator = new Paginator(100, 5, 10, 5);
            Assert.That(paginator.Pages.Count(), Is.EqualTo(5));
        }

        [Test]
        public void Paginator_WhenCurrentPageIsInTheMiddle_ShouldSetStartAndEndPageCorrectly()
        {
            var paginator = new Paginator(100, 5, 10, 5);
            Assert.Multiple(() => {
                Assert.That(paginator.StartPage, Is.EqualTo(3));
                Assert.That(paginator.EndPage, Is.EqualTo(7));
            });
        }

        [Test]
        public void Paginator_WhenCurrentPageIsNearStart_ShouldSetStartPageToOne()
        {
            var paginator = new Paginator(100, 2, 10, 5);
            Assert.That(paginator.StartPage, Is.EqualTo(1));
        }

        [Test]
        public void Paginator_WhenCurrentPageIsNearEnd_ShouldSetEndPageToTotalPages()
        {
            var paginator = new Paginator(100, 10, 10, 5);
            Assert.That(paginator.EndPage, Is.EqualTo(10));
        }
    }
}