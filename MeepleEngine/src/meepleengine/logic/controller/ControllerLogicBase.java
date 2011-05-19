package meepleengine.logic.controller;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.InputProcessor;

import meepleengine.GameEngine;

public abstract class ControllerLogicBase implements InputProcessor
{
	public GameEngine gameEngine;

	public ControllerLogicBase(GameEngine gameEngine)
	{
		this.gameEngine = gameEngine;
	}
}
