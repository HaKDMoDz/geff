package meepleengine;

import meepleengine.logic.controller.ControllerLogicBase;
import meepleengine.logic.gameplay.GamePlayLogicBase;
import meepleengine.logic.render.RenderLogicBase;
import meepleengine.logic.sound.SoundLogicBase;
import meepleengine.logic.ui.screens.ScreenBase;

import com.badlogic.gdx.ApplicationListener;
import com.badlogic.gdx.Gdx;

public class GameEngine implements ApplicationListener
{
	public ControllerLogicBase Controller;
	public RenderLogicBase Render;
	public GamePlayLogicBase GamePlay;
	public SoundLogicBase Sound;
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
		
//		if(Controller != null)
//			Controller.Update(this.DeltaTime);

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
