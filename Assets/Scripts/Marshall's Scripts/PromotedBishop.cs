using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromotedBishop : Piece
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
        this.pt = pieceType.PromotedBishop;
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

        for (int i = 1; i < boardSize; ++i)
        {
            // XZ plane 

            // Up-right direction

            if (currentX + i < boardSize && currentZ + i < boardSize)
            {
                Piece c = Game.Board.board[currentX + i, currentY, currentZ + i].Piece;
                if (c == null)
                {
                    moves.Add(new Vector3(currentX + i, currentY, currentZ + i));

                }
                else if (c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX + i, currentY, currentZ + i));
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        for (int i = 1; i < boardSize; ++i)
        {
            // Up-left direction
            if (currentX - i >= 0 && currentZ + i < boardSize)
            {
                Piece c = Game.Board.board[currentX - i, currentY, currentZ + i].Piece;
                if (c == null)
                {
                    moves.Add(new Vector3(currentX - i, currentY, currentZ + i));
                }
                else if (c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX - i, currentY, currentZ + i));
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        for (int i = 1; i < boardSize; ++i)
        {
            // Down-right direction
            if (currentX + i < boardSize && currentZ - i >= 0)
            {
                Piece c = Game.Board.board[currentX + i, currentY, currentZ - i].Piece;
                if (c == null)
                {
                    moves.Add(new Vector3(currentX + i, currentY, currentZ - i));

                }
                else if (c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX + i, currentY, currentZ - i));
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        for (int i = 1; i < boardSize; ++i)
        {
            // Down-left direction
            if (currentX - i >= 0 && currentZ - i >= 0)
            {
                Piece c = Game.Board.board[currentX - i, currentY, currentZ - i].Piece;
                if (c == null)
                {
                    moves.Add(new Vector3(currentX - i, currentY, currentZ - i));
                }
                else if (c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX - i, currentY, currentZ - i));
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        // YZ plane
        for (int i = 1; i < boardSize; ++i)
        {
            if (currentY + i < boardSize && currentZ + i < boardSize)
            {
                Piece c = Game.Board.board[currentX, currentY + i, currentZ + i].Piece;
                if (c == null)
                {
                    moves.Add(new Vector3(currentX, currentY + i, currentZ + i));
                }
                else if (c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX, currentY + i, currentZ + i));
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        for (int i = 1; i < boardSize; ++i)
        {
            if (currentY - i >= 0 && currentZ - i >= 0)
            {
                Piece c = Game.Board.board[currentX, currentY - i, currentZ - i].Piece;
                if (c == null)
                {
                    moves.Add(new Vector3(currentX, currentY - i, currentZ - i));
                }
                else if (c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX, currentY - i, currentZ - i));
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        for (int i = 1; i < boardSize; ++i)
        {
            if (currentY + 0 < boardSize && currentZ - i >= 0)
            {
                Piece c = Game.Board.board[currentX, currentY + i, currentZ - i].Piece;
                if (c == null)
                {
                    moves.Add(new Vector3(currentX, currentY + i, currentZ - i));
                }
                else if (c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX, currentY + i, currentZ - i));
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        for (int i = 1; i < boardSize; ++i)
        {
            if (currentY - i >= 0 && currentZ + i < boardSize)
            {
                Piece c = Game.Board.board[currentX, currentY - i, currentZ + i].Piece;
                if (c == null)
                {
                    moves.Add(new Vector3(currentX, currentY - i, currentZ + i));

                }
                else if (c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX, currentY - i, currentZ + i));
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        for (int i = 1; i < boardSize; ++i)
        {
            // XY plane

            if (currentX + i < boardSize && currentY + i < boardSize)
            {
                Piece c = Game.Board.board[currentX + i, currentY + i, currentZ].Piece;
                if (c == null)
                {
                    moves.Add(new Vector3(currentX + i, currentY + i, currentZ));

                }
                else if (c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX + i, currentY + i, currentZ));
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        for (int i = 1; i < boardSize; ++i)
        {
            if (currentX - i >= 0 && currentY - i >= 0)
            {
                Piece c = Game.Board.board[currentX - i, currentY - i, currentZ].Piece;
                if (c == null)
                {
                    moves.Add(new Vector3(currentX - i, currentY - i, currentZ));

                }
                else if (c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX - i, currentY - i, currentZ));
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        for (int i = 1; i < boardSize; ++i)
        {
            if (currentX + i < boardSize && currentY - i >= 0)
            {
                Piece c = Game.Board.board[currentX + i, currentY - i, currentZ].Piece;
                if (c == null)
                {
                    moves.Add(new Vector3(currentX + i, currentY - i, currentZ));

                }
                else if (c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX + i, currentY - i, currentZ));
                    break;
                }
                else
                {
                    break;
                }
            }

        }

        for (int i = 1; i < boardSize; ++i)
        {
            if (currentX - i >= 0 && currentY + i < boardSize)
            {
                Piece c = Game.Board.board[currentX - i, currentY + i, currentZ].Piece;
                if (c == null)
                {
                    moves.Add(new Vector3(currentX - i, currentY + i, currentZ));

                }
                else if (c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX - i, currentY + i, currentZ));
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        // All axis
        for (int i = 1; i < boardSize; ++i)
        {
            // Up-right direction
            if (currentX + i < boardSize && currentY + i < boardSize && currentZ + i < boardSize)
            {
                Piece c = Game.Board.board[currentX + i, currentY + i, currentZ + i].Piece;
                if (c == null)
                {
                    moves.Add(new Vector3(currentX + i, currentY + i, currentZ + i));

                }
                else if (c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX + i, currentY + i, currentZ + i));
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        for (int i = 1; i < boardSize; ++i)
        {
            if (currentX + i < boardSize && currentY - i >= 0 && currentZ + i < boardSize)
            {
                Piece c = Game.Board.board[currentX + i, currentY - i, currentZ + i].Piece;
                if (c == null)
                {
                    moves.Add(new Vector3(currentX + i, currentY - i, currentZ + i));

                }
                else if (c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX + i, currentY - i, currentZ + i));
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        for (int i = 1; i < boardSize; ++i)
        {
            // Up-left direction
            if (currentX - i >= 0 && currentY + i < boardSize && currentZ + i < boardSize)
            {
                Piece c = Game.Board.board[currentX - i, currentY + i, currentZ + i].Piece;
                if (c == null)
                {
                    moves.Add(new Vector3(currentX - i, currentY + i, currentZ + i));

                }
                else if (c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX - i, currentY + i, currentZ + i));
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        for (int i = 1; i < boardSize; ++i)
        {
            if (currentX - i >= 0 && currentY - i >= 0 && currentZ + i < boardSize)
            {
                Piece c = Game.Board.board[currentX - i, currentY - i, currentZ + i].Piece;
                if (c == null)
                {
                    moves.Add(new Vector3(currentX - i, currentY - i, currentZ + i));

                }
                else if (c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX - i, currentY - i, currentZ + i));
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        for (int i = 1; i < boardSize; ++i)
        {
            // Down-right direction
            if (currentX + i < boardSize && currentY + i < boardSize && currentZ - i >= 0)
            {
                Piece c = Game.Board.board[currentX + i, currentY + i, currentZ - i].Piece;
                if (c == null)
                {
                    moves.Add(new Vector3(currentX + i, currentY + i, currentZ - i));

                }
                else if (c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX + i, currentY + i, currentZ - i));
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        for (int i = 1; i < boardSize; ++i)
        {
            if (currentX + i < boardSize && currentY - i >= 0 && currentZ - i >= 0)
            {
                Piece c = Game.Board.board[currentX + i, currentY - i, currentZ - i].Piece;
                if (c == null)
                {
                    moves.Add(new Vector3(currentX + i, currentY - i, currentZ - i));

                }
                else if (c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX + i, currentY - i, currentZ - i));
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        for (int i = 1; i < boardSize; ++i)
        {
            // Down-left direction
            if (currentX - i >= 0 && currentY + i < boardSize && currentZ - i >= 0)
            {
                Piece c = Game.Board.board[currentX - i, currentY + i, currentZ - i].Piece;
                if (c == null)
                {
                    moves.Add(new Vector3(currentX - i, currentY + i, currentZ - i));

                }
                else if (c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX - i, currentY + i, currentZ - i));
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        for (int i = 1; i < boardSize; ++i)
        {
            if (currentX - i >= 0 && currentY - i >= 0 && currentZ - i >= 0)
            {
                Piece c = Game.Board.board[currentX - i, currentY - i, currentZ - i].Piece;
                if (c == null)
                {
                    moves.Add(new Vector3(currentX - i, currentY - i, currentZ - i));

                }
                else if (c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX - i, currentY - i, currentZ - i));
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        int[] x = { -1, 0, 1, -1, 0, 1, -1, 0, 1,
                    -1, 0, 1, -1, 1, -1, 0, 1,
                    -1, 0, 1, -1, 0, 1, -1, 0, 1};
        int[] y = { 1, 1, 1, 0, 0, 0, -1, -1, -1,
                    1, 1, 1, 0, 0, -1, -1, -1,
                    1, 1, 1, 0, 0, 0, -1, -1, -1
                    };
        int[] z = { 1, 1, 1, 1, 1, 1, 1, 1, 1,
                    0, 0, 0, 0, 0, 0, 0, 0,
                   -1, -1, -1, -1, -1, -1, -1, -1, -1};

        for (int i = 0; i < 26; ++i)
        {
            int newX = currentX + x[i];
            int newY = currentY + y[i];
            int newZ = currentZ + z[i];

            Piece c = Game.Board.board[newX, newY, newZ].Piece;

            if (newX >= 0 && newX < boardSize && newY >= 0 && newY < boardSize && newZ >= 0 && newZ < boardSize && (c == null || c.isPlayer1 != isPlayer1))
            {
                moves.Add(new Vector3(newX, newY, newZ));
            }
        }

        setPossibleMoves(moves);
    }
}

