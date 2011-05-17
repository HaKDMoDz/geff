package meeplz.logic.ui.screens;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.scenes.scene2d.actions.FadeIn;
import com.badlogic.gdx.scenes.scene2d.actions.FadeOut;
import com.badlogic.gdx.scenes.scene2d.actions.Forever;
import com.badlogic.gdx.scenes.scene2d.actions.Sequence;
import com.badlogic.gdx.scenes.scene2d.actors.Button;
import com.badlogic.gdx.scenes.scene2d.actors.Button.ClickListener;

import meepleengine.GameEngine;
import meepleengine.logic.ui.screens.ScreenBase;

public class GameScreen extends ScreenBase
{
	public GameScreen(GameEngine gameEngine)
	{
		super(gameEngine);
	}

	@Override
	public void show()
	{
		super.show();
		
		Texture texture = new Texture(Gdx.files.internal("data/badlogic.jpg"));
		
		Button btnStart = new Button("Start", texture);
		btnStart.x = 120;
		btnStart.y = 150;
		btnStart.height = 50;
		btnStart.width = 150;
		btnStart.clickListener = btnStartClicked;

		Stage.addActor(btnStart);
	}
	
	Button.ClickListener btnStartClicked = new ClickListener()
	{
		@Override
		public void clicked(Button button)
		{
			GameScreen.this.gameEngine.CurrentScreen = new MainMenu(GameScreen.this.gameEngine);
			GameScreen.this.gameEngine.CurrentScreen.show();
		}
	};
}