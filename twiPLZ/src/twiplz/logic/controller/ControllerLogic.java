package twiplz.logic.controller;

import java.sql.Date;

import plz.engine.Common;
import plz.engine.logic.controller.Pointer;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Input.Keys;
import com.badlogic.gdx.math.Matrix4;
import com.badlogic.gdx.math.Quaternion;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.math.Vector3;

import twiplz.GameEngine;
import twiplz.logic.render.RenderLogic;

public class ControllerLogic extends
		plz.engine.logic.controller.ControllerLogicBase
{
	// Vector2 pointerStart = new Vector2();

	// Vector2[] pointerStart = new Vector2[10];
	// Vector2[] pointerCurrent = new Vector2[10];

	Pointer[] pointers = new Pointer[10];

	float prevCameraZoom = 0;
	Vector2 prevCameraPos;
	float prevCameraAngle = 0;

	public ControllerLogic(GameEngine gameEngine)
	{
		super(gameEngine);
		Gdx.input.setInputProcessor(this);
	}

	@Override
	public boolean keyDown(int keycode)
	{
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
		gameEngine.Render.Camera.zoom += amount;

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
		if (pointer > pointers.length)
			return false;

		// --- Stock le zoom et la position précédente de la caméra
		// TODO : peut être stocker seulement la matrice de transformation
		prevCameraZoom = gameEngine.Render.Camera.zoom;
		prevCameraPos = new Vector2(gameEngine.Render.Camera.position.x,
				gameEngine.Render.Camera.position.y);
		// ---

		// ---> Met à jour les pointeurs Start
		for (int i = 0; i < pointers.length; i++)
		{
			if (pointers[i] != null)
				pointers[i].SwapStartToCurrent();
		}

		// ---> Met le pointeur de départ sur la valeur
		pointers[pointer] = new Pointer(x, y, pointer);

		gameEngine.Render.AddDebugRender("Zoom", prevCameraZoom);

		return true;
	}

	@Override
	public boolean touchUp(int x, int y, int pointer, int button)
	{
		if (pointer > pointers.length)
			return false;

		// --- Stock la position précédente de la caméra
		prevCameraPos = new Vector2(gameEngine.Render.Camera.position.x,
				gameEngine.Render.Camera.position.y);

		// ---> Met le pointeur de départ sur null
		pointers[pointer].Start = null;

		// ---> Met à jour les pointeurs Start
		for (int i = 0; i < pointers.length; i++)
		{
			if (pointers[i] != null)
				pointers[i].SwapStartToCurrent();
		}

		gameEngine.Render.RemoveDebugRender("PointerStart" + pointer);
		gameEngine.Render.RemoveDebugRender("PointerCurrent" + pointer);

		return true;
	}

	@Override
	public boolean touchDragged(int x, int y, int pointer)
	{
		if (pointer > pointers.length)
			return false;

		int countPointerOnScreen = 0;

		Pointer firstLastPointer = null;
		Pointer secondLastPointer = null;

		// --- Mise à jour du pointeur actuel
		pointers[pointer].Current = new Vector2(x, y);
		// ---

		for (int i = 0; i < pointers.length; i++)
		{
			if (pointers[i] != null && pointers[i].Start != null)
			{
				countPointerOnScreen++;

				secondLastPointer = firstLastPointer;
				firstLastPointer = pointers[i];
			}
		}

		
		// ---> Position du centre de l'écran
		Vector2 vecMidScreen = new Vector2(
				Gdx.app.getGraphics().getWidth() / 2, Gdx.app.getGraphics()
						.getHeight() / 2);

		// --- Translation de la caméra avec dernier pointeur
		if (countPointerOnScreen == 1 && pointer == firstLastPointer.Index)
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

		
		// --- Zoom de la caméra avec les deux derniers pointeurs
		if (countPointerOnScreen >= 2
				&& (pointer == firstLastPointer.Index || pointer == secondLastPointer.Index)
				&& (pointers[firstLastPointer.Index].Current != null && pointers[secondLastPointer.Index].Current != null))
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

			// ---> Position du centre entre les points de départ A et B
			Vector2 vecMidPointStart = new Vector2(
					secondLastPointer.Start.x + vecSecondToFirstStart.x / 2f,
					secondLastPointer.Start.y + vecSecondToFirstStart.y / 2f);

			// ---> Position du centre entre les points de départ A et B
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

			gameEngine.Render.Camera.zoom = prevCameraZoom + diffZoom;

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

		// gameEngine.Render.AddDebugRender("X", x);
		// gameEngine.Render.AddDebugRender("Y", y);

		gameEngine.Render.AddDebugRender("PointerStart" + pointer,
				pointers[pointer].Start);
		gameEngine.Render.AddDebugRender("PointerCurrent" + pointer,
				pointers[pointer].Current);

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
}
