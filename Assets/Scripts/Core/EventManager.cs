using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HandVR
{
    namespace Core
    {

        /// <summary>
        /// Static messaging system
        /// </summary>
        public class EventManager : MonoBehaviour
        {
            private Dictionary<string, UnityEvent> eventDictionary;

            private static EventManager eventManager;


            public static EventManager Instance
            {
                get
                {
                    if (!eventManager)
                    {
                        eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                        if (!eventManager)
                        {
                            Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                        }
                        else
                        {
                            eventManager.Init();
                        }
                    }

                    return eventManager;
                }
            }

            void Init()
            {
                if (eventDictionary == null)
                {
                    eventDictionary = new Dictionary<string, UnityEvent>();
                }
            }

            /// <summary>
            /// Starts listening for the given event
            /// </summary>
            /// <param name="eventName">Event to listen for</param>
            /// <param name="listener">Action to trigger on event</param>
            public static void StartListening(string eventName, UnityAction listener)
            {
                UnityEvent thisEvent = null;
                if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
                {
                    thisEvent.AddListener(listener);
                }
                else
                {
                    thisEvent = new UnityEvent();
                    thisEvent.AddListener(listener);
                    Instance.eventDictionary.Add(eventName, thisEvent);
                }
            }

            /// <summary>
            /// Unsubscribes from the event
            /// </summary>
            /// <param name="eventName">Event to unsubscribe from</param>
            /// <param name="listener">Action to stop listening with</param>
            public static void StopListening(string eventName, UnityAction listener)
            {
                if (eventManager == null) return;
                UnityEvent thisEvent = null;
                if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
                {
                    thisEvent.RemoveListener(listener);
                }
            }

            /// <summary>
            /// Triggers an event
            /// </summary>
            /// <param name="eventName">Event to trigger</param>
            public static void TriggerEvent(string eventName)
            {
                UnityEvent thisEvent = null;
                if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
                {
                    thisEvent.Invoke();
                }
            }



        }
    }
}
