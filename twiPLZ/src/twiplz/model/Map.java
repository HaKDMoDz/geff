package twiplz.model;

import java.util.ArrayList;

import plz.engine.Common;
import twiplz.Context;

import com.badlogic.gdx.Gdx;

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

		NewColor();
	}

	public void CalcNeighborough()
	{
		for (Cell cell : Cells)
		{
			CalcNeighborough(cell);
		}
	}

	public void CalcNeighborough(Cell cell)
	{
		cell.Neighbourghs = new Cell[6];

		// if (cell.Coord.y % 2 == 1)
		// {
		// cell.Neighbourghs[0] = GetNeighborough(cell, 0, -2);
		// cell.Neighbourghs[1] = GetNeighborough(cell, 1, -1);
		// cell.Neighbourghs[2] = GetNeighborough(cell, 1, 1);
		// cell.Neighbourghs[3] = GetNeighborough(cell, 0, 2);
		// cell.Neighbourghs[4] = GetNeighborough(cell, 0, 1);
		// cell.Neighbourghs[5] = GetNeighborough(cell, 0, -1);
		// }
		// else
		// {
		// cell.Neighbourghs[0] = GetNeighborough(cell, 0, -2);
		// cell.Neighbourghs[1] = GetNeighborough(cell, 0, -1);
		// cell.Neighbourghs[2] = GetNeighborough(cell, 0, 1);
		// cell.Neighbourghs[3] = GetNeighborough(cell, 0, 2);
		// cell.Neighbourghs[4] = GetNeighborough(cell, -1, 1);
		// cell.Neighbourghs[5] = GetNeighborough(cell, -1, -1);
		// }

		if (cell.Coord.y % 2 == 1)
		{
			cell.Neighbourghs[3] = GetNeighborough(cell, 0, -2);
			cell.Neighbourghs[2] = GetNeighborough(cell, 1, -1);
			cell.Neighbourghs[1] = GetNeighborough(cell, 1, 1);
			cell.Neighbourghs[0] = GetNeighborough(cell, 0, 2);
			cell.Neighbourghs[5] = GetNeighborough(cell, 0, 1);
			cell.Neighbourghs[4] = GetNeighborough(cell, 0, -1);
		}
		else
		{
			cell.Neighbourghs[3] = GetNeighborough(cell, 0, -2);
			cell.Neighbourghs[2] = GetNeighborough(cell, 0, -1);
			cell.Neighbourghs[1] = GetNeighborough(cell, 0, 1);
			cell.Neighbourghs[0] = GetNeighborough(cell, 0, 2);
			cell.Neighbourghs[5] = GetNeighborough(cell, -1, 1);
			cell.Neighbourghs[4] = GetNeighborough(cell, -1, -1);
		}
	}

	private Cell GetNeighborough(Cell cell, int offsetX, int offsetY)
	{
		for (Cell cellNeighbor : Cells)
		{
			if (cellNeighbor.Coord.x == cell.Coord.x + offsetX && cellNeighbor.Coord.y == cell.Coord.y + offsetY)
			{
				return cellNeighbor;
			}
		}

		return null;
	}
	
	public void NewColor()
	{
		for (Cell cell : Cells)
		{
			NewColor(cell);
		}
	}

	public void NewColor(Cell cell)
	{
		// --- Calcul de la couleur
		cell.IsEmpty = Math.random() * 10 > 6;
		cell.ColorType = 0;

		boolean colorFound = false;

		while (!cell.IsEmpty && !colorFound)
		{
			cell.NewColorType();

			for (int i = 0; i < 6; i++)
			{
				if (cell.Neighbourghs[i] != null && !cell.Neighbourghs[i].IsEmpty && cell.Neighbourghs[i].ColorType == cell.ColorType)
				{
					cell.ColorType = 0;
					break;
				}
			}

			if (cell.ColorType != 0)
			{
				colorFound = true;
			}
		}

		// if(!colorFound)
		// {
		// cell.IsEmpty = true;
		// }
		// ---
	}

	public void NewArrows()
	{
		for (Cell cell : Cells)
		{
			cell.NewArrows();
		}
	}
	
	public void CalcArrows()
	{
		for (Cell cell : Cells)
		{
			cell.CalcArrows();
		}
	}

	public void RenewInactivatedCells()
	{
		for (Cell cell : this.Cells)
		{
			cell.Score = 0;
			cell.LeafScore = true;
			
			if (cell.State == CellState.Inactivated)
			{
				// CalcColorAndParts(cell);

				cell.IsEmpty = true;

				for (int i = 0; i < 6; i++)
				{
					cell.Parts[i] = CellPartType.Simple;
				}
			}

			cell.State = CellState.Normal;
		}
	}
}
