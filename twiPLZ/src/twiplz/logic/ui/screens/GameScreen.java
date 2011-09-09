package twiplz.logic.ui.screens;

import plz.engine.GameEngineBase;
import plz.engine.logic.ui.components.SensitiveZone;
import plz.engine.logic.ui.screens.ScreenBase;
import twiplz.logic.gameplay.GamePlayLogic;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.Texture.TextureFilter;
import com.badlogic.gdx.graphics.g2d.BitmapFont;
import com.badlogic.gdx.scenes.scene2d.actors.Label;
import com.esotericsoftware.tablelayout.libgdx.LibgdxToolkit;
import com.esotericsoftware.tablelayout.libgdx.TableLayout;

public class GameScreen extends ScreenBase
{
	private GamePlayLogic GamePlay()
	{
		return (GamePlayLogic)gameEngine.GamePlay;
	}
	
	public GameScreen(GameEngineBase gameEngine)
	{
		super(gameEngine);
	}

	@Override
	public void InitScreen()
	{
		this.screenName = "GameScreen";
		
		Texture texture0 = new Texture(Gdx.files.internal("data/Turn1.png"));

		//--- Create SensitiveZone
		SensitiveZone imgNewCell = AddSensitiveZone("btnStart", texture0);
		imgNewCell.pressListener = NewCell_Selected;

		SensitiveZone imgCenter = AddSensitiveZone("imgCenter");
		
		SensitiveZone imgTurns[] = new SensitiveZone[6];

		for (int i = 1; i < 7; i++)
		{
			Texture texture = new Texture(Gdx.files.internal("data/Turn" + i
					+ ".png"));

			imgTurns[i - 1] = AddSensitiveZone("imgTurn" + i, texture);
			imgTurns[i - 1].Tag = i - 1;
			imgTurns[i - 1].enterListener = TurnNewCell_Enter;
		}
		//---
	}

	SensitiveZone.EnterListener TurnNewCell_Enter = new SensitiveZone.EnterListener()
	{
		@Override
		public void onEnter(SensitiveZone button)
		{
			GamePlay().TurnNewCell((Integer)button.Tag);
		}
	};

	SensitiveZone.PressListener NewCell_Selected = new SensitiveZone.PressListener()
	{
		@Override
		public void pressed(SensitiveZone button)
		{
			GamePlay().CreateNewCell();
		}
	};
}
