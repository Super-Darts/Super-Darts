using Assets.Scripts.Extensions;
using Assets.Scripts.Tweens;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.ObjectPathing
{
    public class ObjectPathEntity : MonoBehaviour
    {
        /// <summary>
        /// The database of paths that this object can follow.
        /// </summary>
        [field: SerializeField] public ObjectPathDatabase PathDatabase { get; set; }

        [field: SerializeField] public bool Global { get; set; }

        /// <summary>
        /// The current path the object is following.
        /// </summary>
        private ObjectPath _path;

        /// <summary>
        /// The current node in the path the object is following.
        /// </summary>
        private Vector3 Target
        {
            get => _target;
            set
            {
                _target = value;
                _OnNewPoint?.Invoke();
            }
        }

        private Vector3 _target;

        private Vector3 _localZero;

        /// <summary>
        /// The current path index the object is following.
        /// </summary>
        private int _pathIndex;

        /// <summary>
        /// Cached Component for translation.
        /// </summary>
        private DoTranslation _doTranslation;

        /// <summary>
        /// Flag if the object is currently moving.
        /// </summary>

        private delegate void PathChanged();

        private event PathChanged _OnNewPoint;

        private event PathChanged _OnFirstPoint;

        private event PathChanged _OnLastPoint;

        private void Awake()
        {
            _CacheVariables();
            _InitializeEvents();
        }

        private void Start() => _InitializeNewPath();

        private void Update() => _UpdateMovement();

        /// <summary>
        /// Debugging Gizmos for Collision Areas & Targets.
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(this.transform.position, 0.5f);
            Gizmos.DrawWireSphere(_target, 1);
            Gizmos.DrawLine(transform.position, _target);
        }

        private void _UpdateMovement()
        {
            // Returns if the path is null or if the path is empty.
            if (_path == null || _path.Path.Count <= 0)
            {
                return;
            }

            // Check if this object is within range of the target.
            if (Vector3.Distance(transform.position, _target) < 0.0001f)
            {
                // Check if the next incrementation is within the range of the path.
                if (_pathIndex < _path.Path.Count - 1)
                {
                    // Increment the path index.
                    _pathIndex++;
                    // Get the next node.
                    Target = _path.Path[_pathIndex] + (Global ? Vector3.zero : _localZero);

                    // Trigger that we are not moving anymore.
                }
                else
                {
                    _OnLastPoint?.Invoke();
                }
            }
        }

        private void _BeginMoving()
        {
            _doTranslation.Stop();

            // Scaled to frame time.
            float randomSpeed = Random.Range(PathDatabase.SpeedRange.Min, PathDatabase.SpeedRange.Max);

            Vector3 distanceDifference = _target - transform.position;

            // How many frames at X distance does it take to finish
            float duration = distanceDifference.magnitude / randomSpeed;

            _doTranslation.Duration = duration;
            // Activate the movement. ( set & forget )
            _doTranslation.Play(_target);
        }

        private IEnumerator _InitializePath(ObjectPath path)
        {
            float timeToWait = Random.Range(PathDatabase.SwitchRateRange.Min, PathDatabase.SwitchRateRange.Max);

            _path = null;

            yield return new WaitForSeconds(timeToWait);

            _pathIndex = 0;
            // Set the target to the first node in the path if it is not null. Otherwise set it to (0,0,0).
            if (path.Path.Count > 0)
            {
                _path = path;
                Target = path.Path[0] + (Global ? Vector3.zero : _localZero);
                _OnFirstPoint?.Invoke();
            }
            else
            {
                Target = Global ? Vector3.zero : _localZero;
            }
        }

        private void _InitializeNewPath()
        {
            if (_path && _path.Loop)
            {
                StartCoroutine(_InitializePath(_path));
                return;
            }

            ObjectPath tempPath = PathDatabase.Paths.RandomElement();
            if (tempPath)
            {
                StartCoroutine(_InitializePath(tempPath));
            }
        }

        private void _InitializeEvents()
        {
            _OnNewPoint += _BeginMoving;
            _OnFirstPoint += () => { Debug.Log("First Point!"); };
            _OnLastPoint += _InitializeNewPath;
        }

        private void _CacheVariables()
        {
            // Get or Add the DoTranslation component.
            _doTranslation = gameObject.GetOrAddComponent<DoTranslation>();

            _localZero = transform.position;
        }
    }
}