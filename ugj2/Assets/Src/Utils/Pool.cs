using UnityEngine;
using System.Collections.Generic;

public class Pool : MonoBehaviour
{
	Stack<MazeWall>[] wallPools = new Stack<MazeWall>[4];
	Stack<MazePassage> passPool = new Stack<MazePassage>();
	
	public MazePassage passagePrefab;
	public MazeWall wallPrefab;

	private MazeWall CreateWall(MazeDirection direction)
	{
		MazeWall wall = Instantiate(wallPrefab) as MazeWall;
		wall.transform.localRotation = direction.ToRotation();
		wall.gameObject.transform.SetParent(this.transform);
		return wall;
	}

	private MazePassage CreatePassage()
	{
		MazePassage pass = Instantiate(passagePrefab) as MazePassage;
		pass.gameObject.transform.SetParent(this.transform);
		return pass;
	}

	public void PreparePool(IntVector2 mazeSize)
	{
		for (int i = 0; i < MazeDirections.Count; i++)
		{
			wallPools[i] = new Stack<MazeWall>();
        }
	}

	[SerializeField]
	private Vector3 HiddenPool;

	public void HidePoolObjects()
	{
		foreach (var wallPool in wallPools)
		foreach (var wall in wallPool)
		{
			wall.gameObject.transform.position = HiddenPool;
		}

		foreach (var pass in passPool)
		{
			pass.gameObject.transform.position = HiddenPool;
		}
	}
	

	public MazeWall GetWall(MazeDirection direction)
	{		
		MazeWall wall = null;
		if (wallPools[(int)direction].Count == 0)
			wall = CreateWall(direction);
		else
		{			
			wall = wallPools[(int)direction].Pop();
		}
		
		
		return wall;
	}

	public MazePassage GetPassage()
	{
		MazePassage pass = null;
		if (passPool.Count == 0)
			pass = CreatePassage();
		else
		{
			pass = passPool.Pop();
		}

		return pass;
	}

	public void ReturnObject(MazeCellEdge edge, MazeDirection dir)
	{
		if (edge is MazeWall)
		{
			wallPools[(int)dir].Push(edge as MazeWall);
        }
		if (edge is MazePassage)
		{
			passPool.Push(edge as MazePassage);
		}
    }
	
	void Start ()
	{
		
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
