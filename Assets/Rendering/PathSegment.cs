using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering
{
    [Serializable]
    public class PathSegment
    {
        public Vector2 start;
        public Vector2 handle;
        public Vector2 end;

        public PathSegment(Vector2 start, Vector2 handle, Vector2 end)
        {
            this.start = start;
            this.handle = handle;
            this.end = end;
        }

        public static PathSegment Line(Vector2 start, Vector2 end)
        {
            return new PathSegment(start, Vector2.Lerp(start, end, 0.5f), end);
        }

        public IList<PathSegment> GenerateSegments(int segments)
        {
            var result = new List<PathSegment>();
            for (var i = 0; i < segments; i++)
            {
                var t = (float) i / (segments - 1);
                var nextT = t + 1f / (segments - 1);
                var linePoint1 = Vector2.Lerp(start, handle, t);
                var linePoint2 = Vector2.Lerp(handle, end, t);
                var subPoint1 = Vector2.Lerp(linePoint1, linePoint2, t);
                var subPoint2 = Vector2.Lerp(linePoint1, linePoint2, nextT);
                
                result.Add(Line(subPoint1, subPoint2));
            }

            return result;
        }
    }
}