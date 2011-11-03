package plz.model.griplz;

import java.util.Date;

public class Tile implements Cloneable
{
	public Cell ParentCell;
	public int TypeTile;
	public Date StartTimeMovement;
	public int DirectionMovement=-1;
	public boolean IsFilled;
	public float PercentMovement;
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
		tile.TypeTile = this.TypeTile;
		tile.StartTimeMovement = this.StartTimeMovement;
		tile.DirectionMovement = this.DirectionMovement;
		tile.IsFilled = this.IsFilled;
		tile.State = this.State;
		
		return tile;
	}
}
