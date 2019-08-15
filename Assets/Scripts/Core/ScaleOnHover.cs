using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HandVR
{
    namespace Core
    {
        /// <summary>
        /// Behaviour for the preview window - enlarges to MaxHeight when the mouse hovers over it
        /// </summary>
        public class ScaleOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
        {

            [SerializeField]
            private RectTransform child;

            [SerializeField]
            private float speed;
            private Vector2 childDiff;

            private RectTransform rt;
            private float minHeight;

            private float MaxHeight
            {
                get => Screen.height - 20f;
            }

            private void Start()
            {
                rt = GetComponent<RectTransform>();
                minHeight = rt.rect.height;
                childDiff = new Vector2(rt.rect.width - child.rect.width, rt.rect.height - child.rect.height);
            }



            public void OnPointerEnter(PointerEventData eventData)
            {
                StopAllCoroutines();
                StartCoroutine(LerpSizeUp());
            }
            public void OnPointerExit(PointerEventData eventData)
            {
                StopAllCoroutines();
                StartCoroutine(LerpSizeDown());
            }

            IEnumerator LerpSizeUp()
            {
                yield return LerpUniformSize(rt, MaxHeight);
            }

            IEnumerator LerpSizeDown()
            {
                yield return LerpUniformSize(rt, minHeight);
            }

            IEnumerator LerpUniformSize(RectTransform rt, float targetSize)
            {
                Vector2 initSize = new Vector2(rt.rect.width, rt.rect.height);
                Vector2 finalSize = Vector2.one * targetSize;

                Vector2 denominator = (initSize.magnitude > finalSize.magnitude) ? initSize : finalSize;
                float diff = Mathf.Abs(((finalSize - initSize) / denominator).magnitude);

                float time = diff / speed;
                //Debug.Log(time);
                float initTime = 0;
                while (initTime < time)
                {
                    Vector2 scale = Vector2.Lerp(initSize, finalSize, initTime / time);
                    Vector2 childScale = scale - childDiff;
                    child.sizeDelta = childScale;
                    rt.sizeDelta = scale;
                    initTime += Time.deltaTime;
                    yield return null;
                }
                yield return null;
            }
        }
    }
}
