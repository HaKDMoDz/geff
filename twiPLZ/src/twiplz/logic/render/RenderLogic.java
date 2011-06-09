package twiplz.logic.render;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.math.Vector2;

import twiplz.Context;
import twiplz.GameEngine;
import twiplz.model.Cell;

public class RenderLogic extends plz.engine.logic.render.RenderLogicBase
{
	Texture texCell;
	
	public Vector2[] PointToDraw = new Vector2[5];
	
	public RenderLogic(GameEngine gameEngine)
	{
		super(gameEngine);
		
		LoadTextures();
	}

	private void LoadTextures()
	{
		texCell = new Texture(Gdx.files.internal("data/Hexa_2D.png"));
		//badlogic.jpg
		//Hexa_2D_mini.jpg
	
	}

	@Override
	public void Render(float deltaTime)
	{
		super.Render(deltaTime);
		
		spriteBatch.setColor(Color.WHITE);
		spriteBatch.begin();
		
		for (Cell cell : Context.Map.Cells)
		{
			DrawCell(cell);
		}
		
		spriteBatch.setColor(Color.RED);
		for (int i = 0; i < PointToDraw.length; i++)
		{
			if(PointToDraw[i] != null)
			{
				Vector2 cellLocation = PointToDraw[i].cpy();
				//cellLocation.mul(512f);
				spriteBatch.draw(texCell, cellLocation.x, cellLocation.y);
			}
		}
		
		
		spriteBatch.end();
	}
	
	private void DrawCell(Cell cell)
	{
		Vector2 cellLocation = cell.Location.cpy();
		
		cellLocation.mul(512f);
		
		spriteBatch.draw(texCell, cellLocation.x, cellLocation.y);
	}
}
