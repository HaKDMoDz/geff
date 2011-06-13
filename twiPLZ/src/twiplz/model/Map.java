package twiplz.model;

import java.util.ArrayList;

public class Map
{
	public ArrayList<Cell> Cells;
	public int Width;
	public int Height;

	public Map(int width, int height)
	{
		this.Width = width;
		this.Height = height;

		CreateGrid();
	}

	public void CreateGrid()
	{
		Cells = new ArrayList<Cell>();

		float width = 0.5f;
		float height = 0.5f * (float) Math.sin(Math.PI / 3);

		for (int y = 1; y <= Height; y++)
		{
			for (int x = 1; x <= Math.round((double) Width / 2); x++)
			{
				float fx = x;
				float fy = y;

				fx = fx * width * 3f;
				fy = fy * height * 2f;

				Cell cell2 = new Cell(this, x, y * 2, fx, fy);

				Cells.add(cell2);
			}

			for (int x = 1; x <= Width / 2; x++)
			{
				float fx = x;
				float fy = y;

				fx = fx * width * 3f + width * 1.5f;
				fy = fy * height * 2f - height;

				Cell cell1 = new Cell(this, x, (y * 2) - 1, fx, fy);

				Cells.add(cell1);
			}
		}

		CalcNeighborough();

		CalcColorAndParts();
	}

	public void CalcNeighborough()
	{
		for (Cell cell : Cells)
		{
			cell.Neighbourghs = new Cell[6];

			if (cell.Coord.y % 2 == 1)
			{
				cell.Neighbourghs[0] = GetNeighborough(cell, 0, -2);
				cell.Neighbourghs[1] = GetNeighborough(cell, 1, -1);
				cell.Neighbourghs[2] = GetNeighborough(cell, 1, 1);
				cell.Neighbourghs[3] = GetNeighborough(cell, 0, 2);
				cell.Neighbourghs[4] = GetNeighborough(cell, 0, 1);
				cell.Neighbourghs[5] = GetNeighborough(cell, 0, -1);
			} else
			{
				cell.Neighbourghs[0] = GetNeighborough(cell, 0, -2);
				cell.Neighbourghs[1] = GetNeighborough(cell, 0, -1);
				cell.Neighbourghs[2] = GetNeighborough(cell, 0, 1);
				cell.Neighbourghs[3] = GetNeighborough(cell, 0, 2);
				cell.Neighbourghs[4] = GetNeighborough(cell, -1, 1);
				cell.Neighbourghs[5] = GetNeighborough(cell, -1, -1);
			}
		}
	}

	public void CalcColorAndParts()
	{
		for (Cell cell : Cells)
		{
			//--- Calcul de la couleur
			boolean colorFound = false;

			while (!colorFound)
			{
				byte colorType = (byte) (1 + Math.random() * 7);

				for (int i = 0; i < 6; i++)
				{
					if (cell.Neighbourghs[i] != null
							&& cell.Neighbourghs[i].ColorType == colorType)
					{
						colorType = 0;
						break;
					}
				}

				if (colorType != 0)
				{
					colorFound = true;
					cell.ColorType = colorType;
				}
			}
			//---
			
			//--- Calcul des flèches
			for (int i = 0; i < 6; i++)
			{
				double percentIn = 0.1;
				double percentOut = 0.1;
				
				double rndPercent = Math.random();
				
				if(rndPercent<= percentIn)
					cell.Parts[i] = CellPartType.In;
				else if(rndPercent <= percentIn+ percentOut)
					cell.Parts[i] = CellPartType.Out;
				else
					cell.Parts[i] = CellPartType.Simple;
			}
			
			//---
		}
	}

	private Cell GetNeighborough(Cell cell, int offsetX, int offsetY)
	{
		for (Cell cellNeighbor : Cells)
		{
			if (cellNeighbor.Coord.x == cell.Coord.x + offsetX
					&& cellNeighbor.Coord.y == cell.Coord.y + offsetY)
			{
				return cellNeighbor;
			}
		}

		return null;
	}

}
