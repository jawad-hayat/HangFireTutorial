namespace HangFireTutorial.Jobs
{
    public class TestJob
    {
        public readonly ILogger _logger;
        public TestJob(ILogger<TestJob> logger) => _logger = logger;

        public void WriteLog(string logMessage)
        {
            _logger.LogInformation($"{DateTime.Now} {logMessage}");
        }
    }
}
