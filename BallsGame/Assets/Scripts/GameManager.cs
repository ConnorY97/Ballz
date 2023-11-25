using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton stuff
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance !=  this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    //Inspector variables----------------------
    public int rows;
    public int cols;
    public float cellSize;
    public float xSpacing;
    public Camera mainCamera;
    public Box boxPrefab;

    private List<Box> spawnRow = new List<Box>();

    //Private varaibles-----------------------
    private int _roundCounter = 0;
    public List<Box> boxes = new List<Box>();

    // Ball stuff
    public GameObject ballSpawn;
    public Ball ballPrefab;
    public float gravityMultipler = 0.75f;

    // Click stuff
    private Vector3 clickPos = Vector3.zero;
    private Vector3 dragPos = Vector3.zero;
    private Vector3 shootDir = Vector3.zero;
    private float shootForce = 0.0f;
    public LineRenderer drag;
    public float multiplyier = 100.0f;
    private int ballz = 10;

    public List<Ball> balls = new List<Ball>();

    private bool isShooting = false;

    int ballsShot = 0;

    public int RoundCounter = 0;
    public TMP_Text roundCounterUI;

    private void Start()
    {
        SpawnBox();
        SpawnBall();
        drag.SetPosition(0, ballSpawn.transform.position);
        drag.SetPosition(1, ballSpawn.transform.position);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ++_roundCounter;
            MoveBox();
        }

        if (!isShooting)
        {
            if (Input.GetMouseButtonDown(0))
            {
                drag.SetPosition(0, ballSpawn.transform.position);
                clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                shootDir = Vector3.zero;
                shootForce = 0.0f;
            }

            if (Input.GetMouseButton(0))
            {
                dragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 endPoint = ballSpawn.transform.position + (clickPos - dragPos);
                endPoint.z = 0;
                drag.SetPosition(1, endPoint);
            }

            if (Input.GetMouseButtonUp(0))
            {
                shootForce = Vector3.Distance(clickPos, dragPos);
                shootDir = (drag.GetPosition(1) - ballSpawn.transform.position).normalized;
                Shoot(shootDir, shootForce);
                drag.SetPosition(1, ballSpawn.transform.position);
                clickPos = Vector3.zero;
                dragPos = Vector3.zero;
            }
        }

        roundCounterUI.text = RoundCounter.ToString();
    }

    public void SpawnBox()
    {
        int amountToSpawn = Random.Range(1, cols + 1);

        for (int i = 0; i < amountToSpawn; i++)
        {
            int spawnChance = Random.Range(0, 10);
            if (spawnChance < 6)
            {
                Box box = Instantiate(boxPrefab, transform);
                box.Init(RoundCounter);
                box.transform.position = new Vector2(transform.position.x + (i * (cellSize + xSpacing)), transform.position.y);
                box.col = i;
                box.row = 0;
                boxes.Add(box);
            }
        }
    }

    public void MoveBox()
    {
        for (int i = 0; i < boxes.Count; i++)
        {
            boxes[i].transform.position = new Vector2(boxes[i].transform.position.x, boxes[i].transform.position.y - cellSize);
            boxes[i].row += 1;
        }
        SpawnBox();
    }

    public void SpawnBall()
    {
        for(int i = 0;i < ballz ;i++)
        {
            Ball ball = Instantiate(ballPrefab, transform);
            ball.transform.position = ballSpawn.transform.position;
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            CircleCollider2D coll = ball.GetComponent<CircleCollider2D>();
            coll.isTrigger = true;
            rb.gravityScale = 0;
            rb.Sleep();
            balls.Add(ball);
        }
    }

    private void Shoot(Vector3 dir, float force)
    {
        StartCoroutine(ShootCoroutine(dir, force));
    }

    IEnumerator ShootCoroutine(Vector3 dir, float force)
    {
        if (dir != Vector3.zero || force != 0f)
        {
            isShooting = true;
            for (int i = 0; i < balls.Count; i++)
            {
                Rigidbody2D rb = balls[i].GetComponent<Rigidbody2D>();
                CircleCollider2D coll = balls[i].GetComponent<CircleCollider2D>();
                if (rb != null)
                {
                    coll.isTrigger = false;
                    rb.WakeUp();
                    rb.gravityScale = gravityMultipler;
                    rb.AddForce((dir * force) * multiplyier);
                    ballsShot++;
                }
                else
                {
                    Debug.Log("RB null");
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public void BoxDestroyed(Box killed)
    {
        boxes.Remove(killed);
    }

    public void TouchedBase(Ball ballToReturn)
    {
        Rigidbody2D rb = ballToReturn.GetComponent<Rigidbody2D>();
        CircleCollider2D coll = ballToReturn.GetComponent<CircleCollider2D>();

        rb.Sleep();
        coll.isTrigger = true;
        rb.velocity = Vector3.zero;
        rb.gravityScale = 0;
        ballToReturn.transform.position = ballSpawn.transform.position;
        ballsShot--;

        if (ballsShot == 0)
        {
            isShooting = false;
            SpawnBox();
            RoundCounter++;
        }
    }
}
