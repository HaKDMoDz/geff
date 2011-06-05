package twiplz.logic.controller;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Input.Keys;
import com.badlogic.gdx.math.Vector2;

import twiplz.GameEngine;

public class ControllerLogic extends
		plz.engine.logic.controller.ControllerLogicBase
{
	// Vector2 pointerStart = new Vector2();

	Vector2[] pointerStart = new Vector2[10];
	Vector2[] pointerCurrent = new Vector2[10];
	float prevZoom = 0;

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
		prevZoom = gameEngine.Render.Camera.zoom;
		SetPointer(pointer, pointerStart, x, y);

		// pointerStart = new
		// Vector2(gameEngine.Render.Camera.position.x+x*gameEngine.Render.Camera.zoom,
		// gameEngine.Render.Camera.position.y-y*gameEngine.Render.Camera.zoom);

		return false;
	}

	@Override
	public boolean touchUp(int x, int y, int pointer, int button)
	{
		pointerStart[pointer] = null;

		return false;
	}

	@Override
	public boolean touchDragged(int x, int y, int pointer)
	{
		
		int countPointerOnScreen = 0;
		Vector2 firstLastPointerOnScreen = null;
		Vector2 secondLastPointerOnScreen = null;

		int firstLastPointerIndex = 0;
		int secondLastPointerIndex = 0;

		// --- Mise à jour du pointeur actuel
		SetPointer(pointer, pointerCurrent, x, y);
		// ---

		for (int i = 0; i < pointerStart.length; i++)
		{
			if (pointerStart[i] != null)
			{
				countPointerOnScreen++;
				secondLastPointerIndex = firstLastPointerIndex;
				firstLastPointerIndex = i;

				secondLastPointerOnScreen = firstLastPointerOnScreen;
				firstLastPointerOnScreen = pointerStart[i];
			}
		}

		// --- Translation de la caméra avec le seul et dernier pointeur
		if (countPointerOnScreen == 1 && pointer == firstLastPointerIndex)
		{
			gameEngine.Render.Camera.position.set(firstLastPointerOnScreen.x
					- x * gameEngine.Render.Camera.zoom,
					firstLastPointerOnScreen.y + y
							* gameEngine.Render.Camera.zoom, 0);
		}
		// ---

		// --- Zoom de la caméra avec les deux derniers pointeurs
		if (countPointerOnScreen >= 2
				&& (pointer == firstLastPointerIndex || pointer == secondLastPointerIndex) &&
				(pointerCurrent[firstLastPointerIndex] != null && pointerCurrent[secondLastPointerIndex] != null)
		)
		{
			float distStart = firstLastPointerOnScreen
					.dst(secondLastPointerOnScreen);
			float distCur = pointerCurrent[firstLastPointerIndex]
					.dst(pointerCurrent[secondLastPointerIndex]);

			float diffZoom = (distStart - distCur)/1000f;
			gameEngine.Render.Camera.zoom = prevZoom + diffZoom;
		}
		// ---

//		 gameEngine.Render.Camera.position.set(pointerStart[0].x-x*gameEngine.Render.Camera.zoom,
//		 pointerStart[0].y+y*gameEngine.Render.Camera.zoom,0);
		
		// gameEngine.Render.Camera.position.set(pointerStart.x-x*gameEngine.Render.Camera.zoom,
		// pointerStart.y+y*gameEngine.Render.Camera.zoom,0);

		return false;
	}

	@Override
	public boolean touchMoved(int x, int y)
	{

		return false;
	}

	private void SetPointer(int pointer, Vector2[] pointerArray, int x, int y)
	{
		try
		{
			if (pointer < pointerArray.length)
			{
				pointerArray[pointer] = new Vector2(
						gameEngine.Render.Camera.position.x + x
								* gameEngine.Render.Camera.zoom,
						gameEngine.Render.Camera.position.y - y
								* gameEngine.Render.Camera.zoom);
			}
		} catch (Exception ex)
		{
			int a = 0;
		}
	}
}
