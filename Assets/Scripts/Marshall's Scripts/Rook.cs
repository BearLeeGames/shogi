using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
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
        List<Vector3> moves = new List<Vector3>();

        int boardSize = Game.Board.boardSize;
        
        // Forward check
        for( int i = 1; i <= (boardSize - currentZ); ++i )
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

        // Backwards check
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

        // Upwards check
        for (int i = 1; i <= (boardSize - currentY); ++i)
        {
            Piece c = Game.Board.board[currentX, currentY + i, currentZ].Piece;
            if (c == null)
            {
                moves.Add(new Vector3(currentX, currentY + i, currentZ));
            }
            else if (c.isPlayer1 != isPlayer1)
            {
                moves.Add(new Vector3(currentX, currentY + i, currentZ));
                break;
            }
            else
            {
                break;
            }
        }

        // Downwards check
        for (int i = 1; i <= currentY; ++i)
        {
            Piece c = Game.Board.board[currentX, currentY - i, currentZ].Piece;
            if (c == null)
            {
                moves.Add(new Vector3(currentX, currentY - i, currentZ));
            }
            else if (c.isPlayer1 != isPlayer1)
            {
                moves.Add(new Vector3(currentX, currentY - i, currentZ));
                break;
            }
            else
            {
                break;
            }
        }
        // Right check
        for (int i = 1; i < (boardSize - currentX); ++i)
        {
            Piece c = Game.Board.board[currentX + i, currentY, currentZ].Piece;
            if (c == null)
            {
                moves.Add(new Vector3(currentX + i, currentY, currentZ));
            }
            else if (c.isPlayer1 != isPlayer1)
            {
                moves.Add(new Vector3(currentX + i, currentY, currentZ));
                break;
            }
            else
            {
                break;
            }
        }
        // Left check
        for (int i = 1; i < currentX; ++i)
        {
            Piece c = Game.Board.board[currentX - i, currentY, currentZ].Piece;
            if (c == null)
            {
                moves.Add(new Vector3(currentX - i, currentY, currentZ));
            }
            else if (c.isPlayer1 != isPlayer1)
            {
                moves.Add(new Vector3(currentX - i, currentY, currentZ));
                break;
            }
            else
            {
                break;
            }
        }
        /*
        for (int i = 1; i < (boardSize - currentZ); ++i)
        {
            
            if (currentX + i < boardSize)
            {
                Piece c = Game.Board.board[currentX + i, currentY, currentZ].Piece;
                if(c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX + i, currentY, currentZ));
                }
            }

            if(currentX - i >= 0)
            {
                Piece c = Game.Board.board[currentX - i, currentY, currentZ].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX - i, currentY, currentZ));
                }
            }

            if(currentY + i < boardSize)
            {
                Piece c = Game.Board.board[currentX, currentY + i, currentZ].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX, currentY + i, currentZ));
                }
            }

            if(currentY - i >= 0)
            {
                Piece c = Game.Board.board[currentX, currentY - i, currentZ].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX , currentY - i, currentZ));
                }
            }

            if (currentZ + i < boardSize)
            {
                Piece c = Game.Board.board[currentX, currentY, currentZ + i].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX, currentY, currentZ + i));
                }
            }

            if (currentZ - i >= 0)
            {
                Piece c = Game.Board.board[currentX, currentY, currentZ - i].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX, currentY, currentZ - i));
                }
            }
        }
        */
        setPossibleMoves(moves);
    }
}

