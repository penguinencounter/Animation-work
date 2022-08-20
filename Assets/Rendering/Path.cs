using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering
{
    /**
     * A path is a list of points that define a polyline.
     *
     * Additionally, a path can have bezier curves, which will be rendered with PathSegment. 
     */
    [Serializable]
    public class Path
    {
        public IList<PathSegment> Segments;

        public IList<PathSegment> Slice(int segPerCurve)
        {
            var stitch = new List<PathSegment>();
            foreach (var s in Segments)
            {
                stitch.AddRange(s.GenerateSegments(segPerCurve));
            }

            return stitch;
        }

        public void SliceInPlace(int segPerCurve)
        {
            Segments = Slice(segPerCurve);
        }

        public IList<PrefabLineRenderer> Render(GameObject prefab, IList<PathSegment> segments = null,
            float strokeWidth = 0.1f, float smoothness = 1f)
        {
            segments ??= Segments;
            return PathSegment.GeneratePrefabs(segments, prefab, strokeWidth, smoothness);
        }
    }
}