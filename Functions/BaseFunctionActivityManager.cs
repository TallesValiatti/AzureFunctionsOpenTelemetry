using System.Diagnostics;
using Microsoft.Azure.Functions.Worker;

namespace Functions;

public abstract class BaseFunctionActivityManager
{
    protected static Activity? StartFunctionActivity(FunctionContext fc, string functionName)
    {
        var manualInstrumentationSource = Activity.Current?.Source;
        
        if (manualInstrumentationSource == null)
        {
            return null;
        }
        
        var activity = manualInstrumentationSource.StartActivity(functionName);
        activity?.AddTag("invocation_id", fc.InvocationId);
        activity?.AddTag("created_at", DateTime.UtcNow.ToString("o"));
        return activity;
    }
    
    protected static Activity? StartActivity(string name)
    {
        var manualInstrumentationSource = Activity.Current?.Source;
        
        if (manualInstrumentationSource == null)
        {
            return null;
        }
        
        var activity = manualInstrumentationSource.StartActivity(name);
        return activity;
    }
    
    protected static void FinishFunctionActivity(Activity? activity, bool failure = false)
    {
        activity?.AddTag("completed_at", DateTime.UtcNow.ToString("o"));
        activity?.AddTag("status", failure ? "failure" : "success");
        activity?.SetStatus(failure ? ActivityStatusCode.Error : ActivityStatusCode.Ok);
    }
}