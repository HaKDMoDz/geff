package plz.logic.render.griplz;

import java.util.HashMap;
import plz.engine.logic.controller.Pointer;
import plz.engine.logic.controller.PointerUsage;
import plz.GameEngine;
import plz.logic.gameplay.griplz.GamePlayLogic;
import plz.logic.ui.screens.griplz.GameScreen;
import plz.model.griplz.Cell;
import plz.model.griplz.CellLayer;
import plz.model.griplz.Context;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.BitmapFont;
import com.badlogic.gdx.graphics.glutils.ShaderProgram;
import com.badlogic.gdx.math.Vector2;

public class RenderLogic extends plz.engine.logic.render.RenderLogicBase
{
	Texture texCellForeground;
	Texture texCellBackground;
	Texture texCircle;
	Texture texSelected;
	Texture[] texArrowsIn = new Texture[6];
	Texture[] texArrowsOut = new Texture[6];
	public BitmapFont fontScore;
	public BitmapFont fontBonus;

	public HashMap<Integer, Color> colors = new HashMap<Integer, Color>();

	ShaderProgram shader;

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
		return (Context)gameEngine.Context;
	}

	public RenderLogic(GameEngine gameEngine)
	{
		super(gameEngine);

		LoadTextures();
	}

	private void LoadTextures()
	{
		texCellForeground = new Texture(Gdx.files.internal("data/CellForeground.png"));
		texCellBackground = new Texture(Gdx.files.internal("data/CellBackground.png"));
		//texCircle = new Texture(Gdx.files.internal("data/Circle.png"));
		texSelected = new Texture(Gdx.files.internal("data/CellSelected.png"));

		// shader = new
		// ShaderProgram(Gdx.files.internal("data/shaders/batch.vert").readString(),
		// Gdx.files.internal("data/shaders/batch.frag").readString());


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
		
//		colors.put(2, new Color(1f, 0f, 1f, 1f));
//		colors.put(6, new Color(1f, 0.7f, 0.3f, 1f));
//		colors.put(4, new Color(0f, 1f, 1f, 1f));
//		colors.put(12, new Color(0f, 1f, 0f, 1f));
//		colors.put(8, new Color(1f, 1f, 0f, 1f));
//		colors.put(10, new Color(1f, 0f, 0f, 1f));

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
				DrawCell(cell, false, false);
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

					spriteBatch.draw(texCellForeground, pointer.Current.x - 20, Gdx.graphics.getHeight() - (pointer.Current.y - 20), 40, 40);
				}
			}
		}

		// shader.end();

		spriteBatch.end();
		// ---

		// spriteBatch.setShader(null);
	}

	private void DrawCell(Cell cell, boolean isUI, boolean isSelectedCell)
	{
		if(cell == null || cell.Location == null)
		{
			int a =0;
			a=1;
		}
		
		Vector2 cellLocation = cell.Location.cpy();

		int width = texCellBackground.getWidth();
		int height = texCellBackground.getHeight();

//		if (isUI)
//		{
//			SensitiveZone imgNewTile = ((GameScreen) this.gameEngine.CurrentScreen).imgNewTile[0];
//
//			height = (int) (imgNewTile.height / 2);
//			width = (int) ((2 * height) / Math.sqrt(3f));
//		}
//		else
		{
			cellLocation.mul(256f);
		}

		if (isSelectedCell)
		{
			if (Context().Mini)
				spriteBatch.setColor(0.3f, 0.3f, 0.3f, 1f);
			else
				spriteBatch.setColor(Color.GREEN);
		}
		
		spriteBatch.setColor(Color.WHITE);

		if(cell.getClass() == CellLayer.class)
		{
			spriteBatch.setColor(colors.get((int)cell.TypeItem));
			
			
		}
		
		spriteBatch.draw(texCellForeground, cellLocation.x, cellLocation.y, width, height);

//		if (Context.Mini)
//			spriteBatch.setColor(Color.BLACK);
//		else
//			spriteBatch.setColor(Color.WHITE);
		//calculer le next color cell
		//Color colorNextCell = colors.get((int) cell.ColorType);
		Color colorNextCell = Color.RED;
		
		spriteBatch.setColor(colorNextCell);
	}
}
