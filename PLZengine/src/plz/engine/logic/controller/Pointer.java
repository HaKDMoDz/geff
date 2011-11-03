package plz.engine.logic.controller;

import java.util.Date;

import com.badlogic.gdx.math.Vector2;

public class Pointer
{
	public Vector2 Start = null;
	public Vector2 Current = null;
	public Date CreationDate = null;
	public Date PreviousCreationDate = null;
	public PointerUsage PreviousUsage = PointerUsage.None;
	public PointerUsage Usage = PointerUsage.None;
	public int Index = -1;
	public boolean Handled = false;
	public Object Tag;
	
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
		this.Current = new Vector2(x, y);
		this.PreviousCreationDate = this.CreationDate;
		this.PreviousUsage = Usage;
		if(Handled = false)
			this.Usage = PointerUsage.None;
			
		this.CreationDate = new Date();
		this.Handled = false;
	}

	public boolean IsDoubleTap()
	{
		Date date = new Date();
		long time = date.getTime();
		if(PreviousUsage == PointerUsage.None && Usage == PointerUsage.None &&
				this.PreviousCreationDate != null && time - this.PreviousCreationDate.getTime() < 300)
			return true;
		
		return false;
	}
}
