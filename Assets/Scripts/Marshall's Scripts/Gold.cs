using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : Piece
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
        this.pieceType = "Gold";
    }

    public void Update()
    {

        // To do: implement checking if piece is selected, and if space to move to has been selected
        updatePossibleMoves();

        // For testing purposes to see knight movement

    }

    public override void updatePossibleMoves()
    {
        int boardSize = Game.Board.boardSize;

        List<Vector3> moves = new List<Vector3>();

        if (isPlayer1)
        {
            for (int z = 1; z >= 0; --z)
            {
                for (int y = 1; y >= -1; --y)
                {
                    for (int x = -1; x <= 1; ++x)
                    {
                        if (!(x == 0 && y == 0 && z == 0))
                        {
                            int newX = currentX + x;
                            int newY = currentY + y;
                            int newZ = currentZ + z;

                            // if within bounds
                            if (newX >= 0 && newX < boardSize && newY >= 0 && newY < boardSize && newZ >= -3 && newZ < boardSize)
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
                }
            }

            if (currentZ - 1 >= 0)
            {
                Piece c = Game.Board.board[currentX, currentY, currentZ - 1].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX, currentY, currentZ - 1));
                }
            }
        }
        else
        {
            for (int z = 1; z >= 0; --z)
            {
                for (int y = 1; y >= -1; --y)
                {
                    for (int x = -1; x <= 1; ++x)
                    {
                        if (!(x == 0 && y == 0 && z == 0))
                        {
                            int newX = currentX + x;
                            int newY = currentY + y;
                            int newZ = currentZ - z;

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
                }
            }

            if (currentZ + 1 >= 0 && currentZ + 1 < boardSize)
            {
                Piece c = Game.Board.board[currentX, currentY, currentZ + 1].Piece;
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX, currentY, currentZ + 1));
                }
            }
        }
        setPossibleMoves(moves);
    }
}

