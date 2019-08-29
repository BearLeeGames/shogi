﻿using System.Collections;
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
        this.player1 = true;
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
            // XZ plane 

            // Up-right direction
            if (currentX + i < 9 && currentZ + i < 9)
            {
                moves.Add(new Vector3(currentX + i, currentY, currentZ + i));
            }

            // Up-left direction
            if (currentX - i >= 0 && currentZ + i < 9)
            {
                moves.Add(new Vector3(currentX - i, currentY, currentZ +i));
            }

            // Down-right direction
            if (currentX + i < 9 && currentZ - i >=0)
            {
                moves.Add(new Vector3(currentX + i, currentY, currentZ - i));
            }

            // Down-left direction
            if (currentX - i >= 0 && currentZ - i >= 0)
            {
                moves.Add(new Vector3(currentX - i, currentY, currentZ - i));
            }

            // YZ plane

            if (currentY + i < 9 && currentZ + i < 9)
            {
                moves.Add(new Vector3(currentX, currentY + i, currentZ +i));
            }

            if (currentY - i >= 0 && currentZ - i >=0)
            {
                moves.Add(new Vector3(currentX, currentY - i, currentZ -i));
            }

            if (currentY + i < 9 && currentZ - i >= 0)
            {
                moves.Add(new Vector3(currentX, currentY + i, currentZ - i));
            }

            if (currentY - i >= 0 && currentZ + i < 9)
            {
                moves.Add(new Vector3(currentX, currentY - i, currentZ + i));
            }

            // XY plane

            if (currentX + i < 9 && currentY + i < 9)
            {
                moves.Add(new Vector3(currentX +1, currentY + i, currentZ));
            }

            if (currentX - i < 9 && currentY - i >= 0)
            {
                moves.Add(new Vector3(currentX - 1, currentY - i, currentZ));
            }

            if (currentX + i < 9 && currentY - i >= 0)
            {
                moves.Add(new Vector3(currentX + 1, currentY - i, currentZ));
            }

            if (currentX - i < 9 && currentY + i < 9)
            {
                moves.Add(new Vector3(currentX - 1, currentY + i, currentZ));
            }

            // All axis

            // Up-right direction
            if (currentX + i < 9 && currentY + i < 9 && currentZ + i < 9)
            {
                moves.Add(new Vector3(currentX + i, currentY + i, currentZ + i));
            }

            if(currentX + i < 9 && currentY - i >= 0 && currentZ + i < 9)
            {
                moves.Add(new Vector3(currentX + i, currentY - i, currentZ + i));
            }

            // Up-left direction
            if (currentX - i >= 0 && currentY + i < 9 && currentZ + i < 9)
            {
                moves.Add(new Vector3(currentX - i, currentY + i, currentZ + i));
            }

            if (currentX - i >= 0 && currentY - i >= 0 && currentZ + i < 9)
            {
                moves.Add(new Vector3(currentX - i, currentY - i, currentZ + i));
            }

            // Down-right direction
            if (currentX + i < 9 && currentY + i < 9 && currentZ - i >= 0)
            {
                moves.Add(new Vector3(currentX + i, currentY + i, currentZ - i));
            }

            if (currentX + i < 9 && currentY - i >= 0 && currentZ - i >= 0)
            {
                moves.Add(new Vector3(currentX + i, currentY - i, currentZ - i));
            }

            // Down-left direction
            if (currentX - i >= 0 && currentY + i < 9 && currentZ - i >= 0)
            {
                moves.Add(new Vector3(currentX - i, currentY + i, currentZ - i));
            }

            if (currentX - i >= 0 && currentY - i >= 0 && currentZ - i >= 0)
            {
                moves.Add(new Vector3(currentX - i, currentY - i, currentZ - i));
            }

        }

        setPossibleMoves(moves);
    }
}
