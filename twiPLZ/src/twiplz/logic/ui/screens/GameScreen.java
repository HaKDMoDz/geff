package twiplz.logic.ui.screens;

import plz.engine.GameEngineBase;
import plz.engine.logic.ui.screens.ScreenBase;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.scenes.scene2d.actors.Button;
import com.badlogic.gdx.scenes.scene2d.actors.Button.ClickListener;


public class GameScreen extends ScreenBase
{
	public GameScreen(GameEngineBase gameEngine)
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
