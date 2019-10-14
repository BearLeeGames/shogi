using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    [SerializeField]
    public bool promoted;

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
        this.pieceType = "Knight";
        
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

        int[] x = {1, -1, 0, 0};
        int[] y = {0, 0, 1, -1};
        int[] z = {2, 2, 2, 2 };


        if(isPlayer1)
        {
            for (int i = 0; i < 4; ++i)
            {
                int newX = currentX + x[i];
                int newY = currentY + y[i];
                int newZ = currentZ + z[i];

                // if within bounds
                if (newX >= 0 && newX < boardSize && newY >= 0 && newY < boardSize && newZ >= 0 && newZ < boardSize)
                {
                    Piece c = Game.Board.board[newX, newY, newZ].Piece;

                    // if there is no piece, or it is not our piece
                    if (c == null || c.isPlayer1 != isPlayer1)
                    {
                        moves.Add(new Vector3(newX, newY, newZ));
                    }
                }

            }
        }
        else
        {
            for (int i = 0; i < 4; ++i)
            {
                int newX = currentX - x[i];
                int newY = currentY + y[i];
                int newZ = currentZ - z[i];

                // if within bounds
                if (newX >= 0 && newX < boardSize && newY >= 0 && newY < boardSize && newZ >= 0 && newZ < boardSize)
                {
                    Piece c = Game.Board.board[newX, newY, newZ].Piece;

                    // if there is no piece, or it is not our piece
                    if (c == null || c.isPlayer1 != isPlayer1)
                    {
                        moves.Add(new Vector3(newX, newY, newZ));
                    }
                }

            }
        }

        setPossibleMoves(moves);
    }
}

