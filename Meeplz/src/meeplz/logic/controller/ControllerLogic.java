package meeplz.logic.controller;

import java.awt.Point;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Input.Keys;

import meeplz.GameEngine;

public class ControllerLogic extends
		meepleengine.logic.controller.ControllerLogicBase
{
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
		gameEngine.Render.Camera.zoom += (float) amount;

		return false;
	}

	@Override
	public boolean keyTyped(char character)
	{
		// TODO Auto-generated method stub
		return false;
	}

	private boolean pressed = false;
	Point pointerStart = new Point();

	@Override
	public boolean touchDown(int x, int y, int pointer, int button)
	{
		pointerStart = new Point((int)gameEngine.Render.Camera.position.x + x, (int)gameEngine.Render.Camera.position.y+ y);
		pressed = true;

		return false;
	}

	@Override
	public boolean touchUp(int x, int y, int pointer, int button)
	{
		pressed = false;


		return false;
	}

	@Override
	public boolean touchDragged(int x, int y, int pointer)
	{
		gameEngine.Render.Camera.position.set(pointerStart.x-x, pointerStart.y+y,0);
 
		return false;
	}

	@Override
	public boolean touchMoved(int x, int y)
	{
		// if(pressed)
		// gameEngine.Render.Camera.position.add(x, y, 0);

		return false;
	}
}
