using System.Collections.Generic;
using UnityEngine;


namespace Game
{
    //<summary>
    //A component the board uses to detect clicks and piece selections.
    //</summary>
    public class BoardSelection : MonoBehaviour
    {
        static BoardSelection m_instance;
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

        // the current mouse hovered spot (a BoardTile)
        private BoardTile oldHoveredSpot;

        // materials used to show hover vs resting cube
        [Header("Tile materials")]
        [SerializeField] [Tooltip("The tile material on hover")] Material hoverMaterial;
        [SerializeField] [Tooltip("The tile material w/o hover")] Material restingMaterial;
        [SerializeField] [Tooltip("The tile material to show movement range indicators")] Material rangeMaterial;
        [SerializeField] [Tooltip("The tile material for enemy in range")] Material enemyInRangeMaterial;
        [SerializeField] [Tooltip("The tile material for piece drop indicators")] Material dropIndicatorMaterial;

        // GameObject used to show the movement/attack range of a pieve
        [SerializeField] private GameObject rangeCube;

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

        public static BoardSelection Instance
        {
            get { return m_instance; }
        }

        #endregion


        /**
         * Any Unity Methods used.
         */
        #region Unity Methods

        void Start()
        {
            m_instance = this;
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
        /* change the whole board into range cubes for picking coordinates to drop a piece
         * from here, user will pick a level (a Y value)
         *
         * bool highlight - true changes to range cubes, false changes back to normal
         */
        public void RangeBoard(bool activate, int clickY, int clickZ)
        {
            if (clickY != -1) 
            {
                for (int x = 0; x < Game.Board.boardSize; x++)
                {
                    // show only the row (y and z specified)
                    if (clickZ != -1)
                    {
                        GameObject tile = Game.Board.board[x, clickY, clickZ].Tile;
                        tile.GetComponent<Renderer>().material = dropIndicatorMaterial;
                        tile.layer = 9;
                    }

                    // show the entire level (just y specified)
                    else
                    {
                        for (int z = 0; z < Game.Board.boardSize; z++)
                        {
                            GameObject tile = Game.Board.board[x, clickY, z].Tile;
                            tile.GetComponent<Renderer>().material = dropIndicatorMaterial;
                            tile.layer = 9;
                        }
                    }
                }
            }

            else
            {
                //show ALL tiles
                for (int x = 0; x < Game.Board.boardSize; x++)
                {
                    for (int y = 0; y < Game.Board.boardSize; y++)
                    {
                        for (int z = 0; z < Game.Board.boardSize; z++)
                        {
                            GameObject tile = Game.Board.board[x, y, z].Tile;

                            // if true, change to range tile
                            if (activate)
                            {
                                tile.GetComponent<Renderer>().material = dropIndicatorMaterial;
                                tile.layer = 9; // Piece Layer
                            }

                            // else change back to regular tile
                            else
                            {
                                Debug.Log("reset");
                                tile.GetComponent<Renderer>().material = restingMaterial;
                                tile.layer = 0; // Default Layer
                            }
                        }
                    }
                }
            }


        }

        /* highlight/unhighlight board accordingly: entire level (just y provided) or just a row (z and y provided)
         *
         * Params:
         *  1. bool highlight - true highlights, false unhighlights
         *  2. int z, y - the coordinates of cubes to highlight
         * 
         */
         void HighlightRows(bool highlight, int hoverX, int hoverY, int hoverZ)
        {
            // x and y are specified, highlight by tile (x given through hover, y and z are already selected)
            if (CapturedPiece.selectedY != -1 && CapturedPiece.selectedZ != -1)
            {
                if (highlight)
                {
                    Game.Board.board[hoverX, CapturedPiece.selectedY, CapturedPiece.selectedZ].Tile.GetComponent<Renderer>().material = hoverMaterial;
                }
                else
                {
                    Game.Board.board[oldHoveredSpot.Position.x, oldHoveredSpot.Position.y, oldHoveredSpot.Position.z].Tile.GetComponent<Renderer>().material = dropIndicatorMaterial;
                }
            }

            else
            {
                for (int x = 0; x < Game.Board.boardSize; x++)
                {
                    // if y is specified, highlight by row (z given through hover, y already selected)
                    if (CapturedPiece.selectedY != -1)
                    {
                        if (highlight)
                        {
                            Game.Board.board[x, CapturedPiece.selectedY, hoverZ].Tile.GetComponent<Renderer>().material = hoverMaterial;
                        }
                        else
                        {
                            Game.Board.board[x, CapturedPiece.selectedY, oldHoveredSpot.Position.z].Tile.GetComponent<Renderer>().material = dropIndicatorMaterial;
                        }
                    }

                    // if no values are specified, highlight by level (y given through hover)
                    else
                    {
                        for (int z = 0; z < Game.Board.boardSize; z++)
                        {
                            if (highlight)
                            {
                                Game.Board.board[x, hoverY, z].Tile.GetComponent<Renderer>().material = hoverMaterial;
                            }
                            else
                            {
                                Game.Board.board[x, oldHoveredSpot.Position.y, z].Tile.GetComponent<Renderer>().material = dropIndicatorMaterial;
                            }
                        }
                    }
                }
            }

        }

        #endregion


        /**
         * Private functions that are only used from
         * within this class.
         */
        #region Member Functions

        /* update the current location of the mouse and highlight pieces
         *
         * - use Raycast to determine where the mouse is hovering on the board
         * - handle hover effects based on if in piece drop mode or piece selection mode
         * - otherwise remove effects if hovering over nothing
         *
         *
         */
        private void CheckHover()
        {

            //Debug.Log("[" + CapturedPiece.selectedX + ", " + CapturedPiece.selectedY + ", " + CapturedPiece.selectedZ + "]");

            RaycastHit hit;

            // 1. ray provided is from camera to screen point (mouse position)
            // 2. out is the result of the collision
            // 3. 50 is max distance of the ray
            // 4. layer mask (only hits objects on this layer)

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

                // if we are in piece drop mode
                if (CapturedPiece.pieceDropMode)
                {
                    // hover based on piece drop selection
                    HandlePieceDropHover();
                }

                else
                {
                    // hover based on regular piece selection
                    HandlePieceSelectHover(currentHoveredPiece);
                }

            }

            else
            {
                // change the color back to normal
                if (oldHoveredSpot != null)
                {
                    // change the material of the previously hovered spot back to normal
                    if (oldHoveredSpot.Tile.tag == "Range")
                    {
                        oldHoveredSpot.Tile.GetComponent<Renderer>().material = rangeMaterial;
                    }
                    else if (oldHoveredSpot.Tile.tag == "EnemyInRange")
                    {
                        oldHoveredSpot.Tile.GetComponent<Renderer>().material = enemyInRangeMaterial;
                    }
                    else
                    {
                        if (CapturedPiece.pieceDropMode)
                        {
                            HighlightRows(false, -1, -1, -1);
                        }
                        else
                        {
                            oldHoveredSpot.Tile.GetComponent<Renderer>().material = restingMaterial;
                        }
                    }
                }


                // set the hovered spot to null (not hovering over anything)
                oldHoveredSpot = null;

                mouseX = -1;
                mouseY = -1;
                mouseZ = -1;
            }
        }

