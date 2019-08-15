
using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandVR {

    namespace ButtonGame
    {
        /// <summary>
        /// Behaviour of buttons for the button game
        /// </summary>
        internal class ButtonGame_buttons : MonoBehaviour
        {
            private InteractionButton button;

            [SerializeField]
            private Material color;
            [SerializeField]
            private MeshRenderer buttonObj;
            [SerializeField]
            private Buttons buttontype;

            /// <summary>
            /// Gets the value of this button
            /// </summary>
            public Buttons Buttontype => buttontype;

            void Start()
            {
                button = GetComponent<InteractionButton>();
                button.OnPress += HandleClick;
                buttonObj.material = color;
            }


            /// <summary>
            /// Event to trigger on button press
            /// </summary>
            public void HandleClick()
            {
                string eventToTrigger;
                if (buttontype == Buttons.LEFT)
                    eventToTrigger = "LeftButton";
                else
                    eventToTrigger = "RightButton";

                Core.EventManager.TriggerEvent(eventToTrigger);
                Core.EventManager.TriggerEvent("ButtonPressed");
            }

        }
    }
}