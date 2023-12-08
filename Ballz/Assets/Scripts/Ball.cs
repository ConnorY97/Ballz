using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    public Rigidbody2D Rigidbody
    {
        get { return rb; }
    }

    [SerializeField]
    private CircleCollider2D coll;
    public CircleCollider2D Collder
    {
        get { return coll; }
    }

    private bool shot = false;

    private float minVelocity;

    private float currentGravity = 0.75f;
    public float CurrentGravity
    {
        get { return currentGravity; }
        set { currentGravity = value; }
    }

    public void Init(float velocity)
    {
        minVelocity = velocity;
        rb.gravityScale = 0.0f;
    }
    public bool Shot
    {
        get { return shot; }
        set { shot = value; }
    }

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
                rb.gravityScale = 0.0f;
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

    private void Update()
    {
        rb.velocity = rb.velocity.normalized * minVelocity;

        if (shot)
        {
            //currentGravity = currentGravity + 0.005f;
            rb.gravityScale = 0.5f;// currentGravity;
        }
    }
}
