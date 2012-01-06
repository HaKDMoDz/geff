package plz.logic.ui.screens.liplz;

import plz.engine.GameEngineBase;
import plz.engine.logic.ui.screens.ScreenBase;
import plz.logic.gameplay.liplz.GamePlayLogic;
import plz.model.liplz.Context;

public class GameScreen extends ScreenBase
{
	public boolean NewTileSelected = false;

	public Context Context()
	{
		return (Context)gameEngine.Context;
	}
	
	private GamePlayLogic GamePlay()
	{
		return (GamePlayLogic) gameEngine.GamePlay;
	}

	public GameScreen(GameEngineBase gameEngine)
	{
		super(gameEngine);
	}

	@Override
	public void InitScreen()
	{
		this.screenName = "GameScreenLiPLZ";

	}

	@Override
	public void LoadScreen()
	{
		layout.layout();
	}

	@Override
	public void render(float delta)
	{
		super.render(delta);
	}
}
