package meeplz.logic.render;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.math.Vector2;

import meeplz.Context;
import meeplz.GameEngine;
import meeplz.model.Cell;

public class RenderLogic extends meepleengine.logic.render.RenderLogicBase
{
	Texture texCell;
	
	public RenderLogic(GameEngine gameEngine)
	{
		super(gameEngine);
		
		LoadTextures();
	}

	private void LoadTextures()
	{
		texCell = new Texture(Gdx.files.internal("data/Hexa_2D.png"));
	}

	@Override
	public void Render(float deltaTime)
	{
		super.Render(deltaTime);
		
		spriteBatch.begin();
		
		for (Cell cell : Context.Map.Cells)
		{
			DrawCell(cell);
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
