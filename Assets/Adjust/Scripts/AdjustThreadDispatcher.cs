using System;
using System.Collections.Generic;
using UnityEngine;

public class AdjustThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<Action> executionQueue = new Queue<Action>();
    private static AdjustThreadDispatcher instance;

    public static void RunOnMainThread(Action action)
    {
        if (action == null)
        {
            return;
        }

        lock (executionQueue)
        {
            executionQueue.Enqueue(action);
        }
    }

    private void Update()
    {
        while (executionQueue.Count > 0)
        {
            Action action;
            lock (executionQueue)
            {
                action = executionQueue.Dequeue();
            }
            if (action != null)
            {
                action.Invoke();
            }
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        if (instance == null)
        {
            GameObject obj = new GameObject("AdjustThreadDispatcher");
            instance = obj.AddComponent<AdjustThreadDispatcher>();
            DontDestroyOnLoad(obj);
        }
    }
}