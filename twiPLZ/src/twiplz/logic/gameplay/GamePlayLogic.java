package twiplz.logic.gameplay;

import twiplz.Context;
import twiplz.GameEngine;
import twiplz.model.*;

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
