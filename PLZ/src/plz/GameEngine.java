package plz;

import plz.engine.ContextBase;
import plz.engine.logic.controller.PLZInputMultiplexer;
import plz.engine.logic.render.RenderLogicBase;

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
	public GameEngine(String[] argv)
	{
		super(argv);
	}
	
	@Override
	public void create()
	{
//		Music music = Gdx.audio.newMusic(Gdx.files.getFileHandle("data/8.12.mp3", FileType.Internal));
//		music.setLooping(true);
//		music.play();
		
		this.Context = new ContextBase();
		this.Render = new RenderLogicBase(this);
		this.CurrentScreen = new MainMenu(this);
		this.CurrentScreen.show();
	
		RegisterInput();
	}
}
