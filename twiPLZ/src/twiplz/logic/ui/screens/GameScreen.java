package twiplz.logic.ui.screens;

import plz.engine.GameEngineBase;
import plz.engine.logic.controller.PointerUsage;
import plz.engine.logic.ui.components.SensitiveZone;
import plz.engine.logic.ui.screens.ScreenBase;
import twiplz.Context;
import twiplz.logic.gameplay.GamePlayLogic;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Texture;

public class GameScreen extends ScreenBase
{
	public SensitiveZone imgNewTile;
	//private SensitiveZone imgCenter;
	SensitiveZone rightBar;
	SensitiveZone leftBar;
	SensitiveZone imgTurn;
	//SensitiveZone imgTurns[];
	
	public boolean NewTileSelected=false;

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
		this.screenName = "GameScreen";

		//Texture texEmptyBlack = new Texture(Gdx.files.internal("data/EmptyBlack.png"));
		Texture texTurnTile = new Texture(Gdx.files.internal("data/Turn.png"));

		rightBar = new SensitiveZone("rightBar", texTurnTile);
		//leftBar = new SensitiveZone("leftBar", texTurnTile);
		
		// --- Create SensitiveZone
		imgNewTile = AddSensitiveZone("btnNewTile");
		imgNewTile.visible=false;
		imgNewTile.pressListener = NewCell_Pressed;
		imgNewTile.releaseListener = NewCell_Released;
		//imgNewTile.enterListener = NewCell_Enter;
		//imgNewTile.leaveListener = NewCell_Leave;

		//imgCenter = AddSensitiveZone("imgCenter");
		
		imgTurn = AddSensitiveZone("imgTurn", texTurnTile);
		imgTurn.pressListener = TurnNewCell_Pressed;
		imgTurn.releaseListener = TurnNewCell_Released;
		imgTurn.dragListener = TrunNewCell_Dragged;
		
		//imgCenter.Controller = gameEngine.Controller;

		//imgTurns = new SensitiveZone[6];

//		for (int i = 1; i < 7; i++)
//		{
//			Texture texture = new Texture(Gdx.files.internal("data/Turn" + i + ".png"));
//
//			imgTurns[i - 1] = AddSensitiveZone("imgTurn" + i, texture);
//			imgTurns[i - 1].Tag = i - 1;
//			imgTurns[i - 1].visible=true;
//			//imgTurns[i - 1].enterListener = TurnNewCell_Enter;
//			imgTurns[i - 1].pressListener = TurnNewCell_Pressed;
//			imgTurns[i - 1].releaseListener = TurnNewCell_Released;
//			imgTurns[i - 1].dragListener = TrunNewCell_Dragged;
//		}
		// ---
	}

	@Override
	public void LoadScreen()
	{
		//GameScreen.this.Stage.getRoot().scrollFocus(imgCenter);
		layout.layout();
			
		rightBar.x = imgNewTile.AbsoluteLocation().x;
		rightBar.width = imgNewTile.width;
		rightBar.height = imgNewTile.parent.height;
		
//		leftBar.x = imgTurn.AbsoluteLocation().x;
//		leftBar.width = imgTurn.width;
//		leftBar.height = imgTurn.parent.height;

		GamePlay().CreateNewTile();
	}

	@Override
	public void render(float delta) {
		super.render(delta);
		
		gameEngine.Render.spriteBatch.begin();
		rightBar.draw(gameEngine.Render.spriteBatch,1f);
		//leftBar.draw(gameEngine.Render.spriteBatch, 0.5f);
		gameEngine.Render.spriteBatch.end();
	}
	
	SensitiveZone.PressListener TurnNewCell_Pressed = new SensitiveZone.PressListener()
	{
		@Override
		public void pressed(SensitiveZone button, float x, float y, int pointer)
		{
			Context.pointers[pointer].Usage = PointerUsage.ButtonTurnTile;
			Context.pointers[pointer].Handled = true;
		}
	};
	
	SensitiveZone.ReleaseListener TurnNewCell_Released = new SensitiveZone.ReleaseListener()
	{
		@Override
		public void released(SensitiveZone button, int pointer, boolean isOnButton)
		{
			Context.pointers[pointer].Usage = PointerUsage.None;
			Context.pointers[pointer].Handled = true;
		}
	};
	
	SensitiveZone.DragListener TrunNewCell_Dragged = new SensitiveZone.DragListener()
	{
		@Override
		public void dragged(SensitiveZone button, float x, float y, int pointer)
		{
			Context.pointers[pointer].Usage = PointerUsage.ButtonTurnTile;
			Context.pointers[pointer].Handled = true;
			
			GamePlay().TurnTile((int)(y/button.height*6));
		}
	};
	
	SensitiveZone.PressListener NewCell_Pressed = new SensitiveZone.PressListener()
	{
		@Override
		public void pressed(SensitiveZone button, float x, float y, int pointer)
		{
			Context.pointers[pointer].Usage = PointerUsage.SelectTile;
			Context.pointers[pointer].Handled = true;
			GamePlay().SelectTile();
		}
	};
	
	SensitiveZone.ReleaseListener NewCell_Released = new SensitiveZone.ReleaseListener()
	{
		@Override
		public void released(SensitiveZone button, int pointer, boolean isOnButton)
		{
			if(isOnButton)
			{
				Context.pointers[pointer].Usage = PointerUsage.UnselectTile;
				Context.pointers[pointer].Handled = true;
			}
		}
	};
	
//	SensitiveZone.EnterListener TurnNewCell_Enter = new SensitiveZone.EnterListener()
//	{
//		@Override
//		public void onEnter(SensitiveZone button)
//		{
//			GamePlay().TurnTile((Integer) button.Tag);
//		}
//	};
//
//	SensitiveZone.EnterListener NewCell_Enter = new SensitiveZone.EnterListener()
//	{
//		@Override
//		public void onEnter(SensitiveZone button)
//		{
//			GameScreen.this.NewTileSelected = true;
//		}
//	};
//	
//	SensitiveZone.LeaveListener NewCell_Leave = new SensitiveZone.LeaveListener()
//	{
//		@Override
//		public void onLeave(SensitiveZone button)
//		{
//			GameScreen.this.NewTileSelected = false;
//		}
//	};
	

}