        /* handle piece drop hovering
         *
         * depending on what has been selected, (un)highlight different parts of the board
         *
         */
        void HandlePieceDropHover()
        {
            if (oldHoveredSpot != null)
            {
                // if we haven't selected a Y yet
                if (CapturedPiece.selectedY == -1)
                {

                    // if we've hovered over a new position/y value
                    if (oldHoveredSpot.Position.y != mouseY)
                    {
                        // highlight the new
                        HighlightRows(true, -1, mouseY, -1);

                        // unhighlight the old
                        HighlightRows(false, -1, -1, -1);
                    }
                }

                // if we've selected a Y position, and choosing a Z
                else if (CapturedPiece.selectedZ == -1)
                {

                    //Debug.Log("Looking for Z");

                    // if we've hovered over a new position/z value
                    // y value must matched our previously selected y
                    if (oldHoveredSpot.Position.z != mouseZ && CapturedPiece.selectedY == mouseY)
                    {
                        // highlight the new
                        HighlightRows(true, -1, mouseY, mouseZ);

                        // unhighlight the old
                        HighlightRows(false, -1, -1, -1);
                    }
                }

                // if we've selected a Z and Y position, and choosing the X
                else if (CapturedPiece.selectedX == -1)
                {

                    Debug.Log(Game.Board.board[mouseX, mouseY, mouseZ].Piece);



                    // if we've hovered over a new position
                    // x and y value must match our previously selected x and y
                    // tile must not have a piece already in it
                    if (oldHoveredSpot.Position.x != mouseX && CapturedPiece.selectedY == mouseY && CapturedPiece.selectedZ == mouseZ)
                    {
                        Debug.Log("found an option!");

                        // highlight the new
                        HighlightRows(true, mouseX, mouseY, mouseZ);

                        // unhighlight the old
                        HighlightRows(false, -1, -1, -1);
                    }
                }
            }


            oldHoveredSpot = Game.Board.board[mouseX, mouseY, mouseZ];

        }

