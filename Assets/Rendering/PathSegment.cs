using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rendering
{
    [Serializable]
    public class PathSegment
    {
        public Vector2 start;
        public bool hasHandle;
        public Vector2 handle;
        public Vector2 end;

        public PathSegment(Vector2 start, Vector2 end, bool hasHandle = false, Vector2 handle = default)
        {
            this.start = start;
            this.hasHandle = hasHandle;
            this.handle = handle;
            this.end = end;
        }

        public PathSegment(Vector2 start, Vector2 end)
        {
            this.start = start;
            hasHandle = false;
            handle = new Vector2(0, 0);
            this.end = end;
        }

        public static PathSegment Line(Vector2 start, Vector2 end)
        {
            return new PathSegment(start, end);
        }

        public IList<PathSegment> GenerateSegments(int segments)
        {
            if (!hasHandle)
            {
                return new[] {this};
            }

            var result1 = new List<PathSegment>();
            var result2 = new List<PathSegment>();
            for (var i = 0; i < segments; i++)
            {
                var t = (float) i / (segments - 1);
                var linePoint1 = Vector2.Lerp(start, handle, t);
                var linePoint2 = Vector2.Lerp(handle, end, t);
                result1.Add(Line(linePoint1, linePoint2));
            }

            // trim down lines to only relevant parts of the curve
            for (var i = 0; i < result1.Count; i++)
            {
                var previous = i == 0 ? null : result1[i - 1];
                var current = result1[i];
                var next = i == result1.Count - 1 ? null : result1[i + 1];
                var current1 = current.start;
                var current2 = current.end;

                if (previous != null)
                {
                    // determine point of intersection from endpoint pairs
                    var previous1 = previous.start;
                    var previous2 = previous.end;

                    if (LineUtil.IntersectLineSegments2D(previous1, previous2, current1, current2,
                            out var intersection))
                    {
                        current1 = intersection;
                    }
                }

                if (next != null)
                {
                    // determine point of intersection from endpoint pairs
                    var next1 = next.start;
                    var next2 = next.end;

                    if (LineUtil.IntersectLineSegments2D(next1, next2, current1, current2, out var intersection))
                    {
                        current2 = intersection;
                    }
                }

                result2.Add(Line(current1, current2));
            }

            return result2;
        }

        public static IList<PrefabLineRenderer> GeneratePrefabs(IEnumerable<PathSegment> segments, GameObject prefab,
            float strokeWidth = 0.1f, float smoothness = 1.0f)
        {
            return (from seg in segments
                    select new PrefabLineRenderer(prefab, seg.start, seg.end, strokeWidth, smoothness))
                .ToList();
        }
    }
}