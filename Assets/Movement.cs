using UnityEngine;

namespace Assets
{
    public class Movement : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
            Debug.Log(TaskScheduler.Inst == null ? "oh no!" : "Task scheduler exists");
            void TestTask(TaskScheduler.Task that)
            {
                transform.Translate(0, -0.01f, 0);
                TaskScheduler.Inst.RescheduleFuture(2, TaskScheduler.TimeUnit.Frames, that);
            }

            if (TaskScheduler.Inst != null) TaskScheduler.Inst.Future(1, TaskScheduler.TimeUnit.Seconds, TestTask);
        }

        // Update is called once per frame
        private void Update()
        {
        
        }
    }
}
