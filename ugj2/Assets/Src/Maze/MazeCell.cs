using UnityEngine;
using System.Collections;

public class MazeCell : MonoBehaviour {

	public IntVector2 coordinates;

	public MazeRoom room;

	public void Initialize(MazeRoom room)
	{
		this.room = room;
		SR.sprite = room.Settings.GetRandomSprite();
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

	private SpriteRenderer m_sr;
	private SpriteRenderer SR
	{
		get
		{
			if (m_sr == null)
			{
				m_sr = GetComponent<SpriteRenderer>();
			}
			return m_sr;
		}
	}

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
				//SR.color = Color.green;
			}
			else
			{
				//SR.color = Color.white;
			}

		}
	}

	public void Start()
	{
	//	SR.sprite = GameCore.mazeInstance.mazePool.GetRandomSprite();
    }

	public void ValidateEdges()
	{
		for (int i = 0; i < MazeDirections.Count; i++)
		{
			if (edges[i].otherCell == null || !edges[i].otherCell.Lit)
			{	
				GameCore.mazeInstance.mazePool.ReturnObject(edges[i]);
				edges[i] = null;
                initializedEdgeCount--;
            }
		}
	}

	public void ClearEdges()
	{
		for (int i = 0; i < MazeDirections.Count; i++)
		{	
			GameCore.mazeInstance.mazePool.ReturnObject(edges[i]);
			edges[i] = null;
			initializedEdgeCount--;
		}
	}


	public void SetEdge(MazeDirection direction, MazeCellEdge edge)
	{
		edges[(int)direction] = edge;
		initializedEdgeCount += 1;
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
