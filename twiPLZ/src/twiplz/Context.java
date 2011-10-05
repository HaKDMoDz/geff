package twiplz;

import plz.engine.logic.controller.Pointer;
import twiplz.logic.controller.SelectionMode;
import twiplz.model.*;

public class Context
{
	public static final long TimeRefresh = 3000;
	public static Map Map;
	public static Pointer[] pointers = new Pointer[10];
	public static boolean Mini = false;
	public static int selectionOffsetY = 0;
	public static SelectionMode selectionMode = SelectionMode.Screentouch;
	public static GameMode gameMode = GameMode.Circular;
	public static int Score = 0;
	public static int AddedScore = 0;
	public static int Combo = 0;
}
