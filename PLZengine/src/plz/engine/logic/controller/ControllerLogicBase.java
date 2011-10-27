package plz.engine.logic.controller;

import plz.engine.ContextBase;
import plz.engine.GameEngineBase;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.InputProcessor;
import com.badlogic.gdx.math.Vector2;


public abstract class ControllerLogicBase implements InputProcessor
{
	public GameEngineBase gameEngine;

	protected Pointer firstLastPointer = null;
	protected Pointer secondLastPointer = null;
	protected int countPointerOnScreen;
	float prevCameraZoom = 0;
	Vector2 prevCameraPos;
	float prevCameraAngle = 0;
	protected Vector2 vecMidScreen;
	
	public ContextBase Context()
	{
		return gameEngine.Context;
	}
	
	public ControllerLogicBase(GameEngineBase gameEngine)
	{
		this.gameEngine = gameEngine;
		
		// ---> Position du centre de l'écran
		vecMidScreen = new Vector2(Gdx.app.getGraphics().getWidth() / 2, Gdx.app.getGraphics().getHeight() / 2);
		
		for (int i = 0; i < 10; i++)
		{
			Context().pointers[i] = new Pointer(i);
		}
	}
	
	private void ProcessPointer()
	{
		countPointerOnScreen = 0;

		firstLastPointer = null;
		secondLastPointer = null;

		for (int i = 0; i < Context().pointers.length; i++)
		{
			if (Context().pointers[i] != null && Context().pointers[i].Start != null)
			{
				countPointerOnScreen++;

				secondLastPointer = firstLastPointer;
				firstLastPointer = Context().pointers[i];
			}
		}
	}
	
	@Override
	public boolean touchDown(int x, int y, int pointer, int button)
	{
		// --- Stock le zoom et la position précédente de la caméra
		// TODO : peut être stocker seulement la matrice de transformation
		prevCameraZoom = gameEngine.Render.Camera.zoom;
		prevCameraPos = new Vector2(gameEngine.Render.Camera.position.x, gameEngine.Render.Camera.position.y);
		// ---
		
		// ---> Met le pointeur de départ sur la valeur
		Context().pointers[pointer].Init(x, y);

		ProcessPointer();
		
		//--- Zoom
		if (countPointerOnScreen >= 2 && (
				(pointer == firstLastPointer.Index && secondLastPointer.Usage == PointerUsage.None) 
				|| 
				(pointer == secondLastPointer.Index && firstLastPointer.Usage == PointerUsage.None)))
		{
			firstLastPointer.Usage = PointerUsage.Zoom;
			secondLastPointer.Usage = PointerUsage.Zoom;
		}
		//---
		
		return true;
	}

	
	@Override
	public boolean touchUp(int x, int y, int pointer, int button)
	{
		if (pointer > Context().pointers.length)
			return false;

		// --- Stock la position précédente de la caméra
		prevCameraPos = new Vector2(gameEngine.Render.Camera.position.x, gameEngine.Render.Camera.position.y);

		ProcessPointer();
		
		// ---> Met le pointeur de départ sur null
		Context().pointers[pointer].Start = null;
		
		
		
		//--- Désactive le zoom
		if(countPointerOnScreen >= 2)
		{
			if(pointer == firstLastPointer.Index && firstLastPointer.Usage == PointerUsage.Zoom)
			{
				secondLastPointer.Start = secondLastPointer.Current;
				secondLastPointer.Usage = PointerUsage.None;
			}
			else if(pointer == secondLastPointer.Index && secondLastPointer.Usage == PointerUsage.Zoom)
			{
				firstLastPointer.Start = firstLastPointer.Current;
				firstLastPointer.Usage = PointerUsage.None;
			}
		}
		//---
		
		
		return true;
	}
	
