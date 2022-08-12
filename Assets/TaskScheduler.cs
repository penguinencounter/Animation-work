using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskScheduler : MonoBehaviour
{
    public static TaskScheduler inst = null;  // for use in Start() behaviors, set this script's execution order to -1 or less (before others use it!) -thx PenguinEncounter
    public enum TimeUnit
    {
        FRAMES,
        SECONDS
    }
    public class Task
    {
        public readonly double time;
        public readonly TimeUnit unit;
        public readonly Action<TaskScheduler.Task> action;

        public Task(double time, TimeUnit unit, Action<TaskScheduler.Task> action)
        {
            this.time = time;
            this.unit = unit;
            this.action = action;
        }

        public Task Copy()
        {
            return new Task(time, unit, action);
        }

        public Task SetTime(double newTime)
        {
            return new Task(newTime, unit, action);
        }

        public Task SetUnit(TimeUnit newUnit)
        {
            return new Task(time, newUnit, action);
        }

        public Task SetAction(Action<TaskScheduler.Task> newAction)
        {
            return new Task(time, unit, newAction);
        }
    }
    public IList<Task> schedule = new List<Task>();

    public double lastTime = -1;  // allow tasks with t=0 to run immediately
    public int lastFrame = -1;

    TaskScheduler() : base()
    {
        if (inst != null) Debug.LogWarning("TaskScheduler: Multiple initializations!!");
        inst = this;  // preinit
    }

    // Start is called before the first frame update
    void Start()
    {
        if (this != inst)
        {
            Debug.LogError("Pre-init TaskScheduler doesn't match Start() call!!!!");
        }
    }

    public void Future(double timeAhead, TimeUnit unit, Action<Task> action)
    {
        double targetTime;
        if (unit == TimeUnit.FRAMES) targetTime = lastFrame;
        else if (unit == TimeUnit.SECONDS) targetTime = lastTime;
        else targetTime = 0;
        targetTime += timeAhead;
        schedule.Add(new Task(targetTime, unit, action));
        Debug.Log("future: " + schedule.Count + " tasks (add, t=" + targetTime + " " + unit + ")");
    }

    public void RescheduleFuture(double timeAhead, TimeUnit unit, Task old)
    {
        Future(timeAhead, unit, old.action);
    }

    // Update is called once per frame
    void Update()
    {
        int currentFrame = Time.frameCount;
        float currentTime = Time.time;
        Debug.Log(schedule.Count + " task(s) in queue");
        IList<Task> toRemove = new List<Task>();
        foreach (Task task in schedule)
        {
            if (task.unit == TimeUnit.FRAMES)
            {
                if (currentFrame >= task.time)
                {
                    task.action.Invoke(task);
                    toRemove.Add(task);
                }
            }
            else if (task.unit == TimeUnit.SECONDS)
            {
                if (currentTime >= task.time)
                {
                    task.action.Invoke(task);
                    toRemove.Add(task);
                }
            }
        }

        foreach (Task remove in toRemove)
        {
            schedule.Remove(remove);
        }

        lastFrame = currentFrame;
        lastTime = currentTime;
    }
}
