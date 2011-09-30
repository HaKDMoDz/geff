package twiplz.model;

import com.badlogic.gdx.math.Vector2;

public class Cell implements Cloneable
{
	public Map Map;
	public Vector2 Coord;
	public Vector2 Location;
	public Vector2 InitialLocation;
	public Cell[] Neighbourghs;
	public CellPartType[] Parts;
	public byte ColorType;
	public boolean Selected;
	public boolean IsEmpty = false;
	public CellState State;

	public Cell()
	{
		this.Parts = new CellPartType[6];

		NewColorType();
		this.CalcArrows();
	}

	public Cell(Map map, int x, int y, float left, float top)
	{
		this.Map = map;
		this.Coord = new Vector2(x, y);
		this.Location = new Vector2(left, top);
		this.InitialLocation = new Vector2(left, top);

		this.Neighbourghs = new Cell[6];
		this.Parts = new CellPartType[6];
	}

	public byte NewColorType()
	{
		byte[] colorValues = new byte[7];

		colorValues[0] = 0;
		colorValues[1] = 2;
		colorValues[2] = 6;
		colorValues[3] = 4;
		colorValues[4] = 12;
		colorValues[5] = 8;
		colorValues[6] = 10;

		int index = (int) (Math.random() * 7);
		this.ColorType = colorValues[index];
		
		return this.ColorType;
	}

	public int IndexPosition()
	{
		return (int) ((this.Coord.y - 1f) * this.Map.Width + this.Coord.x - 1f);
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

	public void CalcArrows()
	{
		if (!this.IsEmpty)
		{
			// --- Calcul des flèches
			for (int i = 0; i < 6; i++)
			{
				double percentIn = 0.1;
				double percentOut = 0.1;

				double rndPercent = Math.random();

				if (rndPercent <= percentIn && !this.IsEmpty)
					this.Parts[i] = CellPartType.In;
				else if (rndPercent <= percentIn + percentOut && !this.IsEmpty)
					this.Parts[i] = CellPartType.Out;
				else
					this.Parts[i] = CellPartType.Simple;
			}

			// ---
		}
	}

	@Override
	public Object clone()
	{
		Cell cell = new Cell();

		cell.ColorType = this.ColorType;
		if (this.Coord != null)
			cell.Coord = new Vector2(this.Coord.x, this.Coord.y);
		if (this.InitialLocation != null)
			cell.InitialLocation = new Vector2(this.InitialLocation.x, this.InitialLocation.y);
		cell.Location = new Vector2(this.Location.x, this.Location.y);
		cell.Map = this.Map;
		cell.Parts = new CellPartType[6];
		cell.IsEmpty = this.IsEmpty;
		
		for (int i = 0; i < 6; i++)
		{
			cell.Parts[i] = this.Parts[i];
		}

		return cell;
	}
}
