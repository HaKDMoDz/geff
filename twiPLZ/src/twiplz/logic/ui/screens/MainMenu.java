package twiplz.logic.ui.screens;

import plz.engine.GameEngineBase;
import plz.engine.logic.ui.screens.ScreenBase;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.scenes.scene2d.ui.Button;
import com.badlogic.gdx.scenes.scene2d.ui.Button.ButtonStyle;

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

//		Button btnStart = new Button("Start", );
//		btnStart.x = 120;
//		btnStart.y = 0;
//		btnStart.height = 50;
//		btnStart.width = 150;
//		btnStart.setClickListener(btnStartClicked);
//
//		Stage.addActor(btnStart);

		//Stage.getRoot().action(Forever.$(Sequence.$(FadeOut.$(3), FadeIn.$(3))));
	}
}
