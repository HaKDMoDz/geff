package plz.engine.logic.ui.screens;

import plz.engine.GameEngineBase;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Screen;


public class ScreenBase implements Screen
{
	public com.badlogic.gdx.scenes.scene2d.Stage Stage;
	protected GameEngineBase gameEngine; 
	
	public ScreenBase(GameEngineBase gameEngine)
	{
		this.gameEngine = gameEngine;
	}
	
	@Override
	public void render(float delta)
	{
		Stage.act(delta);
		Stage.draw();
	}

	@Override
	public void resize(int width, int height)
	{
		// TODO Auto-generated method stub
		
	}

	@Override
	public void show()
	{
		Stage = new com.badlogic.gdx.scenes.scene2d.Stage(Gdx.graphics.getWidth(), Gdx.graphics.getHeight(),false);
		 Gdx.input.setInputProcessor(Stage);
	}

	@Override
	public void hide()
	{
		// TODO Auto-generated method stub
		
	}

	@Override
	public void pause()
	{
		// TODO Auto-generated method stub
		
	}

	@Override
	public void resume()
	{
		// TODO Auto-generated method stub
		
	}

	@Override
	public void dispose()
	{
		// TODO Auto-generated method stub
		
	}
}