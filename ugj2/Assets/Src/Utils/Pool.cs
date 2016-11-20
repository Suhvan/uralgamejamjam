using UnityEngine;
using System.Collections.Generic;

public class Pool : MonoBehaviour
{	
	List<MazeWall> wallPool = new List<MazeWall>();
	List<MazePassage> passPool = new List<MazePassage>();
	
	public MazePassage passagePrefab;
	public MazeWall wallPrefab;

	private MazeWall CreateWall()
	{
		MazeWall wall = Instantiate(wallPrefab) as MazeWall;		
        return wall;
	}

	private MazePassage CreatePassage()
	{
		MazePassage pass = Instantiate(passagePrefab) as MazePassage;
		return pass;
	}

	private void ApplyToPool(MazeCellEdge edge)
	{
		edge.gameObject.SetActive(false);
		edge.gameObject.transform.SetParent(this.transform);
	}

	public void PreparePool(IntVector2 mazeSize)
	{

	}

	public MazeWall GetWall()
	{
		MazeWall wall = null;
		if (wallPool.Count == 0)
			wall = CreateWall();
		else
		{			
			wall = wallPool[0];
			wallPool.RemoveAt(0);
		}
		wall.gameObject.SetActive(true);
		
		return wall;
	}

	public MazePassage GetPassage()
	{
		MazePassage pass = null;
		if (passPool.Count == 0)
			pass = CreatePassage();
		else
		{
			pass = passPool[0];
			passPool.RemoveAt(0);
		}
		pass.gameObject.SetActive(true);

		return pass;
	}

	public void ReturnObject(MazeCellEdge edge)
	{	
		ApplyToPool(edge);
		if (edge is MazeWall)
		{
			wallPool.Add(edge as MazeWall);
        }
		if (edge is MazePassage)
		{
			passPool.Add(edge as MazePassage);
		}
    }
	
	void Start ()
	{
		
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
