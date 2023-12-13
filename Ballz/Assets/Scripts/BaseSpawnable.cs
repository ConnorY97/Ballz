using TMPro;
using UnityEngine;

public class BaseSpawnable : MonoBehaviour
{
    [SerializeField]
    protected TMP_Text uiObject;

    [SerializeField]
    protected SpriteRenderer spriteRenderer;

    [SerializeField]
    protected Rigidbody2D rb;
    public Rigidbody2D Rigidbody
    {
        get { return rb; }
    }

    protected Transform cell;
    public Transform Cell
    {
        get { return cell; }
        set { cell = value; }
    }

    //private int row;
    //public int Row
    //{
    //    get { return row; }
    //    set { row = value; }
    //}
    //private int col;
    //public int Col
    //{
    //    get { return col; }
    //    set { col = value; }
    //}

    protected int objectValue = 0;
    public int ObjectValue
    {
        get { return objectValue; }
        set { objectValue = value; }
    }

    protected GameObject[] column;
    public GameObject[] Column
    {
        set { Column = value; }
    }

    protected int currentCell = 0;
    public int CurrentCell
    {
        get { return currentCell; }
        set { currentCell = value; }
    }

    //public virtual void Init()
    //{
    //    if (uiObject == null)
    //    {
    //        Debug.Log("Please assign UI Object to " + gameObject.name);
    //        return;
    //    }

    //    if (spriteRenderer == null)
    //    {
    //        Debug.Log("Please assign SpriteRenderer to " + gameObject.name);
    //        return;
    //    }
    //}

    public virtual void Init(int value, GameObject[] setColumn)
    {
        if (uiObject == null)
        {
            Debug.Log("Please Assign UI Object to " + gameObject.name);
            return;
        }

        if (spriteRenderer == null)
        {
            Debug.Log("Please assign SpriteRenderer to " + gameObject.name);
            return;
        }

        if (setColumn != null)
        {
            column = setColumn;
        }
    }

    public virtual void Hit() { }

    public void Move()
    {
        ++currentCell;
        // TODO
        // Add some nice lerping to make this look pretty
        transform.position = column[currentCell].transform.position;
    }
}
