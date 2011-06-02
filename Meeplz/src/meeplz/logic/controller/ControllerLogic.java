package meeplz.logic.controller;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Input.Keys;
import com.badlogic.gdx.math.Vector2;

import meeplz.GameEngine;

public class ControllerLogic extends
		plz.engine.logic.controller.ControllerLogicBase
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
		gameEngine.Render.Camera.zoom += amount;

		return false;
	}

	@Override
	public boolean keyTyped(char character)
	{
		// TODO Auto-generated method stub
		return false;
	}

	private boolean pressed = false;
	Vector2 pointerStart = new Vector2();

	@Override
	public boolean touchDown(int x, int y, int pointer, int button)
	{
		pointerStart = new Vector2(gameEngine.Render.Camera.position.x + x, gameEngine.Render.Camera.position.y+ y);
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
