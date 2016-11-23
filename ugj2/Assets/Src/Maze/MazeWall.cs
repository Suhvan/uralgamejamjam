using UnityEngine;
using System.Collections;

public class MazeWall : MazeCellEdge
{

	[SerializeField]
	private SpriteRenderer wallSprite;


	public override void EnableRoomSettings()
	{
		wallSprite.sprite = cell.room.Settings.GetRandomSprite(SpriteType.Wall);
	}
}
