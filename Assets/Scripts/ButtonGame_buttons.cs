
using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandVR {
    public class ButtonGame_buttons : MonoBehaviour
    {
        private InteractionButton button;

        [SerializeField]
        private Material color;
        [SerializeField]
        private MeshRenderer buttonObj;

        [SerializeField]
        private Buttons buttontype;

        void Start()
        {
            button = GetComponent<InteractionButton>();
            button.OnPress += HandleClick;
            buttonObj.material = color;
        }

        public void HandleClick()
        {
            string eventToTrigger;
            if (buttontype == Buttons.LEFT)
                eventToTrigger = "LeftButton";
            else
                eventToTrigger = "RightButton";

            EventManager.TriggerEvent(eventToTrigger);
        }

        // Update is called once per frame
        void Update()
        {

        }

        
    }
}