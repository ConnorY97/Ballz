using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Box : MonoBehaviour
{
    public TextMeshProUGUI visualHitpoints;

    public int row;
    public int col;

    private int hitPoints;

    public void Init(float currentRound)
    {
        int startingHitPoints = Random.Range(1, (int)(5 * currentRound));
        hitPoints = startingHitPoints;
        visualHitpoints.text = startingHitPoints.ToString();
    }

    public void Hit()
    {
        hitPoints--;

        if (hitPoints <= 0)
        {
            GameManager.Instance.BoxDestroyed(this);
            this.gameObject.SetActive(false);
        }

        visualHitpoints.text = hitPoints.ToString();
    }
}
