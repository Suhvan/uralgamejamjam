using UnityEngine;

public enum MazeDirection
{
	DOWN,
	RIGHT,
	UP,
	LEFT
}

public static class MazeDirections
{	
	private static MazeDirection[] opposites = {
		MazeDirection.UP,
		MazeDirection.LEFT,
		MazeDirection.DOWN,
		MazeDirection.RIGHT
	};

	private static IntVector2[] vectors = {
		IntVector2.down,
		IntVector2.right,
		IntVector2.up,
		IntVector2.left,
	};

	private static Quaternion[] rotations = {				
		Quaternion.Euler(0f, 0f, 270f),
		Quaternion.identity,
		Quaternion.Euler(0f, 0f, 90f),
		Quaternion.Euler(0f, 0f, 180f),
	};

	public static int Count
	{
		get
		{
			return 4;
		}
	}

	public static MazeDirection RandomValue
	{
		get
		{
			return ((MazeDirection)Random.Range(0, Count));            
		}
	}

	public static Quaternion ToRotation(this MazeDirection direction)
	{
		return rotations[(int)direction];
	}

	public static IntVector2 ToIntVector2(this MazeDirection direction)
	{
		return vectors[(int)direction];
	}

	public static Vector2 ToVector(this MazeDirection direction)
	{
		switch (direction)
		{
			case MazeDirection.DOWN:
				return Vector2.down;
			case MazeDirection.LEFT:
				return Vector2.left;
			case MazeDirection.RIGHT:
				return Vector2.right;
			case MazeDirection.UP:
				return Vector2.up;
		}
		return Vector2.zero;
	}

	public static MazeDirection GetOpposite(this MazeDirection direction)
	{
		return opposites[(int)direction];
	}
}
