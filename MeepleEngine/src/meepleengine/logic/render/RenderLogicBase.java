package meepleengine.logic.render;

import com.badlogic.gdx.Application;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Graphics;
import com.badlogic.gdx.graphics.GL10;
import com.badlogic.gdx.graphics.OrthographicCamera;
import com.badlogic.gdx.graphics.PerspectiveCamera;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.math.Matrix4;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.math.Vector3;

import meepleengine.GameEngine;

public abstract class RenderLogicBase
{
	protected final SpriteBatch spriteBatch;
	public OrthographicCamera Camera;
	float[] direction = { 1, 0.5f, 0, 0 };

	public GameEngine gameEngine;

	public RenderLogicBase(GameEngine gameEngine)
	{
		this.gameEngine = gameEngine;

		spriteBatch = new SpriteBatch();

		Camera = new OrthographicCamera(Gdx.graphics.getWidth(),
				Gdx.graphics.getHeight());

		Camera.position.set(256, 256, 0);
		Camera.direction.set(0, 0, -1);
		Camera.zoom = 10;
	}

	public void Render(float deltaTime)
	{
		GL10 gl = Gdx.app.getGraphics().getGL10();
		gl.glClear(GL10.GL_COLOR_BUFFER_BIT | GL10.GL_DEPTH_BUFFER_BIT);
		gl.glClearColor(1, 0, 0, 0);
		gl.glViewport(0, 0, Gdx.app.getGraphics().getWidth(), Gdx.app
				.getGraphics().getHeight());

		setProjectionAndCamera();
		setLighting(gl);

		spriteBatch.setProjectionMatrix(Camera.combined);

		gameEngine.CurrentScreen.render(deltaTime);
	}

	private void setProjectionAndCamera()
	{
		Camera.update();
		// Camera.apply(Gdx.gl10);
	}

	private void setLighting(GL10 gl)
	{
		// gl.glEnable(GL10.GL_LIGHTING);
		// gl.glEnable(GL10.GL_LIGHT0);
		// gl.glLightfv(GL10.GL_LIGHT0, GL10.GL_POSITION, direction, 0);
		// gl.glEnable(GL10.GL_COLOR_MATERIAL);
	}
}
