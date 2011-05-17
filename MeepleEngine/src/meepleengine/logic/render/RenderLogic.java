package meepleengine.logic.render;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.GL10;

import meepleengine.GameEngine;

public abstract class RenderLogic
{
	public GameEngine gameEngine;

	public RenderLogic(GameEngine gameEngine)
	{
		this.gameEngine = gameEngine;
	}

	public void Render(float deltaTime)
	{
		Gdx.gl.glClearColor(0f,0f,0f,0f);
		Gdx.gl.glClear(GL10.GL_COLOR_BUFFER_BIT);

		gameEngine.CurrentScreen.render(deltaTime);
	}
}
