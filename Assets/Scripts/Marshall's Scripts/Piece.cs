using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum pieceType { Pawn, Lance, Knight, Bishop, Rook, King, Gold, Silver, PromotedRook, PromotedBishop, PromotedSilver}

public abstract class Piece : MonoBehaviour
{
    [SerializeField] public pieceType pt;
    [SerializeField] public int currentX;
    [SerializeField] public int currentY;
    [SerializeField] public int currentZ;
    [SerializeField] public bool isPlayer1;
    [SerializeField] public bool selected;

    [SerializeField] private List<Vector3> possibleMoves;

    public abstract void updatePossibleMoves();

    // Takes in new position on the board, moves the piece, and then updates its new current position and possible moves
    public void move(int x, int y, int z)
    {

        Vector3 direction = new Vector3(x - currentX, y - currentY, z - currentZ);

        transform.Translate(direction);

        setPosition(x, y, z);

        updatePossibleMoves();
    }

    public List<Vector3> getPossibleMoves()
    {
        return possibleMoves;
    }

    public void setPossibleMoves(List<Vector3> moves)
    {
        this.possibleMoves = moves;
    }

    public void setPosition(int x, int y, int z)
    {
        currentX = x;
        currentY = y;
        currentZ = z; 
    }

    public void setPlayer(bool player)
    {
        isPlayer1 = player;
    }

    public int getX()
    {
        return currentX;
    }
    
    public int getY()
    {
        return currentY;
    }

    public int getZ()
    {
        return currentZ;
    }

    public pieceType getType()
    {
        return pt;
    }
}
