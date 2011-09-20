package twiplz.logic.controller;

import java.sql.Date;

import plz.engine.Common;
import plz.engine.logic.controller.Pointer;
import plz.engine.logic.controller.PointerUsage;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Input.Keys;
import com.badlogic.gdx.math.Matrix4;
import com.badlogic.gdx.math.Quaternion;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.math.Vector3;

import twiplz.Context;
import twiplz.GameEngine;
import twiplz.logic.gameplay.GamePlayLogic;
import twiplz.logic.render.RenderLogic;

public class ControllerLogic extends
		plz.engine.logic.controller.ControllerLogicBase
{
	// Vector2 Context.pointerstart = new Vector2();

	// Vector2[] Context.pointerstart = new Vector2[10];
	// Vector2[] pointerCurrent = new Vector2[10];

	private GamePlayLogic GamePlay()
	{
		return (GamePlayLogic) gameEngine.GamePlay;
	}
	
	float prevCameraZoom = 0;
	Vector2 prevCameraPos;
	float prevCameraAngle = 0;

	public ControllerLogic(GameEngine gameEngine)
	{
		super(gameEngine);
		
		for (int i = 0; i < 10; i++)
		{
			Context.pointers[i] = new Pointer(0,0, i);
		}
		
		//Gdx.input.setInputProcessor(this);
	}

	@Override
	public boolean keyDown(int keycode)
	{
		//gameEngine.CurrentScreen.Stage.keyDown(keycode);
		
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
		//gameEngine.CurrentScreen.Stage.keyUp(keycode);
		
		// TODO Auto-generated method stub
		return false;
	}

	@Override
	public boolean scrolled(int amount)
	{
		//gameEngine.CurrentScreen.Stage.scrolled(amount);

		if(GamePlay().SelectedTile != null)
		{
			GamePlay().TurnTileOffset(amount/Math.abs(amount));
		}
		else
		{
			Zoom(gameEngine.Render.Camera.zoom+amount);
		}
		
		return false;
	}

	@Override
	public boolean keyTyped(char character)
	{
		//gameEngine.CurrentScreen.Stage.keyTyped(character);
		
		// TODO Auto-generated method stub
		return false;
	}

	@Override
	public boolean touchDown(int x, int y, int pointer, int button)
	{
		//gameEngine.CurrentScreen.Stage.touchDown(x, y, pointer, button);
		
		if (pointer > Context.pointers.length)
			return false;

		// --- Stock le zoom et la position pr�c�dente de la cam�ra
		// TODO : peut �tre stocker seulement la matrice de transformation
		prevCameraZoom = gameEngine.Render.Camera.zoom;
		prevCameraPos = new Vector2(gameEngine.Render.Camera.position.x,
				gameEngine.Render.Camera.position.y);
		// ---

		// ---> Met � jour les pointeurs Start
		for (int i = 0; i < Context.pointers.length; i++)
		{
			if (Context.pointers[i] != null)
				Context.pointers[i].SwapStartToCurrent();
		}

		// ---> Met le pointeur de d�part sur la valeur
		Context.pointers[pointer].Init(x, y);
		
		gameEngine.Render.AddDebugRender("Zoom", prevCameraZoom);

		return true;
	}

	@Override
	public boolean touchUp(int x, int y, int pointer, int button)
	{
		//gameEngine.CurrentScreen.Stage.touchUp(x, y, pointer, button);		
		
		if (pointer > Context.pointers.length)
			return false;

		// --- Stock la position pr�c�dente de la cam�ra
		prevCameraPos = new Vector2(gameEngine.Render.Camera.position.x,
				gameEngine.Render.Camera.position.y);

		// ---> Met le pointeur de d�part sur null
		Context.pointers[pointer].Start = null;

		// ---> Met � jour les pointeurs Start
		for (int i = 0; i < Context.pointers.length; i++)
		{
			if (Context.pointers[i] != null)
				Context.pointers[i].SwapStartToCurrent();
		}
		
//		if(GamePlay().SelectedTile!= null)
//		{
//			GamePlay().ReleaseTile();
//		}

		gameEngine.Render.RemoveDebugRender("Context.pointerstart" + pointer);
		gameEngine.Render.RemoveDebugRender("PointerCurrent" + pointer);
		gameEngine.Render.RemoveDebugRender("Cell0");
		gameEngine.Render.RemoveDebugRender("Cell1");

		return true;
	}

	@Override
	public boolean touchDragged(int x, int y, int pointer)
	{
		//gameEngine.CurrentScreen.Stage.touchDragged(x, y, pointer);
		
		if (pointer > Context.pointers.length || pointer < 0)
			return false;

		int countPointerOnScreen = 0;

		Pointer firstLastPointer = null;
		Pointer secondLastPointer = null;

		// --- Mise � jour du pointeur actuel
		Context.pointers[pointer].Current = new Vector2(x, y);
		// ---

		for (int i = 0; i < Context.pointers.length; i++)
		{
			if (Context.pointers[i] != null && Context.pointers[i].Start != null)
			{
				countPointerOnScreen++;

				secondLastPointer = firstLastPointer;
				firstLastPointer = Context.pointers[i];
			}
		}
		
		// ---> Position du centre de l'�cran
		Vector2 vecMidScreen = new Vector2(
				Gdx.app.getGraphics().getWidth() / 2, Gdx.app.getGraphics()
						.getHeight() / 2);

		// --- Translation de la cam�ra avec dernier pointeur
		if (countPointerOnScreen == 1 && pointer == firstLastPointer.Index && firstLastPointer.Usage == PointerUsage.None)
		{
			Vector2 vecTranslation = new Vector2(firstLastPointer.Start.x - x,
					firstLastPointer.Start.y - y);

			vecTranslation.rotate(prevCameraAngle);

			gameEngine.Render.Camera.position.set(prevCameraPos.x
					+ vecTranslation.x * gameEngine.Render.Camera.zoom,
					prevCameraPos.y - vecTranslation.y
							* gameEngine.Render.Camera.zoom, 0);
		
			((RenderLogic)gameEngine.Render).PointToDraw[0] = new Vector2(gameEngine.Render.Camera.position.x+(-vecMidScreen.x+firstLastPointer.Current.x)*gameEngine.Render.Camera.zoom,gameEngine.Render.Camera.position.y-(-vecMidScreen.y+firstLastPointer.Current.y)*gameEngine.Render.Camera.zoom);
		}
		// ---

		// --- D�placement de la tuile s�lection�e
		if (countPointerOnScreen == 1 && pointer == firstLastPointer.Index && firstLastPointer.Usage == PointerUsage.SelectTile)
		{
			Vector2 vecTranslation = new Vector2(firstLastPointer.Start.x - x,
					firstLastPointer.Start.y - y);

			
			vecTranslation.rotate(prevCameraAngle);

			//gameEngine.Render.Camera.position.set(prevCameraPos.x
			//		+ vecTranslation.x * gameEngine.Render.Camera.zoom,
			//		prevCameraPos.y + vecTranslation.y
			//				* gameEngine.Render.Camera.zoom, 0);
		
			Vector2 vec = new Vector2(gameEngine.Render.Camera.position.x+(-vecMidScreen.x+firstLastPointer.Current.x)*gameEngine.Render.Camera.zoom,gameEngine.Render.Camera.position.y-(-vecMidScreen.y+firstLastPointer.Current.y)*gameEngine.Render.Camera.zoom);
			
			
//			Vector3 vec = new Vector3(-vecMidScreen.x+firstLastPointer.Current.x,-vecMidScreen.y+firstLastPointer.Current.y,0f);
//			Vector3 vec = new Vector3(vecMidScreen.x + x, vecMidScreen.y -y,0f);
			
			
//			gameEngine.Render.Camera.unproject(vec);
			
//			GamePlay().SelectedTile.Cells[0].Location = new Vector2(
//					vec.x + GamePlay().CellDisposition[0][GamePlay().CurrentOrientation].x,
//					vec.y + GamePlay().CellDisposition[0][GamePlay().CurrentOrientation].y);
					
			
//			GamePlay().SelectedTile.Cells[0].Location = new Vector2(x/25,-y/25);

			
			
			GamePlay().UpdateTileLocation(new Vector2(vec.x, vec.y));
			
			
		
			gameEngine.Render.AddDebugRender("Cell0", GamePlay().SelectedTile.Cells[0].Location);
			gameEngine.Render.AddDebugRender("Cell1", GamePlay().SelectedTile.Cells[1].Location);
		}
		// ---
		
		// --- Zoom de la cam�ra avec les deux derniers pointeurs
		if (countPointerOnScreen >= 2
				&& (pointer == firstLastPointer.Index || pointer == secondLastPointer.Index)
				&& (Context.pointers[firstLastPointer.Index].Current != null && Context.pointers[secondLastPointer.Index].Current != null)
				&& GamePlay().SelectedTile == null
			)
		{
			float distStart = firstLastPointer.Start
					.dst(secondLastPointer.Start);
			float distCur = firstLastPointer.Current
					.dst(secondLastPointer.Current);

			// ---> Calcul du zoom
			// float diffZoom = distStart / distCur;
			float diffZoom = (distStart - distCur) / 70f;

			gameEngine.Render.AddDebugRender("DiffZoom", diffZoom);

			Vector2 vecSecondToFirstStart = new Vector2(
					firstLastPointer.Start.x - secondLastPointer.Start.x,
					firstLastPointer.Start.y - secondLastPointer.Start.y);
			Vector2 vecSecondToFirstCurrent = new Vector2(
					firstLastPointer.Current.x - secondLastPointer.Current.x,
					firstLastPointer.Current.y - secondLastPointer.Current.y);

			// ---> Position du centre entre les points de d�part A et B
			Vector2 vecMidPointStart = new Vector2(
					secondLastPointer.Start.x + vecSecondToFirstStart.x / 2f,
					secondLastPointer.Start.y + vecSecondToFirstStart.y / 2f);

			// ---> Position du centre entre les points de d�part A et B
			Vector2 vecMidPointCurrent = new Vector2(
					secondLastPointer.Current.x + vecSecondToFirstCurrent.x	/ 2f,
					secondLastPointer.Current.y	+ vecSecondToFirstCurrent.y / 2f);


			// ---> Calcul du vecteur de translation
			Vector2 vecTranslation = new Vector2(
					(vecMidScreen.x	- vecMidPointStart.x) + vecMidPointCurrent.x - vecMidPointStart.x
					,-(vecMidScreen.y - vecMidPointStart.y));// + vecMidPointCurrent.y - vecMidPointStart.y)  );

			((RenderLogic)gameEngine.Render).PointToDraw[0] = new Vector2(gameEngine.Render.Camera.position.x+(-vecMidScreen.x+firstLastPointer.Current.x)*gameEngine.Render.Camera.zoom,gameEngine.Render.Camera.position.y-(-vecMidScreen.y+firstLastPointer.Current.y)*gameEngine.Render.Camera.zoom);
			((RenderLogic)gameEngine.Render).PointToDraw[1] =  new Vector2(gameEngine.Render.Camera.position.x+(-vecMidScreen.x+secondLastPointer.Current.x)*gameEngine.Render.Camera.zoom,gameEngine.Render.Camera.position.y-(-vecMidScreen.y+secondLastPointer.Current.y)*gameEngine.Render.Camera.zoom);
			
			gameEngine.Render.AddDebugRender("vecMidPointStart", vecMidPointStart);
			gameEngine.Render.AddDebugRender("vecMidPointCurrent", vecMidPointCurrent);
			
			// Vector2 vecT = new Vector2(vecMidScreen.x - vecMidPoint2D.x,
			// vecMidScreen.y - vecMidPoint2D.y);

			// vecT.rotate(prevCameraAngle);

			// ---> Calcul de la rotation
			float angle = (Common.GetAngle(vecSecondToFirstCurrent,
					vecSecondToFirstStart) / 6.28f) * 360f;

			//gameEngine.Render.Camera.zoom = prevCameraZoom + diffZoom;
			
			Zoom(prevCameraZoom + diffZoom);

			// float angleRot = prevCameraAngle-angle;
			//
			// prevCameraAngle = angle;
			// gameEngine.Render.Camera.rotate(angleRot, 0f, 0f, 1f);

			gameEngine.Render.Camera.position.set(prevCameraPos.x
					+ vecTranslation.x * diffZoom, prevCameraPos.y
					+ vecTranslation.y * diffZoom, 0);

			gameEngine.Render.Camera.update();

			// gameEngine.Render.AddDebugRender("Angle", angle);
		}
		// ---

		gameEngine.Render.AddDebugRender("Context.pointerstart" + pointer,
				Context.pointers[pointer].Start);
		gameEngine.Render.AddDebugRender("PointerCurrent" + pointer,
				Context.pointers[pointer].Current);

		return false;
	}

	private Vector2 Project(Vector2 vec)
	{
		Vector3 vec3 = new Vector3(vec.x, vec.y, 0);

		gameEngine.Render.Camera.project(vec3);

		return new Vector2(vec3.x, vec3.y);
	}

	@Override
	public boolean touchMoved(int x, int y)
	{

		return false;
	}

	private void SetPointer(int pointer, Vector2[] pointerArray, int x, int y)
	{
		if (pointer < pointerArray.length)
		{
			// pointerArray[pointer] = new Vector2(
			// pointerArrayOrigin.x + x
			// * gameEngine.Render.Camera.zoom,
			// pointerArrayOrigin.y - y
			// * gameEngine.Render.Camera.zoom);

			pointerArray[pointer] = new Vector2(x, y);
		}
	}
	
	private void Zoom(float value)
	{
		if(value < 10 && value >= 2)
			gameEngine.Render.Camera.zoom=value;
	}
}
