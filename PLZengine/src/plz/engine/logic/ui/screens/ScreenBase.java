package plz.engine.logic.ui.screens;

import plz.engine.GameEngineBase;
import plz.engine.logic.ui.components.SensitiveZone;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Screen;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.Texture.TextureFilter;
import com.badlogic.gdx.graphics.g2d.BitmapFont;
import com.esotericsoftware.tablelayout.libgdx.LibgdxToolkit;
import com.esotericsoftware.tablelayout.libgdx.Table;
import com.esotericsoftware.tablelayout.libgdx.TableLayout;

public class ScreenBase implements Screen {
	public com.badlogic.gdx.scenes.scene2d.Stage Stage;
	protected GameEngineBase gameEngine;
	protected TableLayout layout;
	protected String screenName;

	public ScreenBase(GameEngineBase gameEngine) {
		this.gameEngine = gameEngine;
	}

	@Override
	public void render(float delta) {
		Stage.act(delta);
		Stage.draw();
	}

	@Override
	public void resize(int width, int height) {
		// TODO Auto-generated method stub

	}

	@Override
	public void show() {
		Stage = new com.badlogic.gdx.scenes.scene2d.Stage(
				Gdx.graphics.getWidth(), Gdx.graphics.getHeight(), false);

		Gdx.input.setInputProcessor(Stage);

		// ---
		LibgdxToolkit.defaultFont = new BitmapFont();
		BitmapFont largeFont = new BitmapFont();
		largeFont.getRegion().getTexture()
				.setFilter(TextureFilter.Linear, TextureFilter.Linear);
		largeFont.scale(1);
		LibgdxToolkit.registerFont("large", largeFont);

		Table table = new Table();
		Stage.addActor(table);
		table.width = Gdx.graphics.getWidth();
		table.height = Gdx.graphics.getHeight();

		layout = table.getTableLayout();

		InitScreen();

		LoadScreenFile();
	}

	public void InitScreen() {

	}

	@Override
	public void hide() {
		// TODO Auto-generated method stub

	}

	@Override
	public void pause() {
		// TODO Auto-generated method stub

	}

	@Override
	public void resume() {
		// TODO Auto-generated method stub

	}

	@Override
	public void dispose() {
		// TODO Auto-generated method stub

	}

	protected SensitiveZone AddSensitiveZone(String name, Texture texture) {
		SensitiveZone sensitiveZone;

		if (texture != null) {
			sensitiveZone = new SensitiveZone(name, texture,
					gameEngine.Controller);
		} else {
			sensitiveZone = new SensitiveZone(name,
					gameEngine.Controller);
		}

		Stage.addActor(sensitiveZone);
		layout.register(sensitiveZone);

		return sensitiveZone;
	}

	protected SensitiveZone AddSensitiveZone(String name) {

		return AddSensitiveZone(name, null);
	}

	private void LoadScreenFile() {

		layout.parse(Gdx.files.internal("data/ui/" + screenName + ".ui")
				.readString());
	}
}