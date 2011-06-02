package meeplz.logic.ui.screens;

import plz.engine.GameEngineBase;
import plz.engine.logic.ui.screens.ScreenBase;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.scenes.scene2d.actors.Button;
import com.badlogic.gdx.scenes.scene2d.actors.Button.ClickListener;


public class MainMenu extends ScreenBase
{
	public MainMenu(GameEngineBase gameEngine)
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
		btnStart.y = 0;
		btnStart.height = 50;
		btnStart.width = 150;
		btnStart.clickListener = btnStartClicked;

		Stage.addActor(btnStart);

		//Stage.getRoot().action(Forever.$(Sequence.$(FadeOut.$(3), FadeIn.$(3))));
	}

	Button.ClickListener btnStartClicked = new ClickListener()
	{
		@Override
		public void clicked(Button button)
		{
			gameEngine.CurrentScreen = new GameScreen(gameEngine);
			gameEngine.CurrentScreen.show();
		}
	};
}
