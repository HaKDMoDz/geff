package twiplz.logic.ui.screens;

import plz.engine.GameEngineBase;
import plz.engine.logic.controller.PointerUsage;
import plz.engine.logic.ui.components.SensitiveZone;
import plz.engine.logic.ui.screens.ScreenBase;
import twiplz.Context;
import twiplz.logic.gameplay.GamePlayLogic;
import twiplz.logic.render.RenderLogic;
import twiplz.model.GameState;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.scenes.scene2d.Actor;
import com.badlogic.gdx.scenes.scene2d.ui.Button;
import com.badlogic.gdx.scenes.scene2d.ui.Label;

public class GameScreen extends ScreenBase
{
	public SensitiveZone[] imgNewTile;
	SensitiveZone rightBar;
	SensitiveZone leftBar;
	SensitiveZone imgTurn;
	int tileOrientationOnPress = 0;

	public boolean NewTileSelected = false;

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

		Texture texTurnTile = new Texture(Gdx.files.internal("data/Turn.png"));

		rightBar = new SensitiveZone("rightBar", texTurnTile);

		imgNewTile = new SensitiveZone[4];
		
		// --- Create SensitiveZone
		imgNewTile[0] = AddSensitiveZone("btnNewTile1");
		imgNewTile[0].visible = false;
		imgNewTile[0].pressListener = NewCell_Pressed;
		imgNewTile[0].releaseListener = NewCell_Released;
		imgNewTile[0].Tag = 0;
		
		// --- Create SensitiveZone
		imgNewTile[1] = AddSensitiveZone("btnNewTile2");
		imgNewTile[1].visible = false;
		imgNewTile[1].pressListener = NewCell_Pressed;
		imgNewTile[1].releaseListener = NewCell_Released;
		imgNewTile[1].Tag = 1;
		
		// --- Create SensitiveZone
		imgNewTile[2] = AddSensitiveZone("btnNewTile3");
		imgNewTile[2].visible = false;
		imgNewTile[2].pressListener = NewCell_Pressed;
		imgNewTile[2].releaseListener = NewCell_Released;
		imgNewTile[2].Tag = 2;

		// --- Create SensitiveZone
		imgNewTile[3] = AddSensitiveZone("btnSwapTile");
		imgNewTile[3].visible = false;
		imgNewTile[3].pressListener = NewCell_Pressed;
		imgNewTile[3].releaseListener = NewCell_Released;
		imgNewTile[3].Tag = 3;
		
		imgTurn = AddSensitiveZone("imgTurn", texTurnTile);
		//imgTurn = AddSensitiveZone("imgTurn");
		imgTurn.pressListener = TurnNewCell_Pressed;
		imgTurn.releaseListener = TurnNewCell_Released;
		imgTurn.dragListener = TrunNewCell_Dragged;

		AddSensitiveZone("Score");
	}

	@Override
	public void LoadScreen()
	{
		layout.layout();

		rightBar.x = imgNewTile[0].AbsoluteLocation().x;
		rightBar.width = imgNewTile[0].width;
		rightBar.height = imgNewTile[0].parent.height;

		GamePlay().CreateNewTile(0);
		GamePlay().CreateNewTile(1);
		GamePlay().CreateNewTile(2);
		GamePlay().CreateNewTile(3);
	}

	@Override
	public void render(float delta)
	{
		super.render(delta);

		Actor actorScore = Stage.findActor("Score");

		gameEngine.Render.spriteBatch.begin();
		rightBar.draw(gameEngine.Render.spriteBatch, 1f);
		
		if(Context.Combo>0)
			((RenderLogic)gameEngine.Render).fontScore.draw(gameEngine.Render.spriteBatch, " Score : " + Context.Score + " + " + Context.AddedScore + " // Combo : " + Context.Combo, actorScore.parent.x+actorScore.x, actorScore.parent.y+actorScore.y);
		else
			((RenderLogic)gameEngine.Render).fontScore.draw(gameEngine.Render.spriteBatch, " Score : " + Context.Score, actorScore.parent.x+actorScore.x, actorScore.parent.y+actorScore.y);
		
		if(Context.gameStateTime.GameState == GameState.BonusClearScreen)
		{
			((RenderLogic)gameEngine.Render).fontBonus.draw(gameEngine.Render.spriteBatch, "Bravo!!!", actorScore.parent.x+actorScore.x, actorScore.parent.y+actorScore.y-20);
		}
		
		gameEngine.Render.spriteBatch.end();
	}

	SensitiveZone.PressListener TurnNewCell_Pressed = new SensitiveZone.PressListener()
	{
		@Override
		public void pressed(SensitiveZone button, float x, float y, int pointer)
		{
			GameScreen.this.tileOrientationOnPress = GamePlay().CurrentOrientation;

			Context.pointers[pointer].Usage = PointerUsage.ButtonTurnTile;
			Context.pointers[pointer].Handled = true;
		}
	};

	SensitiveZone.ReleaseListener TurnNewCell_Released = new SensitiveZone.ReleaseListener()
	{
		@Override
		public void released(SensitiveZone button, int pointer,
				boolean isOnButton)
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

			if (Context.pointers[pointer].Current != null)
			{
				float offsetY = -Context.pointers[pointer].Start.y
						+ Context.pointers[pointer].Current.y;

				// GamePlay().TurnTile((int)(y/button.height*6));

				GamePlay().TurnTile(
						(int) ((float) tileOrientationOnPress - offsetY / 60f));
			}
		}
	};

	SensitiveZone.PressListener NewCell_Pressed = new SensitiveZone.PressListener()
	{
		@Override
		public void pressed(SensitiveZone button, float x, float y, int pointer)
		{
			Context.pointers[pointer].Usage = PointerUsage.SelectTile;
			Context.pointers[pointer].Handled = true;
			GamePlay().SelectTile((Integer)button.Tag);
		}
	};

	SensitiveZone.ReleaseListener NewCell_Released = new SensitiveZone.ReleaseListener()
	{
		@Override
		public void released(SensitiveZone button, int pointer,
				boolean isOnButton)
		{
			if (isOnButton)
			{
				Context.pointers[pointer].Usage = PointerUsage.UnselectTile;
				Context.pointers[pointer].Handled = true;
			}
		}
	};
}
