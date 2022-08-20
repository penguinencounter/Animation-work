using System;
using System.Collections.Generic;

namespace Rendering
{
    public class BatchLineRenderer
    {
        public readonly IList<PrefabLineRenderer> Renderers;
        
        public BatchLineRenderer()
        {
            Renderers = new List<PrefabLineRenderer>();
        }

        public BatchLineRenderer(IList<PrefabLineRenderer> renderers)
        {
            Renderers = renderers;
        }

        public void RenderAll()
        {
            foreach (var renderer in Renderers)
            {
                renderer.Tick();
            }
        }
        
        public void ForEach(Action<PrefabLineRenderer> action)
        {
            foreach (var renderer in Renderers)
            {
                action(renderer);
            }
        }

        public void UpdateExisting(IEnumerable<PrefabLineRenderer> lines2)
        {
            var i = 0;
            foreach (var line in lines2)
            {
                var existing = Renderers[i];
                if (existing != null)
                {
                    existing.Smoothness = line.Smoothness;
                    existing.TargetEnd = line.TargetEnd;
                    existing.TargetStart = line.TargetStart;
                    existing.TargetStrokeWidth = line.TargetStrokeWidth;
                }

                line.Destroy();
                i++;
            }
        }
    }
}