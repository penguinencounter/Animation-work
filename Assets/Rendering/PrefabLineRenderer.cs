using Containers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Rendering
{
    public class PrefabLineRenderer
    {
        private GameObject _prefab;
    
        private float _currentStrokeWidth;
        public float TargetStrokeWidth;
    
        private Vector2 _currentStart;
        public Vector2 TargetStart;
    
        private Vector2 _currentEnd;
        public Vector2 TargetEnd;

        public float Smoothness;

        private GameObject _containerObject;
        private GameObject _body;
        private GameObject _end1;
        private GameObject _end2;

        private readonly int _updateTicksPerSecond;

        private float _dt;

        public PrefabLineRenderer(GameObject prefab, Vector2 start, Vector2 end, 
            float strokeWidth=0.1f, float smoothness=1f, int updateTicksPerSecond=120)
        {
            _prefab = prefab;
            Smoothness = smoothness;
        
            _currentStrokeWidth = strokeWidth;
            TargetStrokeWidth = strokeWidth;
        
            _currentStart = start;
            TargetStart = start;
        
            _currentEnd = end;
            TargetEnd = end;

            this._updateTicksPerSecond = updateTicksPerSecond;

            Tick();
        }

        public void Destroy()
        {
            Object.Destroy(_containerObject);
            _containerObject = null;
        }

        public void Tick(bool force = false)
        {
            _dt += Time.deltaTime;
            var secondsPerTick = 1f / _updateTicksPerSecond;
            for (var iterCount = 0; iterCount < 10; iterCount++)
            {
                var iterations = (int) (_dt / secondsPerTick);
                if (!force && iterations == 0) return;

                // lerp
                _currentStrokeWidth = Mathf.Lerp(_currentStrokeWidth, TargetStrokeWidth, Smoothness);
                _currentStart = Vector2.Lerp(_currentStart, TargetStart, Smoothness);
                _currentEnd = Vector2.Lerp(_currentEnd, TargetEnd, Smoothness);

                // calculate position, scale, rotation
                var position = (_currentStart + _currentEnd) / 2f;
                var scale = new Vector2(Vector2.Distance(_currentStart, _currentEnd), _currentStrokeWidth);
                var rotation = Quaternion.FromToRotation(Vector2.right, _currentEnd - _currentStart);

                if (!_containerObject)
                {
                    _containerObject = Object.Instantiate(_prefab);
                    // This only happens *once*.
                    var containerData = _containerObject.GetComponent<LinePrefabContainer>();
                    _body = containerData.body;
                    _end1 = containerData.end1;
                    _end2 = containerData.end2;
                }
 
                var t = _containerObject.transform;
                var bt = _body.transform;
                t.position = position;
                t.rotation = rotation;
                bt.localScale = scale;

                var end1 = _end1.transform;
                var end2 = _end2.transform;
                end1.position = _currentStart;
                end2.position = _currentEnd;

                end1.localScale = new Vector2(_currentStrokeWidth, _currentStrokeWidth);
                end2.localScale = new Vector2(_currentStrokeWidth, _currentStrokeWidth);
                _dt -= secondsPerTick;
                _dt = Mathf.Max(_dt, 0);
                if (iterations <= 1) return;
                force = true;
            }
            Debug.LogWarning("Dropping frames!");
            _dt = 0;
        }

        public void SetPoints(Vector2 newStart, Vector2 newEnd)
        {
            TargetStart = newStart;
            TargetEnd = newEnd;
        }

        public void SkipSmoothing()
        {
            _currentStart = TargetStart;
            _currentEnd = TargetEnd;
            _currentStrokeWidth = TargetStrokeWidth;
            Tick(true);
        }
    }
}