	@Override
	public boolean touchDragged(int x, int y, int pointer)
	{
		// --- Mise à jour du pointeur actuel
		Context().pointers[pointer].Current = new Vector2(x, y);
		// ---
	
		 ProcessPointer();
		
		// --- Translation de la caméra avec le dernier pointeur
		Pointer translateMapPointer = null;
	
		if (firstLastPointer != null && pointer == firstLastPointer.Index && firstLastPointer.Usage == PointerUsage.None)
		{
			translateMapPointer = firstLastPointer;
		}
		else if (secondLastPointer != null && pointer == secondLastPointer.Index && secondLastPointer.Usage == PointerUsage.None)
		{
			translateMapPointer = secondLastPointer;
		}
	
		if (translateMapPointer != null)
		{
			Vector2 vecTranslation = new Vector2(x-translateMapPointer.Start.x, y-translateMapPointer.Start.y);
	
			vecTranslation.rotate(prevCameraAngle);
	
			gameEngine.Render.Camera.position.set(prevCameraPos.x - vecTranslation.x * gameEngine.Render.Camera.zoom, prevCameraPos.y + vecTranslation.y * gameEngine.Render.Camera.zoom, 0);
	
			//((RenderLogic) gameEngine.Render).PointToDraw[0] = new Vector2(gameEngine.Render.Camera.position.x + (-vecMidScreen.x + translateMapPointer.Current.x) * gameEngine.Render.Camera.zoom, gameEngine.Render.Camera.position.y - (-vecMidScreen.y + translateMapPointer.Current.y) * gameEngine.Render.Camera.zoom);
		}
		// ---
		

		// --- Zoom de la caméra avec les deux derniers pointeurs
		if ((firstLastPointer != null && pointer == firstLastPointer.Index && firstLastPointer.Usage == PointerUsage.Zoom) ||
			(secondLastPointer != null && pointer == secondLastPointer.Index && secondLastPointer.Usage == PointerUsage.Zoom))	
		{
			float distStart = firstLastPointer.Start.dst(secondLastPointer.Start);
			float distCur = firstLastPointer.Current.dst(secondLastPointer.Current);

			// ---> Calcul du zoom
			float diffZoom = (distStart - distCur) / 70f;

			//gameEngine.Render.AddDebugRender("DiffZoom", diffZoom);

			Vector2 vecSecondToFirstStart = new Vector2(firstLastPointer.Start.x - secondLastPointer.Start.x, firstLastPointer.Start.y - secondLastPointer.Start.y);
			Vector2 vecSecondToFirstCurrent = new Vector2(firstLastPointer.Current.x - secondLastPointer.Current.x, firstLastPointer.Current.y - secondLastPointer.Current.y);

			// ---> Position du centre entre les points de départ A et B
			Vector2 vecMidPointStart = new Vector2(secondLastPointer.Start.x + vecSecondToFirstStart.x / 2f, secondLastPointer.Start.y + vecSecondToFirstStart.y / 2f);

			// ---> Position du centre entre les points de départ A et B
			Vector2 vecMidPointCurrent = new Vector2(secondLastPointer.Current.x + vecSecondToFirstCurrent.x / 2f, secondLastPointer.Current.y + vecSecondToFirstCurrent.y / 2f);

			// ---> Calcul du vecteur de translation
			Vector2 vecTranslation = new Vector2((vecMidScreen.x - vecMidPointStart.x) + vecMidPointCurrent.x - vecMidPointStart.x, (vecMidScreen.y - vecMidPointStart.y) + vecMidPointCurrent.y - vecMidPointStart.y);// +
																																											// vecMidPointCurrent.y
			//((RenderLogic) gameEngine.Render).PointToDraw[0] = new Vector2(gameEngine.Render.Camera.position.x + (-vecMidScreen.x + firstLastPointer.Current.x) * gameEngine.Render.Camera.zoom, gameEngine.Render.Camera.position.y - (-vecMidScreen.y + firstLastPointer.Current.y) * gameEngine.Render.Camera.zoom);
			//((RenderLogic) gameEngine.Render).PointToDraw[1] = new Vector2(gameEngine.Render.Camera.position.x + (-vecMidScreen.x + secondLastPointer.Current.x) * gameEngine.Render.Camera.zoom, gameEngine.Render.Camera.position.y - (-vecMidScreen.y + secondLastPointer.Current.y) * gameEngine.Render.Camera.zoom);
			

			//gameEngine.Render.AddDebugRender("vecMidPointStart", vecMidPointStart);
			//gameEngine.Render.AddDebugRender("vecMidPointCurrent", vecMidPointCurrent);

			// Vector2 vecT = new Vector2(vecMidScreen.x - vecMidPoint2D.x,
			// vecMidScreen.y - vecMidPoint2D.y);

			// vecT.rotate(prevCameraAngle);

			// ---> Calcul de la rotation
			// float angle = (Common.GetAngle(vecSecondToFirstCurrent,
			// vecSecondToFirstStart) / 6.28f) * 360f;

			// gameEngine.Render.Camera.zoom = prevCameraZoom + diffZoom;

			Zoom(prevCameraZoom + diffZoom);

			// float angleRot = prevCameraAngle-angle;
			//
			// prevCameraAngle = angle;
			// gameEngine.Render.Camera.rotate(angleRot, 0f, 0f, 1f);

			gameEngine.Render.Camera.position.set(prevCameraPos.x + vecTranslation.x * diffZoom, prevCameraPos.y + vecTranslation.y * diffZoom, 0);

			gameEngine.Render.Camera.update();

			// gameEngine.Render.AddDebugRender("Angle", angle);
			gameEngine.Render.AddDebugRender("Zoom", gameEngine.Render.Camera.zoom);
		}
		// ---
		
		return true;
	}
	
//	private Vector2 Project(Vector2 vec)
//	{
//		Vector3 vec3 = new Vector3(vec.x, vec.y, 0);
//
//		gameEngine.Render.Camera.project(vec3);
//
//		return new Vector2(vec3.x, vec3.y);
//	}
	
	protected void SetPointer(int pointer, Vector2[] pointerArray, int x, int y)
	{
		if (pointer < pointerArray.length)
		{
			pointerArray[pointer] = new Vector2(x, y);
		}
	}
	
	protected void Zoom(float value)
	{
		if (value < 8  && value >= 0.5f)
			gameEngine.Render.Camera.zoom = value;
	}
}
