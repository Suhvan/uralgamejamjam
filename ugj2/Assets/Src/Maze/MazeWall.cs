using UnityEngine;
using System.Collections;

public class MazeWall : MazeCellEdge
{

	[SerializeField]
	private SpriteRenderer wallSprite;

	public override void Initialize(MazeCell cell, MazeCell otherCell, MazeDirection direction)
	{
		if(this.direction != direction)
			transform.localRotation = direction.ToRotation();
		base.Initialize(cell, otherCell, direction);

		transform.position = new Vector3(cell.transform.position.x, cell.transform.position.y, 0.01f * (1 + (int)direction) + (cell.coordinates.x + cell.coordinates.y) * 0.00001f);
	}

	public override void EnableRoomSettings()
	{
		wallSprite.sprite = cell.room.Settings.GetRandomSprite(SpriteType.Wall);
	}
}
