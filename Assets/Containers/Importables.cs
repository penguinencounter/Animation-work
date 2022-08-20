using System;
using System.Collections.Generic;
using System.Linq;
using Rendering;
using UnityEngine;

namespace Containers
{
    public class Importables
    {
        public interface IData<out T>
        {
            public bool Validate();
            public T Convert();  // Required for ducking and stuff
        }
        
        public static bool AllValid(IEnumerable<IData<object>> toValidate)
        {
            return toValidate.All(data => data.Validate());
        }
        
        [Serializable]
        public class Vf2 : IData<Vector2>
        {
            public List<float> data;

            public bool Validate()
            {
                return data.Count <= 2;
            }

            public Vector2 Convert()
            {
                return new Vector2(data.Count>=1?data[0]:0f, data.Count>=2?data[1]:0f);
            }
        }

        [Serializable]
        public class SPathSegment : IData<PathSegment>
        {
            public Vf2 start;
            public Vf2 handle;
            public Vf2 end;
            
            public bool Validate()
            {
                return start.Validate() && end.Validate();
            }

            public PathSegment Convert()
            {
                return handle != null 
                    ? new PathSegment(start.Convert(), end.Convert(), true, handle.Convert()) 
                    : PathSegment.Line(start.Convert(), end.Convert());
            }
        }

        [Serializable]
        public class SPathSegments : IData<IList<PathSegment>>
        {
            public List<SPathSegment> children;
            
            public bool Validate()
            {
                return AllValid(children);
            }

            public IList<PathSegment> Convert()
            {
                return (from child in children select child.Convert()).ToList();
            }
        }
    }
}