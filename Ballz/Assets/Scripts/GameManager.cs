using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;
using System.Runtime.InteropServices;

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

    #region Vars
    // Inspector values--------------------------
    [Header("Game values")]
    public int rows;
    public int cols;
    public float cellSize;
    public float xSpacing;
    public float ySpacing;
    public float scalar = 160;
    public Camera mainCamera;
    public Box boxPrefab;
    public float minBallVelocity = 100;
    private GameObject[][] cells;

    // Box values--------------------------------
    [SerializeField]
    private List<BaseSpawnable> spawnObjects = new List<BaseSpawnable>();

    // Ball values-------------------------------
    [Header("Ball values")]
    public GameObject ballSpawn;
    public Ball ballPrefab;
    public TMP_Text uiCurrentBallCount;
    public TMP_Text uiReturnedBallCount;
    public Canvas currentBallCountCanvas;
    public Canvas returnedBallCountCanvas;
    public float gravityMultipler = 0.75f;
    public int ballSpawnAmount = 1;

    private List<Ball> balls = new List<Ball>();
    private bool isShooting = false;
    private int ballsShot = 0;
    private int ballsShotAmount = 0;
    private int ballsToShoot = 0;
    private int ballsReturned = 0;
    private bool first = true;


    // Click values------------------------------
    [Header("Shoot values")]
    public LineRenderer drag;
    public float multiplyier = 100.0f;

    private Vector3 clickPos = Vector3.zero;
    private Vector3 dragPos = Vector3.zero;
    private Vector3 shootDir = Vector3.zero;
    private float shootForce = 0.0f;

    // Game manager values-----------------------
    private int RoundCounter = 1;
    public TMP_Text roundCounterUI;

    // Star values-------------------------------
    public Star starPrefab;

    // UI----------------------------------------
    public UnityEngine.UIElements.Button optionsButton = null;
    public Canvas optionsCanvas = null;
    public UnityEngine.UI.Slider masterVolSlider = null;

    // Audio--------------------------------------
    private FMOD.ChannelGroup masterChannelGroup;
    #endregion

    private void Start()
    {
        GameObject temp = new GameObject();
        cells = new GameObject[cols][];
        for (int i = 0; i < cols; i++)
        {
            cells[i] = new GameObject[rows];
            for (int j = 0; j < rows; j++)
            {
                cells[i][j] = new GameObject();
                cells[i][j].transform.position = new Vector3(transform.position.x + (i * (cellSize + xSpacing)), transform.position.y - (j * (cellSize + ySpacing)), 0);
            }
        }

        SpawnSpawnable();
        SpawnBall(ballSpawnAmount);
        drag.SetPosition(0, ballSpawn.transform.position);
        drag.SetPosition(1, ballSpawn.transform.position);

        if (masterVolSlider != null)
        {
            masterVolSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        }

        // Get the master channel group to control audio volume
        FMODUnity.RuntimeManager.CoreSystem.getMasterChannelGroup(out masterChannelGroup);

        // Hide the return count
        returnedBallCountCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RoundCounter++;
            Move();
        }

        if (!isShooting && !optionsCanvas.isActiveAndEnabled)
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
                ballsReturned = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CleanUp();
        }

        roundCounterUI.text = RoundCounter.ToString();
    }

    public void SpawnSpawnable()
    {
        if (boxPrefab != null && starPrefab != null)
        {
            for (int i = 0; i < cols; i++)
            {
                int spawnChance = Random.Range(0, 10);
                if (spawnChance < 3)
                {
                    Box box = Instantiate(boxPrefab, cells[i][0].transform);
                    box.Init(RoundCounter, cells[i]);
                    box.transform.position = cells[i][0].transform.position;
                    spawnObjects.Add(box);
                }
                else if (spawnChance > 8)
                {
                    Star star = Instantiate(starPrefab, cells[i][0].transform);
                    star.Init(RoundCounter, cells[i]);
                    star.transform.position = cells[i][0].transform.position;
                    spawnObjects.Add(star);
                }
            }

            /*
            for (int i = 0; i < cols; i++)
            {
                int spawnChance = Random.Range(0, 10);
                if (spawnChance < 3)
                {
                    Box box = Instantiate(boxPrefab, transform);
                    box.Init(RoundCounter);
                    box.transform.position = new Vector2(transform.position.x + (i * (cellSize + xSpacing)), transform.position.y);
                    box.Col = i;
                    box.Row = 0;
                    spawnObjects.Add(box);
                    box.transform.localScale = new Vector3(cellSize, cellSize, cellSize);
                    continue;
                }
                else if (spawnChance > 8)
                {
                    Star star = Instantiate(starPrefab, transform);
                    star.Init();
                    star.transform.position = new Vector2(transform.position.x + (i * (cellSize + xSpacing)), transform.position.y);
                    star.Col = i;
                    star.Row = 0;
                    spawnObjects.Add(star);
                    star.transform.localScale = new Vector3(cellSize, cellSize, cellSize);
                    continue;
                }
            }
            if (spawnObjects.Count == 0)
            {
                SpawnSpawnable();
            }
            */
        }
        else
        {
            Debug.Log("Missing prefabs");
        }
    }

    public void Move()
    {
        if (spawnObjects.Count != 0)
        {
            for (int i = 0; i < spawnObjects.Count; i++)
            {
                //spawnObjects[i].transform.position = new Vector2(spawnObjects[i].transform.position.x, spawnObjects[i].transform.position.y - cellSize);
                //spawnObjects[i].Row += 1;
                spawnObjects[i].Move();
                if (spawnObjects[i].transform.position.y <= ballSpawn.transform.position.y)
                {
                    Debug.Log("Game Over");
                }
            }
        }
        SpawnSpawnable();
    }

    public void SpawnBall(int amountToSpawn)
    {
        if (ballPrefab != null && ballSpawn != null)
        {
            for (int i = 0; i < amountToSpawn; i++)
            {
                Ball ball = Instantiate(ballPrefab, transform);
                ball.Init(minBallVelocity);
                ball.transform.position = ballSpawn.transform.position;
                ball.Collder.isTrigger = true;
                ball.Rigidbody.Sleep();
                balls.Add(ball);
            }
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
            ballsShotAmount = balls.Count;
            ballsToShoot = balls.Count;
            for (int i = 0; i < balls.Count; i++)
            {
                if (balls[i].Rigidbody != null)
                {
                    balls[i].Collder.isTrigger = false;
                    balls[i].Rigidbody.WakeUp();
                    balls[i].CurrentGravity = gravityMultipler;
                    balls[i].Rigidbody.AddForce(dir * force * multiplyier);
                    balls[i].Shot = true;
                    ballsShot++;

                    ballsShotAmount--;

                    ballsToShoot--;
                    uiCurrentBallCount.text = ballsToShoot.ToString();
                }
                else
                {
                    Debug.Log("RB null");
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public void SpawnableDestroyed(BaseSpawnable killed)
    {
        spawnObjects.Remove(killed);
    }

    public void TouchedBase(Ball ballToReturn)
    {
        // This is the first ball to touch the base.
        //  So we want to reset the spawn position to provide new angle.
        if (first)
        {
            first = false;
            // Make sure that the height of the spawn is maintained.
            ballSpawn.transform.position = new Vector3(ballToReturn.transform.position.x, ballSpawn.transform.position.y, ballToReturn.transform.position.z);

            returnedBallCountCanvas.gameObject.SetActive(true);
        }

        ballToReturn.Rigidbody.Sleep();
        ballToReturn.Collder.isTrigger = true;
        ballToReturn.Rigidbody.velocity = Vector3.zero;
        ballToReturn.transform.position = ballSpawn.transform.position;
        ballToReturn.Shot = false;

        ballsShot--;

        ballsShotAmount++;

        ballsReturned++;

        uiReturnedBallCount.text = ballsReturned.ToString();

        if (ballsShot == 0)
        {
            isShooting = false;
            Move();
            RoundCounter++;
            first = true;
            if (balls.Count != ballSpawnAmount)
            {
                SpawnBall(ballSpawnAmount - balls.Count);
                //uiCurrentBallCount.text = ballsShotAmount.ToString();
            }

            returnedBallCountCanvas.gameObject.SetActive(false);
            currentBallCountCanvas.transform.position = returnedBallCountCanvas.transform.position;
            uiCurrentBallCount.text = uiReturnedBallCount.text;
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }

    private void CleanUp()
    {
        //foreach (var box in spawnObjects)
        //{
        //    Destroy(box.gameObject);
        //}
        //foreach (var ball in balls)
        //{
        //    Destroy(ball.gameObject);
        //}
        //balls.Clear();
        //spawnObjects.Clear();

        //SpawnBall(ballSpawnAmount);
    }

    public void AddBalls(int amount)
    {
        ballSpawnAmount += amount;
    }

    public void GameOver()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QUIT()
    {
        Application.Quit();
    }

    public void Options()
    {
        optionsCanvas.gameObject.SetActive(!optionsCanvas.gameObject.activeInHierarchy);
    }

    private void ValueChangeCheck()
    {
        if (masterChannelGroup.hasHandle())
        {
            masterChannelGroup.setVolume(masterVolSlider.value);
        }
    }

    private void OnDrawGizmos()
    {
        if (cells != null)
        {
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    Gizmos.DrawCube(cells[i][j].transform.position, new Vector3(cellSize, cellSize, cellSize));
                }
            }
        }
    }
}
