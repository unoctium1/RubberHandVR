using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HandVR.HandModifierManager;

namespace HandVR
{
    namespace ModificationExamples
    {

        /// <summary>
        /// Example of a hand modification - scales the given hand to the value in scaleFactor. All actual scaling functionality is in HandModifierManager
        /// </summary>
        public class HandScaler : MonoBehaviour, IHand
        {
            private GameObject curHand;

            public bool IsStarted { get; private set; }
            [SerializeField]
            string _title = "Hand Scaler";
            public string Label { get => _title; }
            public float Value { get => scaleFactor; set => scaleFactor = value; }

            public bool IsFinished { get; private set; }

            [SerializeField]
            private Axis axisToScale;
            [SerializeField]
            private float scaleFactor;

            public IEnumerator StartEffect()
            {
                IsFinished = false;
                IsStarted = true;
                curHand = Core.GameManager.instance.ActiveHand;
                yield return LerpSize(curHand, scaleFactor, 2f, axisToScale);
                IsFinished = true;

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



        }
    }
}
