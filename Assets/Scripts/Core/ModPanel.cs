using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HandVR
{
    public class ModPanel : MonoBehaviour
    {
        [SerializeField]
        private Button startButton;
        [SerializeField]
        private Button resetButton;
        [SerializeField]
        private Text label;

        /*
        [SerializeField]
        private Slider valueSetter;
        */

        private IHand mod;

        public bool IsFinished { get => mod.IsFinished; }

        public void Start()
        {
            mod = GetComponent<IHand>() as IHand;
            label.text = mod.Label;
            startButton.onClick.AddListener(this.StartPressed);
            resetButton.onClick.AddListener(this.ResetPressed);
            //valueSetter.value = mod.Value;
            //valueSetter.onValueChanged.AddListener(UpdateValue);
        }


        private void UpdateValue(float v)
        {
            mod.Value = v;
        }

        public void StartPressed()
        {
            StartCoroutine(mod.StartEffect());
            StartCoroutine(CheckStartAvailable());
            startButton.interactable = false;
        }

        public IEnumerator CheckStartAvailable()
        {
            while (mod.IsStarted)
            {
                yield return null;
            }
            startButton.interactable = true;
            yield return null;
        }
            

        public void ResetPressed()
        {
            StopAllCoroutines();
            StartCoroutine(mod.Reset());
            StartCoroutine(CheckStartAvailable());
        }


    }
}
