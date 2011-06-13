package plz.engine.logic.controller;

import java.util.Date;

import com.badlogic.gdx.math.Vector2;

public class Pointer
{
	public int Index = 0;
	public Vector2 Current;
	public Vector2 Start;
	private Date CreationDate;

	public Pointer(int x, int y, int pointer)
	{
		this.Start = new Vector2(x,y);
		this.Index = pointer;
		this.CreationDate = new Date();
	}

	public void SwapStartToCurrent()
	{
		if(this.Start != null)
			this.Start = this.Current;
	}

}
