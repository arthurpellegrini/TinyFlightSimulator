using System.Collections.Generic;
using UnityEngine;

public class CustomTimer : MonoBehaviour
{
    //static
    private static Dictionary<string, CustomTimer> dicoCustomTimers;

    public bool IsRunning { get; private set; }

    public float TimeScale { get; set; }

    public float DeltaTime => IsRunning ? UnityEngine.Time.deltaTime * TimeScale : 0;

    public float FixedDeltaTime => IsRunning ? UnityEngine.Time.fixedDeltaTime * TimeScale : 0;


    public float Time { get; private set; }

    private void Awake()
    {
        if (dicoCustomTimers == null)
        {
            dicoCustomTimers = new Dictionary<string, CustomTimer>();
            var customTimers = FindObjectsOfType<CustomTimer>();
            foreach (var item in customTimers) dicoCustomTimers.Add(item.name, item);
        }
    }

    public void Reset()
    {
        Time = 0;
    }

    public void Reset(bool startTimer)
    {
        Time = 0;
        IsRunning = startTimer;
    }


    private void Start()
    {
        Reset(false);
    }

    private void Update()
    {
        Time += DeltaTime;
    }

    public static CustomTimer GetCustomTimer(string name)
    {
        CustomTimer customTimer = null;
        dicoCustomTimers.TryGetValue(name, out customTimer);
        return customTimer;
    }

    public void StopTimer()
    {
        IsRunning = false;
    }

    public void StartTimer()
    {
        IsRunning = true;
    }

    public void ResetAndStart()
    {
        Reset(true);
    }

    public void ResetAndStop()
    {
        Reset(false);
    }
}