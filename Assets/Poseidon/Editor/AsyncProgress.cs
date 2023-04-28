#if UNITY_2020_1_OR_NEWER
#define SUPPORTS_PROGRESS_BAR
#elif UNITY_2018_4_OR_NEWER
#define REFLECTION_PROGRESS
#endif

using System.Linq;
using Cinderflame.Poseidon;
using UnityEditor;
using UnityEngine;
#if REFLECTION_PROGRESS
using System.Reflection;
#endif

/// <summary>
/// When using our new asynchronous stage runner
/// we may want ways to actually see the progress
/// that don't involve showing a Blocking Progress bar.
/// However, different Unity platforms include different 
/// types of progress bars... so we need to keep this 
/// out of the DLLs and somewhere we can use #if's for Unity version
/// </summary>
[InitializeOnLoad]
public class AsyncProgress : IProgressReactor
{
    private const string LABEL = "Poseidon Operation";
    private static AsyncProgress _instance;
    private static void RegisterInstance()
    {
        _instance = new AsyncProgress();
        AsyncProgressor.Register(_instance);
    }
    
#if SUPPORTS_PROGRESS_BAR
// In 2020.1 Unity has exposed the proper Background Progress Meter
// We will add a lot more functionality to it in the future, but for
// now it is a general "what's the progress of the current operation"

    static AsyncProgress() => RegisterInstance();

    private int currentProgressId = -1;
    public void OnOperationStarted()
    {
        if (currentProgressId != -1)
        {
            Progress.Finish(currentProgressId, Progress.Status.Canceled);
        }
        
        currentProgressId = Progress.Start(LABEL);
        Progress.RegisterCancelCallback(currentProgressId, Cancel);
    }

    private bool Cancel()
    {
        // This will have unexpected consequences.
        // Use with caution. You may have to force rebuild all the objects afterwards.
        Zeus.Cancel(CancellationSource.ProgressBar);
        return true;
    }

    public void OnOperationEnded()
    {
        if (currentProgressId == -1) return;
        Progress.Finish(currentProgressId, Progress.Status.Succeeded);
        currentProgressId = -1;
    }

    // This is triggered if an operation is canceled through a difference source
    public void OnOperationCanceled()
    {
        if (currentProgressId == -1) return;
        Progress.Finish(currentProgressId, Progress.Status.Canceled);
    }

    public void OnProgressUpdated(float progress)
    {
        if (currentProgressId == -1) return;
        Progress.Report(currentProgressId, progress);
    }
    
#elif REFLECTION_PROGRESS
    // This is a much uglier way of doing things...
    // but in earlier versions of Unity we didn't 
    // have proper progress bar access, but this 
    // can be accessed using a Reflection.
    private static MethodInfo display = null;
    private static MethodInfo clear = null;
    
    static AsyncProgress()
    {
        var editorAssembly = typeof(Editor).Assembly;
        var progressBarType = editorAssembly.GetTypes().FirstOrDefault(t => t.Name == "AsyncProgressBar");
        if (progressBarType == null)
        {
            // Running a version of Unity that doesn't seem to support async progress.
            Debug.LogWarning("AsyncProgressBar does not exist anymore - please turn off asynchronous progress.\n If this annoys you, feel free to comment it out.");
            return;
        }

        display = progressBarType.GetMethod("Display");
        clear = progressBarType.GetMethod("Clear");

        if (clear == null || display == null)
        {
            Debug.LogWarning("AsyncProgressBar doesn't support the correct methods in the current version of Unity.\n Comment out this line if this annoys you.");
            return;
        }
        
        AsyncProgressor.Register(new AsyncProgress());
    }

    private void Clear()
    {
        clear.Invoke(null, new object[] { });
    }

    private void Display(float progress)
    {
        display.Invoke(null, new object[] {LABEL, progress});
    }

    public void OnOperationCanceled() => Clear();
    public void OnOperationStarted() => Display(0);
    public void OnOperationEnded() => Clear();
    public void OnProgressUpdated(float progress)
    {
        if(progress < 0.01f) Clear();
        else Display(progress);   
    }
    
    
#endif
}
