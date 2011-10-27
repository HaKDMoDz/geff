package plz;

import plz.engine.logic.controller.PLZInputMultiplexer;

import com.badlogic.gdx.Application.ApplicationType;
import com.badlogic.gdx.Gdx;

import plz.logic.controller.twiplz.ControllerLogic;
import plz.logic.controller.twiplz.SelectionMode;
import plz.logic.gameplay.twiplz.GamePlayLogic;
import plz.logic.render.twiplz.RenderLogic;
import plz.logic.ui.screens.*;
import plz.logic.ui.screens.twiplz.GameScreen;
import plz.model.twiplz.Context;
import plz.model.twiplz.GameMode;

public class GameEngine extends plz.engine.GameEngineBase
{
	public GameEngine()
	{
	}
	
	@Override
	public void create()
	{
//		Music music = Gdx.audio.newMusic(Gdx.files.getFileHandle("data/8.12.mp3", FileType.Internal));
//		music.setLooping(true);
//		music.play();
		
		this.Context = new Context();
		
		if(Gdx.app.getType() ==ApplicationType.Android)
		{
			((Context)Context).Mini = false;
			((Context)Context).selectionOffsetY = 0;
			((Context)Context).selectionMode = SelectionMode.Screentouch;
			((Context)Context).gameMode = GameMode.Circular;
		}
		else
		{
			((Context)Context).selectionMode = SelectionMode.Screentouch;
			((Context)Context).Mini = true;
		}
		
		this.Render = new RenderLogic(this);
		this.Controller = new ControllerLogic(this);
		this.GamePlay = new GamePlayLogic(this);
		
		this.CurrentScreen = new GameScreen(this);
		this.CurrentScreen.show();
		
		//--- Enregistrement de l'InputMultiplexer
		PLZInputMultiplexer input = new PLZInputMultiplexer();
		input.addProcessor(this.CurrentScreen.Stage);
		input.addProcessor(this.Controller);
		
		Gdx.input.setInputProcessor(input);
		//---
	}
}
