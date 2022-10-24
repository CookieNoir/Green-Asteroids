using UnityEngine;
using UnityEngine.Events;
using Asteroids.Levels;

namespace Asteroids
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private LevelProperties _levelProperties;
        [SerializeField] private LevelCreator _levelCreator;
        public UnityEvent OnLevelDrop;
        public UnityEvent OnLevelSet;
        private bool _isPassing;
        public UnityEvent OnStartPassing;
        public UnityEvent OnStopPassing;

        private void StartLevel(LevelProperties levelProperties)
        {
            DropLevel();
            _levelCreator.CreateLevel(levelProperties);
            OnLevelSet.Invoke();
        }

        private void Start()
        {
            StartLevel(_levelProperties);
        }

        public void DropLevel()
        {
            if (_isPassing) _StopPassing();
            OnLevelDrop.Invoke();
        }

        public void ChangePassing()
        {
            if (_isPassing) _StopPassing();
            else _StartPassing();
        }

        private void _StartPassing()
        {
            OnStartPassing.Invoke();
            //_entitiesManager.StartMoving();
            //_tilePainter.StartPainting();
            _isPassing = true;
        }

        private void _StopPassing()
        {
            _isPassing = false;
            OnStopPassing.Invoke();
            //_entitiesManager.StopMoving();
            //_tilePainter.StopPainting();
        }
    }
}