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

        for (int i = 1; i < (9 - currentZ); ++i)
        {
            if (currentX + i < 9)
            {
                ShogiPiece c = BoardManager.Instance.shogiPieces[currentX + i, currentY, currentZ];
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX + i, currentY, currentZ));
                }
            }

            if (currentX - i >= 0)
            {
                ShogiPiece c = BoardManager.Instance.shogiPieces[currentX - i, currentY, currentZ];
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX - i, currentY, currentZ));
                }
            }

            if (currentY + i < 9)
            {
                ShogiPiece c = BoardManager.Instance.shogiPieces[currentX, currentY + i, currentZ];
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX, currentY + i, currentZ));
                }
            }

            if (currentY - i >= 0)
            {
                ShogiPiece c = BoardManager.Instance.shogiPieces[currentX, currentY - i, currentZ];
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX, currentY - i, currentZ));
                }
            }

            if (currentZ + i < 9)
            {
                ShogiPiece c = BoardManager.Instance.shogiPieces[currentX, currentY, currentZ + i];
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves.Add(new Vector3(currentX, currentY, currentZ + i));
                }
            }

            if (currentZ - i >= 0)
            {
                ShogiPiece c = BoardManager.Instance.shogiPieces[currentX, currentY, currentZ - i];
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


            ShogiPiece c = BoardManager.Instance.shogiPieces[newX, newY, newZ];
            if (newX >= 0 && newX < 9 && newY >= 0 && newY < 9 && newZ >= 0 && newZ < 9 && (c == null || c.isPlayer1 != isPlayer1) )
            {
                moves.Add(new Vector3(newX, newY, newZ));
            }
        }

        setPossibleMoves(moves);
    }
}

