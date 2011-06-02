package meeplz;

import meeplz.logic.controller.ControllerLogic;
import meeplz.logic.gameplay.GamePlayLogic;
import meeplz.logic.render.RenderLogic;
import meeplz.logic.ui.screens.MainMenu;

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
		
		this.CurrentScreen = new MainMenu(this);
		this.CurrentScreen.show();
	}
}
