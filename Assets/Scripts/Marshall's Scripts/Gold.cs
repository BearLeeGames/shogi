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
        this.player1 = true;
        this.selected = false;
    }

    public void Update()
    {

        // To do: implement checking if piece is selected, and if space to move to has been selected
        updatePossibleMoves();

        // For testing purposes to see knight movement

    }

    // Lance can move to any amount of squares ahead of it
    // To do: implement checking of pieces occupying spaces in front of it using gameboard
    public override void updatePossibleMoves()
    {
        List<Vector3> moves = new List<Vector3>();

        for (int z = 1; z >=0; --z)
        {
            for(int y = 1; y >= -1; --y)
            {
                for(int x = -1; x <=1; ++x)
                {
                    if (!(x == 0 && y == 0 && z == 0))
                    {
                        int newX = currentX + x;
                        int newY = currentY + y;
                        int newZ = currentZ + z;

                        if (newX >= 0 && newX < 9 && newY >= 0 && newY < 9 && newZ >= 0 && newZ < 9)
                        {
                            moves.Add(new Vector3(newX, newY, newZ));
                        }
                    }
                }
            }
        }

        if(currentZ - 1 >=0)
        {
            moves.Add(new Vector3(currentX, currentY, currentZ - 1));
        }

        setPossibleMoves(moves);
    }
}

