using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        private GameObject[] modifiers;
        [SerializeField]
        private GameObject modPanelPrefab;
        [SerializeField]
        private GameObject modPanelParent;
        private IList<ModPanel> modPanels;

        [SerializeField]
        private GameObject testPanelPrefab;
        [SerializeField]
        private GameObject testPanelParent;
        private IList<GamePanel> testPanels;

        public IList<TotalData> totalResults;

        [SerializeField]
        private GameObject leftHand;
        [SerializeField]
        private GameObject rightHand;

        internal bool isRightHand = true;

        [SerializeField]
        public GameObject ActiveHand { get
            {
                if (isRightHand)
                {
                    return rightHand;
                }
                else
                {
                    return leftHand;
                }
            }
        }

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

            totalResults = new List<TotalData>();
            testPanels = new List<GamePanel>();
            modPanels = new List<ModPanel>();
            float offset = 0;
            foreach(Test t in tests)
            {
                GameObject p = Instantiate(testPanelPrefab, testPanelParent.transform);
                RectTransform rt = p.GetComponent<RectTransform>();
                Vector3 pos = rt.localPosition;
                pos.y += offset;
                rt.localPosition = pos;
                offset -= rt.rect.height;
                GamePanel panel = p.GetComponent<GamePanel>();
                panel.Init(t.testObject, t.testName);
                testPanels.Add(panel);
            }
            offset = 0;
            foreach (GameObject m in modifiers)
            {
                GameObject p = Instantiate(m, modPanelParent.transform);
                RectTransform rt = p.GetComponent<RectTransform>();
                Vector3 pos = rt.localPosition;
                pos.y += offset;
                rt.localPosition = pos;
                offset -= rt.rect.height;
                ModPanel panel = p.GetComponent<ModPanel>();
                modPanels.Add(panel);
            }
        }

        private void OnApplicationQuit()
        {
            WriteData();
        }

        public void SetRightHand()
        {
            isRightHand = true;
        }

        public void SetLeftHand()
        {
            isRightHand = false;
        }

        [System.Serializable]
        private struct Test
        {
            public GameObject testObject;
            public string testName;
        }

        [System.Serializable]
        public struct TotalData : ITestData
        {
            public string label;
            public IList<ITestData> data;
        }


        public void WriteData()
        {

            string path = Application.streamingAssetsPath;
            string json = JsonUtility.ToJson(totalResults, true);
            string fileName = System.DateTime.Now.ToFileTime().ToString() + ".json";
            File.WriteAllText(path + "\\" + fileName, json);
        }

        public void StartDemo()
        {
            StartCoroutine(DemoLoop());
        }

        public IEnumerator DemoLoop()
        {
            yield return CycleTestRoutine(1f);
            StartCoroutine(CycleTestRoutine(2f));
            yield return new WaitForSeconds(60f);
            foreach (ModPanel mod in modPanels)
            {
                yield return StartModRoutine(mod, 45f);
            }

        }

        private IEnumerator StartTestRoutine(GamePanel p, float timeMins)
        {
            p.StartTest(timeMins);
            yield return new WaitForSeconds(0.2f);
            while (p.IsRunning())
            {
                yield return null;
            }
            p.StopPressed();
        }

        private IEnumerator CycleTestRoutine(float time)
        {
            foreach (GamePanel p in testPanels)
            {
                yield return StartTestRoutine(p, time);
            }
        }

        private IEnumerator StartModRoutine(ModPanel mod, float timeInSeconds)
        {
            mod.StartPressed();
            yield return new WaitForSeconds(0.2f);
            while (!mod.IsFinished)
            {
                yield return null;
            }
            yield return new WaitForSeconds(timeInSeconds);
            mod.ResetPressed();
            yield return mod.CheckStartAvailable();
        }

    }
}
