package twiplz.logic.ui.screens;

import java.io.FileReader;
import java.util.Scanner;

import plz.engine.GameEngineBase;
import plz.engine.logic.ui.screens.ScreenBase;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.BitmapFont;
import com.badlogic.gdx.scenes.scene2d.actors.Button;
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

		/*
		Texture texture = new Texture(Gdx.files.internal("data/badlogic.jpg"));

		Button btnStart = new Button("btnStart", texture);
		btnStart.clickListener = btnStartClicked;

		Button btnTurn = new Button("btnTurn", texture);
		btnTurn.clickListener = btnStartClicked;

		Stage.addActor(btnTurn);
		Stage.addActor(btnStart);
*/
		TableLayout.defaultFont = new BitmapFont();
		TableLayout layout = new Table().layout;
		Stage.addActor(layout.getTable());
		layout.getTable().width = Gdx.graphics.getWidth();
		layout.getTable().height = Gdx.graphics.getHeight();

		/*
		layout.register(btnTurn);
		layout.register(btnStart);
*/
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
