package meeplz.logic.gameplay;

import meeplz.Context;
import meeplz.GameEngine;
import meeplz.model.*;

public class GamePlayLogic extends
		plz.engine.logic.gameplay.GamePlayLogicBase
{
	public GamePlayLogic(GameEngine gameEngine)
	{
		super(gameEngine);
		
		NewMap();
	}
	
	public void NewMap()
	{
		Context.Map = new Map(20,20);
		
	}
}
