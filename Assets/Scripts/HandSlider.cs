using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HandVR.HandModifierManager;

namespace HandVR
{
    public class HandSlider : MonoBehaviour, IHand
    {

        public bool IsStarted { get; private set; }
        [SerializeField]
        string _title = "Hand Slider";
        public string Label { get => _title; }

        [SerializeField]
        private float xTiltDestination = 5;
        [SerializeField]
        private float yOffsetDestination;
        [SerializeField]
        private float zOffsetDestination;



        public IEnumerator StartEffect()
        {
            IsStarted = true;
            yield return instance.LerpCameraProvider(xTiltDestination, yOffsetDestination, zOffsetDestination, 2f);
        }

        public IEnumerator Reset()
        {
            yield return instance.LerpCameraProvider(instance.InitCameraOffset, 2f);
            
            IsStarted = false;
            yield return null;
        }



    }
}