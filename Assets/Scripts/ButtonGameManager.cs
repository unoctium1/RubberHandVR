using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HandVR
{
    namespace ButtonGame
    {

        public class ButtonGameManager : MonoBehaviour
        {

            [SerializeField]
            private Image panel;
            [SerializeField]
            private Color GreenColor, RedColor;

            [SerializeField]
            private TextMesh label;

            [SerializeField]
            private int tests;

            private IList<(float, bool)> results;

            public Buttons LastPressedButton { get; private set; }

            void Start()
            {
                label.gameObject.SetActive(false);
                results = new List<(float, bool)>();
                EventManager.StartListening("LeftButton", Left_UpdateButton);
                EventManager.StartListening("RightButton", Right_UpdateButton);
                StartCoroutine(StartHandGame(tests));
            }

            IEnumerator StartHandGame(int _tests)
            {
                yield return new WaitForSeconds(3f);
                for (int i = 0; i < _tests; i++)
                {
                    Buttons b = (Buttons)Random.Range(0, 2);
                    yield return HandleButton(b);
                    Debug.Log(results[results.Count-1]);
                }
                
            }

            IEnumerator HandleButton(Buttons expectedButton)
            {
                label.gameObject.SetActive(false);
                if (expectedButton == Buttons.LEFT)
                {
                    panel.color = RedColor;
                    
                }
                else
                {
                    panel.color = GreenColor;
                }
                float _time = Time.time;
                yield return new ButtonPressedYield();
                if(LastPressedButton == expectedButton)
                {
                    results.Add((Time.time - _time, true));
                    label.text = "Correct";
                }
                else
                {
                    results.Add((Time.time - _time, false));
                    label.text = "Incorrect";
                }
                label.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.2f);


            }

            public void Left_UpdateButton()
            {
                Debug.Log("Left pressed");
                LastPressedButton = Buttons.LEFT;
            }

            public void Right_UpdateButton()
            {
                Debug.Log("Right pressed");
                LastPressedButton = Buttons.RIGHT;
            }

        }

        public enum Buttons
        {
            LEFT,
            RIGHT
        }
    }
}
