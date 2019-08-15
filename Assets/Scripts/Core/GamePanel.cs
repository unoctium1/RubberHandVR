using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HandVR
{
    namespace Core
    {
        /// <summary>
        /// UI behaviour for tasks - one of these will be created for each object with an ITest component at runtime. Exposes functionality of tests to the user
        /// </summary>
        public class GamePanel : MonoBehaviour
        {
            [SerializeField]
            private Button startButton;
            [SerializeField]
            private Button stopButton;
            [SerializeField]
            private InputField numMinutes;
            [SerializeField]
            private Text label;
            [SerializeField]
            private Text errorText;

            private ITest _test;
            private GameObject _testPrefab;

            private GameObject _testInScene;

            /// <summary>
            /// Sets up the test for the given testObject - called in GameManager
            /// </summary>
            /// <param name="testPrefab">Object with ITest component</param>
            /// <param name="text">Text to display</param>
            public void Init(GameObject testPrefab, string text)
            {
                _testPrefab = testPrefab;
                label.text = text;
                startButton.onClick.AddListener(StartPressed);
                stopButton.onClick.AddListener(StopPressed);
                errorText.gameObject.SetActive(false);
            }

            /// <summary>
            /// Creates the test gameObject and starts the test
            /// </summary>
            public void StartPressed()
            {
                bool isFloat = float.TryParse(numMinutes.text, out float numMins);
                if (isFloat)
                {
                    StartTest(numMins);
                }
                else
                {
                    errorText.gameObject.SetActive(true);
                }


            }

            /// <summary>
            /// Runs the test for the given number of minutes
            /// </summary>
            /// <param name="minutes"></param>
            public void StartTest(float minutes)
            {
                errorText.gameObject.SetActive(false);
                stopButton.interactable = true;
                startButton.interactable = false;
                _testInScene = Instantiate(_testPrefab);
                _test = _testInScene.GetComponent(typeof(ITest)) as ITest;
                _test.SetNumMinutes(minutes);
                StartCoroutine(_test.StartTest());
            }

            /// <summary>
            /// True if the test is running
            /// </summary>
            /// <returns></returns>
            public bool IsRunning()
            {
                return _test.IsRunning;
            }

            /// <summary>
            /// Stops the test and destroys the gameObject
            /// </summary>
            public void StopPressed()
            {
                stopButton.interactable = false;
                startButton.interactable = true;
                _test.StopTest();
                _test = null;
                Destroy(_testInScene);
            }




        }
    }
}