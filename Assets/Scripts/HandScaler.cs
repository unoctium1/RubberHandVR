using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HandVR.HandModifierManager;

namespace HandVR
{
    [System.Serializable]
    public class HandScaler : MonoBehaviour, IHand
    {
        private GameObject curHand;

        public bool IsStarted { get; private set; }
        const string _title = "Hand Scaler";
        public string Label { get => _title; }

        public IEnumerator StartEffect()
        {
            IsStarted = true;
            curHand = GameManager.instance.ActiveHand;
            yield return LerpSize(curHand, HandModifierManager.instance.ScaleFactor, 2f, HandModifierManager.instance.AxisToScale);
        }

        public IEnumerator Reset()
        {
            if (curHand != null)
            {
                yield return LerpSize(curHand, 1, 2f);
                curHand = null;
            }
            IsStarted = false;
            yield return null;
        }

        IEnumerator LerpSize(GameObject go, float targetSize, float time, Axis axis = Axis.none)
        {
            Vector3 initSize = go.transform.localScale;
            Vector3 finalSize = initSize;


            switch (axis)
            {
                case Axis.x:
                    finalSize.x = targetSize;
                    break;
                case Axis.y:
                    finalSize.y = targetSize;
                    break;
                case Axis.z:
                    finalSize.z = targetSize;
                    break;
                default:
                    finalSize = Vector3.one * targetSize;
                    break;
            }

            float initTime = 0;
            while (initTime < time)
            {
                Vector3 scale = Vector3.Lerp(initSize, finalSize, initTime / time);
                go.transform.localScale = scale;
                initTime += Time.deltaTime;
                yield return null;
            }
            yield return null;
        }

    }
}
