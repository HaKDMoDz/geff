package plz.model.liplz;

import plz.engine.Common;

import com.badlogic.gdx.math.Vector2;

public class Cell implements Cloneable
{
	public Map Map;
	public Vector2 Coord;
	public Vector2 Location;
	public Vector2 InitialLocation;
	public Cell[] Neighbourghs;
	public Tile Tile;

	public CellState State;

	public Cell()
	{
		
	}
	
	public Cell(Map map, int x, int y, float left, float top)
	{
		this.Map = map;
		this.Coord = new Vector2(x, y);
		this.Location = new Vector2(left, top);
		this.InitialLocation = new Vector2(left, top);

		this.Neighbourghs = new Cell[6];
		State = CellState.Visible;
		
		this.Tile = new Tile();
	}

	public int IndexPosition()
	{
		return (int) ((this.Coord.y - 1f) * this.Map.Width/2 + this.Coord.x - 1f);
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

	@Override
	public Object clone()
	{
		Cell cell = new Cell();

		if (this.Coord != null)
			cell.Coord = new Vector2(this.Coord.x, this.Coord.y);
		if (this.InitialLocation != null)
			cell.InitialLocation = new Vector2(this.InitialLocation.x, this.InitialLocation.y);
		cell.Location = new Vector2(this.Location.x, this.Location.y);
		cell.Map = this.Map;
		
		return cell;
	}
}
