using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
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
        this.pieceType = "Pawn";
    }

    public void Update()
    {
        // To do: implement checking if piece is selected, and if space to move to has been selected
        updatePossibleMoves();

        this.piecePosition = transform.position;

        // For testing purposes to see pawn movement
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            move((int) piecePosition.x, (int) piecePosition.y , (int) piecePosition.z + 1);

        }
    }

    private void promote()
    {
        promoted = true;
    }

    // Pawn moves one space forward
    // To do: implement checking of pieces occupying spaces in front of it using gameboard
    public override void updatePossibleMoves()
    {
        int boardSize = Game.Board.boardSize;
        List<Vector3> moves = new List<Vector3>();

        if (isPlayer1)
        {
            if (currentZ + 1 < boardSize)
            {
                Piece c = Game.Board.board[currentX, currentY, currentZ + 1].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX, currentY, currentZ + 1));
                }
            }
        }
        else
        {
            if (currentZ - 1 < boardSize)
            {
                Piece c = Game.Board.board[currentX, currentY, currentZ - 1].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX, currentY, currentZ - 1));
                }
            }
        }
        
        setPossibleMoves(moves);
    }
}
