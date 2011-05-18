package meeplz.model;

import java.awt.Point;

import com.badlogic.gdx.math.Vector2;

public class Cell
{
	public Map Map;
	public Point Coord;
	public Vector2 Location;
	public Vector2 InitialLocation;
	public Cell[] Neighbourghs;

	public Cell(Map map, int x, int y, float left, float top)
	{
		this.Map = map;
		this.Coord = new Point(x, y);
		this.Location = new Vector2(left, top);
		this.InitialLocation = new Vector2(left, top);

		this.Neighbourghs = new Cell[6];
	}

	public int IndexPosition()
	{
		return (this.Coord.y - 1) * this.Map.Width + this.Coord.x - 1;
	}

	public Cell GetDirection(int direction, int iteration)
	{
		Cell cell = this;
		for (int i = 0; i < iteration && cell != null; i++)
		{
			cell = cell.Neighbourghs[direction];
		}

		return cell;
	}
}
