using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilverGeneral : ShogiPiece
{
    // script for Silver General fields and behaviors
    

    // TODO: check for which team the piece is on before ruling out an available spot
    public override bool[,,] PossibleMoves()
    {
        int size = BoardManager.Instance.BOARD_SIZE;
        bool[,,] moves = new bool[size, size, size];

        // use this to check for pieces and which team they're on
        ShogiPiece c;

        // determine 'forward' based on which player's piece is moving
        int forward = 1;
        if (!isPlayer1)
        {
            forward = -1;
        }

        // check the panel in front for pieces
        for (int i = -1; i < 2; i++)
        {
            // check left, center, and right positions
            for (int j = -1; j < 2; j++)
            {
                // if out of bounds, skip this option
                if (!checkBounds(currentX + j, currentY + i, currentZ + forward))
                {
                    continue;
                }

                Debug.Log("checking position: " + (currentX + j) + (currentY + i) + (currentZ + forward));

                // get the object at that position
                c = BoardManager.Instance.shogiPieces[currentX + j, currentY + i, currentZ + forward];

                // if there is not one there, or it is an enemy piece, add it as an available move
                if (c == null || c.isPlayer1 != isPlayer1)
                {
                    moves[currentX + j, currentY + i, currentZ + forward] = true;
                }
               
            }
        }

        // check the panel in back for pieces
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                // if out of bounds, skip this option
                if (!checkBounds(currentX + j, currentY + i, currentZ - forward))
                {
                    continue;
                }

                // if there is not one there, add it as an available move (excluding the panel directly behind
                if (BoardManager.Instance.shogiPieces[currentX + j, currentY + i, currentZ - forward] == null && (i !=0 && j != 0))
                {
                    moves[currentX + j, currentY + i, currentZ - forward] = true;
                }
            }
        }

        return moves;
    }
}
