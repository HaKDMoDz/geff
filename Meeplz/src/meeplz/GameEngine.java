package meeplz;

import meeplz.logic.controller.ControllerLogic;
import meeplz.logic.gameplay.GamePlayLogic;
import meeplz.logic.render.RenderLogic;

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
	}
}
