using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    #region Data Members   

    // use this instance in other scripts
    public static BoardManager Instance { set; get; }

    // GameObject prefabs for a single spot on the board (transparent cubes)

    // cube with green outline
    [SerializeField]
    private GameObject cube;

    // cube with white corners
    [SerializeField]
    private GameObject cubeLight;

    // GameObject prefabs for the shogi pieces
    [SerializeField]
    private List<GameObject> piecePrefabs;

    // maps the gameobjects to the name of the piece
    // use it to generate pieces
    private Dictionary<string, GameObject> nameToPiece;



    // how many cubes long the side of the board is
    public int BOARD_SIZE = 7;

    // how long a cube is
    private float CUBE_SIZE = 1.0f;
    public float CUBE_OFFSET = 0.5f;

    // use to face pieces the other way
    private Quaternion flipDirection = Quaternion.Euler(0, 180, 0);

    // determine who's turn it is
    public bool isPlayer1Turn = true;


    // list of all active pieces
    private List<GameObject> activePieces;

    // 3d array of shogi pieces, representing the board logically 
    public ShogiPiece[,,] shogiPieces { set; get; }

    // 3d array of all shogi spots (not the pieces)
    public GameObject[,,] shogiSpots { set; get; }

    /* Maybe want to use 1 3d array of Board Tiles instead
     *  BoardTiles being objects with a Tile and Piece
     *  Use this array to know where pieces are logically
     *  Track their actual locations using the piece's data members
     */


    #endregion


    #region Member Properties

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
    private void setPlayerSide(bool isPlayer1, bool isPawn, out int z, out Quaternion q)
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
            z = BOARD_SIZE - z - 1;

            // face the other direction (180 flip)
            q = flipDirection;
        }
    }


    /* gets the location of the center of where the cube should be based on xyz coordinates
     * used to spawn cubes/pieces
     *
     * Params:
     *  1. x - which row of shogiPieces (if looking top down)
     *  2. y - what height level
     *  3. z - which column of shogiPieces(top down view)
     *
     *  Returns:
     *     origin: Vector3 of location where cube center should be 
     */
    public Vector3 GetCubeCenter(int x, int y, int z)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (CUBE_SIZE * x) + CUBE_OFFSET;
        origin.y += (CUBE_SIZE * y) + CUBE_OFFSET;
        origin.z += (CUBE_SIZE * z) + CUBE_OFFSET;

        return origin;
    }


    #endregion


    #region Unity Methods

    private void Start()
    {
        // for other scripts to use instance of this script
        Instance = this;
       
        // assign the prefabs to dictionary using their names (["Pawn"] instead of [0])
        AssignNames();

        // spawn all of the starting pieces for both players
        GenerateStartingPieces();

        // spawn the cubes that make up the board
        CreateBoard();
    }

	// Update is called once per frame
	void Update () {

        // check for main camera
        if (!Camera.main)
        {
            return;
        }

	}

    #endregion


    #region Constructors
    // assign the list of Shogi Piece prefabs to their string names
    private void AssignNames()
    {
        nameToPiece = new Dictionary<string, GameObject>();
        nameToPiece.Add("Pawn", piecePrefabs[0]);
        nameToPiece.Add("Lance", piecePrefabs[1]);
        nameToPiece.Add("Knight", piecePrefabs[2]);
        nameToPiece.Add("Silver General", piecePrefabs[3]);
        nameToPiece.Add("Gold General", piecePrefabs[4]);
        nameToPiece.Add("Bishop", piecePrefabs[5]);
        nameToPiece.Add("Rook", piecePrefabs[6]);
        nameToPiece.Add("King", piecePrefabs[7]);
    }

    // instantiates each cube that makes up the board
    private void CreateBoard()
    {
        
        shogiSpots = new GameObject[BOARD_SIZE, BOARD_SIZE, BOARD_SIZE];

        // for every spot on the board, create a cube
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                for (int k = 0; k < BOARD_SIZE; k++)
                {
                    GameObject c;

                    // generate a cube around an existing piece
                    // this used to happen for the whole board, but was too cluttered
                    if (shogiPieces[k, j, i] != null)
                    {
                        // make the cube based on xyz coordinates
                        c = Instantiate(cubeLight, new Vector3(k + CUBE_OFFSET, j + CUBE_OFFSET, i + CUBE_OFFSET), Quaternion.identity);

                        // so that the objects fall under the ShogiBoard object
                        c.transform.SetParent(transform);

                        // set which spots on the board have a cube
                        shogiSpots[k, j, i] = c;

                    }
                    else
                    {
                        // currently not putting actual cubes in the locations where there is no piece
                        // this is to make it easier to click on pieces, and could be changed

                        //c = Instantiate(cube, new Vector3(i, j, k), Quaternion.identity);
                    }
                }
            }
        }
    }

    // render the pieces for both teams
    private void GenerateStartingPieces()
    {
        activePieces = new List<GameObject>();
        shogiPieces = new ShogiPiece[BOARD_SIZE, BOARD_SIZE, BOARD_SIZE];

        // the boolean controls which player we are spawning the pieces for
        // (true is player1, and false is player 2)
        SpawnPawns(true);
        SpawnPawns(false);

        SpawnLances(true);
        SpawnLances(false);

        SpawnKnights(true);
        SpawnKnights(false);

        SpawnSilverGenerals(true);
        SpawnSilverGenerals(false);

        SpawnGoldGenerals(true);
        SpawnGoldGenerals(false);

        SpawnBishops(true);
        SpawnBishops(false);

        SpawnRooks(true);
        SpawnRooks(false);

        SpawnKings(true);
        SpawnKings(false);
    }

    #endregion

    // none
    #region Public Methods

    #endregion


    #region Member Functions
    // Pawns
    private void SpawnPawns(bool isPlayer1)
    {
        // z - row, q - direction 
        int z;
        Quaternion q;

        // change row and direction pieces are facing
        setPlayerSide(isPlayer1, true, out z, out q);

        // spawn top and bottom 3 pawns
        for (int x = 2; x < 5; x++)
        {
            SpawnPiece("Pawn", x, BOARD_SIZE - 2, z, q, isPlayer1);
            SpawnPiece("Pawn", x, 1, z, q, isPlayer1);
        }

        // spawn middle height pawns
        for (int x = 0; x < BOARD_SIZE; x++)
        {
            SpawnPiece("Pawn", x, (int)BOARD_SIZE / 2, z, q, isPlayer1);
        }

    }

    // Lances
    private void SpawnLances(bool isPlayer1)
    {
        // z - row, q - direction 
        int z;
        Quaternion q;

        // change row and direction pieces are facing
        setPlayerSide(isPlayer1, false, out z, out q);

        SpawnPiece("Lance", 0, 3, z, q, isPlayer1);
        SpawnPiece("Lance", BOARD_SIZE - 1, 3, z, q, isPlayer1);
    }

    // Knights
    private void SpawnKnights(bool isPlayer1)
    {
        // z - row, q - direction 
        int z;
        Quaternion q;

        // change row and direction pieces are facing
        setPlayerSide(isPlayer1, false, out z, out q);

        // spawn knights on the inner corners of the closest panel
        SpawnPiece("Knight", 1, BOARD_SIZE - 2, z, q, isPlayer1);
        SpawnPiece("Knight", 1, 1, z, q, isPlayer1);
        SpawnPiece("Knight", BOARD_SIZE - 2, BOARD_SIZE - 2, z, q, isPlayer1);
        SpawnPiece("Knight", BOARD_SIZE - 2, 1, z, q, isPlayer1);
    }

    // Silver Generals
    private void SpawnSilverGenerals(bool isPlayer1)
    {
        // z - row, q - direction 
        int z;
        Quaternion q;

        // change row and direction pieces are facing
        setPlayerSide(isPlayer1, false, out z, out q);


        // mirror player 2's silver generals
        if (!isPlayer1)
        {
            // spawn silver generals near the center
            SpawnPiece("Silver General", 2, 2, BOARD_SIZE - 2, q, isPlayer1);
            SpawnPiece("Silver General", 4, 4, BOARD_SIZE - 2, q, isPlayer1);
        }
        else
        {
            SpawnPiece("Silver General", 2, 4, 1, q, isPlayer1);
            SpawnPiece("Silver General", 4, 2, 1, q, isPlayer1);
        }


    }

    // Gold Generals
    private void SpawnGoldGenerals(bool isPlayer1)
    {
        // z - row, q - direction 
        int z;
        Quaternion q;

        // change row and direction pieces are facing
        setPlayerSide(isPlayer1, false, out z, out q);

        // mirror players 2's gold generals
        if (!isPlayer1)
        {
            // spawn gold generals near the center
            SpawnPiece("Gold General", 2, 4, z, q, isPlayer1);
            SpawnPiece("Gold General", 4, 2, z, q, isPlayer1);
        }
        else
        {
            SpawnPiece("Gold General", 2, 2, z, q, isPlayer1);
            SpawnPiece("Gold General", 4, 4, z, q, isPlayer1);
        }


    }

    // Bishops
    private void SpawnBishops(bool isPlayer1)
    {
        // z - row, q - direction
        int z;
        Quaternion q;

        // change row and direction pieces are facing
        setPlayerSide(isPlayer1, false, out z, out q);

        if (!isPlayer1)
        {
            // spawn a Bishop on the left of the King
            SpawnPiece("Bishop", BOARD_SIZE - 2, 3, z, q, isPlayer1);
        }
        else
        {
            SpawnPiece("Bishop", 1, 3, z, q, isPlayer1);
        }

    }

    // Rooks
    private void SpawnRooks(bool isPlayer1)
    {
        // z - row, q - direction
        int z;
        Quaternion q;

        // change row and direction pieces are facing
        setPlayerSide(isPlayer1, false, out z, out q);

        if (!isPlayer1)
        {
            // spawn a Bishop on the left of the King
            SpawnPiece("Rook", 1, 3, z, q, isPlayer1);
        }
        else
        {
            SpawnPiece("Rook", BOARD_SIZE - 2, 3, z, q, isPlayer1);
        }
    }

    // Kings
    private void SpawnKings(bool isPlayer1)
    {
        // z - row, q - direction
        int z;
        Quaternion q;

        // change row and direction pieces are facing
        setPlayerSide(isPlayer1, false, out z, out q);

        // spawn king in center
        SpawnPiece("King", 3, 3, z, q, isPlayer1);
    }

    // spawn a shogi piece on the board
    // provide index of piece prefab, and xyz coordinate location
    private void SpawnPiece(string name, int x, int y, int z, Quaternion direction, bool isPlayer1)
    {
        GameObject piece = Instantiate(nameToPiece[name], GetCubeCenter(x, y, z), direction) as GameObject;

        // give the piece a transform and add it to the board
        piece.transform.SetParent(transform);
        shogiPieces[x, y, z] = piece.GetComponent<ShogiPiece>();
        shogiPieces[x, y, z].SetPosition(x, y, z);
        activePieces.Add(piece);
        piece.GetComponent<ShogiPiece>().setPlayer(isPlayer1);
    }

    #endregion

}
