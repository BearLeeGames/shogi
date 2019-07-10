using System.Collections.Generic;
using UnityEngine;

public class BoardSelection : MonoBehaviour {

    #region Data Members

    // the coordinates of the currently clicked/selected spot
    private int currentX = -1;
    private int currentY = -1;
    private int currentZ = -1;

    // the current selected piece
    private ShogiPiece selectedPiece;

    // the current mouse hovered spot (a CubeLight object)
    private GameObject hoveredSpot;

    // materials used to show hover vs resting cube
    public Material hoverMaterial;
    public Material restingMaterial;

    [SerializeField]
    // material used for rangeCubes
    private Material rangeMaterial;

    [SerializeField]
    // GameObject used to show the movement/attack range of a pieve
    private GameObject rangeCube;



    // list of range cubes
    private List<GameObject> rangeCubes;

    // 3d list of bools showing which locations are allowed
    private bool[,,] allowedMoves;

    #endregion

    #region Member Properties

    // pool the previously used range cubes
    // reuse and create new range cubes as necessary
    private GameObject GetRangeCube()
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

    #endregion

    #region Unity Methods

    void Start () {
        rangeCubes = new List<GameObject>();
	}
	
	void Update () {
        CheckHover();
        CheckClick();
    }

    #endregion

    // none atm
    #region Constructors



    #endregion

    // none atm
    #region Public Methods

    #endregion

    #region Member Functions

    // update the current location of the mouse and highlight pieces
    private void CheckHover()
    {
        RaycastHit hit;

        // 1. ray provided is from camera to screen point (mouse position)
        // 2. out is the result of the collision
        // 3. 50 is max distance of the ray
        // 4. layer mask (have the ray only hit the chess board and not the piece

        // if hovering something
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50.0f, LayerMask.GetMask("PieceLayer")))
        {
            // set the current position of the mouse
            currentX = (int)Mathf.Floor(hit.point.x);
            currentY = (int)Mathf.Floor(hit.point.y);
            currentZ = (int)Mathf.Floor(hit.point.z);

            //Debug.Log("hovering over: " + currentX + " " + currentY + " " + currentZ);

            // Note: currently hovering over the colored block (smaller) and not the surrounding block (fills entire space)
            // the surrounding block sometimes pushes your cursor over to the next block, since it perfectly fills the space to the point where (1, 1.75) becomes (1, 2)

            GameObject currentHover = BoardManager.Instance.shogiSpots[currentX, currentY, currentZ];

            // if there is a cube there
            if (currentHover != null)
            {
                // this if should always be true currently, with only board spots with pieces in them being shown

                // change the design of hovered cubes
                handleHoverCube(currentX, currentY, currentZ);
            }



            // if we have a piece selected, and we're hovering over a range cube
            else if (selectedPiece != null && allowedMoves[currentX, currentY, currentZ])
            {
                Debug.Log("hovering over range cube");
                // change the design of the hovered range cube
                handleHoverCube(currentX, currentY, currentZ);
            }
        }

        else
        {
            // change the color back to normal
            if (hoveredSpot != null)
            {
                if (hoveredSpot.tag == "Range")
                {
                    // change the material of the previously hovered spot back to normal
                    hoveredSpot.GetComponent<Renderer>().material = rangeMaterial;
                }
                else
                {
                    hoveredSpot.GetComponent<Renderer>().material = restingMaterial;
                }
            }

            // set the hovered spot to null (not hovering over anything
            hoveredSpot = null;

            currentX = -1;
            currentY = -1;
            currentZ = -1;
        }
    }

    // handle the material of the cubes being hovered over
    private void handleHoverCube(int currentX, int currentY, int currentZ)
    {
        // if the hovered spot is different from the previously hovered spot
        if (hoveredSpot != BoardManager.Instance.shogiSpots[currentX, currentY, currentZ])
        {

            // if the currently hovered spot isn't null
            if (hoveredSpot != null)
            {

                if (hoveredSpot.tag == "Range")
                {
                    // change the material of the previously hovered spot back to normal
                    hoveredSpot.GetComponent<Renderer>().material = rangeMaterial;
                }
                else
                {
                    hoveredSpot.GetComponent<Renderer>().material = restingMaterial;
                }


            }

            // change the new hovered spot to the hoverMaterial
            BoardManager.Instance.shogiSpots[currentX, currentY, currentZ].GetComponent<Renderer>().material = hoverMaterial;

            // set the new hoveredSpot
            hoveredSpot = BoardManager.Instance.shogiSpots[currentX, currentY, currentZ];
        }
    }

    // check what you've clicked on
    private void CheckClick()
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
            else
            {
                // deactivate the selected piece here
            }

        }
    }

     
    /* select the clicked shogi piece (show shogi piece range)
     * Params:
     *  xyz - coordinates for the shogiPieces board
     *
     */
    private void SelectShogiPiece(int x, int y, int z)
    {
        // don't select anything if there's no piece there
        if (BoardManager.Instance.shogiPieces[x, y, z] == null)
        {
            Debug.Log("No piece there");
            return;
        }
        
        // don't select the piece if it's not your turn
        if (BoardManager.Instance.shogiPieces[x, y, z].isPlayer1 != BoardManager.Instance.isPlayer1Turn)
        {
            Debug.Log("It's not your turn");
            return;
        }

        // set allowed moves based on the piece type
        allowedMoves = BoardManager.Instance.shogiPieces[x, y, z].PossibleMoves();

        // set the selected piece
        selectedPiece = BoardManager.Instance.shogiPieces[x, y, z];

        // show the movement range
        ShowAllowedMoves(allowedMoves);

    }

    /* for each possible move, render a range cube to indicate it
     * Params:
     *  1. moves - 3d boolean array of entire board, with true in areas where move is available
     *
     *  this could probably be more efficient
     */

    private void ShowAllowedMoves(bool[,,] moves)
    {

        // loop through each spot on the board
        for(int i = 0; i < BoardManager.Instance.BOARD_SIZE; i++)
        {
            for (int j = 0; j < BoardManager.Instance.BOARD_SIZE; j++)
            {
                for (int k = 0; k < BoardManager.Instance.BOARD_SIZE; k++)
                {
                    // if the current spot is a valid l
                    if (moves[k, j, i])
                    {

                        
                        // grab an available range cube
                        GameObject cube = GetRangeCube();

                        // activate the cube so we can see the range
                        cube.SetActive(true);

                        // set the position of the cube to the correct location
                        cube.transform.position = new Vector3(k + BoardManager.Instance.CUBE_OFFSET, j + BoardManager.Instance.CUBE_OFFSET, i + BoardManager.Instance.CUBE_OFFSET);

                        // add it to the board of spots
                        BoardManager.Instance.shogiSpots[k, j, i] = cube;
                    }
                }
            }
        }
    }

    #endregion
}
