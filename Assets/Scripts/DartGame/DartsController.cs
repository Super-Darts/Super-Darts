/*
 *  Author: Charlie O'Regan
 *  Usage: DartsController is used for interactions on the Darts objects
 */

using UnityEngine;
using UnityEngine.Events;

// TODO: Fully comment DartsController and enable/disable of trail renderer, maybe a whooshing noise for dart flying
namespace Assets.Scripts.DartGame
{
    public class DartsController : MonoBehaviour
    {
        [SerializeField] public UnityEvent enteredLaminarFlow;
        [SerializeField] public UnityEvent exitLaminarFlow;
        [SerializeField] public float laminarFlowThreshold = 0.25f;

        [HideInInspector] public DartGameManager gameManager;
        private Rigidbody _rb;
        private TrailRenderer _trailRenderer;

        private bool _lerp = false;
        private bool _enteredLaminarFlow = false;
        private bool _nextSpawned = false;

        private bool _thrown = false;

        /// <summary>
        /// This will run on the first frame the object is active in the game
        /// </summary>
        private void Start()
        {
            _rb = GetComponent<Rigidbody>();

            // Ignore own layers collision
            Physics.IgnoreLayerCollision(6, 6);
        }

        /// <summary>
        /// This will run every physics update of the game
        /// </summary>
        private void FixedUpdate()
        {
            if (_lerp)
            {
                if (_rb.velocity.magnitude < laminarFlowThreshold)
                {
                    _lerp = false;
                    _enteredLaminarFlow = false;
                    exitLaminarFlow?.Invoke();
                }
                else
                {
                    if (!_enteredLaminarFlow)
                    {
                        // This should invoke once.
                        enteredLaminarFlow?.Invoke();
                        _enteredLaminarFlow = true;
                    }
                }

                if (_enteredLaminarFlow)
                {
                    transform.rotation = Quaternion.LookRotation(_rb.velocity);
                }
            }
        }

        /// <summary>
        /// This is used to activate and deactivate the constraints of the rigidbody
        /// </summary>
        /// <param name="enable">what to change _lerp to</param>
        public void ToggleLerp(bool enable)
        {
            _lerp = enable;
            _thrown = enable;
        }

        public void TriggerSpawnDart()
        {
            if (_nextSpawned)
                return;

            gameManager.SpawnDart();
            _nextSpawned = true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_thrown)
                Destroy(gameObject);
        }
    }
}