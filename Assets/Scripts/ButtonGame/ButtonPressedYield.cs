using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HandVR
{
    namespace ButtonGame
    {
        /// <summary>
        /// Custom yield instruction - yield instruction stops waiting on a button press
        /// </summary>
        internal class ButtonPressedYield : CustomYieldInstruction
        {
            private bool _isPressed = false;
            private InteractionButton button;

            public ButtonPressedYield()
            {
                Core.EventManager.StartListening("ButtonPressed", HandleButtonClick);
            }

            public ButtonPressedYield(InteractionButton b)
            {
                button = b;
                button.OnPress += HandleButtonClick;
            }

            ~ButtonPressedYield()
            {
                if (button == null)
                {
                    Core.EventManager.StopListening("ButtonPressed", HandleButtonClick);
                }
                else
                {
                    button.OnPress -= HandleButtonClick;
                }
            }

            public void HandleButtonClick()
            {
                _isPressed = true;
            }

            public override bool keepWaiting => !_isPressed;

        }
    }
}

