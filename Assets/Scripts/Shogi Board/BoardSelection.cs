using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSelection : MonoBehaviour {

    // ----------------------- FIELDS RELATED TO SELECTING/MOVING PIECES -----------------------

    // the coordinates of the currently clicked/selected spot
    private int currentX = -1;
    private int currentY = -1;
    private int currentZ = -1;

    // the current selected piece
    public ShogiPiece selectedPiece;

    // the current hovered spot
    private GameObject hoveredSpot;

    // materials used to show hover vs resting cube
    public Material hoverMaterial;
    public Material restingMaterial;

    // GameObject used to show the movement/attack range of a pieve
    public GameObject rangeCube;

    // list of range cubes
    private List<GameObject> rangeCubes;

    private bool[,,] allowedMoves;


    void Start () {
        rangeCubes = new List<GameObject>();
	}
	
	void Update () {
        checkHover();
        checkClick();
    }

    // ----------------------- START BOARD CLICKING/SELECTING CODE -----------------------

    // update the current location of the mouse and highlight pieces
    private void checkHover()
    {
        RaycastHit hit;

        // 1. ray provided is from camera to screen point (mouse position)
        // 2. out is the result of the collision
        // 3. 50 is max distance of the ray
        // 4. layer mask (have the ray only hit the chess board and not the piece
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50.0f, LayerMask.GetMask("PieceLayer")))
        {
            // set the current position of the mouse
            currentX = (int)Mathf.Floor(hit.point.x);
            currentY = (int)Mathf.Floor(hit.point.y);
            currentZ = (int)Mathf.Floor(hit.point.z); 

            //Debug.Log("hovering over: " + currentX + " " + currentY + " " + currentZ);

            // Note: currently hovering over the colored block (smaller) and not the surrounding block (fills entire space)
            // the surrounding block sometimes pushes your cursor over to the next block, since it perfectly fills the space to the point where (1, 1.75) becomes (1, 2)
            // 

            if (BoardManager.Instance.shogiSpots[currentX, currentY, currentZ] != null)
            {
                // this if should always be true currently, with only board spots with pieces in them being shown

                // change the color of the previous spot back to normal
                if (hoveredSpot != null)
                {
                    hoveredSpot.GetComponent<Renderer>().material = restingMaterial;
                }


                BoardManager.Instance.shogiSpots[currentX, currentY, currentZ].GetComponent<Renderer>().material = hoverMaterial;
                hoveredSpot = BoardManager.Instance.shogiSpots[currentX, currentY, currentZ];
            }
        }

        else
        {
            // change the color back to normal
            if (hoveredSpot != null)
            {
                hoveredSpot.GetComponent<Renderer>().material = restingMaterial;
            }

            // set the hovered spot to null (not hovering over anything
            hoveredSpot = null;

            currentX = -1;
            currentY = -1;
            currentZ = -1;
        }
    }

    private void checkClick()
    {
        // check for click
        if (Input.GetMouseButtonDown(0))
        {


            Debug.Log("Clicked on " + currentX + currentY + currentZ);

            // if you clicked within the board
            if (currentX >= 0 && currentY >= 0 && currentZ >= 0)
            {
                // if you have clicked on a piece already, click on it
                if (selectedPiece == null)
                {
                    Debug.Log("clicked on a piece!");
                    // Select the piece
                    SelectShogiPiece(currentX, currentY, currentZ);

                    //Debug.Log(selectedPiece);
                }
                else
                {
                    Debug.Log("piece already clicked on");
                    // go ahead and move the piece
                    //MoveChessman(currentX, currentY, currentZ);
                }
            }

        }
    }

    private void SelectShogiPiece(int x, int y, int z)
    {
        // don't select anything if there's no piece there
        if (BoardManager.Instance.shogiPieces[x, y, z] == null)
        {
            Debug.Log("no piece there?");
            return;
        }

        // don't select the piece if it's not your turn
        if (BoardManager.Instance.shogiPieces[x, y, z].isPlayer1 != BoardManager.Instance.isPlayer1Turn)
        {
            Debug.Log("It's not your turn");
            return;
        }

        //set allowed moves
        allowedMoves = BoardManager.Instance.shogiPieces[x, y, z].PossibleMoves();
        selectedPiece = BoardManager.Instance.shogiPieces[x, y, z];
        showAllowedMoves(allowedMoves);

    }

    // pool the previously used range cubes
    // reuse and create new range cubes as necessary
    private GameObject getRangeCube()
    {
        // return the first available (not active) range cube 
        GameObject cube = rangeCubes.Find(c => !c.activeSelf);

        // if there aren't any available to use, instatiate another one and add it
        if (cube == null)
        {
            cube = Instantiate(rangeCube);
            rangeCubes.Add(cube);
        }

        return cube;
    }

    // for each possible move, render a range cube to indicate it
    public void showAllowedMoves(bool[,,] moves)
    {
        Debug.Log(moves);
        for(int i = 0; i < BoardManager.Instance.BOARD_SIZE; i++)
        {
            for (int j = 0; j < BoardManager.Instance.BOARD_SIZE; j++)
            {
                for (int k = 0; k < BoardManager.Instance.BOARD_SIZE; k++)
                {
                    if (moves[k, j, i])
                    {
                        Debug.Log("found a cube to render");
                        GameObject cube = getRangeCube();
                        cube.SetActive(true);
                        cube.transform.position = new Vector3(k + BoardManager.Instance.CUBE_OFFSET, j + BoardManager.Instance.CUBE_OFFSET, i + BoardManager.Instance.CUBE_OFFSET);
                    }
                }
            }
        }
    }
}
