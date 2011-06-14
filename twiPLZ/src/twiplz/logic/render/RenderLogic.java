package twiplz.logic.render;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.math.Vector2;

import twiplz.Context;
import twiplz.GameEngine;
import twiplz.model.Cell;
import twiplz.model.CellPartType;

public class RenderLogic extends plz.engine.logic.render.RenderLogicBase
{
	Texture texCellForeground;
	Texture texCellBackground;
	Texture[] texArrowsIn = new Texture[6];
	Texture[] texArrowsOut = new Texture[6];
	Color[] colors;

	public Vector2[] PointToDraw = new Vector2[5];

	public RenderLogic(GameEngine gameEngine)
	{
		super(gameEngine);

		LoadTextures();
	}

	private void LoadTextures()
	{
		texCellForeground = new Texture(
				Gdx.files.internal("data/CellForeground.png"));
		texCellBackground = new Texture(
				Gdx.files.internal("data/CellBackground.png"));

		for (int i = 1; i <= 6; i++)
		{
			texArrowsIn[i - 1] = new Texture(Gdx.files.internal("data/ArrowIn"
					+ i + ".png"));
			texArrowsOut[i - 1] = new Texture(
					Gdx.files.internal("data/ArrowOut" + i + ".png"));
		}

		colors = new Color[7];
		colors[0] = new Color(1f, 0.7f, 0.84f, 1f);
		colors[1] = new Color(0.78f, 0.7f, 1f, 1f);
		colors[2] = new Color(0.68f, 0.90f, 1f, 1f);
		colors[3] = new Color(0.68f, 1f, 0.7f, 1f);
		colors[4] = new Color(0.95f, 1f, 0.66f, 1f);
		colors[5] = new Color(1f, 0.79f, 0.68f, 1f);
		colors[6] = Color.WHITE;
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

		/*
		spriteBatch.setColor(Color.RED);
		for (int i = 0; i < PointToDraw.length; i++)
		{
			if (PointToDraw[i] != null)
			{
				Vector2 cellLocation = PointToDraw[i].cpy();
				// cellLocation.mul(512f);
				spriteBatch.draw(texCell, cellLocation.x, cellLocation.y);
			}
		}
		 */
		spriteBatch.end();
	}

	private void DrawCell(Cell cell)
	{
		Vector2 cellLocation = cell.Location.cpy();

		cellLocation.mul(256f);
		spriteBatch.draw(texCellBackground, cellLocation.x, cellLocation.y);
		spriteBatch.setColor(colors[cell.ColorType - 1]);
		spriteBatch.draw(texCellForeground, cellLocation.x, cellLocation.y);

		spriteBatch.setColor(Color.WHITE);

		for (int i = 0; i < 6; i++)
		{
			Texture texturePart = null;

			if (cell.Parts[i] == CellPartType.Out)
			{
				texturePart = texArrowsOut[i];
			} else if (cell.Parts[i] == CellPartType.In)
			{
				texturePart = texArrowsIn[i];
			}

			if (texturePart != null)
			{
				spriteBatch.draw(texturePart, cellLocation.x, cellLocation.y);
			}
		}
	}
}
