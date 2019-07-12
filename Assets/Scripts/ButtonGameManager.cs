using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HandVR
{


    public class ButtonGameManager : MonoBehaviour
    {

        [SerializeField]
        private Image panel;
        [SerializeField]
        private Color GreenColor, RedColor;

        [SerializeField]
        private TextMesh label;


        void Start()
        {
            label.gameObject.SetActive(false);
        }

        IEnumerator WaitForButtonPress(Buttons expectedButton)
        {
            yield return null;
        }

    }

    public enum Buttons
    {
        LEFT,
        RIGHT
    }
}
