using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build.Content;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Inspector variables----------------------
    public int rows;
    public int cols;
    public float cellSize;
    public float xSpacing;
    public Camera mainCamera;
    public Box boxPrefab;

    private List<Box> spawnRow;

    //Private varaibles-----------------------
    private int _roundCounter = 0;
    public List<Box> boxes;

    // Ball stuff
    public GameObject ballSpawn;
    public Ball ballPrefab;

    // Click stuff
    private Vector3 clickPos = Vector3.zero;
    private Vector3 dragPos = Vector3.zero;
    private Vector3 shootDir = Vector3.zero;
    private float shootForce = 0.0f;
    public LineRenderer drag;
    public float multiplyier = 100.0f;
    private int ballz = 1;

    public List<Ball> balls = new List<Ball>();

    private bool isShooting = false;

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
            rb.gravityScale = 0;
            balls.Add(ball);
        }
    }

    private void Shoot(Vector3 dir, float force)
    {
        Debug.Log("Dir: " + dir + " force: " + force);
        isShooting = true;
        if (dir != Vector3.zero || force != 0f)
        {
            for (int i = 0; i < balls.Count; i++)
            {
                Rigidbody2D rb = balls[i].GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.gravityScale = 1;
                    rb.AddForce((dir *  force) * multiplyier);
                }
                else
                {
                    Debug.Log("RB null");
                }
            }
        }
        isShooting = false;
    }
}
