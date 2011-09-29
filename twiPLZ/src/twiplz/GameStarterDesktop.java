package twiplz;

import com.badlogic.gdx.ApplicationListener;
import com.badlogic.gdx.backends.jogl.JoglApplication;

public class GameStarterDesktop
{
	public static void main(String[] argv)
	{
		Context.Mini = false;
		boolean miniWindow = true;
		
		ApplicationListener app = new GameEngine();
		JoglApplication window;

		if(miniWindow)
		{
			window = new JoglApplication(app, "twiPLZ", 400, 400, false);
			window.getJFrame().setLocation(2100, 550);
		}
		else
		{
			window = new JoglApplication(app, "twiPLZ", 800, 600, false);
		}
			
	}
}