package plz.logic.gameplay.liplz;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Random;
import java.util.Vector;

import plz.engine.Common;
import plz.engine.logic.ui.components.SensitiveZone;

import com.badlogic.gdx.math.Rectangle;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.math.Vector3;

import plz.GameEngine;
import plz.logic.controller.liplz.SelectionMode;
import plz.logic.render.liplz.RenderLogic;
import plz.logic.ui.screens.liplz.GameScreen;
import plz.model.liplz.*;

public class GamePlayLogic extends plz.engine.logic.gameplay.GamePlayLogicBase
{
	float tileAnimationDuration = 200f;
	public boolean humanTurn = true;
	
	public Context Context()
	{
		return (Context) gameEngine.Context;
	}

	public GamePlayLogic(GameEngine gameEngine)
	{
		super(gameEngine);

		NewMap();

		Context().gameStateTime = new GameStateTime();
		Context().gameStateTime.Date = new Date();
		Context().gameStateTime.GameState = GameState.Playing;
	}

	public void NewMap()
	{
		Context().Map = new Map(9, 9);
		Context().SelectedCells = new Vector<Cell>();

		SelectCell(Context().Map.CenterCell, false);
		
		CalcCamera();
	}

	private void CalcCamera()
	{
		gameEngine.Render.Camera.position.set(Context().Map.CenterCell.Location.x*256+128,Context().Map.CenterCell.Location.y*256+128, 10f);
		
		gameEngine.Render.Camera.zoom = Context().Map.CountLayer;
	}
	
	public void SelectCell(Cell selectedCell, boolean human)
	{
		Context().SelectedCells.add(selectedCell);

		Context().LastSelectedCell = selectedCell;
		
		for (Cell cell : Context().Map.Cells)
		{
			if(cell.State != CellState.Invisible)
			{
				cell.State = CellState.Stoned;
				
				if(GetCellDistance(selectedCell, cell) < 450 && !Context().SelectedCells.contains(cell))
				{
					cell.State = CellState.Highlighted1;
					
					if(cell.Tile.Symbol == selectedCell.Tile.Symbol || cell.Tile.Color == selectedCell.Tile.Color)
						cell.State = CellState.Highlighted2;
				}
			}
		}
		
		if(human)
		{
			Context().gameStateTime.Date = new Date();
			humanTurn=false;
		}
	}
	
	private void Ai()
	{
		Vector<Cell> cells = new Vector<Cell>();
		
		for (Cell cell : Context().Map.Cells)
		{
			if(cell.State == CellState.Highlighted2)
			{
				cells.add(cell);
			}
		}
		
		Random rnd = new Random();
		
		if(cells.size()>0)
			SelectCell(cells.get(rnd.nextInt(cells.size())), false);
		
		humanTurn = true;
	}
	
	private int GetCellDistance(Cell cell1, Cell cell2)
	{
		int distance = (int) Math.sqrt(Math.pow((cell2.Location.x-cell1.Location.x)*256f,2)+Math.pow((cell2.Location.y-cell1.Location.y)*256f,2));
		
		return distance;
	}
	
	@Override
	public void Update(float deltaTime)
	{
		Date date = new Date();
		
		if(!humanTurn && date.getTime() - Context().gameStateTime.Date.getTime() >= 500)
		{
			Context().gameStateTime.Date = date;
			Ai();
		}
	}

	public Cell PickTile(Vector2 location)
	{
		for (Cell cell : Context().Map.Cells)
		{
			if (PointInCell(cell, location))
			{
				return cell;
			}
		}

		return null;
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
