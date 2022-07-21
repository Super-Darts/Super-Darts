/*
 *  Author: Charlie O'Regan
 *  Usage: DartGameManager is used to control the scoring and respawning of balloons and darts in the dart game
 */

using UnityEngine;
using UnityEngine.Events;

// TODO: Comment DartGameManager
namespace Assets.Scripts.DartGame
{
    public class DartGameManager : MonoBehaviour
    {
        [System.Serializable]
        public struct Level
        {
            public bool completed;
            public float time;
            public int pointsNeeded;
            public int currentPoints;
            public int lives;

            public GameObject[] balloons;
        }

        [System.Serializable]
        public struct BalloonPoints
        {
            public int greenBalloonPoints;
            public int yellowBalloonPoints;
            public int redBalloonPoints;
            public int fakeGreenBalloonPoints;
            public int fakeYellowBalloonPoints;
            public int fakeRedBalloonPoints;
        }

        [SerializeField] private float timer;
        private bool startTimer;

        public int totalScore;
        [SerializeField] public UnityEvent onGameStart;
        [SerializeField] public UnityEvent onGameWin;
        [SerializeField] public UnityEvent onGameLose;

        [SerializeField] private Transform _dartSpawner;
        [SerializeField] private GameObject _dart;

        [SerializeField] private BalloonPoints _balloonPoints;

        [SerializeField] private Level _level;


        // Lewis - add unity events to use in editor (mainly for UI)
        [SerializeField] private UnityEvent<int> _onScoreChanged;

        [SerializeField] private UnityEvent<Level> _onLevelChanged;
        [SerializeField] private UnityEvent<bool> _onGameEnded; // boolean win state

        /// <summary>
        /// This will spawn the dart at the given transform
        /// </summary>
        public void SpawnDart()
        {
            GameObject dart = Instantiate(_dart, _dartSpawner.transform.position, _dartSpawner.rotation);
            dart.GetComponent<DartsController>().gameManager = this;
        }

        private void Start()
        {
            SpawnDart();
            StartLevel();
        }

        private void Update()
        {
            if (startTimer)
            {
                timer += Time.deltaTime;

                if (timer > _level.time)
                {
                    _level.lives--;
                    startTimer = false;

                    RestartCurrentLevel();

                    if (_level.lives <= 0)
                    {
                        Lose();
                    }
                }
            }
        }

        /// <summary>
        /// This will start a level given a certain index in range
        /// </summary>
        private void StartLevel()
        {
            _onLevelChanged.Invoke(_level); // Lewis - event for when level changes

            foreach (GameObject balloon in _level.balloons)
            {
                balloon.SetActive(true);
            }

            startTimer = true;
            timer = 0;
        }

        /// <summary>
        /// This will restart the current level the player is on
        /// </summary>
        public void RestartCurrentLevel()
        {
            totalScore -= _level.currentPoints;
            _level.currentPoints = 0;
            _onScoreChanged.Invoke(totalScore); // Lewis - event for when score changes

            StartLevel();
        }

        /// <summary>
        /// This will check the points against the needed points to proceed to the next level
        /// </summary>
        public void CheckPoints()
        {
            // if win
            if (_level.currentPoints >= _level.pointsNeeded)
            {
                _level.completed = true;

                foreach (GameObject balloon in _level.balloons)
                {
                    balloon.SetActive(false);
                }

                Win();
                startTimer = false;
            }
        }

        /// <summary>
        /// This will add score to the currentPoints and total score
        /// </summary>
        /// <param name="type"></param>
        public void AddScore(BalloonType type)
        {
            switch (type)
            {
                case BalloonType.GREEN:
                    _level.currentPoints += _balloonPoints.greenBalloonPoints;
                    totalScore += _balloonPoints.greenBalloonPoints;
                    break;

                case BalloonType.YELLOW:
                    _level.currentPoints += _balloonPoints.yellowBalloonPoints;
                    totalScore += _balloonPoints.yellowBalloonPoints;
                    break;

                case BalloonType.RED:
                    _level.currentPoints += _balloonPoints.redBalloonPoints;
                    totalScore += _balloonPoints.redBalloonPoints;
                    break;

                case BalloonType.FAKEG:
                    _level.currentPoints += _balloonPoints.fakeGreenBalloonPoints;
                    totalScore += _balloonPoints.fakeGreenBalloonPoints;
                    break;

                case BalloonType.FAKEY:
                    _level.currentPoints += _balloonPoints.fakeYellowBalloonPoints;
                    totalScore += _balloonPoints.fakeYellowBalloonPoints;
                    break;

                case BalloonType.FAKER:
                    _level.currentPoints += _balloonPoints.fakeRedBalloonPoints;
                    totalScore += _balloonPoints.fakeRedBalloonPoints;
                    break;
            }

            _onScoreChanged.Invoke(totalScore); // Lewis - event for when score changes
        }

        private void Win()
        {
            Debug.Log("WIN!");

            PlayerData.Data.Score += totalScore;

            // TODO: Win state
            onGameWin?.Invoke();
            _onGameEnded?.Invoke(true);
        }

        private void Lose()
        {
            Debug.Log("Lost :(");
            onGameLose?.Invoke();
            _onGameEnded?.Invoke(false);
        }
    }
}