        /* handle piece selection hovering
         *
         * (un)highlight different cubes depending on what the selection is
         *
         */
        void HandlePieceSelectHover(Piece current)
        {
            // no piece has been selected yet
            if (selectedPiece == null)
            {
                // just highlight pieces as you hover over
                if (current != null)
                {
                    // change the material of hovered cube
                    HandleHoverCube();
                }
            }

            // a piece is currently selected
            else
            {
                // highlight over hovered range cubes
                if (CheckMove(selectedPiece.getPossibleMoves(), new Vector3(mouseX, mouseY, mouseZ)))
                {
                    HandleHoverCube();
                }
            }
        }

        /* handle the material of the cubes being hovered over
         * when you mouse away from a cube, change to normal material
         * when you mouse on a cube, change to range or enemyInRange material
         *
         * currentX, currentY, currentZ are coordinates to the currently hovered tile
         *
         */
        void HandleHoverCube()
        {

            // if the hovered spot is different from the previously hovered spot
            if (oldHoveredSpot == null || oldHoveredSpot.Tile != Game.Board.board[mouseX, mouseY, mouseZ].Tile)
            {

                // if the currently hovered spot isn't null
                if (oldHoveredSpot != null && oldHoveredSpot.Tile != null)
                {
                    // change the material of the previously hovered spot back to normal
                    if (oldHoveredSpot.Tile.tag == "Range")
                    {
                        oldHoveredSpot.Tile.GetComponent<Renderer>().material = rangeMaterial;
                    }
                    else if (oldHoveredSpot.Tile.tag == "EnemyInRange")
                    {
                        oldHoveredSpot.Tile.GetComponent<Renderer>().material = enemyInRangeMaterial;
                    }
                    else
                    {
                        oldHoveredSpot.Tile.GetComponent<Renderer>().material = restingMaterial;
                    }


                }

                // change the new hovered spot to the hoverMaterial
                Game.Board.board[mouseX, mouseY, mouseZ].Tile.GetComponent<Renderer>().material = hoverMaterial;

                // set the new previouslyHoveredSpot
                oldHoveredSpot = Game.Board.board[mouseX, mouseY, mouseZ];
            }
        }


