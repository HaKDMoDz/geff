package plz.engine.logic.ui.components;

import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.graphics.g2d.TextureRegion;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.scenes.scene2d.Actor;

public class SensitiveZone extends Actor
{
	public Color color = new Color(1, 1, 1, 1);
	//public InputProcessor Controller;
	
	public Vector2 AbsoluteLocation()
	{
		return new Vector2(this.x + this.parent.x, this.y + this.parent.y);
	}

	public interface PressListener
	{
		public void pressed(SensitiveZone button, float x, float y, int pointer);
	}

	public interface ReleaseListener
	{
		public void released(SensitiveZone button, int pointer, boolean isOnButton);
	}
	
	public interface DragListener
	{
		public void dragged(SensitiveZone button, float x, float y, int pointer);
	}

//	public interface EnterListener
//	{
//		public void onEnter(SensitiveZone button, float x, float y, int pointer);
//	}
//
//	public interface LeaveListener
//	{
//		public void onLeave(SensitiveZone button, int pointer);
//	}

	public TextureRegion pressedRegion;
	public TextureRegion unpressedRegion;
	public boolean isPressed = false;
	public boolean isInside = false;
	protected int pointer = -1;

	public PressListener pressListener;
	public ReleaseListener releaseListener;
	public DragListener dragListener;
	//public EnterListener enterListener;
	//public LeaveListener leaveListener;
	public Object Tag;

	/**
	 * Creates a new Button instance with the given name.
	 * 
	 * @param name
	 *            the name
	 */
	public SensitiveZone(String name)
	{
		super(name);
		this.pressedRegion = new TextureRegion();
		this.unpressedRegion = new TextureRegion();
	}

	/**
	 * Creates a new Button instance with the given name, using the complete
	 * supplied texture for displaying the pressed and unpressed state of the
	 * button.
	 * 
	 * @param name
	 *            the name
	 * @param texture
	 *            the {@link Texture}
	 */
	public SensitiveZone(String name, Texture texture)
	{
		super(name);
		originX = texture.getWidth() / 2.0f;
		originY = texture.getHeight() / 2.0f;
		width = texture.getWidth();
		height = texture.getHeight();
		pressedRegion = new TextureRegion(texture);
		unpressedRegion = new TextureRegion(texture);
	}

	public SensitiveZone(String name, TextureRegion region)
	{
		this(name, region, region);
	}

	public SensitiveZone(String name, TextureRegion unpressedRegion, TextureRegion pressedRegion)
	{
		super(name);
		width = Math.abs(unpressedRegion.getRegionWidth());
		height = Math.abs(unpressedRegion.getRegionHeight());
		originX = width / 2.0f;
		originY = height / 2.0f;
		this.unpressedRegion = new TextureRegion(unpressedRegion);
		this.pressedRegion = new TextureRegion(pressedRegion);
	}

	@Override
	public void draw(SpriteBatch batch, float parentAlpha)
	{
		TextureRegion region = isPressed ? pressedRegion : unpressedRegion;
		batch.setColor(color.r, color.g, color.b, color.a * parentAlpha);
		if (region.getTexture() != null)
		{
			if (scaleX == 1 && scaleY == 1 && rotation == 0)
				batch.draw(region, x, y, width, height);
			else
				batch.draw(region, x, y, originX, originY, width, height, scaleX, scaleY, rotation);
		}
	}

	@Override
	public boolean touchDown(float x, float y, int pointer)
	{
		if (isPressed)
			return false;

		boolean result = x > 0 && y > 0 && x < width && y < height;
		isPressed = result;

		if (isPressed)
		{
			parent.focus(this, pointer);
			this.pointer = pointer;

			if (pressListener != null)
				pressListener.pressed(this, x, y, pointer);
		}
		
//		if(Controller != null)
//			Controller.touchDown((int)x, (int)y, pointer, 0);
		
		return false;
	}

	@Override
	public void touchUp(float x, float y, int pointer)
	{
		if (pointer == this.pointer)
		{
			parent.focus(null, pointer);
		}
		isPressed = false;
		
		if (releaseListener != null)
			releaseListener.released(this, pointer, hit(x,y) != null);
		
//		if(Controller != null)
//			Controller.touchUp((int)x, (int)y, pointer, 0);
	}

	@Override
	public void touchDragged(float x, float y, int pointer)
	{
		if(hit(x,y) == null)
			return;
		
		if (dragListener != null)
			dragListener.dragged(this, x, y, pointer);
	}
	
	@Override
	public boolean touchMoved(float x, float y)
	{
		boolean result = x > 0 && y > 0 && x < width && y < height;
		
//		if(result && !isInside)
//		{
//			isInside = true;
//			if(enterListener != null)
//				enterListener.onEnter(this);
//		}
//		else if(!result && isInside)
//		{
//			isInside = false;
//			if(leaveListener != null)
//				leaveListener.onLeave(this);
//		}
		
//		if(Controller != null)
//			Controller.touchDragged((int)x, (int)y, pointer);
		
		return result;
	}

	@Override
	public boolean scrolled(int amount)
	{
		return false;
	}

	public Actor hit(float x, float y)
	{
		Actor ret = x > 0 && y > 0 && x < width && y < height ? this : null;
		
//		if(ret!=null && !isInside)
//		{
//			isInside = true;
//			if(enterListener != null)
//				enterListener.onEnter(this);
//		}
//		else if(ret == null && isInside)
//		{
//			isInside = false;
//			if(leaveListener != null)
//				leaveListener.onLeave(this);
//		}
		
		return ret;
	}

	public void layout()
	{
	}

	public void invalidate()
	{
	}

	public float getPrefWidth()
	{
		return unpressedRegion.getRegionWidth() * scaleX;
	}

	public float getPrefHeight()
	{
		return unpressedRegion.getRegionHeight() * scaleY;
	}
}