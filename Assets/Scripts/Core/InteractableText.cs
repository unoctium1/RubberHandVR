using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HandVR
{
    namespace Core
    {
        /// <summary>
        /// Behaviour for instruction text - displays a set message until the user hits the confirm button.
        /// Usage - in a coroutine, call yield return new Core.InteractableText.TextMessageYield("Your string here");.
        /// The yield will return when the user hits confirm
        /// </summary>
        public class InteractableText : MonoBehaviour
        {
            [SerializeField]
            private InteractionButton button;
            [SerializeField]
            private Text text;

            private static InteractableText instance = null;

            private void Awake()
            {
                if (instance == null)
                {
                    instance = this;
                }
                else
                {
                    Destroy(this);
                }
                gameObject.SetActive(false);
            }

            public void Init(string toDisplay)
            {
                text.text = toDisplay;
                gameObject.SetActive(true);
            }

            public void Hide()
            {
                text.text = null;
                gameObject.SetActive(false);
            }

            /// <summary>
            /// Custom yield instruction - yields when the user hits the button displayed below the message. This should be the only way you are accessing the interactable text!
            /// </summary>
            public class TextMessageYield : CustomYieldInstruction
            {

                private bool _isPressed = false;

                public TextMessageYield(string message)
                {
                    instance.Init(message);
                    instance.button.OnPress += HandleButtonClick;
                }

                ~TextMessageYield()
                {
                    instance.button.OnPress -= HandleButtonClick;
                }

                public override bool keepWaiting => !_isPressed;

                public void HandleButtonClick()
                {
                    _isPressed = true;
                    instance.Hide();
                }

            }

        }
    }

    
}
