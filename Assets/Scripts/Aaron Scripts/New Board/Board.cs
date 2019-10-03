using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Collections;

// took Board.cs from backend branch, adding in BoardManager.cs code (Aaron)

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

        // this is (0, 0, 0) to (7, 7, 7), but pieces will be at (-3, -3, -3) to (3, 3, 3) in space
        static public BoardTile[,,] m_board;
        static Board m_instance;

        [Header("Board information")]
        [SerializeField] [ReadOnly] static int m_boardSize = 7;

        [Header("Tile information")]
        [SerializeField] [ReadOnly] float m_cubeSize = 1.0f;
        [SerializeField] [ReadOnly] public float m_cubeOffset = 0.5f;

        [Header("Tile prefab and information")]
        [SerializeField] [Tooltip("The tile prefab")] GameObject m_tile;
        [SerializeField] [Tooltip("The highlighted tile prefab")] GameObject m_highlight_tile;
        [SerializeField] [Tooltip("The distance between tiles")] static float m_tileOffset;

        [Header("Piece prefab and information")]
        [SerializeField] [Tooltip("List of piece prefabs")] List<GameObject> m_piecePrefabs;

        // maps the name of the pieces to the prefabs
        // used to generate new pieces
        Dictionary<string, GameObject> m_shogiPiecePrefabs;

        // use to face pieces the other way (180)
        Quaternion m_flipDirection = Quaternion.Euler(0, 180, 0);

        // who's turn is it
        static bool m_isPlayer1Turn = true;

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

        public static BoardTile[,,] board
        {
            get { return m_board; }
        }

        public static int boardSize
        {
            get { return m_boardSize; }
        }

        public static bool isPlayer1Turn
        {
            get { return m_isPlayer1Turn; }
        }

        public static float tileOffset
        {
            get { return m_tileOffset; }
        }

        public static void changeTurns()
        {
            m_isPlayer1Turn = !m_isPlayer1Turn;
        }


        /* set which side the pieces are spawning and which direction they're facing
         * this is relative to the logical board (not their physical locations)
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
        void SetPlayerSide(bool isPlayer1, bool isPawn, out int z, out Quaternion q)
        {
            // quaternion.identity means no rotation
            q = Quaternion.identity;

            // pawns are in the front (2nd row)
            if (isPawn)
            {
                z = 1;
            }
            else
            {
                // otherwise put the pieces in the back (1st row)
                z = 0;
            }

            // set based on player 1 or 2
            if (!isPlayer1)
            {
                // player 2 is on the other side of the board
                z = m_boardSize - z - 1;

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
        public static Vector3 GetPieceCenter(int x, int y, int z)
        {
            float boardCenterOffsetOrigin = GetCenterOffsetOrigin();
            float boardCenterOffset = 1 + m_tileOffset;

            Vector3 origin = Vector3.zero;

            origin.x += boardCenterOffsetOrigin + (x * boardCenterOffset);
            origin.y += boardCenterOffsetOrigin + (y * boardCenterOffset);
            origin.z += boardCenterOffsetOrigin + (z * boardCenterOffset);

            return origin;
        }

        /*
         * Calculate where the origin of the board is in space
         */
        public static float GetCenterOffsetOrigin()
        {

            // If the boardSize an even size, then the center of
            // the board is the edge of the two center cubes.
            // Otherwise the center is the central cube.
            if (boardSize % 2 == 0)
            {
                return -(boardSize / 2 * (1 + m_tileOffset)) + 0.5f;
            }
            else
            {
                return -(Mathf.Floor(boardSize / 2) * (1 + m_tileOffset));
            }
        }


        /* Convert spacial coordinates to logical coordinates (3D array)
         * Essentially reverses the GetPieceCenter function
         */
        public static Vector3 SpaceToArrayCoordinates(int x, int y, int z)
        {
            float offset = GetCenterOffsetOrigin();

            float arrayX = (x - offset) / (1 + tileOffset);
            float arrayY = (y - offset) / (1 + tileOffset);
            float arrayZ = (z - offset) / (1 + tileOffset);

            return new Vector3(arrayX, arrayY, arrayZ);
        }


        #endregion



        /**
         * Any Unity Methods used.
         */
        #region Unity Methods

        private void Start()
        {
            // Save the current instance of this class
            m_instance = this;

            AssignNames();

            GenerateBoard(m_boardSize);

            GenerateStartingPieces();
        }

        #endregion


        /**
         * Constructors that are called when building
         * the class.
         */
        #region Constructors

        // assign the list of Shogi Piece prefabs to their string names (just for our own use)
        private void AssignNames()
        {
            m_shogiPiecePrefabs = new Dictionary<string, GameObject>
            {
                { "Pawn", m_piecePrefabs[0] },
                { "Lance", m_piecePrefabs[1] },
                { "Knight", m_piecePrefabs[2] },
                { "Silver General", m_piecePrefabs[3] },
                { "Gold General", m_piecePrefabs[4] },
                { "Bishop", m_piecePrefabs[5] },
                { "Rook", m_piecePrefabs[6] },
                { "King", m_piecePrefabs[7] }
            };
        }


        // ----------------------- TO DO: generate starting board with BoardTiles correctly 

        // instantiates the Piece objects
        void GenerateStartingPieces()
        {
            SpawnPawns();
            SpawnLances();
            SpawnKnights();
            SpawnSilverGenerals();
            SpawnGoldGenerals();
            SpawnBishops();
            SpawnRooks();
            SpawnKings();
        }
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

            // Generate a cube made of cubes given an interval
            // centered on the center most tile.
            foreach (int z in Enumerable.Range(0, boardSize))
            {
                foreach (int y in Enumerable.Range(0, boardSize))
                {
                    foreach (int x in Enumerable.Range(0, boardSize))
                    {

                        // TODO: Change this to spawn based on (0, 0, 0) origin
                        m_board[x, y, z] = new BoardTile(
                            new Vector3Int(x, y, z),
                            Instantiate(m_tile,
                                        GetPieceCenter(x, y, z),
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
         * (Logically?) Places a piece to a specific tile.
         */
        public void PlacePiece(Piece piece, uint x, uint y, uint z)
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
        public void PlacePiece(Piece piece, Vector3Int coordinate)
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

        /* (check for which team the piece is on)
         * Moves a piece to the 2nd set of coordinates. If a piece
         * already exists on the tile, then capture that piece.
         */
        public void MovePiece(uint x1, uint y1, uint z1, uint x2, uint y2, uint z2)
        {
            // Save the current piece into a temp variable and 
            // get rid of the current tile's piece.
            Piece piece = m_board[x1, y1, z1].Piece;
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
            Piece piece = m_board[coordinates1.x, coordinates1.y, coordinates1.z].Piece;
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

        // spawn a shogi piece on the board
        // provide index of piece prefab, and xyz coordinate location
        private void SpawnPiece(string pieceName, int x, int y, int z, Quaternion direction, bool isPlayer1)
        {
            GameObject piece = Instantiate(m_shogiPiecePrefabs[pieceName], GetPieceCenter(x, y, z), direction) as GameObject;

            // give the piece a transform and add it to the board
            piece.transform.SetParent(transform);

            // set position of Piece (not using piece class yet)
            // piece.setPosition(x, y, z);

            m_board[x, y, z].Piece = piece.GetComponent<Piece>(); // do we GetComponent Piece here?
            m_board[x, y, z].Piece.setPosition(x, y, z);

            // piece.GetComponent<ShogiPiece>().setPlayer(isPlayer1);
        }

        void SpawnPawns()
        {
             /*
             *  z = 1 
             *  .  .  .  .  .  .  .
             *  .  .  p  p  p  .  .
             *  .  .  .  .  .  .  .
             *  p  p  p  p  p  p  p
             *  .  .  .  .  .  .  .
             *  .  .  p  p  p  .  .
             *  .  .  .  .  .  .  .
             * 
             * */
            // z - row, q - direction
            int z;
            Quaternion q;

            // spawn top and bottom 3 pawns             
            for (int x = 2; x < 5; x++)
            {
                // -------- PLAYER 1:

                // set the z and q for player 1 (row/direction facing)
                // isPlayer1, isPawn, output z, output q
                SetPlayerSide(true, true, out z, out q);

                // second to last row from the top
                SpawnPiece("Pawn", x, m_boardSize - 2, z, q, true);

                // second to last row from the bottom
                SpawnPiece("Pawn", x, 1, z, q, true);


                // -------- PLAYER 2:

                SetPlayerSide(false, true, out z, out q);

                // second to last row from the top
                SpawnPiece("Pawn", x, m_boardSize - 2, z, q, false);

                // second to last row from the bottom
                SpawnPiece("Pawn", x, 1, z, q, false);
            }

            // spawn middle row
            for (int x = 0; x < m_boardSize; x++)
            {
                // -------- PLAYER 1:

                // set the z and q for player 1 (row/direction facing)
                // isPlayer1, isPawn, output z, output q
                SetPlayerSide(true, true, out z, out q);

                int middle = (int)(Mathf.Ceil(m_boardSize / 2.0f) - 1);

                SpawnPiece("Pawn", x, middle, z, q, true);

                // -------- PLAYER 2:

                SetPlayerSide(false, true, out z, out q);

                SpawnPiece("Pawn", x, middle, z, q, false);
            }

        }

        void SpawnLances()
        {
            /*
            *  z = 0
            *  .  .  .  .  .  .  .
            *  .  .  .  .  .  .  .
            *  .  .  .  .  .  .  .
            *  l  .  .  .  .  .  l
            *  .  .  .  .  .  .  .
            *  .  .  .  .  .  .  .
            *  .  .  .  .  .  .  .
            * 
            * */

            // z - row, q - direction
            int z;
            Quaternion q;


            // -------- PLAYER 1:

            // set the z and q for player 1 (row/direction facing)
            // isPlayer1, isPawn, output z, output q
            SetPlayerSide(true, false, out z, out q);

            int middle = (int)(Mathf.Ceil(m_boardSize / 2.0f) - 1);

            SpawnPiece("Lance", 0, middle, z, q, true);
            SpawnPiece("Lance", m_boardSize - 1, middle, z, q, true);

            // -------- PLAYER 2:

            SetPlayerSide(false, false, out z, out q);
            SpawnPiece("Lance", 0, middle, z, q, false);
            SpawnPiece("Lance", m_boardSize - 1, middle, z, q, false);
        }

        void SpawnKnights()
        {
            /*
            *  z = 0
            *  .  .  .  .  .  .  .
            *  .  k  .  .  .  k  .
            *  .  .  .  .  .  .  .
            *  .  .  .  .  .  .  .
            *  .  .  .  .  .  .  .
            *  .  k  .  .  .  k  .
            *  .  .  .  .  .  .  .
            * 
            * */

            // z - row, q - direction
            int z;
            Quaternion q;

            // -------- PLAYER 1:

            // set the z and q for player 1 (row/direction facing)
            // isPlayer1, isPawn, output z, output q
            SetPlayerSide(true, false, out z, out q);

            SpawnPiece("Knight", 1, 1, z, q, true);
            SpawnPiece("Knight", m_boardSize - 2, 1, z, q, true);
            SpawnPiece("Knight", 1, m_boardSize - 2, z, q, true);
            SpawnPiece("Knight", m_boardSize - 2, m_boardSize - 2, z, q, true);


            // -------- PLAYER 2:

            SetPlayerSide(false, false, out z, out q);

            SpawnPiece("Knight", 1, 1, z, q, false);
            SpawnPiece("Knight", m_boardSize - 2, 1, z, q, false);
            SpawnPiece("Knight", 1, m_boardSize - 2, z, q, false);
            SpawnPiece("Knight", m_boardSize - 2, m_boardSize - 2, z, q, false);

        }

        void SpawnSilverGenerals()
        {
            /*
            *  z = 0
            *  (flipped for player 2)
            *  .  .  .  .  .  .  .
            *  .  .  .  .  .  .  .
            *  .  .  s  .  .  .  .
            *  .  .  .  .  .  .  .
            *  .  .  .  .  s  .  .
            *  .  .  .  .  .  .  .
            *  .  .  .  .  .  .  .
            * 
            * */

            // z - row, q - direction
            int z;
            Quaternion q;

            // -------- PLAYER 1:

            // set the z and q for player 1 (row/direction facing)
            // isPlayer1, isPawn, output z, output q
            SetPlayerSide(true, false, out z, out q);

            SpawnPiece("Silver General", m_boardSize - 3, 2, z, q, true);
            SpawnPiece("Silver General", 2, m_boardSize - 3, z, q, true);

            // -------- PLAYER 2:

            SetPlayerSide(false, false, out z, out q);

            SpawnPiece("Silver General", m_boardSize - 3, m_boardSize - 3, z, q, false);
            SpawnPiece("Silver General", 2, 2, z, q, false);
        }

        void SpawnGoldGenerals()
        {
            /*
            *  z = 0
            *  (flipped for player 2)
            *  .  .  .  .  .  .  .
            *  .  .  .  .  .  .  .
            *  .  .  .  .  g  .  .
            *  .  .  .  .  .  .  .
            *  .  .  g  .  .  .  .
            *  .  .  .  .  .  .  .
            *  .  .  .  .  .  .  .
            * 
            * */

            // z - row, q - direction
            int z;
            Quaternion q;

            // -------- PLAYER 1:

            // set the z and q for player 1 (row/direction facing)
            // isPlayer1, isPawn, output z, output q
            SetPlayerSide(true, false, out z, out q);

            SpawnPiece("Gold General", m_boardSize - 3, m_boardSize - 3, z, q, true);
            SpawnPiece("Gold General", 2, 2, z, q, true);

            // -------- PLAYER 2:

            SetPlayerSide(false, false, out z, out q);

            SpawnPiece("Gold General", m_boardSize - 3, 2, z, q, false);
            SpawnPiece("Gold General", 2, m_boardSize - 3, z, q, false);
        }

        void SpawnBishops()
        {
            /*
            *  z = 0
            *  (flipped for player 2)
            *  .  .  .  .  .  .  .
            *  .  .  .  .  .  .  .
            *  .  .  .  .  .  .  .
            *  .  b  .  .  .  .  .
            *  .  .  .  .  .  .  .
            *  .  .  .  .  .  .  .
            *  .  .  .  .  .  .  .
            * 
            * */

            // z - row, q - direction
            int z;
            Quaternion q;

            int middle = (int)(Mathf.Ceil(m_boardSize / 2.0f) - 1);


            // -------- PLAYER 1:

            // set the z and q for player 1 (row/direction facing)
            // isPlayer1, isPawn, output z, output q
            SetPlayerSide(true, false, out z, out q);

            SpawnPiece("Bishop", 1, middle, z, q, true);

            // -------- PLAYER 2:

            SetPlayerSide(false, false, out z, out q);

            SpawnPiece("Bishop", m_boardSize - 2, middle, z, q, false);
        }

        void SpawnRooks()
        {
            /*
            *  z = 0
            *  (flipped for player 2)
            *  .  .  .  .  .  .  .
            *  .  .  .  .  .  .  .
            *  .  .  .  .  .  .  .
            *  .  .  .  .  .  r  .
            *  .  .  .  .  .  .  .
            *  .  .  .  .  .  .  .
            *  .  .  .  .  .  .  .
            * 
            * */

            // z - row, q - direction
            int z;
            Quaternion q;

            int middle = (int)(Mathf.Ceil(m_boardSize / 2.0f) - 1);


            // -------- PLAYER 1:

            // set the z and q for player 1 (row/direction facing)
            // isPlayer1, isPawn, output z, output q
            SetPlayerSide(true, false, out z, out q);

            SpawnPiece("Rook", m_boardSize - 2, middle, z, q, true);

            // -------- PLAYER 2:

            SetPlayerSide(false, false, out z, out q);

            SpawnPiece("Rook", 1, middle, z, q, false);
        }

        void SpawnKings()
        {
            /*
            *  z = 0
            *  
            *  .  .  .  .  .  .  .
            *  .  .  .  .  .  .  .
            *  .  .  .  .  .  .  .
            *  .  .  .  k  .  .  .
            *  .  .  .  .  .  .  .
            *  .  .  .  .  .  .  .
            *  .  .  .  .  .  .  .
            * 
            * */

            // z - row, q - direction
            int z;
            Quaternion q;

            int middle = (int)(Mathf.Ceil(m_boardSize / 2.0f) - 1);


            // -------- PLAYER 1:

            // set the z and q for player 1 (row/direction facing)
            // isPlayer1, isPawn, output z, output q
            SetPlayerSide(true, false, out z, out q);

            SpawnPiece("King", middle, middle, z, q, true);

            // -------- PLAYER 2:

            SetPlayerSide(false, false, out z, out q);

            SpawnPiece("King", middle, middle, z, q, false);
        }

        #endregion
    }
}