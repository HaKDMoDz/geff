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
	Texture texCell;
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
		texCell = new Texture(Gdx.files.internal("data/Hexa_2D_Metal.png"));

		for (int i = 1; i <= 6; i++)
		{
			// arrowsIn[i-1] = new Texture(Gdx.files.internal("data/ArrowIn" + i
			// + ".png"));
			texArrowsOut[i - 1] = new Texture(
					Gdx.files.internal("data/ArrowOut" + i + ".png"));
		}

		colors = new Color[7];
		colors[0] = Color.BLUE;
		colors[1] = new Color(1f, 0f, 1f, 1f); // VIOLET
		colors[2] = Color.RED;
		colors[3] = new Color(1f, 0.64f, 0f, 1f); // VIOLET
		colors[4] = new Color(1f, 1f, 0f, 1f); // YELLOW
		colors[5] = Color.GREEN;
		colors[6] = Color.WHITE;

		// badlogic.jpg
		// Hexa_2D_mini.jpg
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
			if (PointToDraw[i] != null)
			{
				Vector2 cellLocation = PointToDraw[i].cpy();
				// cellLocation.mul(512f);
				spriteBatch.draw(texCell, cellLocation.x, cellLocation.y);
			}
		}

		spriteBatch.end();
	}

	private void DrawCell(Cell cell)
	{
		Vector2 cellLocation = cell.Location.cpy();

		cellLocation.mul(256f);
		spriteBatch.setColor(colors[cell.ColorType - 1]);
		spriteBatch.draw(texCell, cellLocation.x, cellLocation.y);

		spriteBatch.setColor(Color.WHITE);

		for (int i = 0; i < 6; i++)
		{
			Texture texturePart = null;

			if (cell.Parts[i] == CellPartType.Out)
			{
				texturePart = texArrowsOut[i];
			}
			else if(cell.Parts[i] == CellPartType.In)
			{
				texturePart = texArrowsIn[i];
			}

			if (texturePart != null)
			{
				//Vector2 partLocation = new Vector2(
				//		0.7f * 512f * (float)Math.cos(30 * i),
				//		0.7f * 512f * (float)Math.sin(30 * i));

				//spriteBatch.draw(texturePart, cellLocation.x + partLocation.x, cellLocation.y + partLocation.y);
				spriteBatch.draw(texturePart, cellLocation.x, cellLocation.y);
			}
		}
	}
}
