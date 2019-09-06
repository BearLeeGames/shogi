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

        // this is (0, 0, 0) to (7, 7 7), but pieces will be at (-3, -3, -3) to (3, 3, 3)
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
        public Vector3 GetPieceCenter(int x, int y, int z)
        {
            // distance from middle (0, 0, 0) to outer edges
            // i.e. board size 7, edge is at 3/-3
            float radius = (m_boardSize / 2) - m_cubeOffset;

            Vector3 origin = Vector3.zero;

            // 7x7x7 board, a piece at [1, 1, 1] in the array is at (-2, -2, -2) in space
            // offset is simply placing the GO in th center
            origin.x += (m_cubeSize * (x - radius)) + (x * m_tileOffset);
            origin.y += (m_cubeSize * (y - radius)) + (y * m_tileOffset);
            origin.z += (m_cubeSize * (z - radius)) + (z * m_tileOffset);

            return origin;
        }

        #endregion



        /**
         * Any Unity Methods used.
         */
        #region Unity Methods

        private void Start()
        {
            AssignNames();

            // DEBUG
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
            m_shogiPiecePrefabs = new Dictionary<string, GameObject>();
            m_shogiPiecePrefabs.Add("Pawn", m_piecePrefabs[0]);
            m_shogiPiecePrefabs.Add("Lance", m_piecePrefabs[1]);
            m_shogiPiecePrefabs.Add("Knight", m_piecePrefabs[2]);
            m_shogiPiecePrefabs.Add("Silver General", m_piecePrefabs[3]);
            m_shogiPiecePrefabs.Add("Gold General", m_piecePrefabs[4]);
            m_shogiPiecePrefabs.Add("Bishop", m_piecePrefabs[5]);
            m_shogiPiecePrefabs.Add("Rook", m_piecePrefabs[6]);
            m_shogiPiecePrefabs.Add("King", m_piecePrefabs[7]);
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

        public void GenerateBoard(int boardSize)
        {
            m_board = new BoardTile[boardSize, boardSize, boardSize];

            foreach (int z in Enumerable.Range(0, boardSize))
            {
                foreach (int y in Enumerable.Range(0, boardSize))
                {
                    foreach (int x in Enumerable.Range(0, boardSize))
                    {

                        // TODO: Change this to spawn based on (0, 0, 0) origin
                        m_board[x, y, z] = new BoardTile(
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

        // spawn a shogi piece on the board
        // provide index of piece prefab, and xyz coordinate location
        private void SpawnPiece(string name, int x, int y, int z, Quaternion direction, bool isPlayer1)
        {
            GameObject piece = Instantiate(m_shogiPiecePrefabs[name], GetPieceCenter(x, y, z), direction) as GameObject;

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
            *  (flipped for player 2)
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