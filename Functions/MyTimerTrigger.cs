using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Functions;

public class MyTimerTrigger(ILoggerFactory loggerFactory) : BaseFunctionActivityManager
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<MyTimerTrigger>();

    [Function("MyTimerTrigger")]
    public void Run([TimerTrigger("*/10 * * * * *")] TimerInfo myTimer, FunctionContext fc)
    {
        using var activity = StartFunctionActivity(fc, nameof(MyTimerTrigger));

        try
        {
            // Do some work here
            using (StartActivity("Activity 1"))
            {
                _logger.LogInformation("Task 1 ...: {Time}", DateTime.UtcNow);
            }
            
            using (StartActivity("Activity 2"))
            {
                _logger.LogInformation("Task 2 ...: {Time}", DateTime.UtcNow);
               
                // Activity 2 can have nested activities
                using (StartActivity("Activity 3"))
                {
                    throw new Exception("Error: This is a test exception in Activity 3");
                    _logger.LogInformation("Task 3 ...: {Time}", DateTime.UtcNow);
                }

            }
            
            FinishFunctionActivity(activity);
        }
        catch (Exception e)
        {
            FinishFunctionActivity(activity, failure: true);
            _logger.LogError(e, "error occurred while executing the timer function");
        }
    }
}