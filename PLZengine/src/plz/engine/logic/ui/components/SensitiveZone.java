package plz.engine.logic.ui.components;

import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.graphics.g2d.TextureRegion;
import com.badlogic.gdx.scenes.scene2d.Actor;
import com.badlogic.gdx.scenes.scene2d.actors.Button;

public class SensitiveZone extends Actor {

	public interface PressListener {
		public void pressed (SensitiveZone button);
	}
	
	public interface ReleaseListener {
		public void released (SensitiveZone button);
	}
	
	public interface EnterListener {
		public void onEnter (SensitiveZone button);
	}
	
	public interface LeaveListener {
		public void onLeave (SensitiveZone button);
	}
	
	public TextureRegion pressedRegion;
	public TextureRegion unpressedRegion;
	public boolean isPressed = false;
	protected int pointer = -1;
	
	public PressListener pressListener;
	public ReleaseListener releaseListener;
	public EnterListener enterListener;
	public LeaveListener leaveListener;
	public Object Tag;
	
	/**
	 * Creates a new Button instance with the given name.
	 * @param name the name
	 */
	public SensitiveZone (String name) {
		super(name);
		this.pressedRegion = new TextureRegion();
		this.unpressedRegion = new TextureRegion();
	}

	/**
	 * Creates a new Button instance with the given name, using the complete supplied texture for displaying the pressed and
	 * unpressed state of the button.
	 * @param name the name
	 * @param texture the {@link Texture}
	 */
	public SensitiveZone (String name, Texture texture) {
		super(name);
		originX = texture.getWidth() / 2.0f;
		originY = texture.getHeight() / 2.0f;
		width = texture.getWidth();
		height = texture.getHeight();
		pressedRegion = new TextureRegion(texture);
		unpressedRegion = new TextureRegion(texture);
	}

	public SensitiveZone (String name, TextureRegion region) {
		this(name, region, region);
	}

	public SensitiveZone (String name, TextureRegion unpressedRegion, TextureRegion pressedRegion) {
		super(name);
		width = Math.abs(unpressedRegion.getRegionWidth());
		height = Math.abs(unpressedRegion.getRegionHeight());
		originX = width / 2.0f;
		originY = height / 2.0f;
		this.unpressedRegion = new TextureRegion(unpressedRegion);
		this.pressedRegion = new TextureRegion(pressedRegion);
	}

	@Override protected void draw (SpriteBatch batch, float parentAlpha) {
		TextureRegion region = isPressed ? pressedRegion : unpressedRegion;
		batch.setColor(color.r, color.g, color.b, color.a * parentAlpha);
		if (region.getTexture() != null) {
			if (scaleX == 1 && scaleY == 1 && rotation == 0)
				batch.draw(region, x, y, width, height);
			else
				batch.draw(region, x, y, originX, originY, width, height, scaleX, scaleY, rotation);
		}
	}

	@Override protected boolean touchDown (float x, float y, int pointer) {
		if(isPressed) return false;

		boolean result = x > 0 && y > 0 && x < width && y < height;
		isPressed = result;
		
		if (isPressed) {
			parent.focus(this, pointer);
			this.pointer = pointer;
			
			if (pressListener != null) pressListener.pressed(this);
		}
		return result;
	}

	@Override protected boolean touchUp (float x, float y, int pointer) {
		if (!isPressed) return false;

		if(pointer == this.pointer) {
			parent.focus(null, pointer);
		}
		isPressed = false;
		if (releaseListener != null) releaseListener.released(this);
		return true;
	}

	@Override protected boolean touchDragged (float x, float y, int pointer) {
		return isPressed;
	}

	public Actor hit (float x, float y) {
		return x > 0 && y > 0 && x < width && y < height ? this : null;
	}

	public void layout () {
	}

	public void invalidate () {
	}

	public float getPrefWidth () {
		return unpressedRegion.getRegionWidth() * scaleX;
	}

	public float getPrefHeight () {
		return unpressedRegion.getRegionHeight() * scaleY;
	}
}