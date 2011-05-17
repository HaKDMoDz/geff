package meepleengine.logic.controller;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.InputProcessor;

import meepleengine.GameEngine;

public class ControllerLogic implements InputProcessor
{
	public GameEngine gameEngine;

	public ControllerLogic(GameEngine gameEngine)
	{
		this.gameEngine = gameEngine;
		Gdx.input.setInputProcessor(this);
	}

	public void Update(float deltaTime)
	{
	}

	@Override
	public boolean keyDown(int keycode)
	{
		// TODO Auto-generated method stub
		return false;
	}

	@Override
	public boolean keyUp(int keycode)
	{
		// TODO Auto-generated method stub
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
		gameEngine.CurrentScreen.Stage.touchDown(x, y, pointer, button);
		return false;
	}

	@Override
	public boolean touchUp(int x, int y, int pointer, int button)
	{
		gameEngine.CurrentScreen.Stage.touchUp(x, y, pointer, button);
		return false;
	}

	@Override
	public boolean touchDragged(int x, int y, int pointer)
	{
		// TODO Auto-generated method stub
		return false;
	}

	@Override
	public boolean touchMoved(int x, int y)
	{
		// TODO Auto-generated method stub
		return false;
	}

	@Override
	public boolean scrolled(int amount)
	{
		// TODO Auto-generated method stub
		return false;
	}
}
