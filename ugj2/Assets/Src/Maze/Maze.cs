﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour {

	public IntVector2 size;

	public float cellSize = 0.16f;

	public MazeCell cellPrefab;

	public Pool mazePool;

	private MazeCell[,] cellPool;

	private List<MazeCell> activeCells = new List<MazeCell>();

	public MazeRoomSettings[] roomSettings;

	private List<MazeRoom> rooms = new List<MazeRoom>();

	private int m_compIdCount = 0;

	[SerializeField]
	private IntVector2 startCellCoords;
	
	[SerializeField]
	private MazeDirection entryDirection;

	[Range(0f, 1f)]
	public float doorProbability;

	private MazeRoom CreateRoom(int indexToExclude)
	{	
		var settingsIndex = Random.Range(0, roomSettings.Length);
		if (settingsIndex == indexToExclude)
		{
			settingsIndex = (settingsIndex + 1) % roomSettings.Length;
		}
		MazeRoom newRoom = new MazeRoom(rooms.Count, settingsIndex);
		rooms.Add(newRoom);
		return newRoom;
	}

	public void ShiftMaze()
	{
		m_compIdCount = 0;
		Ready = false;
		activeCells = new List<MazeCell>();
		var litCells = RemoveDarkCells();		
        foreach (var cell in litCells)
		{
			cell.Component = -1;
			cell.ValidateEdges();
			if (!cell.IsFullyInitialized)
			{
				activeCells.Add(cell);
			}
		}

		MakeComponents(litCells);
		//StartCoroutine(Generate());
		Generate();
    }
		 

	public void MakeComponents(List<MazeCell> litCells)
	{
		var curComp = new List<MazeCell>();
		while (litCells.Count > 0)
		{	
			curComp.Add(litCells[0]);
			while (curComp.Count > 0)
			{
				MazeCell currentCell = curComp[0];
				if (currentCell.Component == -1)
					currentCell.Component = m_compIdCount++;
				curComp.AddRange(currentCell.WaveSearch());
				curComp.Remove(currentCell);
				litCells.Remove(currentCell);
			}
        }
	}

	public void SpreadComponent(MazeCell cell, int comp)
	{
		List<MazeCell> curComp = new List<MazeCell>();
		curComp.Add(cell);
        while (curComp.Count > 0)
		{
			MazeCell currentCell = curComp[0];
			if (currentCell.Component != comp)
				currentCell.Component = comp;
            curComp.AddRange(currentCell.WaveSearch());
			curComp.Remove(currentCell);
		}
	}

	private List<MazeCell> RemoveDarkCells()
	{
		List<MazeCell> litCells = new List<MazeCell>();
        for (int x = 0; x < size.x; x++)
		{
			for (int y=0; y < size.y; y++)
			{
				if (!cellPool[x, y].Lit)
				{
					//Destroy(cellPool[x, y].gameObject);
					cellPool[x, y].ClearEdges();
                    cellPool[x, y].Amazed = false;
                }
				else
				{
					litCells.Add(cellPool[x, y]);
                }
			}
		}
		return litCells;
    }

	public bool Ready { get; set; } 

	//public  IEnumerator Generate()
	public void Generate()
	{	
		while (activeCells.Count > 0)
		{
	//		yield return new WaitForSeconds(0.01f);
			DoGenerationStep();
        }
		Ready = true;
		mazePool.HidePoolObjects();
    }

	public void Init(GenerationMode mode)
	{
		Ready = false;
		this.mode = mode;

		MazeCoords.CellSize = cellSize;
		MazeCoords.MazeSize = size;

		cellPool = new MazeCell[size.x, size.y];

		mazePool.PreparePool( size);

		foreach (var rs in roomSettings)
		{
			rs.Init();
		}

		for (int x = 0; x < size.x; x++)
		{
			for (int y = 0; y < size.y; y++)
			{
				CreateCell(new IntVector2(x, y));
            }
		}
		var newCell = GetCellFromPool(startCellCoords);
		newCell.Initialize(CreateRoom(-1), m_compIdCount++);		
		activeCells.Add(newCell);
	}

	public enum GenerationMode
	{
		FIRST,
		LAST, 
		MEDIUM,
		RANDOM
	}

	private GenerationMode mode;


	private void DoGenerationStep()
	{
		int currentIndex = 0;
        switch (mode)
        {
			case GenerationMode.FIRST:
				currentIndex = 0;
				break;
			case GenerationMode.LAST:
				currentIndex = activeCells.Count - 1;
				break;
			case GenerationMode.RANDOM:
				currentIndex = Random.Range(0, activeCells.Count);
				break;
			case GenerationMode.MEDIUM:
				currentIndex = activeCells.Count / 2;
				break;
		}
		
		MazeCell currentCell = activeCells[currentIndex];

		if (currentCell.IsFullyInitialized)
		{
			activeCells.RemoveAt(currentIndex);
			return;
		}
		MazeDirection direction = currentCell.RandomUninitializedDirection;
		IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
		MazeCell neighbor = GetCell(coordinates);

		if (!MazeCoords.ContainsCoordinates(coordinates))
		{
			if (currentCell.coordinates.x == startCellCoords.x && currentCell.coordinates.y == startCellCoords.y)
			{
				currentCell.MakeEntry(direction);
            }
			else
			{
				CreateWall(currentCell, null, direction);
			}
			return;
		}

		if (neighbor == null)
		{
			neighbor = GetCellFromPool(coordinates);
			CreatePassage(currentCell, neighbor, direction);
			activeCells.Add(neighbor);
		}
		else if (currentCell.room.Id == neighbor.room.Id)
		{
			CreatePassage(currentCell, neighbor, direction, doorsPissible: false);
		}
		else if (neighbor != null && neighbor.Component != currentCell.Component)
		{
			SpreadComponent(neighbor, currentCell.Component);
			CreatePassage(currentCell, neighbor, direction, doorsPissible: false);
		}
		else
		{
			CreateWall(currentCell, neighbor, direction);
		}	
		
	}


	private void CreatePassage(MazeCell cell, MazeCell otherCell, MazeDirection direction, bool doorsPissible = true )
	{
		MazePassage passage = mazePool.GetPassage();
		if (doorsPissible)
		{
			passage.DoorPassage = Random.value < doorProbability;
			if (passage.DoorPassage)
			{
				otherCell.Initialize(CreateRoom(cell.room.SettingsIndex), cell.Component);
			}
			else
			{
				otherCell.Initialize(cell.room, cell.Component);
			}
		}
		passage.Initialize(cell, otherCell, direction);
		passage.isEntry = false;
		passage = mazePool.GetPassage();
		passage.isEntry = false;
		passage.Initialize(otherCell, cell, direction.GetOpposite());
	}

	private void CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction)
	{
		MazeWall wall = mazePool.GetWall(direction);
        wall.Initialize(cell, otherCell, direction);
		if (otherCell != null)
		{
			wall = mazePool.GetWall(direction.GetOpposite());
			wall.Initialize(otherCell, cell, direction.GetOpposite());
		}
	}

	public MazeCell GetCell(IntVector2 coordinates)
	{
		if (!MazeCoords.ContainsCoordinates(coordinates) || cellPool[coordinates.x, coordinates.y].Amazed == false)
			return null;		
		return cellPool[coordinates.x, coordinates.y];
	}

	private MazeCell GetCellFromPool(IntVector2 coordinates)
	{
		if (!MazeCoords.ContainsCoordinates(coordinates))
			return null;
		cellPool[coordinates.x, coordinates.y].Amazed = true;
        return cellPool[coordinates.x, coordinates.y];
		
	}

	private MazeCell CreateCell(IntVector2 coordinates)
	{
		MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
		cellPool[coordinates.x, coordinates.y] = newCell;
		newCell.coordinates = coordinates;
		newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.y;
		newCell.transform.parent = transform;
		newCell.transform.localPosition = MazeCoords.CellToWorldCoords(coordinates);
		newCell.Amazed = false;
		return newCell;
	}
}
