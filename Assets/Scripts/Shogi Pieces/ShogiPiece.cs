using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShogiPiece : MonoBehaviour
{
    // shogi piece base logic and fields go here

    #region Data Members
    // coordinates for the shogi piece
    public int currentX { set; get; }
    public int currentY { set; get; }
    public int currentZ { set; get; }

    public bool isPlayer1;

    #endregion

    #region Member Properties

    #endregion

    #region Unity Methods
    #endregion

    #region Constructors

    #endregion

    #region Public Methods
    /*
     * Returns:
     *  3d array of bools showing where on the board the piece can move
     */
    public virtual bool[,,] PossibleMoves()
    {
        int size = BoardManager.Instance.BOARD_SIZE;
        return new bool[size, size, size];
    }

    public void SetPosition(int x, int y, int z)
    {
        currentX = x;
        currentY = y;
        currentZ = z;
    }

    public void setPlayer(bool player)
    {
        isPlayer1 = player;
    }

    #endregion


    #region Member Functions
    protected bool checkBounds(int x, int y, int z)
    {
        int s = BoardManager.Instance.BOARD_SIZE;
        return x > 0 && y > 0 && z > 0 && x < s && y < s && z < s;
    }
    #endregion








}

// 4.5 3 15