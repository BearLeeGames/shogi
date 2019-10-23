using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silver : Piece
{
    [SerializeField]

    private Vector3 piecePosition;

    public void Awake()
    {
        this.piecePosition = transform.position;
        this.currentX = (int)piecePosition.x;
        this.currentY = (int)piecePosition.y;
        this.currentZ = (int)piecePosition.z;
        this.isPlayer1 = true;
        this.selected = false;
        this.pt = pieceType.Silver;
    }

    public void Update()
    {

        // To do: implement checking if piece is selected, and if space to move to has been selected
        updatePossibleMoves();

        // For testing purposes to see knight movement

    }

    // To do: implement checking of pieces occupying spaces in front of it using gameboard
    public override void updatePossibleMoves()
    {
        int boardSize = Game.Board.boardSize;

        List<Vector3> moves = new List<Vector3>();

        if (isPlayer1)
        {
            for (int y = 1; y >= -1; --y)
            {
                for (int x = -1; x <= 1; ++x)
                {
                    int newX = currentX + x;
                    int newY = currentY + y;
                    int newZ = currentZ + 1;

                    // if within bounds
                    if (newX >= 0 && newX < boardSize && newY >= 0 && newY < boardSize && newZ >= 0 && newZ < boardSize)
                    {
                        Piece c = Game.Board.board[newX, newY, newZ].Piece;

                        // if there is no piece, or it is not our piece
                        if (c == null || c.isPlayer1 != isPlayer1)
                        {
                            moves.Add(new Vector3(newX, newY, newZ));

                            if (newZ - 2 >= 0)
                            {
                                Piece c2 = Game.Board.board[newX, newY, newZ - 2].Piece;
                                // Adds the circular spaces on back slice, excluding the center
                                if (!(y == 0 && x == 0) && (c2 == null || c2.isPlayer1 != isPlayer1))
                                {
                                    moves.Add(new Vector3(newX, newY, newZ - 2));
                                }
                            }
                        }
                    }

                }
            }
        }
        else
        {
            for (int y = 1; y >= -1; --y)
            {
                for (int x = -1; x <= 1; ++x)
                {
                    int newX = currentX + x;
                    int newY = currentY + y;
                    int newZ = currentZ - 1;

                    // if within bounds
                    if (newX >= 0 && newX < boardSize && newY >= 0 && newY < boardSize && newZ >= 0 && newZ < boardSize)
                    {
                        Piece c = Game.Board.board[newX, newY, newZ].Piece;

                        // if there is no piece, or it is not our piece
                        if (c == null || c.isPlayer1 != isPlayer1)
                        {
                            moves.Add(new Vector3(newX, newY, newZ));

                            if (newZ + 2 >= 0)
                            {
                                Piece c2 = Game.Board.board[newX, newY, newZ - 2].Piece;
                                // Adds the circular spaces on back slice, excluding the center
                                if (!(y == 0 && x == 0) && (c2 == null || c2.isPlayer1 != isPlayer1))
                                {
                                    moves.Add(new Vector3(newX, newY, newZ + 2));
                                }
                            }
                        }
                    }

                }
            }
        }
        setPossibleMoves(moves);
    }
}

