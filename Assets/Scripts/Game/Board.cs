using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



namespace Game
{
    public class Board : MonoBehaviour
    {
        /**
         * Holds all the data members that the class
         * contains.
         */
        #region Data Members

        BoardTile[,,] m_board;
        [Header("Board information")]
        [SerializeField][ReadOnly] int m_boardSize;

        [Header("Tile prefab and information")]
        [SerializeField][Tooltip("The tile prefab")] GameObject m_tile;
        [SerializeField][Tooltip("The distance between tiles")] float m_tileOffset;

        #endregion


        /**
         * Modifies the data members so that they may
         * be read-only, return specific values, or
         * expose certain data members to the public
         */
        #region Member Properties

        public BoardTile this[uint x, uint y, uint z]
        {
            get { return m_board[x, y, z]; }
        }

        public BoardTile this[Vector3Int vector]
        {
            get { return m_board[vector.x, vector.y, vector.z]; }
        }

        #endregion


        /**
         * Any Unity Methods used.
         */
        #region Unity Methods

        private void Start()
        {
            // DEBUG
            GenerateBoard(5);
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

        public void GenerateBoard(int boardSize)
        {
            m_board = new BoardTile[boardSize, boardSize, boardSize];

            foreach (int z in Enumerable.Range(0, boardSize))
            {
                foreach (int y in Enumerable.Range(0, boardSize))
                {
                    foreach (int x in Enumerable.Range(0, boardSize))
                    {
                        m_board[x, y, z] = new BoardTile(
                            Instantiate(m_tile, 
                                        new Vector3(x + (x * m_tileOffset), y + (y * m_tileOffset), z + (z * m_tileOffset)), 
                                        Quaternion.Euler(0, 0, 0), 
                                        this.transform), 
                            null);
                        m_board[x, y, z].Tile.name = "Tile " + x + "," + y + "," + z;
                    }
                }
            }
        }

        public bool IsTileAvailable(uint x, uint y, uint z)
        {
            return m_board[x, y, z].Piece != null;
        }

        public bool IsTileAvailable(Vector3Int vector)
        {
            return m_board[vector.x, vector.y, vector.z].Piece != null;
        }

        public void HighlightTile(uint x, uint y, uint z)
        {
            // Currently empty, need Aaron's component
        }

        public void HighlightTile(Vector3Int vector)
        {
            // Currently empty, need Aaron's component
        }

        #endregion


        /**
         * Private functions that are only used from
         * within this class.
         */
        #region Member Functions

        #endregion
    }
}
