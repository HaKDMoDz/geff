package plz.model.liplz;

import java.util.Vector;

import plz.engine.ContextBase;
import plz.engine.logic.controller.Pointer;
import plz.logic.controller.twiplz.SelectionMode;
import plz.model.*;

public class Context extends ContextBase
{
	public Vector<Cell> SelectedCells = new Vector<Cell>();
	public Map Map;
	public int selectionOffsetY = 0;
	public int Score = 0;
	public int AddedScore = 0;
	public int Combo = 0;
	public GameStateTime gameStateTime;
	public Cell LastSelectedCell = null;
}
