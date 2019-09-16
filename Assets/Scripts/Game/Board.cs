using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



namespace Game
{
    /// <summary>
    /// A instance referenceable Board that holds all of the tiles and pieces,
    /// as well as allowing piece movement within the board itself among other
    /// Board related logic.
    /// </summary>
    public class Board : MonoBehaviour
    {
        /**
         * Holds all the data members that the class
         * contains.
         */
        #region Data Members

        BoardTile[,,] m_board;
        static Board m_instance;

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

        /*
         * Modified indexers to the Board class that
         * can take integers as indexes, or a Vector3Int
         * object
         */
        public BoardTile this[uint x, uint y, uint z]
        {
            get { return m_board[x, y, z]; }
        }

        public BoardTile this[Vector3Int vector]
        {
            get { return m_board[vector.x, vector.y, vector.z]; }
        }

        /*
         * Allows a publicly accessible board instance
         * for easy access to the Board itself
         */
        public static Board Instance
        {
            get { return m_instance; }
        }

        #endregion


        /**
         * Any Unity Methods used.
         */
        #region Unity Methods

        #endregion


        /**
         * Constructors that are called when building
         * the class.
         */
        #region Constructors

        private void Start()
        {
            // Save the current instance of this class
            m_instance = this;
        }
        /*
        // DEBUG
        bool doOnce = false;
        private void Update()
        {
            if (!doOnce)
            {
                GenerateBoard(5);
                PlacePiece(Instantiate(m_tile, m_board[0, 0, 0].Tile.transform, true), m_board[0, 0, 0].Position);
                Debug.Log("Start"); Tools.Wait(5f, this); Debug.Log("End");
                //Tools.InterpolateVector3Position(m_board[0, 0, 0].Piece.transform, m_board[0, 0, 0].Position, m_board[4, 4, 4].Position, 5f, this);
                //GenerateBoard(4);

                doOnce = true;
            }
        }
        */
        #endregion


        /**
         * Methods that are able to be called from
         * outside of the class.
         */
        #region Public Methods

        /*
         * Generates a board of a specified size given the class's
         * m_tileOffset (distance between tiles) and saves the
         * generated board into m_board.
         */
        public void GenerateBoard(int boardSize)
        {
            if (m_board != null)
                DestroyBoard();

            // Set m_board to specified value size
            m_board     = new BoardTile[boardSize, boardSize, boardSize];
            m_boardSize = boardSize;
            float boardCenterOffsetOrigin;
            float boardCenterOffset;

            // If the boardSize an even size, then the center of
            // the board is the edge of the two center cubes.
            // Otherwise the center is the central cube.
            if (boardSize % 2 == 0)
            {
                boardCenterOffsetOrigin = -(boardSize / 2 * (1 + m_tileOffset)) + 0.5f;
                boardCenterOffset       = 1 + m_tileOffset;
            }
            else
            {
                boardCenterOffsetOrigin = -(Mathf.Floor(boardSize / 2) * (1 + m_tileOffset));
                boardCenterOffset       = 1 + m_tileOffset;
            }

            // Generate a cube made of cubes given an interval
            // centered on the center most tile.
            foreach (int z in Enumerable.Range(0, boardSize))
            {
                foreach (int y in Enumerable.Range(0, boardSize))
                {
                    foreach (int x in Enumerable.Range(0, boardSize))
                    {
                        m_board[x, y, z] = new BoardTile(
                            new Vector3Int(x, y, z),
                            Instantiate(m_tile, 
                                        new Vector3(boardCenterOffsetOrigin + (x * boardCenterOffset), 
                                                    boardCenterOffsetOrigin + (y * boardCenterOffset), 
                                                    boardCenterOffsetOrigin + (z * boardCenterOffset)), 
                                        Quaternion.Euler(0, 0, 0), 
                                        this.transform), 
                             null);
                        m_board[x, y, z].Tile.name = "Tile " + x + "," + y + "," + z;
                    }
                }
            }
        }

        /*
         * Takes the current saved board in m_board and the scene
         * and destroyed all associated tile and piece objects.
         */
        public void DestroyBoard()
        {
            foreach (int z in Enumerable.Range(0, m_boardSize))
            {
                foreach (int y in Enumerable.Range(0, m_boardSize))
                {
                    foreach (int x in Enumerable.Range(0, m_boardSize))
                    {
                        DestroyTile(m_board[x, y, z]);
                        m_board[x, y, z] = null;
                    }
                }
            }
        }

        /*
         * Places a piece to a specific tile.
         */
        public void PlacePiece(GameObject piece, uint x, uint y, uint z)
        {
            if (m_board[x, y, z].Piece)
            {
                Destroy(m_board[x, y, z].Piece);
                m_board[x, y, z].Piece = piece;
            }
            else
            {
                m_board[x, y, z].Piece = piece;
            }
        }

        /*
         * Places a piece to a specific tile.
         */
        public void PlacePiece(GameObject piece, Vector3Int coordinate)
        {
            if (m_board[coordinate.x, coordinate.y, coordinate.z].Piece)
            {
                Destroy(m_board[coordinate.x, coordinate.y, coordinate.z].Piece);
                m_board[coordinate.x, coordinate.y, coordinate.z].Piece = piece;
            }
            else
            {
                m_board[coordinate.x, coordinate.y, coordinate.z].Piece = piece;
            }
        }

        /*
         * Moves a piece to the 2nd set of coordinates. If a piece
         * already exists on the tile, then capture that piece.
         */
        public void MovePiece(uint x1, uint y1, uint z1, uint x2, uint y2, uint z2)
        {
            // Save the current piece into a temp variable and 
            // get rid of the current tile's piece.
            GameObject piece = m_board[x1, y1, z1].Piece;
            Destroy(m_board[x1, y1, z1].Piece);

            // If there is a piece on the new tile, capture it.
            if (m_board[x2, y2, z2].Piece)
            {
                CapturePiece(m_board[x2, y2, z2]);
            }
            else
            {
                m_board[x2, y2, z2].Piece = piece;
            }
        }

        /*
         * Moves a piece to the 2nd set of coordinates. If a piece
         * already exists on the tile, then capture that piece.
         */
        public void MovePiece(Vector3Int coordinates1, Vector3Int coordinates2)
        {
            // Save the current piece into a temp variable and 
            // get rid of the current tile's piece.
            GameObject piece = m_board[coordinates1.x, coordinates1.y, coordinates1.z].Piece;
            Destroy(m_board[coordinates1.x, coordinates1.y, coordinates1.z].Piece);

            // If there is a piece on the new tile, capture it.
            if (m_board[coordinates2.x, coordinates2.y, coordinates2.z].Piece)
            {
                CapturePiece(m_board[coordinates2.x, coordinates2.y, coordinates2.z]);
            }
            else
            {
                m_board[coordinates2.x, coordinates2.y, coordinates2.z].Piece = piece;
            }
        }

        /*
         * The two IsTileAvailable methods check and returns whether
         * a piece exists in the specified tile.
         */
        public bool IsTileAvailable(uint x, uint y, uint z)
        {
            return m_board[x, y, z].Piece != null;
        }

        public bool IsTileAvailable(Vector3Int vector)
        {
            return m_board[vector.x, vector.y, vector.z].Piece != null;
        }

        /*
         * The two HighlightTile methods are currently null
         */
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

        private void DestroyTile(BoardTile tile)
        {
            Destroy(tile.Piece);
            Destroy(tile.Tile);
        }

        private void CapturePiece(BoardTile tile)
        {
            Destroy(tile.Piece);
        }

        #endregion
    }
}
