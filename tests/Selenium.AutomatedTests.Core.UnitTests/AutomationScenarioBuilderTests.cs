using Xunit;
using OpenQA.Selenium;
using Moq;
using Selenium.AutomatedTests.Core.Steps;

namespace Selenium.AutomatedTests.Core.UnitTests;

public class AutomationScenarioBuilderTests
{
    private IWebDriver _webDriver;
    private AutomationScenarioBuilder _automationScenarioBuilder;

    public AutomationScenarioBuilderTests()
    {
        _webDriver = Mock.Of<IWebDriver>();
        _automationScenarioBuilder = new AutomationScenarioBuilder(_webDriver);
    }

    [Fact]
    public void Runs_with_no_steps_just_ok()
    {
        var testReport = _automationScenarioBuilder.BuildAndRun();

        Assert.False(testReport.HasFailure);
    }

    [Fact]
    public void Runs_with_single_step_ok()
    {
        var step = Mock.Of<IStep>(s => s.Description == "Step description");
        _automationScenarioBuilder.WithStep(step);

        var testReport = _automationScenarioBuilder.BuildAndRun();

        var expectedSummary = $"\n\nStep 1: Step description : ";
        Assert.False(testReport.HasFailure);
        Assert.Equal(expectedSummary, testReport.GetSummary());
    }

    [Fact]
    public void Runs_with_two_steps_ok()
    {
        var firstStep = Mock.Of<IStep>(s => s.Description == "First step description");
        var secondStep = Mock.Of<IStep>(s => s.Description == "Second step description");
        _automationScenarioBuilder.WithStep(firstStep);
        _automationScenarioBuilder.WithStep(secondStep);

        var testReport = _automationScenarioBuilder.BuildAndRun();

        var expectedSummary = $"\n\nStep 1: First step description : \n\n" +
                                  $"Step 2: Second step description : ";
        Assert.False(testReport.HasFailure);
        Assert.Equal(expectedSummary, testReport.GetSummary());
    }

    [Fact]
    public void Executes_steps_in_order()
    {
        Assert.False(true);
    }
}
