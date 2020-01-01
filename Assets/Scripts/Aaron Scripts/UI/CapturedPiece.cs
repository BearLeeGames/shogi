using UnityEngine;


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

    static CapturedPiece m_instance;

    [Header("Captured Piece Info")]
    public pieceType type;
    public bool isPlayer1;
    public int count;

    static bool s_pieceDropMode = false;
    static int s_selectedX = -1;
    static int s_selectedY = -1;
    static int s_selectedZ = -1;

    #endregion

    /**
     * Modifies the data members so that they may
     * be read-only, return specific values, or
     * expose certain data members to the public
     */
    #region Member Properties
    public static CapturedPiece Instance
    {
        get { return m_instance; }
    }

    public static bool pieceDropMode
    {
        get { return s_pieceDropMode; }
    }

    public static int selectedX
    {
        get { return s_selectedX; }
    }

    public static int selectedY
    {
        get { return s_selectedY; }
    }

    public static int selectedZ
    {
        get { return s_selectedZ; }
    }

    public static void setPieceDropMode(bool mode)
    {
        s_pieceDropMode = mode;
    }

    public static void setSelectedX(int x)
    {
        s_selectedX = x;
    }

    public static void setSelectedY(int y)
    {
        s_selectedY = y;
    }

    public static void setSelectedZ(int z)
    {
        s_selectedZ = z;
    }

    #endregion

    /**
     * Any Unity Methods used.
     */
    #region Unity Methods

    void Start()
    {
        // Save the current instance of this class
        m_instance = this;
    }

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

    public void StartPieceDrop()
    {
        setSelectedX(-1);
        setSelectedY(-1);
        setSelectedZ(-1);
        if (isPlayer1 == Game.Board.isPlayer1Turn)
        {
            // turn on piece drop flag
            setPieceDropMode(true);

            // set the board to all highlighted
            Game.BoardSelection.Instance.RangeBoard(true, -1, -1);
        }
        else
        {
            Debug.Log("INVALID MOVE: not your turn");
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
