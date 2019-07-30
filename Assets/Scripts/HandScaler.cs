using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HandVR.HandModifierManager;

namespace HandVR
{
    public class HandScaler : MonoBehaviour, IHand
    {
        private GameObject curHand;

        public bool IsStarted { get; private set; }
        [SerializeField]
        string _title = "Hand Scaler";
        public string Label { get => _title; }

        [SerializeField]
        private Axis axisToScale;
        [SerializeField]
        private float scaleFactor;

        [SerializeField]
        private GameObject cameraParent;

        public IEnumerator StartEffect()
        {
            IsStarted = true;
            curHand = GameManager.instance.ActiveHand;
            if (HandModifierManager.instance.ScaleCamera)
            {
                yield return LerpSize(HandModifierManager.instance.CameraParent, scaleFactor, 2f, axisToScale);
            }
            else { yield return LerpSize(curHand, scaleFactor, 2f, axisToScale); }
            
        }

        public IEnumerator Reset()
        {
            if (curHand != null)
            {
                if (HandModifierManager.instance.ScaleCamera)
                {
                    yield return LerpSize(HandModifierManager.instance.CameraParent, 1, 2f);
                }
                else { yield return LerpSize(curHand, 1, 2f); }
                curHand = null;
            }
            IsStarted = false;
            yield return null;
        }

        

    }
}
