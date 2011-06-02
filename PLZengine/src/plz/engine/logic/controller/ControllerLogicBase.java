package plz.engine.logic.controller;

import plz.engine.GameEngineBase;

import com.badlogic.gdx.InputProcessor;


public abstract class ControllerLogicBase implements InputProcessor
{
	public GameEngineBase gameEngine;

	public ControllerLogicBase(GameEngineBase gameEngine)
	{
		this.gameEngine = gameEngine;
	}
}
