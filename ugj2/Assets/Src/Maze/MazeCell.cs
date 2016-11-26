using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeCell : MonoBehaviour {

	public IntVector2 coordinates;

	public MazeRoom room;

	public int Component = -1;

	public bool Amazed;

	public void Initialize(MazeRoom room, int component)
	{
		Component = component;
        this.room = room;
		sprite.sprite = room.Settings.GetRandomSprite( SpriteType.Cell);
    }

	public void MakeEntry(MazeDirection direction)
	{
		MazePassage entry = GameCore.mazeInstance.mazePool.GetPassage();
		entry.isEntry = true;
        entry.Initialize(this, null, direction);
	}

	private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];

	public MazeCellEdge GetEdge(MazeDirection direction)
	{
		return edges[(int)direction];
	}

	private int initializedEdgeCount;

	public bool IsFullyInitialized
	{
		get
		{
			return initializedEdgeCount == MazeDirections.Count;
		}
	}

	[SerializeField]
	private SpriteRenderer sprite;


	[SerializeField]
	private int m_litSrc;

	public bool Lit
	{
        get
		{
			return m_litSrc >0;
		}
		set
		{
			if (value)
			{
				m_litSrc++;
			}
			else
			{
				m_litSrc--;
			}

			if (Lit)
			{
				//sprite.color = Color.green;
			}
			else
			{
				//sprite.color = Color.white;
			}

		}
	}

	public void Start()
	{
		GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
		GetComponent<Renderer>().receiveShadows = true;
	}

	public void ValidateEdges()
	{
		for (int i = 0; i < MazeDirections.Count; i++)
		{
			if (edges[i].otherCell == null || !edges[i].otherCell.Lit)
			{	
				GameCore.mazeInstance.mazePool.ReturnObject(edges[i],(MazeDirection)i);
				edges[i] = null;
                initializedEdgeCount--;
            }
		}
	}

	public void ClearEdges()
	{
		for (int i = 0; i < MazeDirections.Count; i++)
		{	
			GameCore.mazeInstance.mazePool.ReturnObject(edges[i], (MazeDirection)i);
			edges[i] = null;
			initializedEdgeCount--;
		}
	}

	public void SetEdge(MazeDirection direction, MazeCellEdge edge)
	{
		edges[(int)direction] = edge;
		edge.EnableRoomSettings();
		initializedEdgeCount += 1;
	}

	public List<MazeCell> WaveSearch()
	{
		List<MazeCell> res = new List<MazeCell>();
		foreach (var edge in edges)
		{
			MazePassage mp = edge as MazePassage;
			if (mp != null && !mp.SameComponents)
			{
				res.Add(mp.otherCell);
				mp.otherCell.Component = Component;
            }
		}
		return res;
    }

	public MazeDirection RandomPassage
	{
		get
		{	
            int skips = Random.Range(0, MazeDirections.Count);
			for (int i = 0; i < MazeDirections.Count; i++)
			{
				int index = (skips + i) % MazeDirections.Count;
				var pass = edges[index] as MazePassage;
                if (pass != null && !pass.isEntry)
				{
					return (MazeDirection)index;
				}
			}
			throw new System.InvalidOperationException("MazeCell has no passage.");
		}
	}

	public MazeDirection RandomUninitializedDirection
	{
		get
		{
			int skips = Random.Range(0, MazeDirections.Count - initializedEdgeCount);
			for (int i = 0; i < MazeDirections.Count; i++)
			{
				if (edges[i] == null)
				{
					if (skips == 0)
					{
						return (MazeDirection)i;
					}
					skips -= 1;
				}
			}
			throw new System.InvalidOperationException("MazeCell has no uninitialized directions left.");
		}
	}

}
