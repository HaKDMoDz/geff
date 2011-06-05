package twiplz;

import com.badlogic.gdx.ApplicationListener;
import com.badlogic.gdx.backends.jogl.JoglApplication;

public class GameStarterDesktop
{
	public static void main(String[] argv)
	{
		ApplicationListener app = (ApplicationListener) new GameEngine();
		new JoglApplication(app, "twiPLZ", 800, 600, false);
	}
}