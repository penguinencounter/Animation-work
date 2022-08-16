using TMPro;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private bool _go;
    private bool _collided;

    private Rigidbody2D _rigidbody2D;

    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        Debug.Log(TaskScheduler.Inst == null ? "oh no!" : "Task scheduler exists");
        TaskScheduler.Inst.Future(2, TaskScheduler.TimeUnit.Seconds, _ =>
        {
            Debug.Log("Started!");
            _rigidbody2D.constraints = RigidbodyConstraints2D.None;
            _rigidbody2D.AddForce(Vector2.zero);
        });
    }

    // Update is called once per frame
    private void Update()
    {
    }
}