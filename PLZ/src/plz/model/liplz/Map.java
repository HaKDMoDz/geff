package plz.model.liplz;

import java.util.ArrayList;
import java.util.Iterator;
import java.util.Random;

import plz.engine.Common;

import com.badlogic.gdx.Gdx;

public class Map
{
	public ArrayList<Cell> Cells;
	public int Width;
	public int Height;
	public Cell CenterCell;
	public int CountLayer = 0;
	
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

				if (x == Width / 4 && y == Height / 2)
				{
					int a = 0;
				}
			}
		}

		CalcNeighborough();
		CalcBoard();
		CalcColorsAndSymbols();
	}

	public void CalcBoard()
	{
		int indexCentre = 0;
		Cell prevCell = null;

		// --- Calcul de la cellule située au centre de la map
		if (Common.mod(Width, 4) == 3)
		{
			indexCentre = Width / 4 + Width * (Height / 2);
		} else if (Common.mod(Width, 4) == 1)
		{
			indexCentre = 3 * Width / 4 + Width * (Height / 2);
		}

		for (Cell cell : Cells)
		{
			if (cell.IndexPosition() == indexCentre)
			{
				cell.State = CellState.Visible;
				prevCell = cell;
			} else
				cell.State = CellState.Invisible;
		}
		// ---

		// --- Calcul la taille de la zone
		CountLayer = 1000;
		CenterCell = prevCell;

		for (int i = 0; i < 6; i++)
		{
			int layer = 0;
			prevCell = CenterCell;

			while (prevCell != null)
			{
				layer++;

				prevCell = prevCell.Neighbourghs[i];
			}

			if (layer - 1 < CountLayer)
				CountLayer = layer - 1;
		}
		// ---

		// ---> Détermine quelles sont les cellules situées dans la zone de jeu
		prevCell = CenterCell.Neighbourghs[0];
		prevCell.State = CellState.Visible;

		for (int i = 0; i < CountLayer; i++)
		{
			for (int j = 4; j >= -1; j--)
			{
				for (int k = 0; k < i + 1; k++)
				{
					int j2 = j;
					if (j == -1)
						j2 = 5;

					if ((j == -1 & k < i) || j != -1)
					{
						prevCell = prevCell.Neighbourghs[j2];

						prevCell.State = CellState.Visible;
					}
				}
			}

			if (i < CountLayer-1)
			{
				prevCell = prevCell.Neighbourghs[5].Neighbourghs[0];
				if(prevCell != null)
					prevCell.State = CellState.Visible;
			}
			
		}

	}
	
	private void CalcColorsAndSymbols()
	{
		Random rnd = new Random();
		
		for (Cell cell : Cells)
		{
			cell.Tile.Symbol = rnd.nextInt(5)+1;
			cell.Tile.Color = rnd.nextInt(5)+1;
		}
	}

	private void SwapCell(Cell cellDest, Cell cellOrig)
	{
		int index = cellDest.Map.Cells.indexOf(cellDest);

		SwapCell(cellDest, cellOrig, index);
	}

	private void SwapCell(Cell cellDest, Cell cellOrig, int index)
	{
		cellOrig.Map = cellDest.Map;
		cellOrig.Coord = cellDest.Coord;
		cellOrig.InitialLocation = cellDest.InitialLocation;
		cellOrig.Neighbourghs = cellDest.Neighbourghs;

		for (int i = 0; i < 6; i++)
		{
			if (cellOrig.Neighbourghs[i] != null)
			{
				for (int j = 0; j < 6; j++)
				{
					if (cellOrig.Neighbourghs[i].Neighbourghs[j] == cellDest)
					{
						cellOrig.Neighbourghs[i].Neighbourghs[j] = cellOrig;
					}
				}
			}
		}

		cellOrig.Location = cellDest.Location;

		cellDest.Map.Cells.remove(cellDest);
		cellDest.Map.Cells.add(index, cellOrig);
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

		if (cell.Coord.y % 2 == 1)
		{
			cell.Neighbourghs[3] = GetNeighborough(cell, 0, -2);
			cell.Neighbourghs[2] = GetNeighborough(cell, 1, -1);
			cell.Neighbourghs[1] = GetNeighborough(cell, 1, 1);
			cell.Neighbourghs[0] = GetNeighborough(cell, 0, 2);
			cell.Neighbourghs[5] = GetNeighborough(cell, 0, 1);
			cell.Neighbourghs[4] = GetNeighborough(cell, 0, -1);
		} else
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
			if (cellNeighbor.Coord.x == cell.Coord.x + offsetX
					&& cellNeighbor.Coord.y == cell.Coord.y + offsetY)
			{
				return cellNeighbor;
			}
		}

		return null;
	}
}
