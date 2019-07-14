using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HandVR
{
    public class GameManager : MonoBehaviour
    {

        public static GameManager instance = null;

        [SerializeField]
        private Test[] tests;

        [SerializeField]
        private GamePanel panel;

        //private IList<ITest<ITestData>> tests;

        void Awake()
        {
            if(instance == null)
            {
                instance = this;

            }
            else
            {
                Destroy(this);
            }

            panel.Init(tests[0].testObject, tests[0].testName);
            Debug.Log("Initialized game panel");
        }

        [System.Serializable]
        private struct Test
        {
            public GameObject testObject;
            public string testName;
        }
    }
}
