package plz.logic.controller.griplz;

import plz.GameEngine;
import plz.engine.Common;
import plz.engine.logic.controller.Pointer;
import plz.engine.logic.controller.PointerUsage;
import plz.logic.gameplay.griplz.GamePlayLogic;
import plz.model.griplz.Cell;
import plz.model.griplz.CellState;
import plz.model.griplz.Context;

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
		return (Context)gameEngine.Context;
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
		
		/*
		// --- En mode Touchscreen, si la tuile est en mode sleep, sélectionner
		// la tuile si possible
		if (GamePlay().SelectedTile != null && GamePlay().SelectedTile.State == TileState.Sleep)
		{
			Vector2 vec = new Vector2(gameEngine.Render.Camera.position.x + (-vecMidScreen.x + x) * gameEngine.Render.Camera.zoom, gameEngine.Render.Camera.position.y - (-vecMidScreen.y + y) * gameEngine.Render.Camera.zoom);

			if (GamePlay().PickTile(vec))
				Context().pointers[pointer].Usage = PointerUsage.SelectTile;
			else if(GamePlay().PickInactiveCellTile(vec))
				Context().pointers[pointer].Usage = PointerUsage.ManualTurnTile;
		}
		// ---
		*/
		
		if (curPointer.Usage == PointerUsage.None)
		{
			Vector2 vec = new Vector2(gameEngine.Render.Camera.position.x + (-vecMidScreen.x + x) * gameEngine.Render.Camera.zoom, gameEngine.Render.Camera.position.y - (-vecMidScreen.y + y) * gameEngine.Render.Camera.zoom);
			
			Cell selectedCell = GamePlay().PickTile(vec);
			
			if(selectedCell!= null && selectedCell.Tile != null)
			{
				curPointer.Usage = PointerUsage.SelectTile;
				selectedCell.State = CellState.Selected;
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
		
		/*
		//--- Lâche la tuile sélectionnée en mode Desktop, débute la validation
		if (GamePlay().SelectedTile != null && Context().pointers[pointer].Usage == PointerUsage.SelectTile)
		{
			GamePlay().ReleaseTile();
		}
		
		//--- Dépose la tuile sélectionnée en mode screentouch, la tuille est en veille
		if (GamePlay().SelectedTile != null && Context().pointers[pointer].Usage == PointerUsage.SelectTile)
		{
			GamePlay().SelectedTile.State = TileState.Sleep;
		}

		//--- Déselectionne la tuile
		if (GamePlay().SelectedTile != null && Context().pointers[pointer].Usage == PointerUsage.UnselectTile)
		{
			GamePlay().UnselectTile();
		}
	
		//--- Débute la validation pour un double-tap
		if (Context().selectionMode == SelectionMode.Screentouch && Context().pointers[pointer].Usage == PointerUsage.None && GamePlay().SelectedTile != null && GamePlay().SelectedTile.State == TileState.Sleep && Context().pointers[pointer].IsDoubleTap())
		{
			Context().pointers[pointer].PreviousCreationDate = null;
			GamePlay().ReleaseTile();
		}

		//--- Met la tuile en veille après une rotation manuelle
		if (GamePlay().SelectedTile != null && Context().pointers[pointer].Usage == PointerUsage.ManualTurnTile)
		{
			GamePlay().SelectedTile.State = TileState.Sleep;
		}
				*/
		

		
		//--- Si une tuile est sélectionnée, déclenche ou annule le mouvement
		if (curPointer.Usage == PointerUsage.SelectTile)
		{
			/*
			if(curPointer.Current.dst(curPointer.Start) >= 10)
			{
				((Cell)curPointer.Tag).State = CellState.Move;
			}
			else
			{
				((Cell)curPointer.Tag).State = CellState.Visible;
				((Cell)curPointer.Tag).Tile.DirectionMovement=-1;
			}*/
		}
		
		//--- Explose la tuile sélectionnée ou renouvelle la map si il y'a un double tap
		if (curPointer.Usage == PointerUsage.None && curPointer.IsDoubleTap())
		{
			Vector2 vec = new Vector2(gameEngine.Render.Camera.position.x + (-vecMidScreen.x + x) * gameEngine.Render.Camera.zoom, gameEngine.Render.Camera.position.y - (-vecMidScreen.y + y) * gameEngine.Render.Camera.zoom);
			
			Cell selectedCell = GamePlay().PickTile(vec);
			
			if(selectedCell != null)
				GamePlay().ExplodeTile(selectedCell);
			else
				GamePlay().NewMap();
		}
		
		curPointer.Usage = PointerUsage.None;
		
		return true;
	}
	
	@Override
	public boolean touchDragged(int x, int y, int pointer)
	{
		if (pointer > Context().pointers.length || pointer < 0)
			return false;

		super.touchDragged(x, y, pointer);

		Pointer curPointer = Context().pointers[pointer];
		
		/*
		// --- Déplacement de la tuile sélectionée
		Pointer selectTilePointer = null;

		if (firstLastPointer != null && pointer == firstLastPointer.Index && firstLastPointer.Usage == PointerUsage.SelectTile)
		{
			selectTilePointer = firstLastPointer;
		}
		else if (secondLastPointer != null && pointer == secondLastPointer.Index && secondLastPointer.Usage == PointerUsage.SelectTile)
		{
			selectTilePointer = secondLastPointer;
		}

		if (selectTilePointer != null)
		{
			Vector2 vec = new Vector2(gameEngine.Render.Camera.position.x + (-vecMidScreen.x + selectTilePointer.Current.x) * gameEngine.Render.Camera.zoom, gameEngine.Render.Camera.position.y - (-vecMidScreen.y + selectTilePointer.Current.y) * gameEngine.Render.Camera.zoom);

			GamePlay().UpdateTileLocation(new Vector2(vec.x, vec.y + Context().selectionOffsetY));
		}
		// ---
		
		//--- Tourne manellement la tuile
		Pointer turnTilePointer = null;

		if (firstLastPointer != null && pointer == firstLastPointer.Index && firstLastPointer.Usage == PointerUsage.ManualTurnTile)
		{
			turnTilePointer = firstLastPointer;
		}
		else if (secondLastPointer != null && pointer == secondLastPointer.Index && secondLastPointer.Usage == PointerUsage.ManualTurnTile)
		{
			turnTilePointer = secondLastPointer;
		}
		
		if (turnTilePointer != null)
		{
			Vector2 vec = new Vector2(gameEngine.Render.Camera.position.x + (-vecMidScreen.x + turnTilePointer.Current.x) * gameEngine.Render.Camera.zoom, gameEngine.Render.Camera.position.y - (-vecMidScreen.y + turnTilePointer.Current.y) * gameEngine.Render.Camera.zoom);

			vec = vec.mul(1f/256f);
			vec = vec.sub(0.5f, 0.5f);
			vec = vec.sub(GamePlay().SelectedTile.ActiveCell.Location);
			vec = vec.nor();
			
			float angle = (-Common.GetAngle(new Vector2(1,0), vec)+(float)Math.PI)/(float)(Math.PI*2) * 6; 
			
			GamePlay().TurnTile((int)angle-1);
		}
		//---
*/
		
		
		
		if (curPointer.Usage == PointerUsage.SelectTile)
		{
			Vector2 vec = new Vector2(gameEngine.Render.Camera.position.x + (-vecMidScreen.x + x) * gameEngine.Render.Camera.zoom, gameEngine.Render.Camera.position.y - (-vecMidScreen.y + y) * gameEngine.Render.Camera.zoom);
			
			vec = vec.mul(1f/256f);
			vec = vec.sub(0.5f, 0.5f);
			vec = vec.sub(((Cell)curPointer.Tag).Location);
			vec = vec.nor();
			
			float angle = (-Common.GetAngle(new Vector2(1,0), vec)+(float)Math.PI)/(float)(Math.PI*2)*6f; 
			
			((Cell)curPointer.Tag).Tile.DirectionMovement=(int)angle;
			
			if(((Cell)curPointer.Tag).Tile.DirectionMovement < 0 || ((Cell)curPointer.Tag).Tile.DirectionMovement > 5)
				((Cell)curPointer.Tag).Tile.DirectionMovement = -1;
			
			gameEngine.Render.AddDebugRender("Angle", angle);
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
