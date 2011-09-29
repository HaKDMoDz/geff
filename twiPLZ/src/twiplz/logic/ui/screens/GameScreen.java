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

		// --- Create SensitiveZone
		imgNewTile = AddSensitiveZone("btnNewTile");
		imgNewTile.visible = false;
		imgNewTile.pressListener = NewCell_Pressed;
		imgNewTile.releaseListener = NewCell_Released;

		imgTurn = AddSensitiveZone("imgTurn", texTurnTile);
		imgTurn.pressListener = TurnNewCell_Pressed;
		imgTurn.releaseListener = TurnNewCell_Released;
		imgTurn.dragListener = TrunNewCell_Dragged;
	}

	@Override
	public void LoadScreen()
	{
		layout.layout();

		rightBar.x = imgNewTile.AbsoluteLocation().x;
		rightBar.width = imgNewTile.width;
		rightBar.height = imgNewTile.parent.height;

		GamePlay().CreateNewTile();
	}

	@Override
	public void render(float delta)
	{
		super.render(delta);

		gameEngine.Render.spriteBatch.begin();
		rightBar.draw(gameEngine.Render.spriteBatch, 1f);
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
			GamePlay().SelectTile();
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

	// SensitiveZone.EnterListener TurnNewCell_Enter = new
	// SensitiveZone.EnterListener()
	// {
	// @Override
	// public void onEnter(SensitiveZone button)
	// {
	// GamePlay().TurnTile((Integer) button.Tag);
	// }
	// };
	//
	// SensitiveZone.EnterListener NewCell_Enter = new
	// SensitiveZone.EnterListener()
	// {
	// @Override
	// public void onEnter(SensitiveZone button)
	// {
	// GameScreen.this.NewTileSelected = true;
	// }
	// };
	//
	// SensitiveZone.LeaveListener NewCell_Leave = new
	// SensitiveZone.LeaveListener()
	// {
	// @Override
	// public void onLeave(SensitiveZone button)
	// {
	// GameScreen.this.NewTileSelected = false;
	// }
	// };

}
