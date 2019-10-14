using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
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
        this.pieceType = "Bishop";
    }

    public void Update()
    {
        // To do: implement checking if piece is selected, and if space to move to has been selected
        updatePossibleMoves();

        // For testing purposes to see lance movement
        //if (Input.GetKeyDown(KeyCode.Keypad1))
        //{
        //    move((int)piecePosition.x, (int)piecePosition.y, (int)piecePosition.z + 1);

        //}
        //else if (Input.GetKeyDown(KeyCode.Keypad2))
        //{
        //    move((int)piecePosition.x, (int)piecePosition.y, (int)piecePosition.z + 2);
        //}
        //else if (Input.GetKeyDown(KeyCode.Keypad3))
        //{
        //    move((int)piecePosition.x, (int)piecePosition.y, (int)piecePosition.z + 3);
        //}
    }

    private void promote()
    {
        promoted = true;
    }

    // To do: implement checking of pieces occupying spaces in front of it using gameboard
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
                if (c == null )
                {
                    moves.Add(new Vector3(currentX + i, currentY, currentZ + i));

                }
                else if(c.isPlayer1 != isPlayer1)
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
                else if(c.isPlayer1 != isPlayer1)
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
                else if(c.isPlayer1 != isPlayer1)
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
                else if(c.isPlayer1 != isPlayer1)
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
                else if(c.isPlayer1 != isPlayer1)
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
                else if(c.isPlayer1 != isPlayer1)
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
                else if(c.isPlayer1 != isPlayer1)
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
                else if(c.isPlayer1 != isPlayer1)
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
                else if(c.isPlayer1 != isPlayer1)
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
                else if(c.isPlayer1 != isPlayer1)
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
                else if(c.isPlayer1 != isPlayer1)
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
                else if(c.isPlayer1 != isPlayer1)
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
                else if(c.isPlayer1 != isPlayer1)
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
                else if(c.isPlayer1 != isPlayer1)
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
                else if(c.isPlayer1 != isPlayer1)
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
                else if(c.isPlayer1 != isPlayer1)
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

        setPossibleMoves(moves);
    }
}

