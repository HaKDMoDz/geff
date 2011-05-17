package meepleengine;

import meepleengine.logic.controller.ControllerLogic;
import meepleengine.logic.gameplay.GamePlayLogic;
import meepleengine.logic.render.RenderLogic;
import meepleengine.logic.sound.SoundLogic;
import meepleengine.logic.ui.screens.ScreenBase;

import com.badlogic.gdx.ApplicationListener;
import com.badlogic.gdx.Gdx;

public class GameEngine implements ApplicationListener
{
	public ControllerLogic Controller;
	public RenderLogic Render;
	public GamePlayLogic GamePlay;
	public SoundLogic Sound;
	public float DeltaTime;
	public ScreenBase CurrentScreen;
	public ScreenBase NextScreen;
	
	@Override
	public void create()
	{
		// TODO Auto-generated method stub
	}

	@Override
	public void resume()
	{
		// TODO Auto-generated method stub

	}

	@Override
	public void render()
	{
		this.DeltaTime = Gdx.app.getGraphics().getDeltaTime();
		
		if(Controller != null)
			Controller.Update(this.DeltaTime);

		if(GamePlay != null)
			GamePlay.Update(this.DeltaTime);
		
		if (Render != null)
			Render.Render(this.DeltaTime);
	}

	@Override
	public void resize(int width, int height)
	{
		// TODO Auto-generated method stub

	}

	@Override
	public void pause()
	{
		// TODO Auto-generated method stub

	}

	@Override
	public void dispose()
	{
		// TODO Auto-generated method stub

	}
}
