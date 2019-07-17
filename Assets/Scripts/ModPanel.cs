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

        private IHand mod;


        public void Init(IHand mod, string text)
        {
            label.text = text;
            startButton.onClick.AddListener(this.StartPressed);
            resetButton.onClick.AddListener(this.ResetPressed);
        }

        public void StartPressed()
        {
            StartCoroutine(mod.Start());
            StartCoroutine(CheckStartAvailable());
            startButton.interactable = false;
        }

        private IEnumerator CheckStartAvailable()
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
            StopCoroutine(mod.Start());
            StartCoroutine(mod.Reset());
        }


    }
}
