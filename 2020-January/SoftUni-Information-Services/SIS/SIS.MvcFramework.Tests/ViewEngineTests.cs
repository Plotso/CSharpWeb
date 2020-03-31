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
            var actualResult = viewEngine.GetHtml(viewContent, viewModel, "dummyUser");

            actualResult.Should().Be(expectedResultContent);
        }
        
        [Fact]
        public void TestGetHtmlWithTemplateModel()
        {
            var viewModel = new List<int> { 1, 2, 3 };

            var viewContent = @"
@foreach (var num in Model)
{
<p>@num</p>
}";
            var expectedResultContent = @"
<p>1</p>
<p>2</p>
<p>3</p>
";

            var viewEngine = new ViewEngine();
            var actualResult = viewEngine.GetHtml(viewContent, viewModel, null);

            Assert.Equal(expectedResultContent, actualResult);
        }
    }
}