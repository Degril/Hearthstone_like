using System;
using UnityEngine;

namespace Utils.CoroutineAnimation
{
    public class ObjectMover : AnimationBehaviour
    {
        public bool MoveToMouse;

        public void Move(Vector3 position, Quaternion rotation)
        {
            var startAnimationPosition = transform.localPosition;
            var startAnimationEulerAngles = Quaternion.Euler(transform.eulerAngles);
            Animate((percent) =>
            {
                transform.localPosition = Vector3.Lerp(startAnimationPosition, position, percent);
                transform.rotation = Quaternion.Lerp(startAnimationEulerAngles, rotation, percent);
            });
        }


        private void Update()
        {
            if(MoveToMouse)
                transform.position = Input.mousePosition - Vector3.up * ((transform as RectTransform).rect.width * 0.5f);
        }
    }
}