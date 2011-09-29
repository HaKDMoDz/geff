package plz.engine.logic.render;

import java.util.HashMap;
import java.util.Map;
import java.util.Map.Entry;

import plz.engine.GameEngineBase;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.GL10;
import com.badlogic.gdx.graphics.GL20;
import com.badlogic.gdx.graphics.OrthographicCamera;
import com.badlogic.gdx.graphics.g2d.BitmapFont;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;

public abstract class RenderLogicBase
{
	private final Map<String, Object> mapDebug = new HashMap<String, Object>();
	private final BitmapFont fontDebug;
	public final SpriteBatch spriteBatch;
	public OrthographicCamera Camera;
	float[] direction = { 1, 0.5f, 0, 0 };

	public GameEngineBase gameEngine;

	public RenderLogicBase(GameEngineBase gameEngine)
	{
		this.gameEngine = gameEngine;

		fontDebug = new BitmapFont();
		fontDebug.setColor(Color.WHITE);
		fontDebug.scale(0.1f);

		spriteBatch = new SpriteBatch();

		Camera = new OrthographicCamera();
		Camera.setToOrtho(false);
		Camera.position.set(1024, 1024, 0);
		Camera.zoom = 5;
	}

	public void Render(float deltaTime)
	{
		GL10 gl = Gdx.app.getGraphics().getGL10();
		//GL20 gl = Gdx.app.getGraphics().getGL20();
		
		gl.glViewport(0, 0, Gdx.app.getGraphics().getWidth(), Gdx.app
				.getGraphics().getHeight());

		//gl.glClear(GL20.GL_COLOR_BUFFER_BIT | GL20.GL_DEPTH_BUFFER_BIT);
		gl.glClear(GL10.GL_COLOR_BUFFER_BIT | GL10.GL_DEPTH_BUFFER_BIT);
		
		gl.glClearColor(0, 0, 0, 0);

		setProjectionAndCamera();
		//setLighting(gl);

		ProjectGame();
	}

	public void RenderUI(float deltaTime)
	{
		gameEngine.CurrentScreen.render(deltaTime);
	}

	public void RenderDebug(float deltaTime)
	{
		if (mapDebug.size() > 0)
		{
			ProjectUI();
			
			spriteBatch.begin();
			spriteBatch.setColor(Color.WHITE);
			
			float y = 30;
			for (Entry<String, Object> entry : mapDebug.entrySet())
			{
				fontDebug.draw(spriteBatch, entry.getKey() + " : " + entry.getValue().toString(), 30, y);
				y += fontDebug.getCapHeight()+5;
			}
			
			spriteBatch.end();
			
			ProjectGame();
		}
	}
	
	public void ProjectGame()
	{
		spriteBatch.setProjectionMatrix(Camera.combined);
	}
	
	public void ProjectUI()
	{
		spriteBatch.getProjectionMatrix().setToOrtho2D(0, 0, Gdx.app.getGraphics().getWidth(), Gdx.app
				.getGraphics().getHeight());
	}

	private void setProjectionAndCamera()
	{
		Camera.update();
		Camera.apply(Gdx.gl10);
	}

	private void setLighting(GL10 gl)
	{
		// gl.glEnable(GL10.GL_LIGHTING);
		// gl.glEnable(GL10.GL_LIGHT0);
		// gl.glLightfv(GL10.GL_LIGHT0, GL10.GL_POSITION, direction, 0);
		// gl.glEnable(GL10.GL_COLOR_MATERIAL);
	}

	public void AddDebugRender(String objectName, Object obj)
	{
		mapDebug.put(objectName, obj);
	}
	
	public void RemoveDebugRender(String objectName)
	{
		mapDebug.remove(objectName);
	}
}
