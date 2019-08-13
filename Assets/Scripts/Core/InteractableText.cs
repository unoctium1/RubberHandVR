using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HandVR
{
    public class InteractableText : MonoBehaviour
    {
        [SerializeField]
        private InteractionButton button;
        [SerializeField]
        private Text text;

        private static InteractableText instance = null;

        private void Awake()
        {
            if(instance == null)
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


        public class TextMessageYield : CustomYieldInstruction
        {

            private bool _isPressed = false;

            public TextMessageYield (string message)
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
