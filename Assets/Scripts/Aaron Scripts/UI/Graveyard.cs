using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graveyard : MonoBehaviour {

    /**
     * Holds all the data members that the class
     * contains.
     */
    #region Data Members

    // these will save the count of how many of each piece a graveyard contains
    Dictionary<pieceType, GameObject> Graveyard1 = new Dictionary<pieceType, GameObject>();
    Dictionary<pieceType, GameObject> Graveyard2 = new Dictionary<pieceType, GameObject>();

    [SerializeField] [Tooltip("Object used to depict a captured piece")] public GameObject capturedPieceUI;

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

    private void Start()
    {
        ResetGraveyards();
    }
    #endregion

    /**
     * Constructors that are called when building
     * the class.
     */
    #region Constructors

    void ResetGraveyards()
    {
        Graveyard1.Clear();
        Graveyard2.Clear();
    }

    #endregion

    /**
     * Methods that are able to be called from
     * outside of the class.
     */
    #region Public Methods

    /* 
     * Add a captured piece to a player's graveyard:
     *  Increase count if captured piece exists
     *  Create new UI object if newly captured piece type
     *  
     *  (if Player 1 captures Player 2's piece, it goes into Player 1's graveyard)
     * 
     */
    public void AddPiece(pieceType pieceType, bool isPlayer1)
    {
        // check if player 1 or 2
        if (isPlayer1)
        {
            // increase count accordingly
            if (Graveyard1.ContainsKey(pieceType))
            {
                Graveyard1[pieceType].GetComponent<CapturedPiece>().count += 1;
            }
            else
            {
                // create a new UI object
                GameObject capturedPiece = Instantiate(capturedPieceUI, transform);

                // get the CapturedPiece class and set the data
                CapturedPiece cpData = capturedPiece.GetComponent<CapturedPiece>();

                cpData.count = 1;
                cpData.type = pieceType;
                cpData.isPlayer1 = isPlayer1;

                // set the text
                Text cpText = capturedPiece.GetComponentInChildren<Text>();

                cpText.text = cpData.type.ToString() + ": " + cpData.count;

                // set the UI object in the graveyard dictionary
                Graveyard1[pieceType] = capturedPiece;
            }
        }
        else
        {
            if (Graveyard2.ContainsKey(pieceType))
            {
                Graveyard2[pieceType].GetComponent<CapturedPiece>().count += 1;
            }
            else
            {
                // create a new UI object
                GameObject capturedPiece = Instantiate(capturedPieceUI, transform);

                // get the CapturedPiece class and set the data
                CapturedPiece cpData = capturedPiece.GetComponent<CapturedPiece>();

                cpData.count = 1;
                cpData.type = pieceType;
                cpData.isPlayer1 = isPlayer1;

                // set the text
                Text cpText = capturedPiece.GetComponentInChildren<Text>();

                cpText.text = cpData.type.ToString() + ": " + cpData.count;

                // set the UI object in the graveyard dictionary
                Graveyard2[pieceType] = capturedPiece;
            }
        }
    }


    //TODO: SET UI POSITIONS RELATIVE TO GRAVEYARD
    // CREATE UI OBJECT USING FUNCTION
    // MAKE LOOK PRETTIER
    // TOGGLE WITH BUTTON


    /*
     * Show the user which pieces have been captured
     * call when "show" button is toggled on
     */
     public void ShowCapturedPieces()
    {
        // get the keys in the dicionary
        List<pieceType> keys = new List<pieceType>(Graveyard1.Keys);

        // loop through and show the captured piece UI objects
        for(int i = 0; i < Graveyard1.Count; i++)
        {
            pieceType piece = keys[i];

            Graveyard1[piece].SetActive(true);
        }
    }

    /*
     * Show the user which pieces have been captured
     * call when "show" button is toggled on
     */
    public void HideCapturedPieces()
    {
        // get the keys in the dicionary
        List<pieceType> keys = new List<pieceType>(Graveyard1.Keys);

        // loop through and hide the captured piece UI objects
        for (int i = 0; i < Graveyard1.Count; i++)
        {
            pieceType piece = keys[i];

            Graveyard1[piece].SetActive(false);
        }
    }

    #endregion

    /**
     * Private functions that are only used from
     * within this class.
     */
    #region Member Functions
    #endregion
}
