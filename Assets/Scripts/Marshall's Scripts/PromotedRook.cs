using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromotedRook : Piece
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
        // To do: implement checking if piece is selected, and if space to move to has been selected
        updatePossibleMoves();

        // For testing purposes to see lance movement
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            move((int)piecePosition.x, (int)piecePosition.y, (int)piecePosition.z + 1);

        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            move((int)piecePosition.x, (int)piecePosition.y, (int)piecePosition.z + 2);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
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
        List<Vector3> moves = new List<Vector3>();

        int boardSize = Game.Board.boardSize;

        for (int i = 1; i < (boardSize - currentZ); ++i)
        {
            if (currentX + i < boardSize)
            {
                Piece c = Game.Board.board[currentX + i, currentY, currentZ].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX + i, currentY, currentZ));
                }
            }

            if (currentX - i >= 0)
            {
                Piece c = Game.Board.board[currentX - i, currentY, currentZ].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX - i, currentY, currentZ));
                }
            }

            if (currentY + i < boardSize)
            {
                Piece c = Game.Board.board[currentX, currentY + i, currentZ].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX, currentY + i, currentZ));
                }
            }

            if (currentY - i >= 0)
            {
                Piece c = Game.Board.board[currentX, currentY - i, currentZ].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX, currentY - i, currentZ));
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
            if (newX >= 0 && newX < boardSize && newY >= 0 && newY < boardSize && newZ >= 0 && newZ < boardSize && (c == null || c.isPlayer1 != isPlayer1) )
            {
                moves.Add(new Vector3(newX, newY, newZ));
            }
        }

        setPossibleMoves(moves);
    }
}

