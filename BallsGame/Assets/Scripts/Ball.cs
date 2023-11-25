using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Box")
        {
            Box tmp = collision.gameObject.GetComponent<Box>();
            if (tmp != null)
            {
                tmp.Hit();
            }
        }
        else if (collision.gameObject.tag == "Base")
        {
            GameManager.Instance.TouchedBase(this);
        }
    }
}
