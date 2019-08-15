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
        /// <summary>
        /// Sample test - users must hit the colored button matching the central pattern
        /// </summary>
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

            #region PROPERTIES
            /// <summary>
            /// Returns the last pressed button
            /// </summary>
            internal Buttons LastPressedButton { get; private set; }

            /// <summary>
            /// Returns the list of results for the trials - see ButtonData for result structure
            /// </summary>
            public IList<ITestData> Results { get; private set; }

            /// <summary>
            /// True if the test is currently in progress
            /// </summary>
            public bool IsRunning { get; private set; } = false;
            #endregion //PROPERTIES

            #region UNITY_MONOBEHAVIOUR_METHODS
            private void Start()
            {

                Results = new List<ITestData>();
            }

            private void OnEnable()
            {
                Core.EventManager.StartListening("LeftButton", Left_UpdateButton);
                Core.EventManager.StartListening("RightButton", Right_UpdateButton);
            }

            private void OnDisable()
            {
                Core.EventManager.StopListening("LeftButton", Left_UpdateButton);
                Core.EventManager.StopListening("RightButton", Right_UpdateButton);
            }
            #endregion //UNITY_MONOBEHAVIOUR_METHODS

            #region PUBLIC

            /// <summary>
            /// Stops the test early
            /// </summary>
            public void StopTest()
            {
                StopAllCoroutines();
                IsRunning = false;
            }

            /// <summary>
            /// Sets the number of trials to perform - use setNumMins instead
            /// </summary>
            /// <param name="numTests"></param>
            [System.Obsolete] public void SetNumTests(int numTests)
            {
                tests = numTests;
            }

            /// <summary>
            /// Sets how long the test should run for
            /// </summary>
            /// <param name="numMins"></param>
            public void SetNumMinutes(float numMins)
            {
                minutes = numMins;
            }

            /// <summary>
            /// Runs through the task for the given number of trials - use StartTest instead
            /// </summary>
            /// <returns></returns>
            [System.Obsolete] public IEnumerator StartTest_trials()
            {
                resultLabel.gameObject.SetActive(false);
                gameObject.SetActive(false);
                yield return new Core.InteractableText.TextMessageYield(instructions);
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

            /// <summary>
            /// Starts the test - runs for a fixed number of minutes
            /// </summary>
            /// <returns></returns>
            public IEnumerator StartTest()
            {
                IsRunning = true;
                resultLabel.gameObject.SetActive(false);
                gameObject.SetActive(false);
                yield return new Core.InteractableText.TextMessageYield(instructions);
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
                Core.GameManager.instance.totalResults.Add(new Core.GameManager.TotalData { label = "Button Game", data = Results });
                yield return new WaitForSeconds(0.5f);
                resultLabel.gameObject.SetActive(false);
                IsRunning = false;

            }
            #endregion //PUBLIC

            #region PRIVATE_METHODS
            /// <summary>
            /// Helper coroutine - waits for a button to be pressed, and then logs in Results if the pressed button was the expected/correct button
            /// </summary>
            /// <param name="expectedButton"></param>
            /// <returns></returns>
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

        /// <summary>
        /// Struct for storing button data - logs how long it took for a trial to complete and whether it was the correct button
        /// </summary>
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
