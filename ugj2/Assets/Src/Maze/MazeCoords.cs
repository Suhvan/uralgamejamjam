using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static	class MazeCoords
{
	public static float CellSize;
	public static IntVector2 MazeSize;

	public static IntVector2 WorldToCellCoords(Vector3 coords)
	{
		IntVector2 cellCoords = new IntVector2();
		float cellXFloat = coords.x / CellSize + MazeSize.x / 2;
		cellCoords.x = (int)System.Math.Truncate(cellXFloat);
		if (cellXFloat - cellCoords.x > 0.5)
		{
			cellCoords.x++;
		}

		float cellYFloat = coords.y / CellSize + MazeSize.y / 2;
		cellCoords.y = (int)System.Math.Truncate(cellYFloat);
		if (cellYFloat - cellCoords.y > 0.5)
		{
			cellCoords.y++;
		}

		return cellCoords;
	}

	public static Vector2 CellToWorldCoords(IntVector2 coords)
	{
		return new Vector2((coords.x - MazeSize.x / 2) * CellSize, (coords.y - MazeSize.y / 2) * CellSize);
	}

	public static IntVector2 RandomCoords
	{
		get
		{
			return new IntVector2(UnityEngine.Random.Range(0, MazeSize.x), UnityEngine.Random.Range(0, MazeSize.y));
		}
	}

	public static bool ContainsCoordinates(IntVector2 coordinate)
	{
		return coordinate.x >= 0 && coordinate.x < MazeSize.x && coordinate.y >= 0 && coordinate.y < MazeSize.y;
	}
}

