using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShogiPiece : MonoBehaviour
{
    // shogi piece base logic and fields go here

    public bool isPlayer1;

    public bool[,,] PossibleMoves()
    {
        return new bool[7, 7, 7];
    }
}
