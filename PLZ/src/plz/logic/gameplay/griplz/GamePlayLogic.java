package plz.logic.gameplay.griplz;

import java.util.ArrayList;
import java.util.Date;

import plz.engine.Common;
import plz.engine.logic.ui.components.SensitiveZone;

import com.badlogic.gdx.math.Rectangle;
import com.badlogic.gdx.math.Vector2;

import plz.GameEngine;
import plz.logic.controller.griplz.SelectionMode;
import plz.logic.render.griplz.RenderLogic;
import plz.logic.ui.screens.griplz.GameScreen;
import plz.model.griplz.*;
import plz.model.griplz.Cell;
import plz.model.griplz.CellState;
import plz.model.griplz.Context;
import plz.model.griplz.GameState;
import plz.model.griplz.GameStateTime;
import plz.model.griplz.Map;

public class GamePlayLogic extends plz.engine.logic.gameplay.GamePlayLogicBase
{

	public Context Context()
	{
		return (Context)gameEngine.Context;
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
		Context().Map = new Map(20, 20);
	}



	@Override
	public void Update(float deltaTime)
	{
		
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
