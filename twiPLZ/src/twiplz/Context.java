package twiplz;

import plz.engine.logic.controller.Pointer;
import twiplz.logic.controller.SelectionMode;
import twiplz.model.*;

public class Context
{
	public static Map Map;
	public static Pointer[] pointers = new Pointer[10];
	public static boolean Mini = false;
	public static int selectionOffsetY = 0;
	public static SelectionMode selectionMode = SelectionMode.Screentouch;
}
