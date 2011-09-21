package twiplz;

import plz.engine.logic.controller.PLZInputMultiplexer;

import com.badlogic.gdx.Gdx;
import twiplz.logic.controller.ControllerLogic;
import twiplz.logic.gameplay.GamePlayLogic;
import twiplz.logic.render.RenderLogic;
import twiplz.logic.ui.screens.*;

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
