using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HandVR.HandModifierManager;

namespace HandVR
{
    namespace ModificationExamples
    {

        /// <summary>
        /// Example of a hand modification - displaces the given hand to the value in displacement. If usePostProcess is checked, actual functionality is in DisplacementPostProcess.
        /// Otherwise, functionality is in HandModifierManager. Both illustrate different ways to achieve similar effects.
        /// </summary>
        public class HandSlider : MonoBehaviour, IHand
        {

            public bool IsStarted { get; private set; }
            public bool IsFinished { get; private set; }
            [SerializeField]
            string _title = "Hand Slider";
            public string Label { get => _title; }
            public float Value { get => displacement; set => displacement = value; }
            [SerializeField]
            private bool usePostProcess = true;
            [Header("Post process values")]
            [SerializeField]
            private float displacement;

            [Header("Non post process values - Uses Camera Offset")]
            [SerializeField]
            private float xTiltDestination = 5;
            [SerializeField]
            private float yOffsetDestination;
            [SerializeField]
            private float zOffsetDestination;

            public IEnumerator StartEffect()
            {
                IsFinished = false;
                IsStarted = true;
                if (usePostProcess)
                {
                    instance.ActivateDisplacementHands(displacement);
                    while (!instance.IsDisplacementFinished)
                    {
                        yield return null;
                    }
                    IsFinished = true;
                }
                else
                {
                    yield return instance.LerpCameraProvider(xTiltDestination, yOffsetDestination, zOffsetDestination, 2f);
                }
            }

            public IEnumerator Reset()
            {
                if (usePostProcess)
                {
                    instance.DeactivateDisplacementHands();
                    while (!instance.IsDisplacementReset)
                    {
                        yield return null;
                    }

                }
                else
                {
                    yield return instance.LerpCameraProvider(instance.InitCameraOffset, 2f);

                }
                IsStarted = false;
                yield return null;

            }



        }
    }
}