package plz.model.template;

import java.util.Date;

public class Tile implements Cloneable
{
	public Cell ParentCell;
	public int TypeTile;
	public boolean IsFilled;
	public TileState State;
	public Cell TargetCell;

	public Tile()
	{
		State = TileState.Normal;
	}
	
	@Override
	public Object clone()
	{
		Tile tile = new Tile();
		
		tile.ParentCell = this.ParentCell;
		tile.TypeTile = this.TypeTile;
		tile.IsFilled = this.IsFilled;
		tile.State = this.State;
		
		return tile;
	}
}
