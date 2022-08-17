using System;
using System.Collections.Generic;
using UnityEngine;

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
    }
}