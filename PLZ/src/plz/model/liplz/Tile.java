package plz.model.liplz;

import java.util.Date;

public class Tile implements Cloneable
{
	public Cell ParentCell;
	public int Symbol;
	public int Color;
	public TileState State;
	
	public Tile()
	{
		State = TileState.Normal;
	}
	
	@Override
	public Object clone()
	{
		Tile tile = new Tile();
		
		tile.ParentCell = this.ParentCell;
		tile.Symbol = this.Symbol;
		tile.Color = this.Color;
		tile.State = this.State;
		
		return tile;
	}
}
