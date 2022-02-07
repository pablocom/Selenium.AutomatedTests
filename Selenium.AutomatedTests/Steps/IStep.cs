namespace Selenium.AutomatedTests.Steps
{
    public interface IStep
    {
        bool HasFailed { get; }
        string Description { get; }
        string Result { get; }

        void Execute();
        void Fail(string message);
    }
}
