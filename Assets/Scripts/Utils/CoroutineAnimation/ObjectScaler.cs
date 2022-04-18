using UnityEngine;

namespace Utils.CoroutineAnimation
{
    public class ObjectScaler : AnimationBehaviour
    {
        public void ChangeScale(Vector3 scale)
        {
            var startScale = transform.localScale;
            Animate((percent) =>
            {
                transform.localScale = Vector3.Lerp(startScale, scale, percent);
            });
        }
    }
}