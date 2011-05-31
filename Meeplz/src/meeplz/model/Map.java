package meeplz.model;

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
			}
			else
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
