using System.Collections;
using TMPro;
using UnityEngine;

public class BaseSpawnable : MonoBehaviour
{
    [SerializeField]
    protected TMP_Text uiObject;

    [SerializeField]
    protected SpriteRenderer spriteRenderer;

    [SerializeField]
    protected float lerpTime = 2;

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
        if (column[currentCell] == null)
        {
            GameManager.Instance.GameOver();
        }

        StartCoroutine(LerpFunction(column[currentCell].transform.position));
    }

    IEnumerator LerpFunction(Vector3 targetYPos)
    {
        float time = 0;
        Vector3 startPos = transform.position;
        while (time < lerpTime)
        {
            transform.position = Vector3.Lerp(startPos, targetYPos, time / lerpTime);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetYPos;
    }
}
