using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;


/// <summary>
    //Object that describes a set of captured pieces of a certain type
/// </summary>
public class CapturedPiece : MonoBehaviour
{

    /**
     * Holds all the data members that the class
     * contains.
     */
    #region Data Members

    [Header("Captured Piece Info")]
    public pieceType type;
    public bool isPlayer1;
    public int count;

    #endregion

    /**
     * Modifies the data members so that they may
     * be read-only, return specific values, or
     * expose certain data members to the public
     */
    #region Member Properties


    #endregion

    /**
     * Any Unity Methods used.
     */
    #region Unity Methods
    #endregion

    /**
     * Constructors that are called when building
     * the class.
     */
    #region Constructors
    //public CapturedPiece(pieceType type, int count, bool isPlayer1)
    //{
    //    this.type = type;
    //    this.count = count;
    //    this.isPlayer1 = isPlayer1;
    //}
    #endregion


    /**
     * Methods that are able to be called from
     * outside of the class.
     */
    #region Public Methods
    #endregion

    /**
     * Private functions that are only used from
     * within this class.
     */
    #region Member Functions
    #endregion


}
