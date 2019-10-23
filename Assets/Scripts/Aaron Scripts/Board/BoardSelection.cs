using System.Collections.Generic;
using UnityEngine;


namespace Game
{
    //<summary>
    //A component the board uses to detect clicks and piece selections.
    //</summary>
    public class BoardSelection : MonoBehaviour
    {
        /**
         * Holds all the data members that the class
         * contains.
         */
        #region Data Members

        // the coordinates of the currently clicked/selected spot
        int mouseX = -1;
        int mouseY = -1;
        int mouseZ = -1;

        // the current selected piece
        private Piece selectedPiece;

        // the current mouse hovered spot (a CubeLight object)
        private GameObject hoveredSpot;

        // materials used to show hover vs resting cube
        [Header("Tile materials")]
        [SerializeField] [Tooltip("The tile material on hover")] Material hoverMaterial;
        [SerializeField] [Tooltip("The tile material w/ no hover")] Material restingMaterial;
        [SerializeField] [Tooltip("The tile material for range indicators")] Material rangeMaterial;


        // GameObject used to show the movement/attack range of a pieve
        [SerializeField] private GameObject rangeCube;

        // list of range cubes
        List<GameObject> rangeCubes;

        // list of Vector3 moves that a piece can take
        List<Vector3> allowedMoves;

        // object for controlling the piece graveyard
        [SerializeField] [Tooltip("Object that shows all pieces captured")] public GameObject GraveyardObject;

        Graveyard Graveyard;

        #endregion


        /**
         * Modifies the data members so that they may
         * be read-only, return specific values, or
         * expose certain data members to the public
         */
        #region Member Properties


        /*  NOTE: Not used if we only change cube materials instead of the whole object
         *  pool the previously used range cubes
         *  reuse and create new range cubes as necessary
         */
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


        /**
         * Any Unity Methods used.
         */
        #region Unity Methods

        void Start()
        {
            rangeCubes = new List<GameObject>();
            Graveyard = GraveyardObject.GetComponent<Graveyard>();
        }

        void Update()
        {
            CheckHover();
            CheckClick();
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

        #endregion


        /**
         * Private functions that are only used from
         * within this class.
         */
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
                mouseX = (int)Mathf.RoundToInt(hit.point.x);
                mouseY = (int)Mathf.RoundToInt(hit.point.y);
                mouseZ = (int)Mathf.RoundToInt(hit.point.z);


                // convert the spacial coordinates into array coordinates
                Vector3 arrayCoordinates = Game.Board.SpaceToArrayCoordinates(mouseX, mouseY, mouseZ);

                mouseX = (int)arrayCoordinates.x;
                mouseY = (int)arrayCoordinates.y;
                mouseZ = (int)arrayCoordinates.z;

                //Debug.Log("hovering over: " + mouseX + " " + mouseY + " " + mouseZ);

                // Note: game is currently only hovering over the colored block (smaller) and not the surrounding block (fills entire space)
                // the surrounding block sometimes pushes your cursor over to the next block, since it perfectly fills the space to the point where (1, 1.75) becomes (1, 2)

                Piece currentHoveredPiece = Game.Board.board[mouseX, mouseY, mouseZ].Piece;

                // no piece selected yet
                if (selectedPiece == null)
                {
                    // just highlight pieces
                    if (currentHoveredPiece != null)
                    {
                        // change the design of hovered cubes
                        HandleHoverCube(mouseX, mouseY, mouseZ);
                    }
                }

                // a piece has been selected
                else
                {
                    // highlight range cubes
                    if (CheckMove(selectedPiece.getPossibleMoves(), new Vector3(mouseX, mouseY, mouseZ)))
                    {
                        HandleHoverCube(mouseX, mouseY, mouseZ);
                    }
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

                mouseX = -1;
                mouseY = -1;
                mouseZ = -1;
            }
        }

