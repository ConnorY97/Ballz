using JetBrains.Annotations;
using UnityEngine;

public class Star : BaseSpawnable
{
    public override void Init(int roundCounter, GameObject[] setColumn)
    {
        base.Init(roundCounter, setColumn);

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
