package meepleengine.logic.render;

import com.badlogic.gdx.Application;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Graphics;
import com.badlogic.gdx.graphics.GL10;
import com.badlogic.gdx.graphics.PerspectiveCamera;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.math.Matrix4;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.math.Vector3;

import meepleengine.GameEngine;

public abstract class RenderLogic
{
	protected final SpriteBatch spriteBatch;
	private Matrix4 viewMatrix = new Matrix4();
	private Matrix4 transformMatrix = new Matrix4();
	private PerspectiveCamera camera;
	float[] direction = { 1, 0.5f, 0, 0 };

	public GameEngine gameEngine;

	public RenderLogic(GameEngine gameEngine)
	{
		this.gameEngine = gameEngine;

		spriteBatch = new SpriteBatch();
		camera = new PerspectiveCamera(67, Gdx.graphics.getWidth(),
				Gdx.graphics.getHeight());
	}

	public void Render(float deltaTime)
	{
		GL10 gl = Gdx.app.getGraphics().getGL10();
		gl.glClear(GL10.GL_COLOR_BUFFER_BIT | GL10.GL_DEPTH_BUFFER_BIT);
		gl.glViewport(0, 0, Gdx.app.getGraphics().getWidth(), Gdx.app
				.getGraphics().getHeight());

		// renderBackground(gl);

		// gl.glDisable(GL10.GL_DITHER);
		// gl.glEnable(GL10.GL_DEPTH_TEST);
		// gl.glEnable(GL10.GL_CULL_FACE);

		setProjectionAndCamera();
		setLighting(gl);

		gl.glEnable(GL10.GL_TEXTURE_2D);

		// renderShip(gl, simulation.ship, app);
		// renderInvaders(gl, simulation.invaders);

		// gl.glDisable(GL10.GL_TEXTURE_2D);
		// renderBlocks(gl, simulation.blocks);

		// gl.glDisable(GL10.GL_LIGHTING);
		// renderShots(gl, simulation.shots);

		// gl.glEnable(GL10.GL_TEXTURE_2D);
		// renderExplosions(gl, simulation.explosions);

		// gl.glDisable(GL10.GL_CULL_FACE);
		// gl.glDisable(GL10.GL_DEPTH_TEST);

		spriteBatch.setProjectionMatrix(viewMatrix);
		spriteBatch.setTransformMatrix(transformMatrix);
		// spriteBatch.begin();
		// // if (simulation.ship.lives != lastLives || simulation.score !=
		// lastScore || simulation.wave != lastWave) {
		// // status = "lives: " + simulation.ship.lives + " wave: " +
		// simulation.wave + " score: " + simulation.score;
		// // lastLives = simulation.ship.lives;
		// // lastScore = simulation.score;
		// // lastWave = simulation.wave;
		// // }
		// spriteBatch.enableBlending();
		// spriteBatch.setBlendFunction(GL10.GL_ONE,
		// GL10.GL_ONE_MINUS_SRC_ALPHA);
		// //font.draw(spriteBatch, status, 0, 320);
		// spriteBatch.end();

		gameEngine.CurrentScreen.render(deltaTime);
	}

	private void setProjectionAndCamera()
	{
		//viewMatrix.setToOrtho2D(0, 0, 400, 320);
		
		camera.position.set(0, 6, 100);
		camera.direction.set(0, 0, -4).sub(camera.position).nor();

		viewMatrix.setToLookAt(camera.position, camera.direction, new Vector3(0,1,0));

		camera.update();
		camera.apply(Gdx.gl10);
	}

	private void setLighting(GL10 gl)
	{
		gl.glEnable(GL10.GL_LIGHTING);
		gl.glEnable(GL10.GL_LIGHT0);
		gl.glLightfv(GL10.GL_LIGHT0, GL10.GL_POSITION, direction, 0);
		gl.glEnable(GL10.GL_COLOR_MATERIAL);
	}
}
