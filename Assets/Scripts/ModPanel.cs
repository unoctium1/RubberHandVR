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

        public void Start()
        {
            mod = GetComponent<IHand>() as IHand;
            label.text = mod.Label;
            startButton.onClick.AddListener(this.StartPressed);
            resetButton.onClick.AddListener(this.ResetPressed);
        }


        public void StartPressed()
        {
            StartCoroutine(mod.StartEffect());
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
            StopAllCoroutines();
            StartCoroutine(mod.Reset());
            StartCoroutine(CheckStartAvailable());
        }


    }
}
