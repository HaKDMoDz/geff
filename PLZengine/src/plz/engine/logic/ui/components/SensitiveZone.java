package plz.engine.logic.ui.components;

import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.graphics.g2d.TextureRegion;
import com.badlogic.gdx.scenes.scene2d.Actor;

public class SensitiveZone extends Actor {
	public TextureRegion region;

	public SensitiveZone (String name) {
		super(name);
		this.region = new TextureRegion();
	}

	public SensitiveZone (String name, Texture texture) {
		super(name);
		this.originX = texture.getWidth() / 2.0f;
		this.originY = texture.getHeight() / 2.0f;
		this.width = texture.getWidth();
		this.height = texture.getHeight();
		this.region = new TextureRegion(texture);
	}

	public SensitiveZone (String name, TextureRegion region) {
		super(name);
		width = Math.abs(region.getRegionWidth());
		height = Math.abs(region.getRegionHeight());
		originX = width / 2.0f;
		originY = height / 2.0f;
		this.region = new TextureRegion(region);
	}

	@Override protected void draw (SpriteBatch batch, float parentAlpha) {
		if (region.getTexture() != null) {
			batch.setColor(color.r, color.g, color.b, color.a * parentAlpha);
			if (scaleX == 0 && scaleY == 0 && rotation == 0)
				batch.draw(region, x, y, width, height);
			else
				batch.draw(region, x, y, originX, originY, width, height, scaleX, scaleY, rotation);
		}
	}

	@Override protected boolean touchDown (float x, float y, int pointer) {
		return x > 0 && y > 0 && x < width && y < height;
	}

	@Override protected boolean touchUp (float x, float y, int pointer) {
		return false;
	}

	@Override protected boolean touchDragged (float x, float y, int pointer) {
		return false;
	}

	public Actor hit (float x, float y) {
		if (x > 0 && x < width) if (y > 0 && y < height) return this;

		return null;
	}
}