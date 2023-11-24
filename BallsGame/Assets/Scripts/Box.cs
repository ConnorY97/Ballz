using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Box : MonoBehaviour
{
    public TextMeshProUGUI visualHitpoints;

    public int row;
    public int col;

    private int _hitpoints;
    // Start is called before the first frame update
    void Start()
    {
        int hitpoints = Random.Range(1, 101);
        _hitpoints = hitpoints;
        visualHitpoints.text = hitpoints.ToString();
    }

    public void Hit()
    {
        _hitpoints--;
    }
}
