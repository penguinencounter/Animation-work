using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(TaskScheduler.inst == null ? "oh no!" : "ok we good");
        static void testTask(TaskScheduler.Task that)
        {
            Debug.Log("tick" + that.time);
            TaskScheduler.inst.RescheduleFuture(0.1, TaskScheduler.TimeUnit.SECONDS, that);
        }
        TaskScheduler.inst.Future(1, TaskScheduler.TimeUnit.SECONDS, testTask);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
