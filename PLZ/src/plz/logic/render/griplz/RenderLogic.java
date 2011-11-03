package plz.logic.render.griplz;

import java.util.HashMap;

import plz.engine.Common;
import plz.engine.logic.controller.Pointer;
import plz.engine.logic.controller.PointerUsage;
import plz.GameEngine;
import plz.logic.gameplay.griplz.GamePlayLogic;
import plz.logic.ui.screens.griplz.GameScreen;
import plz.model.griplz.Cell;
import plz.model.griplz.CellLayer;
import plz.model.griplz.CellSeed;
import plz.model.griplz.Context;
import plz.model.griplz.TileState;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.BitmapFont;
import com.badlogic.gdx.graphics.g2d.TextureRegion;
import com.badlogic.gdx.graphics.glutils.ShaderProgram;
import com.badlogic.gdx.math.Vector2;

public class RenderLogic extends plz.engine.logic.render.RenderLogicBase
{
	Texture[] texCellForeground;
	Texture texCellBackground;
	Texture texCircle;
	TextureRegion texArrow;
	Texture texSelected;

	public BitmapFont fontScore;
	public BitmapFont fontBonus;

	public HashMap<Integer, Color> colors = new HashMap<Integer, Color>();

	public Vector2[] PointToDraw = new Vector2[5];

	private boolean showCursor = false;
	private boolean showColor = true;

	private GamePlayLogic GamePlay()
	{
		return (GamePlayLogic) gameEngine.GamePlay;
	}

	private GameScreen GameScreen()
	{
		return (GameScreen) gameEngine.CurrentScreen;
	}

	public Context Context()
	{
		return (Context) gameEngine.Context;
	}

	public RenderLogic(GameEngine gameEngine)
	{
		super(gameEngine);

		LoadTextures();
	}

	private void LoadTextures()
	{
		String path = "data/";

		if (Context().Mini)
			path += "Mini/";

		texCellForeground = new Texture[3];

		texCellForeground[0] = new Texture(Gdx.files.internal(path + "CellForeground_Small.png"));
		texCellForeground[1] = new Texture(Gdx.files.internal(path + "CellForeground_Medium.png"));
		texCellForeground[2] = new Texture(Gdx.files.internal(path + "CellForeground_Large.png"));

		texCellBackground = new Texture(Gdx.files.internal(path + "CellBackground.png"));
		texArrow = TextureRegion.split(new Texture(Gdx.files.internal(path + "Arrow.png")), 256, 256)[0][0];

		// texCircle = new Texture(Gdx.files.internal("data/Circle.png"));
		// texSelected = new Texture(Gdx.files.internal(path +
		// "CellSelected.png"));

		fontScore = new BitmapFont();
		fontScore.setColor(new Color(0, 1, 1, 1));
		fontScore.scale(0.6f);

		fontBonus = new BitmapFont();
		fontBonus.setColor(new Color(1, 1, 0, 1));
		fontBonus.scale(2f);

		colors.put(0, Color.GREEN);
		colors.put(1, new Color(1f, 0.7f, 0.84f, 1f));
		colors.put(2, new Color(0.78f, 0.7f, 1f, 1f));
		colors.put(3, new Color(0.68f, 0.90f, 1f, 1f));
		colors.put(4, new Color(0.68f, 1f, 0.7f, 1f));
		colors.put(5, new Color(0.95f, 1f, 0.66f, 1f));
		colors.put(6, new Color(1f, 0.79f, 0.68f, 1f));
		colors.put(7, Color.RED);

		// colors.put(2, new Color(1f, 0f, 1f, 1f));
		// colors.put(6, new Color(1f, 0.7f, 0.3f, 1f));
		// colors.put(4, new Color(0f, 1f, 1f, 1f));
		// colors.put(12, new Color(0f, 1f, 0f, 1f));
		// colors.put(8, new Color(1f, 1f, 0f, 1f));
		// colors.put(10, new Color(1f, 0f, 0f, 1f));

		if (Context().Mini)
		{
			for (Integer key : colors.keySet())
			{
				colors.get(key).mul(0.5f);
			}
		}
	}

