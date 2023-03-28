using UnityEngine;

public class Logger
{
    /// <summary>
    /// Toggle for enabling/disabling global logging.
    /// </summary>
    public bool EnableLogging = true;

    public string Prefix;

    public void Log(string msg)
    {
        #if DEBUG
        if (EnableLogging)
            Debug.Log($"[{Prefix}] {msg}");
        #endif
    }    
}
