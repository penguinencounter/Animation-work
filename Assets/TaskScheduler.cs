using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class TaskScheduler : MonoBehaviour
    {
        public static TaskScheduler Inst;  // for use in Start() behaviors, set this script's execution order to -1 or less (before others use it!) -thx PenguinEncounter
        public enum TimeUnit
        {
            Frames,
            Seconds
        }
        public class Task
        {
            public readonly double Time;
            public readonly TimeUnit Unit;
            public readonly Action<Task> Action;

            public Task(double time, TimeUnit unit, Action<Task> action)
            {
                Time = time;
                Unit = unit;
                Action = action;
            }

            public Task Copy()
            {
                return new Task(Time, Unit, Action);
            }

            public Task SetTime(double newTime)
            {
                return new Task(newTime, Unit, Action);
            }

            public Task SetUnit(TimeUnit newUnit)
            {
                return new Task(Time, newUnit, Action);
            }

            public Task SetAction(Action<Task> newAction)
            {
                return new Task(Time, Unit, newAction);
            }
        }
        public IList<Task> Schedule = new List<Task>();

        public double LastTime = -1;  // allow tasks with t=0 to run immediately
        public int LastFrame = -1;

        public TaskScheduler()
        {
            if (Inst != null) Debug.LogWarning("TaskScheduler: Multiple initializations!!");
            Inst = this;  // preinit
        }

        // Start is called before the first frame update
        private void Start()
        {
            if (this != Inst)
            {
                Debug.LogError("Pre-init TaskScheduler doesn't match Start() call!!!!");
            }
        }

        public void Future(double timeAhead, TimeUnit unit, Action<Task> action)
        {
            var targetTime = unit switch
            {
                TimeUnit.Frames => LastFrame,
                TimeUnit.Seconds => LastTime,
                _ => 0
            };
            targetTime += timeAhead;
            Schedule.Add(new Task(targetTime, unit, action));
        }

        public void RescheduleFuture(double timeAhead, TimeUnit unit, Task old)
        {
            Future(timeAhead, unit, old.Action);
        }

        // Update is called once per frame
        private void Update()
        {
            var currentFrame = Time.frameCount;
            var currentTime = Time.time;
            IList<Task> toRemove = new List<Task>();
            foreach (var task in new List<Task>(Schedule))
            {
                switch (task.Unit)
                {
                    case TimeUnit.Frames:
                    {
                        if (currentFrame >= task.Time)
                        {
                            task.Action.Invoke(task);
                            toRemove.Add(task);
                        }

                        break;
                    }
                    case TimeUnit.Seconds:
                    {
                        if (currentTime >= task.Time)
                        {
                            task.Action.Invoke(task);
                            toRemove.Add(task);
                        }

                        break;
                    }
                }
            }

            foreach (var remove in toRemove)
            {
                Schedule.Remove(remove);
            }

            LastFrame = currentFrame;
            LastTime = currentTime;
        }
    }
}
