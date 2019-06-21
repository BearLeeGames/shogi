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

    public Material hoverMaterial;
    public Material restingMaterial;

    private bool[,,] allowedMoves;
    void Start () {
		
	}
	
	void Update () {
        checkHover();

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
            currentX = (int)hit.point.x;
            currentY = (int)hit.point.y;
            currentZ = (int)hit.point.z;

            Debug.Log("hovering over: " + currentX + " " + currentY + " " + currentZ);

            if (BoardManager.Instance.ShogiSpots[currentX, currentY, currentZ] != null)
            {
                // this if should always be true currently, with only board spots with pieces in them being shown
                Debug.Log("hovering over a piece");


                // change the color of the previous spot back to normal
                if (hoveredSpot != null)
                {
                    hoveredSpot.GetComponent<Renderer>().material = restingMaterial;
                }


                BoardManager.Instance.ShogiSpots[currentX, currentY, currentZ].GetComponent<Renderer>().material = hoverMaterial;
                hoveredSpot = BoardManager.Instance.ShogiSpots[currentX, currentY, currentZ];
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
                    // Select the piece
                    //SelectShogiPiece(clickedX, clickedY, clickedZ);

                    //Debug.Log(selectedPiece);
                }
            }

        }
    }

    private void SelectShogiPiece(int x, int y, int z)
    {
        // don't select anything if there's no piece there
        if (BoardManager.Instance.ShogiPieces[x, y, z] == null)
        {
            return;
        }

        // don't select the piece if it's not your turn
        if (BoardManager.Instance.ShogiPieces[x, y, z].isPlayer1 != BoardManager.Instance.isPlayer1Turn)
        {
            return;
        }

        // set allowed moves
        allowedMoves = BoardManager.Instance.ShogiPieces[x, y, z].PossibleMoves();
        selectedPiece = BoardManager.Instance.ShogiPieces[x, y, z];

    }
}
