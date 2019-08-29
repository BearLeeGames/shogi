using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
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
        this.player1 = true;
        this.selected = false;
        this.promoted = false;
        
    }

    public void Update()
    {
        
        // To do: implement checking if piece is selected, and if space to move to has been selected
        updatePossibleMoves();

        // For testing purposes to see knight movement
        
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

        int[] x = {1, -1, 0, 0};
        int[] y = {0, 0, 1, -1};
        int[] z = {2, 2, 2, 2 };

        for (int i = 0; i < 4; ++i)
        {
            int newX = currentX + x[i];
            int newY = currentY + y[i];
            int newZ = currentZ + z[i];

            if( newX >=0 && newX <9 && newY >=0 && newY <9 && newZ >=0 && newZ <9)
            {
                moves.Add(new Vector3(newX, newY, newZ));
            }
            
        }

        setPossibleMoves(moves);
    }
}

