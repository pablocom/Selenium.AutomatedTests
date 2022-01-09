using Xunit;
using OpenQA.Selenium;
using Selenium.AutomatedTests.Core.Steps;
using NSubstitute;

namespace Selenium.AutomatedTests.Core.UnitTests;

public class AutomationScenarioBuilderTests
{
    private IWebDriver _webDriver;
    private AutomationScenarioBuilder _automationScenarioBuilder;

    public AutomationScenarioBuilderTests()
    {
        _webDriver = Substitute.For<IWebDriver>();
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
        AssumeStepsInScenario(Substitute.For<IStep>());

        var testReport = _automationScenarioBuilder.BuildAndRun();

        Assert.False(testReport.HasFailure);
    }

    [Fact]
    public void Runs_with_two_steps_ok_and_generates_summary()
    {
        var firstStep = Substitute.For<IStep>();
        var secondStep = Substitute.For<IStep>();
        firstStep.Description.Returns("First step description");
        secondStep.Description.Returns("Second step description");        
        AssumeStepsInScenario(firstStep, secondStep);
        
        var testReport = _automationScenarioBuilder.BuildAndRun();

        var expectedSummary = $"\n\nStep 1: First step description : \n\n" +
                                  $"Step 2: Second step description : ";
        Assert.False(testReport.HasFailure);
        Assert.Equal(expectedSummary, testReport.GetSummary());
    }

    [Fact]
    public void Executes_steps_in_order()
    {
        var firstStep = Substitute.For<IStep>();
        var secondStep = Substitute.For<IStep>();
        AssumeStepsInScenario(firstStep, secondStep);

        var testReport = _automationScenarioBuilder.BuildAndRun();

        Assert.False(testReport.HasFailure);
        Received.InOrder(() =>
        {
            firstStep.Execute(_webDriver);
            secondStep.Execute(_webDriver);
        });
    }

    [Fact]
    public void Stops_scenario_execution_if_exception_is_thrown()
    {
        var firstStep = Substitute.For<IStep>();
        firstStep.Description.Returns("First step description");
        var secondStep = new FakeFailureStep("Failure step");
        var thirdStep = Substitute.For<IStep>();
        AssumeStepsInScenario(firstStep, secondStep, thirdStep);

        var testReport = _automationScenarioBuilder.BuildAndRun();
        
        var expectedSummaryStart = $"\n\nStep 1: First step description : \n\n" +
                                       $"Step 2: Failure step";
        Assert.True(testReport.HasFailure);
        Assert.Contains(expectedSummaryStart, testReport.GetSummary());
        thirdStep.DidNotReceive().Execute(_webDriver);
    }

    private void AssumeStepsInScenario(params IStep[] steps)
    {
        foreach (var step in steps) 
            _automationScenarioBuilder.WithStep(step);
    }
}