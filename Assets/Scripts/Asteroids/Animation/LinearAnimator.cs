using System.Collections;
using UnityEngine;

namespace Asteroids.Animation
{
    public class LinearAnimator : MonoBehaviour
    {
        [SerializeField] private IAnimated[] _animatedObjects;
        [SerializeField] private AnimationCurve _animationCurve;
        [SerializeField, Min(0.01f)] private float _animationLength = 1f;
        private IEnumerator _coroutineHandler;

        private void Awake()
        {
            _coroutineHandler = _AnimationCoroutine();
        }

        public void Animate()
        {
            StopCoroutine(_coroutineHandler);
            _coroutineHandler = _AnimationCoroutine();
            StartCoroutine(_coroutineHandler);
        }

        private IEnumerator _AnimationCoroutine()
        {
            float time = 0f;
            while (time < _animationLength)
            {
                float factorValue = _animationCurve.Evaluate(time / _animationLength);
                foreach (IAnimated animated in _animatedObjects)
                {
                    animated.Animate(_animationCurve.Evaluate(factorValue));
                }
                yield return new WaitForEndOfFrame();
                time += Time.deltaTime;
            }
            foreach (IAnimated animated in _animatedObjects)
            {
                animated.Animate(_animationCurve.Evaluate(1f));
            }
        }
    }
}