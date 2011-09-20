package plz.engine.logic.controller;

import java.util.Date;

import com.badlogic.gdx.math.Vector2;

public class Pointer
{
	public Vector2 Start = null;
	public Vector2 Current = null;
	public Date CreationDate = null;
	public PointerUsage Usage = PointerUsage.None;
	public int Index = -1;
	
	public Pointer(int x, int y, int index)
	{
		Init(x,y);
		this.Index = index;
	}
	
	public void SwapStartToCurrent()
	{
		if (Start != null)
			Start = Current;
	}
	
	public void Init(int x, int y)
	{
		this.Start = new Vector2(x, y);
		this.Current = null;
		this.CreationDate = new Date();
	}
}
