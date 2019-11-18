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
        this.pt = pieceType.Pawn;
    }

    public void Update()
    {
        updatePossibleMoves();
    }

    private void promote()
    {
        promoted = true;
    }

    public override void updatePossibleMoves()
    {
        int boardSize = Game.Board.boardSize;
        List<Vector3> moves = new List<Vector3>();

        if (isPlayer1)
        {
            if (currentZ + 1 < boardSize && currentZ + 1 >= 0)
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
            if (currentZ - 1 < boardSize && currentZ - 1 >= 0)
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
