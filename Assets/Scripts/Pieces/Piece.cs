using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    [SerializeField] public int currentX;
    [SerializeField] public int currentY;
    [SerializeField] public int currentZ;
    [SerializeField] public bool player1;
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

    private void swapPlayers()
    {
        player1 = !player1;
    }
}
