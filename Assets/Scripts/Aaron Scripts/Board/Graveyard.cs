using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graveyard : MonoBehaviour
{
    void Start()
    {
        Dictionary<string, int> Graveyard1 = new Dictionary<string, int>();
        Dictionary<string, int> Graveyard2 = new Dictionary<string, int>();

        // Player 1 Graveyard initialization
        Graveyard1.Add("Pawn", 0);
        Graveyard1.Add("Lance", 0);
        Graveyard1.Add("Knight", 0);
        Graveyard1.Add("Gold", 0);
        Graveyard1.Add("Silver", 0);
        Graveyard1.Add("Bishop", 0);
        Graveyard1.Add("Rook", 0);
        Graveyard1.Add("PromotedRook", 0);
        Graveyard1.Add("PromotedBishop", 0);
        Graveyard1.Add("PromotedPawn", 0);
        Graveyard1.Add("PromotedKnight", 0);
        Graveyard1.Add("PromotedSilver", 0);
        Graveyard1.Add("PromotedLance", 0);

        // Player 2 Graveyard initialization
        Graveyard2.Add("Pawn", 0);
        Graveyard2.Add("Lance", 0);
        Graveyard2.Add("Knight", 0);
        Graveyard2.Add("Gold", 0);
        Graveyard2.Add("Silver", 0);
        Graveyard2.Add("Bishop", 0);
        Graveyard2.Add("Rook", 0);
        Graveyard2.Add("PromotedRook", 0);
        Graveyard2.Add("PromotedBishop", 0);
        Graveyard2.Add("PromotedPawn", 0);
        Graveyard2.Add("PromotedKnight", 0);
        Graveyard2.Add("PromotedSilver", 0);
        Graveyard2.Add("PromotedLance", 0);
    }
}
