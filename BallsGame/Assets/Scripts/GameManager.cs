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
	public Box prefab;

	private List<Box> spawnRow; 


	//Private varaibles-----------------------
	private int _roundCounter = 0;
	public List<Box> boxes; 

	private void Start()
	{
		SpawnBox();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			++_roundCounter; 
			MoveBox();
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
				Box box = Instantiate(prefab, transform);
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

}
