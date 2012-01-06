package plz.logic.render.liplz;

import java.util.Dictionary;
import java.util.HashMap;

import plz.engine.Common;
import plz.engine.logic.controller.Pointer;
import plz.engine.logic.controller.PointerUsage;
import plz.GameEngine;
import plz.logic.gameplay.liplz.GamePlayLogic;
import plz.logic.ui.screens.liplz.GameScreen;
import plz.model.liplz.Cell;
import plz.model.liplz.CellState;
import plz.model.liplz.Context;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.Pixmap.Format;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.BitmapFont;
import com.badlogic.gdx.graphics.g2d.TextureRegion;
import com.badlogic.gdx.graphics.glutils.ShaderProgram;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.graphics.g2d.Gdx2DPixmap;

public class RenderLogic extends plz.engine.logic.render.RenderLogicBase
{
	Texture[] texCellSymbol;
	Texture texCellBackground;
	Texture texCircle;
	TextureRegion texArrow;
	Texture texSelected;
	Texture texEmpty;

	public BitmapFont fontScore;
	public BitmapFont fontBonus;

	public HashMap<Integer, Color> colors = new HashMap<Integer, Color>();

	public Vector2[] PointToDraw = new Vector2[5];

	private boolean showCursor = false;
	private boolean showColor = true;
	private Gdx2DPixmap pixmap;

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

		Init();
	}

	private void LoadTextures()
	{
		String path = "data/";

		if (Context().Mini)
			path += "Mini/";

		texCellSymbol = new Texture[6];

		for (int i = 0; i < 6; i++)
		{
			texCellSymbol[i] = new Texture(Gdx.files.internal(path + "Symbol"
					+ (i + 1) + ".png"));
		}

		texCellBackground = new Texture(Gdx.files.internal(path
				+ "CellBackground.png"));
		texEmpty = new Texture(Gdx.files.internal(path + "EmptyWhite.png"));

		texArrow = TextureRegion.split(
				new Texture(Gdx.files.internal(path + "Arrow.png")), 256, 256)[0][0];

		// texCircle = new Texture(Gdx.files.internal("data/Circle.png"));
		// texSelected = new Texture(Gdx.files.internal(path +
		// "CellSelected.png"));

		fontScore = new BitmapFont();
		fontScore.setColor(new Color(0, 1, 1, 1));
		fontScore.scale(2f);

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

	private void Init()
	{
		// pixmap = new Gdx2DPixmap(Gdx.app.getGraphics().getWidth(),
		// Gdx.app.getGraphics().getHeight(),
		// Gdx2DPixmap.GDX2D_FORMAT_RGBA8888);

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

			Cell prevCell = Context().Map.CenterCell;
			boolean p = true;
			for (Cell selectedCell : Context().SelectedCells)
			{
				Color color = Color.RED;
				if (p)
					color = Color.BLUE;

				DrawLine(prevCell.Location.x * 256 + 128,
						prevCell.Location.y * 256 + 128,
						selectedCell.Location.x * 256 + 128,
						selectedCell.Location.y * 256 + 128, color);
				prevCell = selectedCell;

				p = !p;
			}

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

					// spriteBatch.draw(texCellForeground[0], pointer.Current.x
					// - 20, Gdx.graphics.getHeight() - (pointer.Current.y -
					// 20), 40, 40);
				}
			}
		}

		// shader.end();

		spriteBatch.end();
		// ---

		// spriteBatch.setShader(null);
	}

	private void DrawLine(float x1, float y1, float x2, float y2, Color color)
	{
		spriteBatch.setColor(color);

		int width = (int) Math
				.sqrt(Math.pow(x2 - x1, 2) + Math.pow(y2 - y1, 2));
		float rotation = Common.GetAngle(new Vector2(1, 0), new Vector2(
				x2 - x1, y2 - y1));

		spriteBatch.draw(texEmpty, x1, y1, 0, 0, width, 30f, 1f, 1f,
				180 / ((float) Math.PI / rotation), 1, 1, 0, 0, false, false);
	}

	private void DrawCell(Cell cell, RenderItem renderItem)
	{

		if (cell.State == CellState.Invisible)
			return;

		Vector2 cellLocation = cell.Location.cpy();

		int width = texCellBackground.getWidth();
		int height = texCellBackground.getHeight();

		cellLocation.mul(256f);

		if (renderItem == RenderItem.BackGround)
		{
			// --- Cell Background
			Color cellColor = colors.get(cell.Tile.Color);

			if (cell.State == CellState.Stoned)
			{
				cellColor = new Color(cellColor.r/3f, cellColor.g/3f, cellColor.b/3f, 1f);
			}
			else if (cell.State == CellState.Highlighted1)
			{
				cellColor = new Color(cellColor.r*0.75f, cellColor.g*0.75f, cellColor.b*0.75f, 1f);
			}

			spriteBatch.setColor(cellColor);
			spriteBatch.draw(texCellBackground, cellLocation.x, cellLocation.y,	width, height);
		}
		if (renderItem == RenderItem.Tile)
		{
			if (cell.Tile.Symbol > 0)
			{
				spriteBatch.setColor(0.2f, 0.2f, 0.2f, 1f);

				if (cell.State == CellState.Highlighted1)
				{
					spriteBatch.setColor(0.5f, 0.5f, 0.5f, 1f);
				}
				
				spriteBatch.draw(texCellSymbol[cell.Tile.Symbol],
						cellLocation.x, cellLocation.y, width, height);
			}
		}
	}
}
