package twiplz.model;

import com.badlogic.gdx.math.Vector2;

public class Tile
{
	public Cell[] Cells;
	public Vector2 Location;
	public Cell ActiveCell;
	public Cell InactiveCell;
	public TileState State;
	
	public Tile()
	{
		State = State.Move;
		Location = new Vector2();
		
		Cells = new Cell[2];
		
		Cells[0] = new Cell();
		Cells[1] = new Cell();
		
		ActiveCell = Cells[0];
		InactiveCell = Cells[1];
	}
}
