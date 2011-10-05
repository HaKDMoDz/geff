package twiplz.logic.gameplay;

import java.util.ArrayList;
import java.util.Date;

import plz.engine.Common;
import plz.engine.logic.ui.components.SensitiveZone;

import com.badlogic.gdx.math.Rectangle;
import com.badlogic.gdx.math.Vector2;

import twiplz.Context;
import twiplz.GameEngine;
import twiplz.logic.controller.SelectionMode;
import twiplz.logic.render.RenderLogic;
import twiplz.logic.ui.screens.GameScreen;
import twiplz.model.*;

public class GamePlayLogic extends plz.engine.logic.gameplay.GamePlayLogicBase
{
	public Tile Tile;
	public Tile SelectedTile;
	public int CurrentOrientation = 0;
	public Vector2[][] CellDisposition;
	private Date FirstTileReleased;
	byte[] colorValues = new byte[7];

	public GamePlayLogic(GameEngine gameEngine)
	{
		super(gameEngine);

		NewMap();

		CellDisposition = new Vector2[2][6];
		float fh = (float) Math.sin(Math.PI / 3);

		CellDisposition[0][3] = new Vector2(0, 0.5f * fh);
		CellDisposition[0][2] = new Vector2(-3f / 8f, 1f / 4f * fh);
		CellDisposition[0][1] = new Vector2(-3f / 8f, -1f / 4f * fh);
		CellDisposition[0][0] = new Vector2(0, -0.5f * fh);
		CellDisposition[0][5] = new Vector2(3f / 8f, -1f / 4f * fh);
		CellDisposition[0][4] = new Vector2(3f / 8f, 1f / 4f * fh);

		CellDisposition[1][3] = new Vector2(0, -0.5f * fh);
		CellDisposition[1][2] = new Vector2(3f / 8f, -1f / 4f * fh);
		CellDisposition[1][1] = new Vector2(3f / 8f, 1f / 4f * fh);
		CellDisposition[1][0] = new Vector2(0, 0.5f * fh);
		CellDisposition[1][5] = new Vector2(-3f / 8f, 1 / 4f * fh);
		CellDisposition[1][4] = new Vector2(-3f / 8f, -1f / 4f * fh);

		colorValues[0] = 0;
		colorValues[1] = 2;
		colorValues[2] = 6;
		colorValues[3] = 4;
		colorValues[4] = 12;
		colorValues[5] = 8;
		colorValues[6] = 10;
	}

