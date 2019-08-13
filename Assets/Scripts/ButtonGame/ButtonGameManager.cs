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

        public class ButtonGameManager : MonoBehaviour, ITest
        {
            const string instructions = "When the test begins, the central panel will turn either green or red. As fast as you can, try to push down the corresponding green or red button. Text on the screen will indicate if you are correct. The test is over when the text displays 'Finished!'";
            [SerializeField]
            private Image panel;
            [SerializeField]
            private Color GreenColor, RedColor;

            [SerializeField]
            private TextMesh resultLabel;

            [System.Obsolete]
            private int tests;

            [SerializeField]
            private float minutes;

            //private const string _label = "Button Pressing Reaction Test";

            internal Buttons LastPressedButton { get; private set; }

            #region UNITY_MONOBEHAVIOUR_METHODS
            private void Start()
            {

                Results = new List<ITestData>();
                
                //StartCoroutine(StartHandGame(tests));
            }

            private void OnEnable()
            {
                EventManager.StartListening("LeftButton", Left_UpdateButton);
                EventManager.StartListening("RightButton", Right_UpdateButton);
            }

            private void OnDisable()
            {
                EventManager.StopListening("LeftButton", Left_UpdateButton);
                EventManager.StopListening("RightButton", Right_UpdateButton);
            }
            #endregion //UNITY_MONOBEHAVIOUR_METHODS

            #region PUBLIC
            public IList<ITestData> Results { get; private set; }

            public bool IsRunning { get; private set; } = false;

            //public string Label => _label;

            public void StopTest()
            {
                StopAllCoroutines();
                IsRunning = false;
            }

            [System.Obsolete]
            public void SetNumTests(int numTests)
            {
                tests = numTests;
            }

            public void SetNumMinutes(float numMins)
            {
                minutes = numMins;
            }

            [System.Obsolete]
            public IEnumerator StartTest_trials()
            {
                resultLabel.gameObject.SetActive(false);
                gameObject.SetActive(false);
                yield return new InteractableText.TextMessageYield(instructions);
                this.gameObject.SetActive(true);
                for (int i = 0; i < tests; i++)
                {
                    Buttons b = (Buttons)Random.Range(0, 2);
                    yield return HandleButton(b);
                    //Debug.Log(Results[Results.Count-1]);
                }
                resultLabel.text = "Finished!";
                resultLabel.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.2f);

            }

            public IEnumerator StartTest()
            {
                IsRunning = true;
                resultLabel.gameObject.SetActive(false);
                gameObject.SetActive(false);
                yield return new InteractableText.TextMessageYield(instructions);
                float stopTime = minutes * 60f;
                float initTime = Time.time;
                this.gameObject.SetActive(true);
                while((Time.time - initTime) < stopTime)
                {
                    Buttons b = (Buttons)Random.Range(0, 2);
                    yield return HandleButton(b);
                    //Debug.Log(Results[Results.Count-1]);
                }
                resultLabel.text = "Finished!";
                resultLabel.gameObject.SetActive(true);
                GameManager.instance.totalResults.Add(new GameManager.TotalData { label = "Button Game", data = Results });
                yield return new WaitForSeconds(0.5f);
                resultLabel.gameObject.SetActive(false);
                IsRunning = false;

            }
            #endregion //PUBLIC

            #region PRIVATE_METHODS
            private IEnumerator HandleButton(Buttons expectedButton)
            {
                resultLabel.gameObject.SetActive(false);
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
                float t = Time.time - _time;
                if (LastPressedButton == expectedButton)
                {
                    Results.Add(new ButtonData{time = t, correct = true });
                    resultLabel.text = "Correct";
                }
                else
                {
                    Results.Add(new ButtonData { time = t, correct = false });
                    resultLabel.text = "Incorrect";
                }
                resultLabel.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.2f);


            }

            private void Left_UpdateButton()
            {
                //Debug.Log("Left pressed");
                LastPressedButton = Buttons.LEFT;
            }

            private void Right_UpdateButton()
            {
                //Debug.Log("Right pressed");
                LastPressedButton = Buttons.RIGHT;
            }
            #endregion //PRIVATE_METHODS

        }

        [System.Serializable]
        public struct ButtonData : ITestData
        {
            public float time;
            public bool correct;

            public override string ToString()
            {
                return (time, correct).ToString();
            }
        }

        internal enum Buttons
        {
            LEFT,
            RIGHT
        }
    }
}
