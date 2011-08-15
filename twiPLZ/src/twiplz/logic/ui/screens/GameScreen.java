package twiplz.logic.ui.screens;

import plz.engine.GameEngineBase;
import plz.engine.logic.ui.components.SensitiveZone;
import plz.engine.logic.ui.screens.ScreenBase;
import twiplz.logic.gameplay.GamePlayLogic;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.BitmapFont;
import com.badlogic.gdx.scenes.scene2d.actors.Button;
import com.badlogic.gdx.scenes.scene2d.actors.Image;
import com.badlogic.gdx.scenes.scene2d.actors.Label;
import com.badlogic.gdx.scenes.scene2d.actors.Button.ClickListener;
import com.esotericsoftware.tablelayout.libgdx.Table;
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
	public void show()
	{
		super.show();

		Texture texture0 = new Texture(Gdx.files.internal("data/Turn1.png"));

		SensitiveZone imgNewCell = new SensitiveZone("btnStart", texture0);
		imgNewCell.pressListener = NewCell_Selected;

		SensitiveZone imgTurns[] = new SensitiveZone[6];

		for (int i = 1; i < 7; i++)
		{
			Texture texture = new Texture(Gdx.files.internal("data/Turn" + i
					+ ".png"));

			imgTurns[i - 1] = new SensitiveZone("imgTurn" + i, texture);
			imgTurns[i - 1].Tag = i - 1;
			imgTurns[i - 1].enterListener = TurnNewCell_Enter;

			Stage.addActor(imgTurns[i - 1]);
		}

		Stage.addActor(imgNewCell);

		TableLayout.defaultFont = new BitmapFont();
		TableLayout layout = new Table().layout;
		Stage.addActor(layout.getTable());
		layout.getTable().width = Gdx.graphics.getWidth();
		layout.getTable().height = Gdx.graphics.getHeight();

		layout.register(imgNewCell);

		for (int i = 1; i < 7; i++)
		{
			layout.register(imgTurns[i - 1]);
		}

		layout.parse(Gdx.files.internal("data/ui/GameScreen.ui").readString());
	}

	SensitiveZone.EnterListener TurnNewCell_Enter = new SensitiveZone.EnterListener()
	{
		@Override
		public void onEnter(SensitiveZone button)
		{
			GamePlay().TurnNewCell((int)button.Tag);
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
