using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Collections;

// took Board.cs from backend branch, adding in BoardManager.cs code (Aaron)

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
        [SerializeField] [ReadOnly] int m_boardSize = 7;

        [Header("Tile information")]
        [SerializeField] [ReadOnly] float m_cubeSize = 1.0f;
        [SerializeField] [ReadOnly] public float m_cubeOffset = 0.5f;

        [Header("Tile prefab and information")]
        [SerializeField] [Tooltip("The tile prefab")] GameObject m_tile;
        [SerializeField] [Tooltip("The highlighted tile prefab")] GameObject m_highlight_tile;
        [SerializeField] [Tooltip("The distance between tiles")] float m_tileOffset;

        [Header("Piece prefab and information")]
        [SerializeField] [Tooltip("List of piece prefabs")] List<GameObject> m_piecePrefabs;

        // maps the name of the pieces to the prefabs
        // used to generate new pieces
        Dictionary<string, GameObject> m_shogiPiecePrefabs;

        // use to face pieces the other way (180)
        Quaternion m_flipDirection = Quaternion.Euler(0, 180, 0);

        // who's turn is it
        public bool isPlayer1Turn = true;

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

        /* set which side the pieces are spawning and which direction they're facing
         *
         * Params:
         *  1. isPlayer1 - determines which player
         *  2. isPawn - determines if Pawn (the only piece to start in front currently)
         *  3. z - the value determining which row a piece is on (varies depending on player)
         *  4. q - value determining which direct the piece object faces (looks the same atm, bc we're using cubes)
         *
         *  Returns: (through using "out" param)
         *  1. z - set which row piece should spawn on
         *  2. q - set which direction piece should face
         *
         */
         void setPlayerSide(bool isPlayer1, bool isPawn, out int z, out Quaternion q)
        {
            // distance from middle (0, 0, 0) to outer edges
            // i.e. board size 7, radius is 3/-3
            int radius = m_boardSize / 2;

            // quaternion.identity means no rotation
            q = Quaternion.identity;

            // pawns are in the front (2nd row)
            if (isPawn)
            {
                z = -radius + 1;
            }
            else
            {
                // otherwise put the pieces in the back (1st row)
                z = -radius;
            }

            // set based on player 1 or 2
            if (!isPlayer1)
            {
                // player 2 is on the other side of the board
                z *= -1;

                // face the other direction (180 flip)
                q = m_flipDirection;
            }
        }


        /* gets the location of the center of where the tile should be in space based on xyz coordinates
         * used to spawn cubes/pieces
         *
         * Params:
         *  1. x - which row of m_board (if looking top down)
         *  2. y - what height level
         *  3. z - which column of m_board(top down view)
         *
         *  Returns:
         *     origin: Vector3 of location where cube center should be 
         */
        public Vector3 GetCubeCenter(int x, int y, int z)
        {
            // distance from middle (0, 0, 0) to outer edges
            // i.e. board size 7, edge is at 3/-3
            int radius = m_boardSize / 2;

            Vector3 origin = Vector3.zero;

            // 7x7x7 board, a piece at [1, 1, 1] in the array is at (-2, -2, -2) in space
            // offset is simply placing the GO in th center
            origin.x += (m_cubeSize * (x - radius)) + m_cubeOffset;
            origin.y += (m_cubeSize * (y - radius)) + m_cubeOffset;
            origin.z += (m_cubeSize * (z - radius)) + m_cubeOffset;

            return origin;
        }

        #endregion


        // ----------------------- TO DO --------------------------

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