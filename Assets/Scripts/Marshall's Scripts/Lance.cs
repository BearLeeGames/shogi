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
        this.pt = pieceType.Lance;
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
            for (int i = 1; i < (boardSize - currentZ); ++i)
            {
                // if within bounds
                if (currentZ + i < boardSize)
                {
                    Piece c = Game.Board.board[currentX, currentY, currentZ + i].Piece;
                    if (c == null)
                    {
                        moves.Add(new Vector3(currentX, currentY, currentZ + i));
                    }
                    else if (c.isPlayer1 != isPlayer1)
                    {
                        moves.Add(new Vector3(currentX, currentY, currentZ + i));
                        break;
                    }
                    else
                    {
                        break;
                    }
                }

            }
        }
        else
        {
            for (int i = 1; i <= currentZ; ++i)
            {
                Piece c = Game.Board.board[currentX, currentY, currentZ - i].Piece;
                if (c == null)
                {
                    moves.Add(new Vector3(currentX, currentY, currentZ - i));
                }
                else if (c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX, currentY, currentZ - i));
                    break;
                }
                else
                {
                    break;
                }
            }
        }
        
        setPossibleMoves(moves);
    }
}

