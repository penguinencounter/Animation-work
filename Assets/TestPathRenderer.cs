using System;
using UnityEngine;
using UnityEngine.Serialization;

public class TestPathRenderer : MonoBehaviour
{
    private Vector2 point1 = new(-2.5f, -2.5f);
    private Vector2 point2 = new(2.5f, -2.5f);
    private Vector2 point3 = new(2.5f, 2.5f);
    private Vector2 point4 = new(-2.5f, 2.5f);

    public GameObject LinePrefab;

    private int _state;

    private PrefabLineRenderer _plr1;

    // Start is called before the first frame update
    private void Start()
    {
        _plr1 = new PrefabLineRenderer(LinePrefab, new Vector2(0, 0), new Vector2(0, 0), 0.1f, 0.02f);
        InvokeRepeating(nameof(SwitchLines), 0.5f, 0.5f);
    }

    private void SwitchLines()
    {
        Vector2[] seq =
        {
            point1,
            point2,
            point3,
            point4
        };
        var firstPoint = seq[_state % seq.Length];
        var secondPoint = seq[(_state + 1) % seq.Length];
        _plr1.SetPoints(firstPoint, secondPoint);
        _state++;
    }
    
    private void Update()
    {
        _plr1.Tick();
    }
}

