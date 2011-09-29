package twiplz.logic.gameplay;

import java.util.Date;

import plz.engine.Common;
import plz.engine.logic.ui.components.SensitiveZone;

import com.badlogic.gdx.math.Rectangle;
import com.badlogic.gdx.math.Vector2;

import twiplz.Context;
import twiplz.GameEngine;
import twiplz.logic.controller.SelectionMode;
import twiplz.logic.ui.screens.GameScreen;
import twiplz.model.*;

public class GamePlayLogic extends plz.engine.logic.gameplay.GamePlayLogicBase
{
	public Tile Tile;
	public Tile SelectedTile;
	public int CurrentOrientation = 0;
	public Vector2[][] CellDisposition;
	private Date FirstTileReleased;

	public GamePlayLogic(GameEngine gameEngine)
	{
		super(gameEngine);

		NewMap();

		CellDisposition = new Vector2[2][6];
		float fh = (float) Math.sin(Math.PI / 3);

		CellDisposition[0][0] = new Vector2(0, 0.5f * fh);
		CellDisposition[0][1] = new Vector2(-3f / 8f, 1f / 4f * fh);
		CellDisposition[0][2] = new Vector2(-3f / 8f, -1f / 4f * fh);
		CellDisposition[0][3] = new Vector2(0, -0.5f * fh);
		CellDisposition[0][4] = new Vector2(3f / 8f, -1f / 4f * fh);
		CellDisposition[0][5] = new Vector2(3f / 8f, 1f / 4f * fh);

		CellDisposition[1][0] = new Vector2(0, -0.5f * fh);
		CellDisposition[1][1] = new Vector2(3f / 8f, -1f / 4f * fh);
		CellDisposition[1][2] = new Vector2(3f / 8f, 1f / 4f * fh);
		CellDisposition[1][3] = new Vector2(0, 0.5f * fh);
		CellDisposition[1][4] = new Vector2(-3f / 8f, 1 / 4f * fh);
		CellDisposition[1][5] = new Vector2(-3f / 8f, -1f / 4f * fh);
	}

	public void NewMap()
	{
		Context.Map = new Map(8, 8);
		
		Context.Map.Cells.get(0).Highlighted = true;
	}

	public void SelectTile()
	{
		SelectedTile = new Tile();

		SelectedTile.Cells[0] = (Cell) Tile.Cells[0].clone();
		SelectedTile.Cells[1] = (Cell) Tile.Cells[1].clone();
		SelectedTile.ActiveCell = SelectedTile.Cells[0];
		SelectedTile.InactiveCell = SelectedTile.Cells[1];
	}

	public void CreateNewTile()
	{
		Tile = new Tile();
		SelectedTile = null;

		UpdateTileOrientation();
	}

	public void TurnTileOffset(int delta)
	{
		TurnTile(CurrentOrientation - delta);
	}

