/*
 *  Author: Charlie O'Regan
 *  Usage: BalloonController is used for interactions on the Balloon objects
 */

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.DartGame
{
    public enum BalloonType
    {
        GREEN, YELLOW, RED, FAKEG, FAKEY, FAKER
    }

    // TODO: Comment BalloonController properly
    public class BalloonController : MonoBehaviour
    {

        [SerializeField] public UnityEvent onBalloonHit;

        [SerializeField] private float _respawnTimer = 5f;
        [SerializeField] private GameObject _spawnPrefab;
        [SerializeField] private ParticleSystem _particleSystem;
        private DartGameManager _gameManager;

        [SerializeField] private BalloonType _balloonType;

        private SphereCollider _collider;
        [SerializeField] private MeshRenderer _meshRenderer;

        private void Awake()
        {
            _gameManager = FindObjectOfType<DartGameManager>();
            _collider = GetComponent<SphereCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Dart"))
            {
                StartCoroutine(BalloonHit());
            }
        }

        /// <summary>
        /// This is used for when the Balloon is hit by a dart
        /// </summary>
        private IEnumerator BalloonHit()
        {
            // Disable components
            _collider.enabled = false;
            _meshRenderer.enabled = false;

            onBalloonHit?.Invoke();

            if (_gameManager)
            {
                _gameManager.AddScore(_balloonType);
                _gameManager.CheckPoints();
            }
            else
            {
#if DEBUG
                Debug.LogError($"The GameManager reference does not exist for object : {this}");
#endif
            }

            if (_particleSystem) // Lewis: Added if so script execution continues if not assigned
            {
                _particleSystem.Play();
            }

            yield return new WaitForSeconds(_respawnTimer);

            // Enable components
            _collider.enabled = true;
            _meshRenderer.enabled = true;
        }
    }
}