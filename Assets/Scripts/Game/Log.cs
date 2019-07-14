using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



namespace Game
{
    public class Log : MonoBehaviour
    {
        /**
         * Holds all the data members that the class
         * contains.
         */
        #region Data Members

        List<LogEntry> m_log;

        #endregion


        /**
         * Modifies the data members so that they may
         * be read-only, return specific values, or
         * expose certain data members to the public
         */
        #region Member Properties

        #endregion


        /**
         * Any Unity Methods used.
         */
        #region Unity Methods

        private void Awake()
        {
            m_log = new List<LogEntry>();
        }

        #endregion


        /**
         * Constructors that are called when building
         * the class.
         */
        #region Constructors

        #endregion


        /**
         * Methods that are able to be called from
         * outside of the class.
         */
        #region Public Methods

        public void AddEntry(LogType action, string initiator, string receiver, string piece, Vector3 initiatorLocation, Vector3 receiverLocation)
        {
            switch (action)
            {
                case LogType.Move:
                    m_log.Add(new LogEntry
                    {
                        description = initiator + " moved " + piece + " " + initiatorLocation.ToString() + " to " + receiverLocation.ToString(),
                        initiator = initiator,
                        receiver = receiver,
                        piece = piece,
                        initiatorLocation = initiatorLocation,
                        receiverLocation = receiverLocation
                    });
                    break;

                case LogType.Take:
                    m_log.Add(new LogEntry
                    {
                        description = initiator + " took " + piece + " " + initiatorLocation.ToString() + " from " + receiver + " " + receiverLocation.ToString(),
                        initiator = initiator,
                        receiver = receiver,
                        piece = piece,
                        initiatorLocation = initiatorLocation,
                        receiverLocation = receiverLocation
                    });
                    break;

                case LogType.Promote:
                    m_log.Add(new LogEntry
                    {
                        description = initiator + " promoted " + piece + " " + initiatorLocation.ToString(),
                        initiator = initiator,
                        receiver = receiver,
                        piece = piece,
                        initiatorLocation = initiatorLocation,
                        receiverLocation = receiverLocation
                    });
                    break;

                case LogType.Win:
                    m_log.Add(new LogEntry
                    {
                        description = initiator + " has won",
                        initiator = initiator,
                        receiver = receiver,
                        piece = piece,
                        initiatorLocation = initiatorLocation,
                        receiverLocation = receiverLocation
                    });
                    break;

                default:
                    m_log.Add(new LogEntry
                    {
                        description = "[ERROR] Illegal LogType",
                        initiator = "",
                        receiver = "",
                        piece = "",
                        initiatorLocation = new Vector3(0, 0, 0),
                        receiverLocation = new Vector3(0, 0, 0)
                    });
                    break;
            }

            PrintEntry(m_log.Last());
        }

        public void AddEntry(string message)
        {
            m_log.Add(new LogEntry
            {
                description = message,
                initiator = "",
                receiver = "",
                piece = "",
                initiatorLocation = new Vector3(0, 0, 0),
                receiverLocation = new Vector3(0, 0, 0)
            });
        }

        #endregion


        /**
         * Private functions that are only used from
         * within this class.
         */
        #region Member Functions

        private void PrintEntry(LogEntry log)
        {
            Debug.Log(log.description);
        }

        #endregion
    }
}
