package plz.engine.logic.controller;

import java.util.Date;

import com.badlogic.gdx.math.Vector2;

public class Pointer
{
	public Vector2 Start = null;
	public Vector2 Current = null;
	public Date CreationDate = null;
	public int Index = -1;
	
	public Pointer(int x, int y, int index)
	{
		this.Start = new Vector2(x, y);
		this.Index = index;
		this.CreationDate = new Date();
	}
	
	public void SwapStartToCurrent()
	{
		if (Start != null)
			Start = Current;
	}
}
