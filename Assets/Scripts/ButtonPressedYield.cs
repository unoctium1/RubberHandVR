using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HandVR
{
    namespace ButtonGame
    {
        public class ButtonPressedYield : CustomYieldInstruction
        {
            private bool _isPressed = false;
            public ButtonPressedYield(Buttons b){
                if(b == Buttons.LEFT)
                {
                    EventManager.StartListening("LeftButton", HandleButtonClick);
                }
                else
                {
                    EventManager.StartListening("RightButton", HandleButtonClick);
                }
            }
            public ButtonPressedYield()
            {
                EventManager.StartListening("ButtonPressed", HandleButtonClick);
            }

            public void HandleButtonClick()
            {
                _isPressed = true;
            }

            public override bool keepWaiting => !_isPressed;

        }
    }
}

