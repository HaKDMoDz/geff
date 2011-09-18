package twiplz.logic.render;

import plz.engine.logic.ui.components.SensitiveZone;
import twiplz.Context;
import twiplz.GameEngine;
import twiplz.logic.gameplay.GamePlayLogic;
import twiplz.logic.ui.screens.GameScreen;
import twiplz.model.Cell;
import twiplz.model.CellPartType;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.math.Vector2;

public class RenderLogic extends plz.engine.logic.render.RenderLogicBase
{
	Texture texCellForeground;
	Texture texCellBackground;
	Texture[] texArrowsIn = new Texture[6];
	Texture[] texArrowsOut = new Texture[6];
	Color[] colors;

	public Vector2[] PointToDraw = new Vector2[5];

	private boolean mini = false;
	private boolean showColor = true;
	private boolean showCursor = false;

	private GamePlayLogic GamePlay()
	{
		return (GamePlayLogic) gameEngine.GamePlay;
	}

	private GameScreen GameScreen()
	{
		return (GameScreen) gameEngine.CurrentScreen;
	}

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

		if (mini)
		{
			for (int i = 0; i < colors.length; i++)
			{
				colors[i].mul(0.25f);
			}
		}
	}

	@Override
	public void Render(float deltaTime)
	{
		super.Render(deltaTime);

		if (mini)
			spriteBatch.setColor(Color.BLACK);
		else
			spriteBatch.setColor(Color.WHITE);

		spriteBatch.begin();

		if (showColor)
		{
			// --- Cellules de la map
			for (Cell cell : Context.Map.Cells)
			{
				// if (!cell.Selected)
				DrawCell(cell, false, false);
			}
			// ---
		}

		if (PointToDraw.length > 0 && PointToDraw[0] != null && showCursor)
		{
			spriteBatch.setColor(Color.WHITE);
			spriteBatch.draw(texCellForeground, PointToDraw[0].x,
					PointToDraw[0].y, 256, 256);
		}

		// --- Tuile sélectionnée
		if (GamePlay().SelectedTile != null)
		{

			DrawCell(GamePlay().SelectedTile.Cells[0], false, true);
			DrawCell(GamePlay().SelectedTile.Cells[1], false, true);
		}
		// ---

		spriteBatch.end();
	}

	@Override
	public void RenderUI(float deltaTime)
	{
		ProjectUI();

		gameEngine.CurrentScreen.render(deltaTime);

		// --- Bouton 'Nouvelle tuile'
		spriteBatch.begin();

		DrawCell(GamePlay().Tile.Cells[0], true, false);
		DrawCell(GamePlay().Tile.Cells[1], true, false);

		spriteBatch.end();
		// ---

	}

	private void DrawCell(Cell cell, boolean isUI, boolean isSelectedCell)
	{
		Vector2 cellLocation = cell.Location.cpy();

		int width = texCellBackground.getWidth();
		int height = texCellBackground.getHeight();

		if (isUI)
		{
			SensitiveZone imgNewTile = ((GameScreen) this.gameEngine.CurrentScreen).imgNewTile;

			// height = (int) imgNewTile.height;
			// width = (int) imgNewTile.width;

			height = (int) (imgNewTile.height / 2);
			// height = (int) (imgNewTile.height /
			// (4f*(1+(2/Math.sqrt(3f)))))*4;

			width = (int) ((2 * height) / Math.sqrt(3f));
		} else
		{
			cellLocation.mul(256f);
		}

		if (isSelectedCell)
			spriteBatch.setColor(Color.GREEN);
		else
			spriteBatch.setColor(Color.WHITE);

		spriteBatch.draw(texCellBackground, cellLocation.x, cellLocation.y,
				width, height);

		if (!cell.IsEmpty)
		{
			spriteBatch.setColor(colors[cell.ColorType - 1]);
			spriteBatch.draw(texCellForeground, cellLocation.x, cellLocation.y,
					width, height);
		}

		if (mini)
			spriteBatch.setColor(Color.BLACK);
		else
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
				spriteBatch.draw(texturePart, cellLocation.x, cellLocation.y,
						width, height);
			}
		}
	}
}
