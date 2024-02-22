namespace NovaFinds.MVC.Test
{
    using Utils;

    [TestFixture]
    public class UrlTests
    {
        [Test]
        public void CheckLastSlashEmptyTest()
        {
            var exceptionResult = Assert.Throws<ArgumentNullException>(() => Url.CheckLastSlash(string.Empty));
            Assert.That(exceptionResult, Is.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void CheckLastSlashOneSlashTest()
        {
            var path = "/";
            var result = Url.CheckLastSlash(path);
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void CheckLastSlashUriWithoutSlashTest()
        {
            var path = "/Uri/To/Test";
            var result = Url.CheckLastSlash(path);
            Assert.That(result, Is.EqualTo("/Uri/To/Test"));
        }

        [Test]
        public void CheckLastSlashUriWithSlashTest()
        {
            var path = "/Uri/To/Test/";
            var result = Url.CheckLastSlash(path);
            Assert.That(result, Is.EqualTo("/Uri/To/Test"));
        }

        [Test]
        public void CheckLastSlashUrlWithoutSlashTest()
        {
            var path = "example.com/Uri/To/Test";
            var result = Url.CheckLastSlash(path);
            Assert.That(result, Is.EqualTo("example.com/Uri/To/Test"));
        }

        [Test]
        public void CheckLastSlashUrlWithSlashTest()
        {
            var path = "example.com/Uri/To/Test/";
            var result = Url.CheckLastSlash(path);
            Assert.That("example.com/Uri/To/Test", Is.EqualTo(result));
        }
        
        [Test]
        public void ReplaceElementInUrlWithOnlyOneInQueryTest()
        {
            var path = "example.com/Uri/To/Test?page=0";
            var element = "page";
            var replacement = "100";
            var patternElement = "([0-9])*";

            var result = Url.CheckElementInQueryString(path, element, $"{element}={patternElement}", replacement);

            Assert.That(result, Is.EqualTo("example.com/Uri/To/Test?page=100"));
        }
        
        [Test]
        public void ReplaceElementInUrlWithTwoInQueryTest()
        {
            var path = "example.com/Uri/To/Test?page=0&sort=asc";
            var element = "page";
            var replacement = "100";
            var patternElement = "([0-9])*";

            var result = Url.CheckElementInQueryString(path, element, $"{element}={patternElement}", replacement);

            Assert.That(result, Is.EqualTo("example.com/Uri/To/Test?page=100&sort=asc"));
        }
    }
}