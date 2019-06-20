using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    // a single spot on the board
    public GameObject cube;

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


    private void Start()
    {
        assignNames();
        createBoard();
        generateStartingPieces();
    }

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
                    GameObject c = Instantiate(cube, new Vector3(i, j, k), Quaternion.identity);
                    c.transform.SetParent(transform);
                }
            }
        }
    }

    private void generateStartingPieces()
    {
        activePieces = new List<GameObject>();
        ShogiPieces = new ShogiPiece[BOARD_SIZE, BOARD_SIZE, BOARD_SIZE];

        // generate line of Pawns in 3rd row
        for (int i  = 0; i < BOARD_SIZE; i++)
        {
            // piece name, x, y, z, direction
            SpawnPiece("Pawn", i, 0, 2, Quaternion.identity);
        }

        // generate Lances at ends of 1st row
        SpawnPiece("Lance", 0, 0, 0, Quaternion.identity);
        SpawnPiece("Lance", BOARD_SIZE - 1, 0, 0, Quaternion.identity);

        // generate Knights next to first row Lances
        SpawnPiece("Knight", 1, 0, 0, Quaternion.identity);
        SpawnPiece("Knight", BOARD_SIZE - 2, 0, 0, Quaternion.identity);

        // generate Silver Generals in 2nd row due to smaller board
        SpawnPiece("Silver General", 2, 0, 1, Quaternion.identity);
        SpawnPiece("Silver General", BOARD_SIZE - 3, 0, 1, Quaternion.identity);

        // generate Golden Generals in 1st row next to center
        SpawnPiece("Gold General", 2, 0, 0, Quaternion.identity);
        SpawnPiece("Gold General", BOARD_SIZE - 3, 0, 0, Quaternion.identity);

        // generate the King in 1st row center
        SpawnPiece("King", 3, 0, 0, Quaternion.identity);
    }

    // spawn a shogi piece on the board
    // provide index of piece prefab, and xyz coordinate location
    private void SpawnPiece(string name, int x, int y, int z, Quaternion direction)
    {
        GameObject piece = Instantiate(nameToPiece[name], GetCubeCenter(x, y, z), direction) as GameObject;

        // give the piece a transform and add it to the board
        piece.transform.SetParent(transform);
        ShogiPieces[x, y, z] = piece.GetComponent<ShogiPiece>();
        
        activePieces.Add(piece);
    }

    private Vector3 GetCubeCenter(int x, int y, int z)
    {
        Vector3 origin = Vector3.zero;
        origin.x += CUBE_SIZE * x;
        origin.y += CUBE_SIZE * y;
        origin.z += CUBE_SIZE * z;

        return origin;
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
