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
        [System.Obsolete]
        void SetNumTests(int numTests);

        /// <summary>
        /// Sets total time to perform the trials
        /// </summary>
        /// <param name="numMins">Time for the game (in minutes)</param>
        void SetNumMinutes(float numMins);

        /// <summary>
        /// Stops test early
        /// </summary>
        void StopTest();

        bool IsRunning { get; }
    }

    /// <summary>
    /// Interface for results of tests
    /// </summary>
    public interface ITestData
    {
        string ToString();
    }
}
