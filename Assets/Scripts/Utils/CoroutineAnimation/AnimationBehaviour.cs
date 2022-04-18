using System;
using System.Collections;
using UnityEngine;

namespace Utils.CoroutineAnimation
{
    public class AnimationBehaviour : MonoBehaviour
    {
        [SerializeField] private float animationTime;
        [SerializeField] private AnimationCurve percentSpeedAtTime;

        private Coroutine _lastActiveCoroutine;

        protected void Animate(Action<float> onAnimate)
        {
            if(_lastActiveCoroutine!= null)
                StopCoroutine(_lastActiveCoroutine);
            
            _lastActiveCoroutine = StartCoroutine(AnimateCoroutine(onAnimate));
        }
        

        private IEnumerator AnimateCoroutine(Action<float> onAnimate)
        {
            float currentAnimationTime = 0;

            while (currentAnimationTime < animationTime)
            {
                currentAnimationTime += Time.deltaTime;
                var percent = percentSpeedAtTime.Evaluate(currentAnimationTime / animationTime);
                onAnimate.Invoke(percent);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}