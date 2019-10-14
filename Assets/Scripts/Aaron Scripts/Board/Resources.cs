using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Game
{
    /// <summary>
    /// Used as a container to hold the board tile and shogi piece GameObjects
    /// </summary>
    public class BoardTile
    {
        /**
         * Data members for tile and piece and the Member properties for said
         * data. They are separeted for consistency and accessibility, where
         * Tile should be readonly while Piece can be changed.
         */
        private Vector3Int m_position;
        private GameObject m_tile;
        private Piece m_piece;

        public Vector3Int Position { get { return m_position; } }
        public GameObject Tile { get { return m_tile; } }
        public Piece Piece { get { return m_piece; } set { m_piece = value; } }

        /**
         * Basic constructors for the class
         */
        public BoardTile()
        {
            m_position = new Vector3Int();
            m_tile = null;
            m_piece = null;
        }

        public BoardTile(Vector3Int position, GameObject tile, Piece piece)
        {
            m_position = position;
            m_tile = tile;
            m_piece = piece;
        }
    }

    /// <summary>
    /// Categories of console logging actions
    /// </summary>
    public enum LogType
    {
        Move = 0x00,
        Take = 0x01,
        Promote = 0x02,
        Win = 0x03
    }

    /// <summary>
    /// Container to hold all logging information for easy printing
    /// </summary>
    struct LogEntry
    {
        public string description;
        public string initiator;
        public string receiver;
        public string piece;
        public Vector3 initiatorLocation;
        public Vector3 receiverLocation;
    }
}