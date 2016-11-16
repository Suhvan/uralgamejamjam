using System;

[Serializable]
public struct IntVector2
{

	public static IntVector2 zero = new IntVector2(0, 0);
	public static IntVector2 up = new IntVector2(0, 1);
	public static IntVector2 down = new IntVector2(0, -1);
	public static IntVector2 left = new IntVector2(-1, 0);
	public static IntVector2 right = new IntVector2(1, 0);

	public int x, y;

	public IntVector2(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public static IntVector2 operator +(IntVector2 a, IntVector2 b)
	{
		a.x += b.x;
		a.y += b.y;
		return a;
	}

}