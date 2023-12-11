using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickAndDirty : MonoBehaviour
{
    public GameObject obj;
    float cellWidth;
    public float scaler = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        cellWidth = Screen.width / 7;
        obj.transform.localScale = new Vector3(cellWidth/ scaler, cellWidth/ scaler, cellWidth/ scaler);

    }
}
