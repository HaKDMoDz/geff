package plz.logic.controller.griplz;

import java.util.Date;

import plz.GameEngine;
import plz.engine.Common;
import plz.engine.logic.controller.Pointer;
import plz.engine.logic.controller.PointerUsage;
import plz.logic.gameplay.griplz.GamePlayLogic;
import plz.model.griplz.Cell;
import plz.model.griplz.CellState;
import plz.model.griplz.Context;
import plz.model.griplz.TileState;

import com.badlogic.gdx.Input.Keys;
import com.badlogic.gdx.math.Vector2;

public class ControllerLogic extends
		plz.engine.logic.controller.ControllerLogicBase
{
	private GamePlayLogic GamePlay()
	{
		return (GamePlayLogic) gameEngine.GamePlay;
	}

	public ControllerLogic(GameEngine gameEngine)
	{
		super(gameEngine);
	}

	public Context Context()
	{
		return (Context) gameEngine.Context;
	}

	@Override
	public boolean keyDown(int keycode)
	{
		// gameEngine.CurrentScreen.Stage.keyDown(keycode);

		if (keycode == Keys.LEFT)
		{
			gameEngine.Render.Camera.position.add(1, 0, 0);
		}

		if (keycode == Keys.RIGHT)
		{
			gameEngine.Render.Camera.position.add(-1, 0, 0);
		}

		if (keycode == Keys.UP)
		{
			gameEngine.Render.Camera.position.add(0, 1, 0);
		}

		if (keycode == Keys.DOWN)
		{
			gameEngine.Render.Camera.position.add(0, -1, 0);
		}

		return false;
	}

	@Override
	public boolean keyUp(int keycode)
	{
		// TODO Auto-generated method stub
		return false;
	}

	@Override
	public boolean scrolled(int amount)
	{
		// gameEngine.CurrentScreen.Stage.scrolled(amount);

		Zoom(gameEngine.Render.Camera.zoom + amount);

		return false;
	}

	@Override
	public boolean keyTyped(char character)
	{
		// TODO Auto-generated method stub
		return false;
	}

	@Override
	public boolean touchDown(int x, int y, int pointer, int button)
	{
		if (pointer > Context().pointers.length)
			return false;

		super.touchDown(x, y, pointer, button);

		Pointer curPointer = Context().pointers[pointer];


		//--- Sélectionne une tuile
		if (curPointer.Usage == PointerUsage.None)
		{
			Vector2 vec = new Vector2(gameEngine.Render.Camera.position.x + (-vecMidScreen.x + x) * gameEngine.Render.Camera.zoom, gameEngine.Render.Camera.position.y - (-vecMidScreen.y + y) * gameEngine.Render.Camera.zoom);

			Cell selectedCell = GamePlay().PickTile(vec);

			if (selectedCell != null && selectedCell.Tile != null)
			{
				curPointer.Usage = PointerUsage.SelectTile;
				selectedCell.Tile.State = TileState.Selected;
				curPointer.Tag = selectedCell;
			}

		}

		return true;
	}

	@Override
	public boolean touchUp(int x, int y, int pointer, int button)
	{
		if (pointer > Context().pointers.length)
			return false;

		super.touchUp(x, y, pointer, button);

		Pointer curPointer = Context().pointers[pointer];

		// --- Si une tuile est sélectionnée, déclenche ou annule le mouvement
		if (curPointer.Usage == PointerUsage.SelectTile)
		{
			Cell selectedCell = ((Cell) curPointer.Tag);
			
			if (curPointer.Current.dst(curPointer.Start) >= 20 && selectedCell.Tile.DirectionMovement > -1)
			{
				selectedCell.Tile.State = TileState.Move;
				selectedCell.Tile.StartTimeMovement = new Date();
			}
			else
			{
				selectedCell.Tile.State = TileState.Normal;
				selectedCell.Tile.DirectionMovement = -1;
			}
		}

		// --- Explose la tuile sélectionnée ou renouvelle la map si il y'a un
		// double tap
		if (curPointer.IsDoubleTap())
		{
			Vector2 vec = new Vector2(gameEngine.Render.Camera.position.x + (-vecMidScreen.x + x) * gameEngine.Render.Camera.zoom, gameEngine.Render.Camera.position.y - (-vecMidScreen.y + y) * gameEngine.Render.Camera.zoom);

			Cell selectedCell = GamePlay().PickTile(vec);

			if (selectedCell != null && selectedCell.Tile != null)
				GamePlay().ExplodeTile(selectedCell);
			else
				GamePlay().NewMap();
		}

		curPointer.Reset();

		return true;
	}

	@Override
	public boolean touchDragged(int x, int y, int pointer)
	{
		if (pointer > Context().pointers.length || pointer < 0)
			return false;

		super.touchDragged(x, y, pointer);

		Pointer curPointer = Context().pointers[pointer];

		if (curPointer.Usage == PointerUsage.SelectTile)
		{
			Vector2 vec = new Vector2(gameEngine.Render.Camera.position.x + (-vecMidScreen.x + x) * gameEngine.Render.Camera.zoom, gameEngine.Render.Camera.position.y - (-vecMidScreen.y + y) * gameEngine.Render.Camera.zoom);

			vec = vec.mul(1f / 256f);
			vec = vec.sub(0.5f, 0.5f);
			vec = vec.sub(((Cell) curPointer.Tag).Location);
			vec = vec.nor();

			float angle = (-Common.GetAngle(new Vector2(1, 0), vec) + (float) Math.PI) / (float) (Math.PI * 2) * 6f;

			int direction = Common.mod(((int) angle)+2, 6);

			Cell selectedCell = ((Cell) curPointer.Tag);
			
			if (curPointer.Current.dst(curPointer.Start) >= 20 && selectedCell.Neighbourghs[direction] != null && selectedCell.Neighbourghs[direction].Tile == null)
			{
				selectedCell.Tile.DirectionMovement = direction;
			}
			else
			{
				selectedCell.Tile.DirectionMovement = -1;
			}
			
			
			
			// gameEngine.Render.AddDebugRender("Angle", angle);
		}

		return false;
	}

	@Override
	public boolean touchMoved(int x, int y)
	{
		// TODO Auto-generated method stub
		return false;
	}
}