        // check what you've clicked on
        void CheckClick()
        {

            int boardSize = Game.Board.boardSize;

            // check for click
            if (Input.GetMouseButtonDown(0))
            {

                //Debug.Log("Clicked on " + mouseX + mouseY + mouseZ);

                // if you clicked within the board (3d array coordinates)
                if (mouseX >= 0 && mouseX < Game.Board.boardSize && mouseY >= 0 && mouseY < boardSize && mouseZ >= 0 && mouseZ < boardSize)
                {
                    if (CapturedPiece.pieceDropMode)
                    {
                        HandlePieceDropClick();
                    }
                    else
                    {
                        HandleSelectClick();
                    }

                }

                // clicked outside the board
                else
                {
                    // deactivate any selected pieces or modes
                    DeactivateBoard();
                }

            }
        }

        /* Handle a click to do a piece drop
         *
         * 1. select a level (Y axis)
         * 2. select a row (X axis)
         * 3. select a tile (Z axis)
         *      - drop the piece here, remove from graveyard
         */
        void HandlePieceDropClick()
        {
            // reset the hover tiles
            HighlightRows(false, -1, mouseY, -1);

            // reset which tiles are clickable
            RangeBoard(false, -1, -1);

            Debug.Log("click");

            if (CapturedPiece.selectedY == -1)
            {
                Debug.Log("just set the level");
                CapturedPiece.setSelectedY(mouseY);
                RangeBoard(true, mouseY, -1);
            }
            else if (CapturedPiece.selectedZ == -1 && CapturedPiece.selectedY == mouseY)
            {
                CapturedPiece.setSelectedZ(mouseZ);
                RangeBoard(true, mouseY, mouseZ);
            }
            else if (CapturedPiece.selectedX == -1 && CapturedPiece.selectedY == mouseY && CapturedPiece.selectedZ == mouseZ)
            {
                CapturedPiece.setSelectedX(mouseX);
            }
        }

        /* Handle a click to select or move a piece
         *
         * - if nothing selected, see if you can select a piece
         * - if something selected, try moving the piece
         *
         */
        void HandleSelectClick()
        {
            // if you haven't clicked on a piece already, click on it
            if (selectedPiece == null)
            {
                SelectShogiPiece(mouseX, mouseY, mouseZ);
            }
            else
            {
                // check if the move is allowed
                if (CheckMove(selectedPiece.getPossibleMoves(), new Vector3(mouseX, mouseY, mouseZ)))
                {
                    MoveShogiPiece(mouseX, mouseY, mouseZ);
                }
                // hide the range cubes and reset selectedPiece
                HideRange(selectedPiece.getPossibleMoves());
                selectedPiece = null;
            }
        }

        /* Deactivate any selected pieces or modes
         *
         * - occurs when clicking outside of the the board
         *
         */
        void DeactivateBoard()
        {
            // deactivate the selected piece here
            if (selectedPiece != null)
            {
                // hide the range cubes and reset selectedPiece
                HideRange(selectedPiece.getPossibleMoves());
                selectedPiece = null;
            }

            // deactivate piece drop mode
            if (CapturedPiece.pieceDropMode)
            {
                CapturedPiece.setPieceDropMode(false);
                RangeBoard(false, -1, -1);
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
        void MoveShogiPiece(int x, int y, int z)
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
            Destroy(piece.gameObject);

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

                if (Game.Board.board[(int)moves[i].x, (int)moves[i].y, (int)moves[i].z].Piece != null)
                {
                    if (Game.Board.board[(int)moves[i].x, (int)moves[i].y, (int)moves[i].z].Piece.isPlayer1 != selectedPiece.isPlayer1)
                    {
                        rangeTile.GetComponent<Renderer>().material = enemyInRangeMaterial;
                        rangeTile.tag = "EnemyInRange";
                    }
                }
                else
                {
                    rangeTile.GetComponent<Renderer>().material = rangeMaterial;
                    rangeTile.tag = "Range";
                }
                

                // this is the PieceLayer, which is hoverable
                rangeTile.layer = 9;
                
            }
        }


        // for each rangecube, set active to false
        private void HideRange(List<Vector3> moves)
        {
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
