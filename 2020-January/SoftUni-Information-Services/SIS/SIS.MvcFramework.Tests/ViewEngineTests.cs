namespace SIS.MvcFramework.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using FluentAssertions;
    using Xunit;
    public class ViewEngineTests
    {
        [Theory]
        [InlineData("OnlyHtmlView")]
        [InlineData("ForForeachIfView")]
        [InlineData("ViewModelView")]
        public void TestGetHtml(string testName)
        {
            var viewModel = new TestViewModel()
            {
                Name = "TestUser",
                Year = 2020,
                Numbers = new List<int> { 1, 10, 100, 1000, 10000 },
            };
            
            var viewContent = File.ReadAllText($"ViewTests/{testName}.html");
            var expectedResultContent = File.ReadAllText($"ViewTests/{testName}.Expected.html");
            
            IViewEngine viewEngine = new ViewEngine();
            var actualResult = viewEngine.GetHtml(viewContent, viewModel);

            actualResult.Should().Be(expectedResultContent);
        }
    }
}