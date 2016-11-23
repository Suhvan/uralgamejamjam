using UnityEngine;
using System.Collections;
using System;

public class MazePassage : MazeCellEdge
{
	public bool DoorPassage { get; set; }

	[SerializeField]
	private SpriteRenderer passSprite;


	public override void EnableRoomSettings()
	{
		passSprite.sprite = cell.room.Settings.GetRandomSprite(SpriteType.Pass);
	}
}
