using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandVR
{

    /// <summary>
    /// Interface for test activities
    /// </summary>
    public interface ITest
    {
        /*/// <summary>
        /// Returns name of this test
        /// </summary>
        string Label { get; }*/

        /// <summary>
        /// Returns results of test
        /// </summary>
        IList<ITestData> Results { get; }

        /// <summary>
        /// Begins performing the test
        /// </summary>
        /// <returns></returns>
        IEnumerator StartTest();

        /// <summary>
        /// Sets number of test repitions to perform
        /// </summary>
        /// <param name="numTests"></param>
        void SetNumTests(int numTests);

        /// <summary>
        /// Stops test early
        /// </summary>
        void StopTest();
    }

    /// <summary>
    /// Interface for results of tests
    /// </summary>
    public interface ITestData
    {
        string ToString();
    }
}