        // handle the material of the cubes being hovered over
        private void HandleHoverCube(int currentX, int currentY, int currentZ)
        {

            // if the hovered spot is different from the previously hovered spot
            if (hoveredSpot != Game.Board.board[currentX, currentY, currentZ].Tile)
            {

                // if the currently hovered spot isn't null
                if (hoveredSpot != null)
                {
                    // change the material of the previously hovered spot back to normal
                    if (hoveredSpot.tag == "Range")
                    {
                        hoveredSpot.GetComponent<Renderer>().material = rangeMaterial;
                    }
                    else
                    {
                        hoveredSpot.GetComponent<Renderer>().material = restingMaterial;
                    }


                }

                // change the new hovered spot to the hoverMaterial
                Game.Board.board[currentX, currentY, currentZ].Tile.GetComponent<Renderer>().material = hoverMaterial;

                // set the new hoveredSpot
                hoveredSpot = Game.Board.board[currentX, currentY, currentZ].Tile;
            }
        }

        // check what you've clicked on
        private void CheckClick()
        {

            int boardSize = Game.Board.boardSize;

            // check for click
            if (Input.GetMouseButtonDown(0))
            {

                //Debug.Log("Clicked on " + mouseX + mouseY + mouseZ);

                // if you clicked within the board (3d array coordinates)
                if (mouseX >= 0 && mouseX < Game.Board.boardSize && mouseY >= 0 && mouseY < boardSize && mouseZ >= 0 && mouseZ < boardSize)
                {
                    // if you haven't clicked on a piece already, click on it
                    if (selectedPiece == null)
                    {
                        Debug.Log("SELECT PIECE");
                        SelectShogiPiece(mouseX, mouseY, mouseZ);
                    }
                    else
                    {
                        // check if the move is allowed
                        if (CheckMove(selectedPiece.getPossibleMoves(), new Vector3(mouseX, mouseY, mouseZ)))
                        {
                            Debug.Log("MOVE PIECE");
                            MoveShogiPiece(mouseX, mouseY, mouseZ);
                        }
                        // hide the range cubes and reset selectedPiece
                        HideRange(selectedPiece.getPossibleMoves());
                        selectedPiece = null;
                    }
                }
                else
                {
                    // deactivate the selected piece here
                    if (selectedPiece != null)
                    {
                        // hide the range cubes and reset selectedPiece
                        HideRange(selectedPiece.getPossibleMoves());
                        selectedPiece = null;
                    }

                }

            }
        }


        /*
         * Moves a piece to the given location IF valid
         * 
         * Params: x, y, z
         * 
         * check if move is allowed (based on the piece function)
         * check/destroy if there's a piece in the targeted location
         * 
         * move to Board.cs?
         * 
         */
        private void MoveShogiPiece(int x, int y, int z)
        {

            Piece piece = Game.Board.board[x, y, z].Piece;

            // remove the selectedPiece from original position in the 3d array/logical board
            Game.Board.board[selectedPiece.currentX, selectedPiece.currentY, selectedPiece.currentZ].Piece = null;

            // if there's a piece there, check the team and CAPTURE THE ENEMY
            if (piece != null && piece.isPlayer1 != Game.Board.isPlayer1Turn)
            {
                CapturePiece(piece);
            }

            // set the piece in the 3d array/logical board
            Game.Board.board[x, y, z].Piece = selectedPiece;

            // set position in space of selected piece to the new location
            selectedPiece.transform.position = Game.Board.GetPieceCenter(x, y, z);

            // set the piece position (for it's own properties)
            selectedPiece.setPosition(x, y, z);

            // end turn
            Game.Board.changeTurns();

        }

