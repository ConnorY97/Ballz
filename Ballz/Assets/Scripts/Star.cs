using UnityEngine;

public class Star : BaseSpawnable
{
    public override void Init()
    {
        base.Init();

        objectValue = Random.Range(1, 10);
        uiObject.text = objectValue.ToString();
    }

    public override void Hit()
    {
        GameManager.Instance.AddBalls(objectValue);
        GameManager.Instance.SpawnableDestroyed(this);
        Destroy(gameObject);
    }
}
