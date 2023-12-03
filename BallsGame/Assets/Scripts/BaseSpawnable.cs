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

    private int row;
    public int Row
    {
        get { return row; }
        set { row = value; }
    }
    private int col;
    public int Col
    {
        get { return col; }
        set { col = value; }
    }

    protected int objectValue = 0;
    public int ObjectValue
    {
        get { return objectValue; }
        set { objectValue = value; }
    }

    public virtual void Init()
    {
        if (uiObject == null)
        {
            Debug.Log("Please assign UI Object to " + gameObject.name);
            return;
        }

        if (spriteRenderer == null)
        {
            Debug.Log("Please assign SpriteRenderer to " + gameObject.name);
            return;
        }
    }
    public virtual void Init(int value)
    {
        if (uiObject == null)
        {
            Debug.Log("Please Assign UI Object to " + gameObject.name);
            return;
        }
    }

    public virtual void Hit() { }
}
