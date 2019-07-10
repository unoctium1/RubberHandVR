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
        private Canvas headsetCanvas;

        // Start is called before the first frame update
        void Start()
        {
            headsetCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            headsetCanvas.worldCamera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void LeftButtonPressed()
        {
            Debug.Log("Left");
            panel.color = RedColor;
        }

        public void RightButtonPressed()
        {
            Debug.Log("Right");
            panel.color = GreenColor;
        }


    }
}
