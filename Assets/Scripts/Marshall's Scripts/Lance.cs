using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lance : Piece
{
    [SerializeField] public bool promoted;

    private Vector3 piecePosition;

    public void Awake()
    {
        this.piecePosition = transform.position;
        this.currentX = (int)piecePosition.x;
        this.currentY = (int)piecePosition.y;
        this.currentZ = (int)piecePosition.z;
        this.isPlayer1 = true;
        this.selected = false;
        this.promoted = false;
    }

    public void Update()
    {
        // To do: implement checking if piece is selected, and if space to move to has been selected
        updatePossibleMoves();

        // For testing purposes to see lance movement
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            move((int)piecePosition.x, (int)piecePosition.y, (int)piecePosition.z + 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            move((int)piecePosition.x, (int)piecePosition.y, (int)piecePosition.z + 2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            move((int)piecePosition.x, (int)piecePosition.y, (int)piecePosition.z + 3);
        }
    }

    private void promote()
    {
        promoted = true;
    }

    // Lance can move to any amount of squares ahead of it
    // To do: implement checking of pieces occupying spaces in front of it using gameboard
    public override void updatePossibleMoves()
    {
        int boardSize = Game.Board.boardSize;

        List<Vector3> moves = new List<Vector3>();

        for(int i = 1; i < (boardSize - currentZ); ++i)
        {
            // if within bounds
            if (currentZ + 1 < boardSize)
            {
                Piece c = Game.Board.board[currentX, currentY, currentZ + 1].Piece;

                // if there is no piece, or it is not our piece
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX, currentY, currentZ + i));
                }
            }

        }
        
        setPossibleMoves(moves);
    }
}

