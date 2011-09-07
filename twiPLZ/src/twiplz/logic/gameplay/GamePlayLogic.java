package twiplz.logic.gameplay;

import twiplz.Context;
import twiplz.GameEngine;
import twiplz.model.*;

public class GamePlayLogic extends
		plz.engine.logic.gameplay.GamePlayLogicBase
{
	public Cell NewCell;
	
	public GamePlayLogic(GameEngine gameEngine)
	{
		super(gameEngine);
		
		NewMap();
	}
	
	public void NewMap()
	{
		Context.Map = new Map(20,20);
		
	}
	
	public void CreateNewCell()
	{
	
	}
	
	public void TurnNewCell(int orientation)
	{
	
	}
}
