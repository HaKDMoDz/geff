package plz.engine;

import plz.engine.logic.controller.ControllerLogicBase;
import plz.engine.logic.controller.PLZInputMultiplexer;
import plz.engine.logic.gameplay.GamePlayLogicBase;
import plz.engine.logic.render.RenderLogicBase;
import plz.engine.logic.sound.SoundLogicBase;
import plz.engine.logic.ui.screens.ScreenBase;

import com.badlogic.gdx.ApplicationListener;
import com.badlogic.gdx.Gdx;

public class GameEngineBase implements ApplicationListener {
	public ControllerLogicBase Controller;
	public RenderLogicBase Render;
	public GamePlayLogicBase GamePlay;
	public SoundLogicBase Sound;
	public ContextBase Context;
	public float DeltaTime;
	public ScreenBase CurrentScreen;
	public ScreenBase NextScreen;

	@Override
	public void create() {
		// TODO Auto-generated method stub
	}

	@Override
	public void resume() {
		// TODO Auto-generated method stub

	}

	@Override
	public void render() {
		try {
			this.DeltaTime = Gdx.app.getGraphics().getDeltaTime();

			// ---> Pas besoin d'appeler la couche Controller car libgdx le fait
			// avant
			// d'appeler la méthode render de l'ApplicationListener grace à
			// l'enregistrement de l'InputProcessor

			if (GamePlay != null)
				GamePlay.Update(this.DeltaTime);

			if (Render != null) {
				Render.Render(this.DeltaTime);
				Render.RenderUI(this.DeltaTime);
				//Render.RenderDebug(this.DeltaTime);
			}
		} catch (Exception ex)
		{
			Gdx.app.log("Erreur", ex.getMessage());
			ex.printStackTrace();
		}
	}

	@Override
	public void resize(int width, int height) {
		// TODO Auto-generated method stub
	}

	@Override
	public void pause() {
		// TODO Auto-generated method stub

	}

	@Override
	public void dispose() {
		// TODO Auto-generated method stub

	}

	public void RegisterInput()
	{
		//--- Enregistrement de l'InputMultiplexer
		PLZInputMultiplexer input = new PLZInputMultiplexer();
		input.addProcessor(this.CurrentScreen.Stage);
		if(this.Controller != null)
			input.addProcessor(this.Controller);
		
		Gdx.input.setInputProcessor(input);
		//---
	}
}