	public void TurnTile(int orientation)
	{
		// int t = orientation;
		// orientation = orientation % 6;

		orientation = Common.mod(orientation, 6);

		int offset = orientation - CurrentOrientation;

		try
		{
			CurrentOrientation = orientation;

			if (CurrentOrientation < 0)
				CurrentOrientation = 5;
			else if (CurrentOrientation > 5)
				CurrentOrientation = 0;

			TurnTileCellPart(Tile, offset);
			if (SelectedTile != null)
				TurnTileCellPart(SelectedTile, offset);

			UpdateTileOrientation();
		}
		catch (Exception e)
		{
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}

	private void TurnTileCellPart(Tile tile, int offset)
	{
		TurnCellPart(tile.Cells[0], offset);
		TurnCellPart(tile.Cells[1], offset);
	}

	private void TurnCellPart(Cell cell, int offset)
	{
		CellPartType[] parts = new CellPartType[6];

		for (int i = 0; i < 6; i++)
		{
			parts[i] = cell.Parts[i];
		}

		for (int i = 0; i < 6; i++)
		{
			int newDirection = i + offset;

			if (newDirection < 0)
				newDirection += 6;
			else if (newDirection > 5)
				newDirection -= 6;

			cell.Parts[i] = parts[newDirection];
		}
	}

	private void UpdateTileOrientation()
	{
		SensitiveZone imgNewTile = ((GameScreen) this.gameEngine.CurrentScreen).imgNewTile;

		// int h = (int) (imgNewTile.height / (4f * (1 + (2 / Math.sqrt(3f)))));
		int h = (int) (imgNewTile.height / 4f);// * (1 + (2 / Math.sqrt(3f)))));
		int width = (int) ((2 * h) / Math.sqrt(3f));

		Tile.Cells[0].Location = new Vector2(imgNewTile.AbsoluteLocation().x + imgNewTile.width / 2 - width + CellDisposition[0][CurrentOrientation].x * width * 2, imgNewTile.AbsoluteLocation().y + imgNewTile.height / 2 - h + CellDisposition[0][CurrentOrientation].y * h * 2);
		Tile.Cells[1].Location = new Vector2(imgNewTile.AbsoluteLocation().x + imgNewTile.width / 2 - width + CellDisposition[1][CurrentOrientation].x * width * 2, imgNewTile.AbsoluteLocation().y + imgNewTile.height / 2 - h + CellDisposition[1][CurrentOrientation].y * h * 2);

		if (SelectedTile != null)
		{
			UpdateTileLocation(SelectedTile.Location);
		}
	}

	public void UpdateTileLocation(Vector2 location)
	{
		SelectedTile.Location = new Vector2(location.x, location.y);

		Cell selectedCell = GetSelectedCell();

		if (selectedCell != null && selectedCell.Neighbourghs[CurrentOrientation] != null)
		{
			SelectedTile.Cells[0].Location = selectedCell.Location;
			SelectedTile.Cells[1].Location = selectedCell.Neighbourghs[CurrentOrientation].Location;
		}
	}

	private Cell GetSelectedCell()
	{
		Cell selectedCell = null;

		for (Cell cell : Context.Map.Cells)
		{
			cell.Selected = false;

			if (PointInCell(cell, SelectedTile.Location))
			{
				cell.Selected = true;
				selectedCell = cell;
			}
			
			cell.Highlighted=false;
		}

//		if(selectedCell !=null && selectedCell.Neighbourghs[0] != null)
//			selectedCell.Neighbourghs[0].Highlighted=true;
		
		return selectedCell;
	}

	private boolean PointInCell(Cell cell, Vector2 location)
	{
		int w = 256 / 2;
		int dw = w / 2;
		int h = (int) (w * (Math.sqrt(3f) / 2));
		float k = (int) ((1f - (Math.sqrt(3f) / 2f)) * w);
		int Lx = (int) (cell.Location.x * 256f);
		int Ly = (int) (cell.Location.y * 256f + k);
		// Point point = new Point((int) location.x, (int) location.y);

		Vector2 point = new Vector2(location.x, location.y);

		// 1 : Test du rectangle englobant
		Rectangle rec = new Rectangle(Lx, Ly, w * 2, 2 * h);
		if (!rec.contains(location.x, location.y))
			return false;

		// 2 : Test du rectangle principale
		rec = new Rectangle(Lx + dw, Ly, w, 2 * h);

		if (rec.contains(location.x, location.y))
			return true;

		// 3 : Test du triangle 2a
		Vector2[] triangle = new Vector2[3];
		triangle[0] = new Vector2(Lx + dw, Ly);
		triangle[1] = new Vector2(Lx + dw, Ly + h);
		triangle[2] = new Vector2(Lx, Ly + h);

		if (Common.IsPointInsideTriangle(triangle, point))
			return true;

		// 4 : Test du triangle 2b
		triangle[0] = new Vector2(Lx, Ly + h);
		triangle[1] = new Vector2(Lx + dw, Ly + h);
		triangle[2] = new Vector2(Lx + dw, Ly + 2 * h);

		if (Common.IsPointInsideTriangle(triangle, point))
			return true;

		// 5 : Test du triangle 3a
		triangle[0] = new Vector2(Lx + 3 * dw, Ly);
		triangle[1] = new Vector2(Lx + 2 * w, Ly + h);
		triangle[2] = new Vector2(Lx + 3 * dw, Ly + h);

		if (Common.IsPointInsideTriangle(triangle, point))
			return true;

		// 6 : Test du triangle 3b
		triangle[0] = new Vector2(Lx + 3 * dw, Ly + h);
		triangle[1] = new Vector2(Lx + 2 * w, Ly + h);
		triangle[2] = new Vector2(Lx + 3 * dw, Ly + 2 * h);

		if (Common.IsPointInsideTriangle(triangle, point))
			return true;

		return false;
	}

	public void ReleaseTile()
	{
		Cell selectedCell = GetSelectedCell();

		if (selectedCell != null && selectedCell.Neighbourghs[CurrentOrientation] != null)
		{
			SwapCell(selectedCell, SelectedTile.Cells[0]);
			SwapCell(selectedCell.Neighbourghs[CurrentOrientation], SelectedTile.Cells[1]);

			FirstTileReleased = new Date();

			Context.Map.CalcNeighborough(SelectedTile.Cells[0]);
			Context.Map.CalcNeighborough(SelectedTile.Cells[1]);

			CreateNewTile();
			SelectedTile = null;
		}
	}

	public void UnselectTile()
	{
		SelectedTile = null;
	}

	private void SwapCell(Cell cellDest, Cell cellOrig)
	{
		cellOrig.Map = cellDest.Map;
		cellOrig.Coord = cellDest.Coord;
		cellOrig.InitialLocation = cellDest.InitialLocation;
		cellOrig.Neighbourghs = cellDest.Neighbourghs;

		// int index = cellDest.Map.Cells.indexOf(cellDest);

		cellDest.Map.Cells.remove(cellDest);
		cellDest.Map.Cells.add(cellOrig);
		// cellDest.Map.Cells.add(index, cellOrig);

		// cellDest = cellOrig;
	}

	public boolean PickTile(Vector2 location)
	{
		if (PointInCell(SelectedTile.ActiveCell, location))
		{
			SelectedTile.State = TileState.Move;
			return true;
		}

		return false;
	}

	public boolean PickInactiveCellTile(Vector2 location)
	{
		if (PointInCell(SelectedTile.InactiveCell, location))
		{
			SelectedTile.State = TileState.Turn;
			return true;
		}

		return false;
	}

	@Override
	public void Update(float deltaTime)
	{
		CalcMapColors();
	}

	public void CalcMapColors()
	{
		Date currentTime = new Date();

		if (FirstTileReleased == null || currentTime.getTime() - FirstTileReleased.getTime() <= 500)
			return;

		FirstTileReleased = currentTime;

		// --- Clone la map
		Map tempMap = new Map(Context.Map.Width, Context.Map.Height);

		for (Cell cell : Context.Map.Cells)
		{
			Cell cellDest = (Cell) cell.clone();

			tempMap.Cells.add(cellDest);
			cellDest.Highlighted = false;

		}
		// ---

		// --- Passe 0 : Neutraliser les parttype si il sont en inversion avec
		// leur voisin
		for (Cell cell : Context.Map.Cells)
		{
			
			for (int i = 0; i < 6; i++)
			{
//				if(i==1)
//					cell.Parts[i] = CellPartType.Out;
//				else
//					cell.Parts[i] = CellPartType.Simple;

				try
				{
					Cell cellN = cell.Neighbourghs[i];
					
					int j = Common.mod(i + 3, 6);
					if (cellN != null && cell.Parts[i].ordinal() + cellN.Parts[j].ordinal() == 3)
					{
//						cell.Parts[i] = CellPartType.Simple;
//						cellN.Parts[j] = CellPartType.Simple;
						
						cell.Highlighted = true;
						cellN.Highlighted = true;
					}
				}
				catch (Exception ex)
				{
					ex.printStackTrace();
				}
			}
		}
		// ---

		// --- Passe 1 : Marque les cellules dont au moins un voisin est de la
		// même couleur
		for (Cell cell : Context.Map.Cells)
		{

			for (int i = 0; i < 6; i++)
			{
				Cell cellN = cell.Neighbourghs[i];

				if (cellN != null && cell.ColorType == cellN.ColorType)
				{
					//cell.Highlighted = true;
					//cellN.Highlighted = true;
				}
			}
		}
		// ---

	}
}