	@Override
	public void Render(float deltaTime)
	{
		super.Render(deltaTime);

		if (Context().Mini)
			spriteBatch.setColor(Color.BLACK);
		else
			spriteBatch.setColor(Color.WHITE);

		spriteBatch.begin();

		if (showColor)
		{
			// --- Cellules de la map
			for (Cell cell : Context().Map.Cells)
			{
				// if (!cell.Selected)
				DrawCell(cell, RenderItem.BackGround);
			}
			// ---

			// --- Cellules de la map
			for (Cell cell : Context().Map.Cells)
			{
				// if (!cell.Selected)
				DrawCell(cell, RenderItem.Tile);
			}
			// ---
			
			// --- Cellules de la map
			for (Cell cell : Context().Map.Cells)
			{
				// if (!cell.Selected)
				DrawCell(cell, RenderItem.Arrow);
			}
			// ---
		}

		spriteBatch.end();
	}

	@Override
	public void RenderUI(float deltaTime)
	{
		ProjectUI();

		gameEngine.CurrentScreen.render(deltaTime);

		// --- Bouton 'Nouvelle tuile'
		// spriteBatch.setShader(shader);

		spriteBatch.begin();
		// shader.begin();

		if (showCursor)
		{
			for (Pointer pointer : Context().pointers)
			{
				if (pointer.Current != null)
				{
					if (pointer.Usage == PointerUsage.SelectTile)
						spriteBatch.setColor(Color.GREEN);
					else if (pointer.Usage == PointerUsage.ButtonTurnTile)
						spriteBatch.setColor(Color.RED);
					else
						spriteBatch.setColor(Color.BLUE);

					spriteBatch.draw(texCellForeground[0], pointer.Current.x - 20, Gdx.graphics.getHeight() - (pointer.Current.y - 20), 40, 40);
				}
			}
		}

		// shader.end();

		spriteBatch.end();
		// ---

		// spriteBatch.setShader(null);
	}

	private void DrawCell(Cell cell, RenderItem renderItem)
	{

		Vector2 cellLocation = cell.Location.cpy();

		int width = texCellBackground.getWidth();
		int height = texCellBackground.getHeight();

		cellLocation.mul(256f);


		if (renderItem == RenderItem.BackGround)
		{
			// --- Cell Background
			if (cell.getClass() == CellLayer.class)
			{
				float v = ((float) ((CellLayer) cell).LayerNumber) / 12f;

				spriteBatch.setColor(0.1f + v, 0.1f + v, 0.1f + v, 1f);
			}
			else if (cell.getClass() == CellSeed.class)
			{
				spriteBatch.setColor(0.2f, 0.2f, 0.35f, 1f);
			}
			else
			{
				spriteBatch.setColor(0.2f, 0.2f, 0.2f, 2f);
			}

			spriteBatch.draw(texCellBackground, cellLocation.x, cellLocation.y, width, height);
		}
		else if(renderItem == RenderItem.Tile)
		{
			// --- Cell Foreground
			Texture texForeGround = null;

			if (cell.Tile != null)
			{
				if (cell.Tile.IsFilled)
					spriteBatch.setColor(Color.BLUE);
				else if (Context().Mini)
				{
					spriteBatch.setColor(0.3f, 0.3f, 0.3f, 1f);
				}
				else
				{
					spriteBatch.setColor(0.8f, 0.8f, 0.8f, 1f);
				}

				texForeGround = texCellForeground[cell.Tile.TypeTile];

				if (cell.Tile.DirectionMovement > -1)
				{
					Vector2 nextCellLocation = cell.Neighbourghs[cell.Tile.DirectionMovement].Location.cpy();
					nextCellLocation.mul(256f);

					cellLocation.x = Common.Lerp(cellLocation.x, nextCellLocation.x, cell.Tile.PercentMovement);
					cellLocation.y = Common.Lerp(cellLocation.y, nextCellLocation.y, cell.Tile.PercentMovement);
				}
			}

			if (texForeGround != null)
				spriteBatch.draw(texForeGround, cellLocation.x, cellLocation.y, width, height);
		}
		else if(renderItem == RenderItem.Arrow)
		{
			// --- Fleche
			if (cell.Tile != null && cell.Tile.State == TileState.Selected && cell.Tile.DirectionMovement>-1)
			{
				spriteBatch.setColor(Color.WHITE);
				spriteBatch.draw(texArrow, cellLocation.x, cellLocation.y+height/2, width/2, 0f, width, height, 1f, 1f, (float) (-cell.Tile.DirectionMovement)*60f);
			}
		}
	}
}
