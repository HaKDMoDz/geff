package twiplz;

import twiplz.logic.controller.SelectionMode;

import com.badlogic.gdx.ApplicationListener;
import com.badlogic.gdx.backends.jogl.JoglApplication;

public class GameStarterDesktop
{
	public static void main(String[] argv)
	{
		Context.selectionMode = SelectionMode.Desktop;
		Context.Mini = false;
		boolean miniWindow = false;
		
		ApplicationListener app = new GameEngine();
		JoglApplication window;

		if(miniWindow)
		{
			window = new JoglApplication(app, "twiPLZ", 400, 400, false);
			window.getJFrame().setLocation(2100, 550);
		}
		else
		{
			window = new JoglApplication(app, "twiPLZ", 1024, 600, false);
		}
			
	}
}