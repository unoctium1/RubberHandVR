using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HandVR
{
    public class GamePanel : MonoBehaviour
    {
        [SerializeField]
        private Button startButton;
        [SerializeField]
        private Button stopButton;
        [SerializeField]
        private InputField numTests;
        [SerializeField]
        private Text label;

        private ITest _test;
        private GameObject _testPrefab;

        private GameObject _testInScene;

        public void Init(GameObject testPrefab, string text)
        {
            _testPrefab = testPrefab;
            label.text = text;
            startButton.onClick.AddListener(this.StartPressed);
            stopButton.onClick.AddListener(this.StopPressed);
        }

        public void StartPressed()
        {
            //Debug.Log("start pressed");
            _testInScene = Instantiate(_testPrefab);
            _test = _testInScene.GetComponent(typeof(ITest)) as ITest;
            //Debug.Log(_test);
            _test.SetNumTests(int.Parse(numTests.text));
            StartCoroutine(_test.StartTest());

        }

        public void StopPressed()
        {
            _test.StopTest();
            _test = null;
            Destroy(_testInScene);
        }



    }
}