using System;
using System.Collections.Generic;
using System.Linq;
using Rendering;
using UnityEngine;
using UnityEngine.Serialization;

public class TestPathRenderer : MonoBehaviour
{
    private Vector2 _point1 = new(-5, 0f);
    private Vector2 _point2 = new(0f, 0f);
    private Vector2 _point3 = new(5f, 0f);
    private Vector2 _anchor1A = new(-2.5f, 2.5f);
    private Vector2 _anchor1B = new(-2.5f, -2.5f);
    private Vector2 _anchor2A = new(2.5f, -2.5f);
    private Vector2 _anchor2B = new(2.5f, 2.5f);

    public GameObject LinePrefab;

    private bool _state;

    private PathSegment _seg;
    private BatchLineRenderer _rend;
    private PathSegment _seg2;
    private BatchLineRenderer _rend2;

    // Start is called before the first frame update
    private void Start()
    {
        _seg = new PathSegment(_point1, _point2, true, _anchor1A);
        _seg2 = new PathSegment(_point2, _point3, true, _anchor2A);
        var lines1 = PathSegment.GeneratePrefabs(_seg.GenerateSegments(20), LinePrefab,
            0.1f, 0.1f);
        var lines2 = PathSegment.GeneratePrefabs(_seg2.GenerateSegments(20), LinePrefab,
            0.1f, 0.1f);
        _rend = new BatchLineRenderer(lines1);
        _rend2 = new BatchLineRenderer(lines2);
        
        InvokeRepeating(nameof(SwitchLines), 0.5f, 0.5f);
    }

    private void SwitchLines()
    {
        _seg.handle = _state?_anchor1A:_anchor1B;
        _seg2.handle = _state?_anchor2A:_anchor2B;
        var lines1 = PathSegment.GeneratePrefabs(_seg.GenerateSegments(20), LinePrefab, 0.1f, 0.1f);
        var lines2 = PathSegment.GeneratePrefabs(_seg2.GenerateSegments(20), LinePrefab, 0.1f, 0.1f);
        
        _rend.UpdateExisting(lines1);
        _rend2.UpdateExisting(lines2);
        
        _state = !_state;
    }
    
    private void Update()
    {
        _rend.RenderAll();
        _rend2.RenderAll();
    }
}

