using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Box":
                Box box = collision.gameObject.GetComponent<Box>();
                if (box != null)
                {
                    box.Hit();
                }
                break;
            case "Base":
                GameManager.Instance.TouchedBase(this);
                break;
            case "Star":
                Star star = collision.gameObject.GetComponent<Star>();
                if (star != null)
                {
                    star.Hit();
                }
                break;
            default:
                break;
        }
    }

    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}
