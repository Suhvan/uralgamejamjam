
using UnityEngine;

public abstract class MazeCellEdge : MonoBehaviour
{
	public MazeCell cell, otherCell;

	public MazeDirection direction;

	public void Initialize(MazeCell cell, MazeCell otherCell, MazeDirection direction)
	{
		this.cell = cell;
		this.otherCell = otherCell;
		this.direction = direction;
		cell.SetEdge(direction, this);
		transform.parent = cell.transform ;
		transform.localPosition = Vector3.zero;
		transform.localRotation = direction.ToRotation();
		transform.position = new Vector3(transform.position.x, transform.position.y, 0.01f * (1 + (int)direction) + (float)(cell.coordinates.x + cell.coordinates.y) * 0.00001f);
	}

	public abstract void EnableRoomSettings();
	
}

