using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            yield return LerpUniformSize(curHand, HandModifierManager.instance.UniformScaleFactor, 2f);
        }

        public IEnumerator Reset()
        {
            if (curHand != null)
            {
                yield return LerpUniformSize(curHand, 1, 2f);
                curHand = null;
            }
            IsStarted = false;
            yield return null;
        }

        IEnumerator LerpUniformSize(GameObject go, float targetSize, float time)
        {
            Vector3 initSize = go.transform.localScale;
            Vector3 finalSize = Vector3.one * targetSize;

            float initTime = 0;
            while(initTime  < time)
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
