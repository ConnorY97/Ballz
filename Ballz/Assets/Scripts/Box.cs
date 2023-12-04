using UnityEngine;

public class Box : BaseSpawnable
{
    public override void Init(int currentRound)
    {
        base.Init();

        objectValue = Random.Range(currentRound, 5 * currentRound);
        uiObject.text = objectValue.ToString();

        if (objectValue < 5)
        {
            spriteRenderer.color = Color.green;
        }
        else
        {
            spriteRenderer.color = Color.red;
        }
    }

    public override void Hit()
    {
        objectValue--;

        if (objectValue <= 0)
        {
            GameManager.Instance.SpawnableDestroyed(this);
            Destroy(this.gameObject);
        }

        uiObject.text = objectValue.ToString();

        if (objectValue < 5)
        {
            spriteRenderer.color = Color.green;
        }
        else
        {
            spriteRenderer.color = Color.red;
        }
    }
}
