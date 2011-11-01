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
	public GameEngine()
	{
	}
	
	@Override
	public void create()
	{
//		Music music = Gdx.audio.newMusic(Gdx.files.getFileHandle("data/8.12.mp3", FileType.Internal));
//		music.setLooping(true);
//		music.play();
		
		
//		this.Context = new ContextBase();
//		this.Render = new RenderLogicBase(this);
//		this.CurrentScreen = new MainMenu(this);
//		this.CurrentScreen.show();
		
		
		plz.model.griplz.Context context =new plz.model.griplz.Context();
		context.Mini = false;
		Context = context;
		
		if(Gdx.app.getType() == ApplicationType.Android)
		{
			((plz.model.griplz.Context)Context).Mini = false;
			((plz.model.griplz.Context)Context).selectionOffsetY = 0;
		}
		else
		{
			((plz.model.griplz.Context)Context).Mini = false;
		}
		
		Render = new plz.logic.render.griplz.RenderLogic((GameEngine)this);
		Controller = new plz.logic.controller.griplz.ControllerLogic((GameEngine)this);
		GamePlay = new plz.logic.gameplay.griplz.GamePlayLogic((GameEngine)this);
		
		//this.CurrentScreen = new GameScreen(this);
		CurrentScreen = new plz.logic.ui.screens.griplz.GameScreen((GameEngine)this);
		CurrentScreen.show();
		
	
		RegisterInput();
	}
}
