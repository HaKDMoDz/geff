package plz.engine.logic.controller;

import java.util.Date;

import com.badlogic.gdx.math.Vector2;

public class Pointer
{
	public Vector2 Start = null;
	public Vector2 Current = null;
	public Date CreationDate = null;
	public Date PreviousCreationDate = null;
	public PointerUsage Usage = PointerUsage.None;
	public int Index = -1;
	
	public Pointer(int x, int y, int index)
	{
		Init(x,y);
		this.Index = index;
	}
	
	public Pointer(int index)
	{
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
		this.PreviousCreationDate = this.CreationDate;
		this.CreationDate = new Date();
	}

	public boolean IsDoubleTap()
	{
		long time = new Date().getTime();
		if(this.PreviousCreationDate != null && time - this.PreviousCreationDate.getTime() < 2000)
			return true;
		
		return false;
	}
}
