using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    // prefabs for a single spot on the board
    public GameObject cube;
    public GameObject cubeLight;

    // how many cubes long the side of the board is
    private int BOARD_SIZE = 7;

    // how long a cube is
    private float CUBE_SIZE = 1.0f;
    private float CUBE_OFFSET = 0.5f;

    // holds the shogi piece GameObjects in this order: Pawn, Lance, Knight, Silver General, Gold General, Bishop, Rook, King
    // use it to generate pieces
    public List<GameObject> piecePrefabs;
    private Dictionary<string, GameObject> nameToPiece;

    // list of all active pieces
    private List<GameObject> activePieces;

    // 3d array of shogi pieces, representing the board
    public ShogiPiece[,,] ShogiPieces { set; get; }

    // use to face pieces the other way
    private Quaternion flipDirection = Quaternion.Euler(0, 180, 0);


    // ----------------------- FIELDS RELATED TO SELECTING/MOVING PIECES -----------------------

    // the coordinates of the currently clicked/selected spot
    private int clickedX = -1;
    private int clickedY = -1;
    private int clickedZ = -1;

    // the current selected piece
    public ShogiPiece selectedPiece;


    // --------------------------------------------------------------------------------------------


    private void Start()
    {
        assignNames();
        generateStartingPieces();
        createBoard();
    }

	// Update is called once per frame
	void Update () {

        // check for main camera
        if (!Camera.main)
        {
            return;
        }

        checkClick();
	}

    // ----------------------- START BOARD GENERATION CODE -----------------------

    // assign the list of Shogi Piece prefabs to their string names
    private void assignNames()
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
    private void createBoard()
    {
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                for (int k = 0; k < BOARD_SIZE; k++)
                {
                    GameObject c;
                    if (ShogiPieces[k, j, i] != null)
                    {
                        c = Instantiate(cubeLight, new Vector3(k, j, i), Quaternion.identity);
                        c.transform.SetParent(transform);

                    }
                    else
                    {
                        //c = Instantiate(cube, new Vector3(i, j, k), Quaternion.identity);
                    }
                }
            }
        }
    }

    // render the pieces for both teams
    private void generateStartingPieces()
    {
        activePieces = new List<GameObject>();
        ShogiPieces = new ShogiPiece[BOARD_SIZE, BOARD_SIZE, BOARD_SIZE];

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
            SpawnPiece("Pawn", x, BOARD_SIZE - 2, z, q);
            SpawnPiece("Pawn", x, 1, z, q);
        }

        // spawn middle height pawns
        for (int x = 0; x < BOARD_SIZE; x++)
        {
            SpawnPiece("Pawn", x, (int)BOARD_SIZE / 2, z, q);
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

        SpawnPiece("Lance", 0, 3, z, q);
        SpawnPiece("Lance", BOARD_SIZE - 1, 3, z, q);
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
        SpawnPiece("Knight", 1, BOARD_SIZE - 2, z, q);
        SpawnPiece("Knight", 1, 1, z, q);
        SpawnPiece("Knight", BOARD_SIZE - 2, BOARD_SIZE - 2, z, q);
        SpawnPiece("Knight", BOARD_SIZE - 2, 1, z, q);
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
            SpawnPiece("Silver General", 2, 2, z, q);
            SpawnPiece("Silver General", 4, 4, z, q);
        }
        else
        {
            SpawnPiece("Silver General", 2, 4, z, q);
            SpawnPiece("Silver General", 4, 2, z, q);
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
            SpawnPiece("Gold General", 2, 4, z, q);
            SpawnPiece("Gold General", 4, 2, z, q);
        }
        else
        {
            SpawnPiece("Gold General", 2, 2, z, q);
            SpawnPiece("Gold General", 4, 4, z, q);
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
            SpawnPiece("Bishop", BOARD_SIZE - 2, 3, z, q);
        }
        else
        {
            SpawnPiece("Bishop", 1, 3, z, q);
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
            SpawnPiece("Rook", 1, 3, z, q);
        }
        else
        {
            SpawnPiece("Rook", BOARD_SIZE - 2, 3, z, q);
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
        SpawnPiece("King", 3, 3, z, q);
    }

    // set which side the pieces are spawning and which direction they're facing
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

    // spawn a shogi piece on the board
    // provide index of piece prefab, and xyz coordinate location
    private void SpawnPiece(string name, int x, int y, int z, Quaternion direction)
    {
        GameObject piece = Instantiate(nameToPiece[name], GetCubeCenter(x, y, z), direction) as GameObject;

        // give the piece a transform and add it to the board
        piece.transform.SetParent(transform);
        ShogiPieces[x, y, z] = piece.GetComponent<ShogiPiece>();
        //ShogiPieces[x, y, z].SetPosition(x, y, z);
        activePieces.Add(piece);
    }

    // gets the center of where the cube should be based on xyz
    private Vector3 GetCubeCenter(int x, int y, int z)
    {
        Vector3 origin = Vector3.zero;
        origin.x += CUBE_SIZE * x;
        origin.y += CUBE_SIZE * y;
        origin.z += CUBE_SIZE * z;

        return origin;
    }

    // ----------------------- END BOARD GENERATION CODE -----------------------

    // ----------------------- START BOARD CLICKING/SELECTING CODE -----------------------

    private void checkClick()
    {
        // check for click
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            // 1. ray provided is from camera to screen point (mouse position)
            // 2. out is the result of the collision
            // 3. 50 is max distance of the ray
            // 4. layer mask (have the ray only hit the chess board and not the piece
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50.0f, LayerMask.GetMask("PieceLayer")))
            {
                // if mouseover ray hits something

                // set the current position of the mouse
                clickedX = (int)hit.point.x;
                clickedY = (int)hit.point.y;
                clickedZ = (int)hit.point.z;

                // if you clicked within the board
                if (clickedX >= 0 && clickedY >= 0 && clickedZ >= 0)
                {
                    // if you have clicked on a piece already, click on it
                    if (selectedPiece == null)
                    {
                        // Select the piece
                        Select(clickedX, clickedY, clickedZ);
                    }
                }

            } else
            {
                clickedX = -1;
                clickedY = -1;
                clickedZ = -1;
            }
        }
    }








    // generate a cube from code ----------------  NOT BEING USED  ---------------------
    private void generateCube()
    {
        // creating a cube at 0, 0, 0
        Vector3[] vertices = getVertices(new Vector3(0, 0, 0));
        int[] triangles = getTriangles();

        createMesh(vertices, triangles);
    }

    // param: Vector3 of the bottom left back corner
    // return: all the other vertices that make it a cube
    private Vector3[] getVertices(Vector3 v)
    {

        Vector3[] vertices = {
            new Vector3 (v.x, v.y, v.z),
            new Vector3 (v.x + 1, v.y, v.z),
            new Vector3 (v.x + 1, v.y + 1, v.z),
            new Vector3 (v.x, v.y + 1, v.z),
            new Vector3 (v.x, v.y + 1, v.z + 1),
            new Vector3 (v.x + 1, v.y + 1, v.z + 1),
            new Vector3 (v.x + 1, 0, v.z + 1),
            new Vector3 (v.x, v.y, v.z + 1),
        };

        return vertices;
    }

    // param: none
    // return: list integers that form the triangles needed to create the cube
    private int[] getTriangles()
    {
        // Note: this is based entirely on the getVertices format
        int[] triangles =
        {
            0, 2, 1, //face front
			0, 3, 2,
            2, 3, 4, //face top
			2, 4, 5,
            1, 2, 5, //face right
			1, 5, 6,
            0, 7, 4, //face left
			0, 4, 3,
            5, 4, 7, //face back
			5, 7, 6,
            0, 6, 7, //face bottom
			0, 1, 6
        };

        return triangles;
    }

    // param: list of Vector3's (vertices) and list of ints (triangles)
    // return: mesh object
    private Mesh createMesh(Vector3[] vertices, int[] triangles)
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }


}   