        private void MoveShogiPiece(Vector3Int coordinate)
        {

            Piece piece = Game.Board.board[coordinate.x, coordinate.y, coordinate.z].Piece;

            // remove the selectedPiece from original position in the 3d array/logical board
            Game.Board.board[selectedPiece.currentX, selectedPiece.currentY, selectedPiece.currentZ].Piece = null;

            // if there's a piece there, check the team and CAPTURE THE ENEMY
            if (piece != null && piece.isPlayer1 != Game.Board.isPlayer1Turn)
            {
                CapturePiece(piece);
            }

            // set the piece in the 3d array/logical board
            Game.Board.board[coordinate.x, coordinate.y, coordinate.z].Piece = selectedPiece;

            // set position in space of selected piece to the new location
            selectedPiece.transform.position = Game.Board.GetPieceCenter(coordinate.x, coordinate.y, coordinate.z);

            // set the piece position (for it's own properties)
            selectedPiece.setPosition(coordinate.x, coordinate.y, coordinate.z);

            // end turn
            Game.Board.changeTurns();

        }

        /*
         * Capture an enemy piece
         *
         */
        private void CapturePiece(Piece piece)
        {
            // destroy the piece
            Destroy(piece);

            // add the count to the other player's graveyard
            Graveyard.AddPiece(piece.pt, !piece.isPlayer1);
        }

        /* select the clicked shogi piece (show shogi piece range)
         * Params:
         *  xyz - coordinates for the shogiPieces board
         *
         */
        private void SelectShogiPiece(int x, int y, int z)
        {
            // don't select anything if there's no piece there
            if (Game.Board.board[x, y, z].Piece == null)
            {
                Debug.Log("INVALID MOVE: No piece there");
                return;
            }

            // don't select the piece if it's not your turn
            if (Game.Board.board[x, y, z].Piece.isPlayer1 != Game.Board.isPlayer1Turn)
            {
                Debug.Log("INVALID MOVE: It's not your turn");
                return;
            }

            // set allowed moves based on the piece type
            // 3d array of bools (true if spot is valid)
            allowedMoves = Game.Board.board[x, y, z].Piece.getPossibleMoves();

            // set the selected piece
            selectedPiece = Game.Board.board[x, y, z].Piece;

            // show the movement range
            ShowAllowedMoves(allowedMoves);

        }

        /* for each possible move, render a range cube to indicate it
         * Params:
         *  1. moves - 3d boolean array of entire board, with true in areas where move is available
         *
         *  this could probably be more efficient
         */

        private void ShowAllowedMoves(List<Vector3> moves)
        {
            // loop through the list of moves
            for (int i = 0; i < moves.Count; i++)
            {
                // NOTE: Commented out code is for changing tile object entirely, vs. only changing the material of the tile

                //// grab an available range cube
                //GameObject cube = GetRangeCube();

                //// activate the cube so we can see the range
                //cube.SetActive(true);

                // set the position of the cube to the correct location
                //cube.transform.position = moves[i];

                // set those tile materials to range materials
                GameObject rangeTile = Game.Board.board[(int)moves[i].x, (int)moves[i].y, (int)moves[i].z].Tile;

                rangeTile.GetComponent<Renderer>().material = rangeMaterial;

                // this is the PieceLayer, which is hoverable
                rangeTile.layer = 9;
                rangeTile.tag = "Range";
            }
        }


        // for each rangecube, set active to false
        private void HideRange(List<Vector3> moves)
        {
            // NOTE: use this if we use rangeCube objects
            //foreach (GameObject cube in rangeCubes)
            //{
            //    cube.SetActive(false);
            //}

            // if not, change material of tiles back to normal
            for (int i = 0; i < moves.Count; i++)
            {
                // get the tile used for the range indicator
                GameObject rangeTile = Game.Board.board[(int)moves[i].x, (int)moves[i].y, (int)moves[i].z].Tile;

                rangeTile.GetComponent<Renderer>().material = restingMaterial;

                rangeTile.layer = 0;
                rangeTile.tag = "Untagged";
            }
        }

        // check if a tile is a valid move
        private bool CheckMove(List<Vector3> moves, Vector3 move)
        {
            for (int i = 0; i < moves.Count; i++)
            {
                if (moves[i] == move)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }

}
