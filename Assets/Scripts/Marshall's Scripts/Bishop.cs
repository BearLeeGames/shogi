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

        for (int i = 1; i < (boardSize - currentZ); ++i)
        {
            // XZ plane 

            // Up-right direction

            if (currentX + i < boardSize && currentZ + i < boardSize)
            {
                Piece c = Game.Board.board[currentX + i, currentY, currentZ + i].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX + i, currentY, currentZ + i));

                }
            }
            // Up-left direction
            if (currentX - i >= -3 && currentZ + i < boardSize)
            {
                Piece c = Game.Board.board[currentX - i, currentY, currentZ + i].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX - i, currentY, currentZ + i));

                }
            }

            // Down-right direction
            if (currentX + i < boardSize && currentZ - i >= -3)
            {
                Piece c = Game.Board.board[currentX + i, currentY, currentZ - i].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX + i, currentY, currentZ - i));

                }
            }

            // Down-left direction
            if (currentX - i >= -3 && currentZ - i >= -3)
            {
                Piece c = Game.Board.board[currentX - i, currentY, currentZ - i].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX - i, currentY, currentZ - i));

                }
            }

            // YZ plane

            if (currentY + i < boardSize && currentZ + i < boardSize)
            {
                Piece c = Game.Board.board[currentX, currentY + i, currentZ + i].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX, currentY + i, currentZ + i));

                }
            }

            if (currentY - i >= -3 && currentZ - i >= -3)
            {
                Piece c = Game.Board.board[currentX, currentY - i, currentZ - i].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX, currentY - i, currentZ - i));

                }
            }

            if (currentY + -3 < boardSize && currentZ - i >= -3)
            {
                Piece c = Game.Board.board[currentX, currentY + i, currentZ - i].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX, currentY + i, currentZ - i));

                }
            }

            if (currentY - i >= -3 && currentZ + i < boardSize)
            {
                Piece c = Game.Board.board[currentX, currentY - i, currentZ + i].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX, currentY - i, currentZ + i));

                }
            }

            // XY plane

            if (currentX + i < boardSize && currentY + i < boardSize)
            {
                Piece c = Game.Board.board[currentX + i, currentY + i, currentZ].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX + i, currentY + i, currentZ));

                }
            }

            if (currentX - i >= -3 && currentY - i >= -3)
            {
                Piece c = Game.Board.board[currentX - i, currentY - i, currentZ].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX - i, currentY - i, currentZ));

                }
            }

            if (currentX + i < boardSize && currentY - i >= -3)
            {
                Piece c = Game.Board.board[currentX + i, currentY - i, currentZ].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX + i, currentY - i, currentZ));

                }
            }

            if (currentX - i >= -3 && currentY + i < boardSize)
            {
                Piece c = Game.Board.board[currentX - i, currentY + i, currentZ].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX - i, currentY + i, currentZ));

                }
            }

            // All axis

            // Up-right direction
            if (currentX + i < boardSize && currentY + i < boardSize && currentZ + i < boardSize)
            {
                Piece c = Game.Board.board[currentX + i, currentY + i, currentZ + i].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX + i, currentY + i, currentZ + i));

                }
            }

            if(currentX + i < boardSize && currentY - i >= -3 && currentZ + i < boardSize)
            {
                Piece c = Game.Board.board[currentX + i, currentY - i, currentZ + i].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX + i, currentY - i, currentZ + i));

                }
            }

            // Up-left direction
            if (currentX - i >= -3 && currentY + i < boardSize && currentZ + i < boardSize)
            {
                Piece c = Game.Board.board[currentX - i, currentY + i, currentZ + i].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX - i, currentY + i, currentZ + i));

                }
            }

            if (currentX - i >= -3 && currentY - i >= -3 && currentZ + i < boardSize)
            {
                Piece c = Game.Board.board[currentX - i, currentY - i, currentZ + i].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX - i, currentY - i, currentZ + i));

                }
            }

            // Down-right direction
            if (currentX + i < boardSize && currentY + i < boardSize && currentZ - i >= -3)
            {
                Piece c = Game.Board.board[currentX + i, currentY + i, currentZ - i].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX + i, currentY + i, currentZ - i));

                }
            }

            if (currentX + i < boardSize && currentY - i >= -3 && currentZ - i >= -3)
            {
                Piece c = Game.Board.board[currentX + i, currentY - i, currentZ - i].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX + i, currentY - i, currentZ - i));

                }
            }

            // Down-left direction
            if (currentX - i >= -3 && currentY + i < boardSize && currentZ - i >= -3)
            {
                Piece c = Game.Board.board[currentX - i, currentY + i, currentZ - i].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX - i, currentY + i, currentZ - i));

                }
            }

            if (currentX - i >= -3 && currentY - i >= -3 && currentZ - i >= -3)
            {
                Piece c = Game.Board.board[currentX - i, currentY - i, currentZ - i].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX - i, currentY - i, currentZ - i));

                }
            }

        }

        setPossibleMoves(moves);
    }
}