	public void NewMap()
	{
		Context.Map = new Map(7, 5);

		if (Context.gameMode == GameMode.Arrow)
			Context.Map.NewArrows();
		else if (Context.gameMode == GameMode.Circular)
			Context.Map.CalcArrows();
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

		if (Context.gameMode == GameMode.Arrow)
		{
			Tile.Cells[0].NewArrows();
			Tile.Cells[1].NewArrows();
		}
		// else if(Context.gameMode == GameMode.Circular)
		// Context.Map.CalcArrows();

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

		int offset = CurrentOrientation - orientation;

		try
		{
			CurrentOrientation = orientation;

			if (CurrentOrientation < 0)
				CurrentOrientation = 5;
			else if (CurrentOrientation > 5)
				CurrentOrientation = 0;

			boolean canTurn = true;

			if (SelectedTile != null)
				canTurn = CanTurn(offset);

			if (!canTurn)
			{
				if (GetSelectedCell() != null)
					TurnTileOffset(offset);
				else
				{
					TurnTileCellPart(Tile, offset);
					UpdateTileOrientation();
				}
			}
			else
			{
				TurnTileCellPart(Tile, offset);

				if (SelectedTile != null)
					TurnTileCellPart(SelectedTile, offset);

				UpdateTileOrientation();
			}
		}
		catch (Exception e)
		{
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}

	private boolean CanTurn(int offset)
	{
		boolean canTurn = false;

		Cell selectedCell = GetSelectedCell();

		if (selectedCell != null && selectedCell.Neighbourghs[CurrentOrientation] != null)
			canTurn = true;

		return canTurn;
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
		Vector2 oldLocation = SelectedTile.Location;
		SelectedTile.Location = new Vector2(location.x, location.y);

		Cell selectedCell = GetSelectedCell();

		if (selectedCell != null && selectedCell.Neighbourghs[CurrentOrientation] != null)
		{
			SelectedTile.Cells[0].Location = selectedCell.Location;
			SelectedTile.Cells[1].Location = selectedCell.Neighbourghs[CurrentOrientation].Location;

			if (Context.gameMode == GameMode.Circular)
			{
				// --- Clone la map
				Map tempMap = new Map(Context.Map.Width, Context.Map.Height);
				tempMap.Cells.clear();

				for (Cell cell : Context.Map.Cells)
				{
					Cell cellDest = (Cell) cell.clone();
					cellDest.Map = tempMap;
					tempMap.Cells.add(cellDest);
					cellDest.State = CellState.Normal;
				}

				// ---

				// --- Place les cellules de la tuile dans la map temporaire
				int index = Context.Map.Cells.indexOf(selectedCell);
				Cell cellDest = tempMap.Cells.get(index);

				SwapCell(cellDest, SelectedTile.Cells[0], index);

				index = Context.Map.Cells.indexOf(selectedCell.Neighbourghs[CurrentOrientation]);
				cellDest = tempMap.Cells.get(index);

				SwapCell(cellDest, SelectedTile.Cells[1], index);
				// ---

				// ---
				tempMap.CalcNeighborough();
				tempMap.CalcArrows();

				for (int i = 0; i < tempMap.Cells.size(); i++)
				{
					for (int j = 0; j < 6; j++)
					{
						Context.Map.Cells.get(i).Parts[j] = tempMap.Cells.get(i).Parts[j];
					}
				}
			}

			// ---

			// SelectedTile.Cells[0].Neighbourghs = selectedCell.Neighbourghs;
			// SelectedTile.Cells[1].Neighbourghs =
			// selectedCell.Neighbourghs[CurrentOrientation].Neighbourghs;

			// SelectedTile.Cells[0].CalcArrows();
			// SelectedTile.Cells[1].CalcArrows();
		}
		else
		{
			SelectedTile.Location = oldLocation;
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
		}

		// if(selectedCell !=null && selectedCell.Neighbourghs[0] != null)
		// selectedCell.Neighbourghs[0].Highlighted=true;

		return selectedCell;
	}

	public void ReleaseTile()
	{
		Cell selectedCell = GetSelectedCell();

		if (selectedCell != null && selectedCell.Neighbourghs[CurrentOrientation] != null)
		{
			SwapCell(selectedCell, SelectedTile.Cells[0]);
			SwapCell(selectedCell.Neighbourghs[CurrentOrientation], SelectedTile.Cells[1]);

			if (FirstTileReleased == null)
			{
				FirstTileReleased = new Date();
				FirstTileReleased.setTime(new Date().getTime() - Context.TimeRefresh);
			}

			Context.Map.CalcNeighborough();// SelectedTile.Cells[0]);
			// Context.Map.CalcNeighborough(SelectedTile.Cells[1]);

			SelectedTile.Cells[0].State = CellState.Normal;
			SelectedTile.Cells[1].State = CellState.Normal;

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
		// cellOrig.Map = cellDest.Map;
		// cellOrig.Coord = cellDest.Coord;
		// cellOrig.InitialLocation = cellDest.InitialLocation;
		// cellOrig.Neighbourghs = cellDest.Neighbourghs;

		int index = cellDest.Map.Cells.indexOf(cellDest);

		SwapCell(cellDest, cellOrig, index);

		// cellDest.Map.Cells.remove(cellDest);
		// cellDest.Map.Cells.add(cellOrig);
		// cellDest.Map.Cells.add(index, cellOrig);

		// cellDest = cellOrig;
	}

	private void SwapCell(Cell cellDest, Cell cellOrig, int index)
	{
		cellOrig.Map = cellDest.Map;
		cellOrig.Coord = cellDest.Coord;
		cellOrig.InitialLocation = cellDest.InitialLocation;
		cellOrig.Neighbourghs = cellDest.Neighbourghs;

		// int index = cellDest.Map.Cells.indexOf(cellDest);

		cellDest.Map.Cells.remove(cellDest);
		// cellDest.Map.Cells.add(cellOrig);
		cellDest.Map.Cells.add(index, cellOrig);

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

		if (FirstTileReleased == null || currentTime.getTime() - FirstTileReleased.getTime() < Context.TimeRefresh)
			return;

		FirstTileReleased = currentTime;

		if (Context.gameMode == GameMode.Arrow)
		{
			// --- 1 : Clone la map
			Map tempMap = new Map(Context.Map.Width, Context.Map.Height);
			tempMap.Cells.clear();

			for (Cell cell : Context.Map.Cells)
			{
				Cell cellDest = (Cell) cell.clone();
				cellDest.Map = tempMap;
				tempMap.Cells.add(cellDest);
				cellDest.State = CellState.Normal;
			}

			tempMap.CalcNeighborough();
			// ---

			// --- 2 : Neutraliser les parttype si il sont en inversion avec
			// leur voisin
			for (Cell cell : tempMap.Cells)
			{
				if (!cell.IsEmpty)
				{
					for (int i = 0; i < 6; i++)
					{
						Cell cellN = cell.Neighbourghs[i];

						if (cellN != null && !cellN.IsEmpty)
						{
							int j = Common.mod(i + 3, 6);

							try
							{
								if (cell.Parts[i].ordinal() + cellN.Parts[j].ordinal() == 3)
								{
									cell.Parts[i] = CellPartType.Simple;
									cellN.Parts[j] = CellPartType.Simple;
								}
							}
							catch (Exception e)
							{
								e.printStackTrace();
							}
						}
					}
				}
			}
			// ---

			// --- 3 : Pour toutes les cellules précédement activées, activer
			// leurs PartType, changer leur état
			for (Cell cell : Context.Map.Cells)
			{
				if (!cell.IsEmpty && cell.State == CellState.Activated)
				{
					int index = Context.Map.Cells.indexOf(cell);
					Cell cellTmp = tempMap.Cells.get(index);

					for (int i = 0; i < 6; i++)
					{
						Cell cellN = cell.Neighbourghs[i];

						if (cellN != null)
						{
							index = Context.Map.Cells.indexOf(cellN);

							try
							{
								Cell cellNTmp = tempMap.Cells.get(index);

								if (cell.Parts[i] == CellPartType.In)
								{
									cellTmp.ColorType += cellN.ColorType;
								}
								else if (cell.Parts[i] == CellPartType.Out)
								{
									cellNTmp.ColorType += cell.ColorType;
								}
							}
							catch (Exception e)
							{
								e.printStackTrace();
							}
						}
					}

					cell.State = CellState.Inactivated;
				}
			}

			// ---> Applique la couleur dans la map
			for (Cell cell : Context.Map.Cells)
			{
				int index = Context.Map.Cells.indexOf(cell);
				Cell cellTmp = tempMap.Cells.get(index);

				if (((RenderLogic) gameEngine.Render).colors.containsKey((int) cellTmp.ColorType))
				{
					cell.ColorType = cellTmp.ColorType;
				}
				else
					cell.ColorType = 0;
			}
			// ---
		}
		else if (Context.gameMode == GameMode.Circular)
		{
			for (Cell cell : Context.Map.Cells)
			{
				if (!cell.IsEmpty && cell.State == CellState.Activated)
				{
					int index = getIndexColor(cell.ColorType);
					index = (index + 1) % colorValues.length;

					if (index == 0)
						index = 1;

					cell.ColorType = colorValues[index];
					cell.State = CellState.Inactivated;
				}
			}
		}

		// --- Passe 1 : Marque les cellules dont au moins un voisin est de la
		// même couleur
		int countNewActivated = 0;

		for (Cell cell : Context.Map.Cells)
		{
			if (!cell.IsEmpty && cell.State == CellState.Normal)
			{
				for (int i = 0; i < 6; i++)
				{
					Cell cellN = cell.Neighbourghs[i];

					if (cellN != null && !cellN.IsEmpty && cell.ColorType == cellN.ColorType)
					{
						countNewActivated++;

						cell.State = CellState.Activated;

						if (cellN.State == CellState.Normal)
						{
							cellN.State = CellState.Activated;
							cellN.Score = 1;
							cell.Score = 1;
						}
						else
						{
							cellN.LeafScore = false;
							cell.Score = cellN.Score + 1;
						}
					}
				}
			}
		}

		ScoreEvaluation();

		if (countNewActivated == 0)
		{
			FirstTileReleased = null;

			Context.Map.RenewInactivatedCells();

			if (Context.gameMode == GameMode.Circular)
				Context.Map.CalcArrows();

			Context.Score += Context.AddedScore;
			Context.AddedScore = 0;
			Context.Combo = 0;
		}
		// ---
	}

	public void ScoreEvaluation()
	{
		Context.AddedScore = 0;
		int countCell = 0;

		for (Cell cell : Context.Map.Cells)
		{
			if (!cell.IsEmpty && (cell.State == CellState.Activated || cell.State == CellState.Inactivated) && cell.LeafScore)
			{
				Context.AddedScore += Math.pow(cell.Score, 2);

				if (Context.Combo < cell.Score)
					Context.Combo = cell.Score;
			}

			if (!cell.IsEmpty && cell.State == CellState.Normal)
				countCell++;
		}

		if (countCell == 0)
		{
			Context.AddedScore += 1000;
			Context.Combo = 999;
		}
	}

	private int getIndexColor(byte colorType)
	{
		for (int i = 0; i < colorValues.length; i++)
		{
			if (colorValues[i] == colorType)
				return i;
		}

		return -1;
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
}
