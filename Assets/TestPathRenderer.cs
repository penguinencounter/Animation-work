using System;
using System.Collections.Generic;
using System.Linq;
using Rendering;
using UnityEngine;
using UnityEngine.Serialization;

public class TestPathRenderer : MonoBehaviour
{
    private Vector2 _point1 = new(-2.5f, -2.5f);
    private Vector2 _point2 = new(2.5f, -2.5f);
    private Vector2 _anchor = new(0f, 5f);

    public GameObject LinePrefab;

    private int _state;

    private PathSegment _seg;
    private BatchLineRenderer _rend;

    // Start is called before the first frame update
    private void Start()
    {
        _seg = new PathSegment(_point1, _anchor, _point2);
        var lines = (from seg in _seg.GenerateSegments(20)
            select new PrefabLineRenderer(LinePrefab, seg.start, seg.end, 0.1f, 0.01f))
            .ToList();
        _rend = new BatchLineRenderer(lines);
        
        InvokeRepeating(nameof(SwitchLines), 0.5f, 0.5f);
    }

    private void SwitchLines()
    {
        
    }
    
    private void Update()
    {
        _rend.RenderAll();
    }
}

