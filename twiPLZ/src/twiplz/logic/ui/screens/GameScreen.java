package twiplz.logic.ui.screens;

import plz.engine.GameEngineBase;
import plz.engine.logic.ui.screens.ScreenBase;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.BitmapFont;
import com.badlogic.gdx.scenes.scene2d.actors.Button;
import com.badlogic.gdx.scenes.scene2d.actors.Image;
import com.badlogic.gdx.scenes.scene2d.actors.Label;
import com.badlogic.gdx.scenes.scene2d.actors.Button.ClickListener;
import com.esotericsoftware.tablelayout.libgdx.Table;
import com.esotericsoftware.tablelayout.libgdx.TableLayout;

public class GameScreen extends ScreenBase {
	public GameScreen(GameEngineBase gameEngine) {
		super(gameEngine);
	}

	@Override
	public void show() {
		super.show();


		Texture texture0 = new Texture(Gdx.files.internal("data/Turn1.png"));
		
		Button btnStart = new Button("btnStart", texture0);
		btnStart.clickListener = btnStartClicked;

		Image imgTurns[] = new Image[6];
		
		for (int i = 1; i < 7; i++)
		{
			Texture texture = new Texture(Gdx.files.internal("data/Turn" + i + ".png"));

			imgTurns[i-1] = new Image("imgTurn" + i, texture);
			//btnTurns[i].clickListener = btnStartClicked;

			Stage.addActor(imgTurns[i-1]);
		}

		Stage.addActor(btnStart);

		TableLayout.defaultFont = new BitmapFont();
		TableLayout layout = new Table().layout;
		Stage.addActor(layout.getTable());
		layout.getTable().width = Gdx.graphics.getWidth();
		layout.getTable().height = Gdx.graphics.getHeight();

		
		layout.register(btnStart);

		for (int i = 1; i < 7; i++)
		{
			layout.register(imgTurns[i-1]);
		}
		
		layout.parse(Gdx.files.internal("data/ui/GameScreen.ui").readString());
	}

	Button.ClickListener btnStartClicked = new ClickListener() {
		@Override
		public void clicked(Button button) {
			GameScreen.this.gameEngine.CurrentScreen = new MainMenu(
					GameScreen.this.gameEngine);
			GameScreen.this.gameEngine.CurrentScreen.show();
		}
	};
}
