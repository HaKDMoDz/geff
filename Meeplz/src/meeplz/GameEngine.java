package meeplz;

import meeplz.logic.controller.ControllerLogic;
import meeplz.logic.gameplay.GamePlayLogic;
import meeplz.logic.render.RenderLogic;
import meeplz.logic.ui.screens.MainMenu;

public class GameEngine extends meepleengine.GameEngine
{
	public GameEngine()
	{
	}
	
	@Override
	public void create()
	{
		this.Controller = new ControllerLogic(this);
		this.Render = new RenderLogic(this);
		this.GamePlay = new GamePlayLogic(this);
		
		this.CurrentScreen = new MainMenu(this);
		this.CurrentScreen.show();
	}
}
