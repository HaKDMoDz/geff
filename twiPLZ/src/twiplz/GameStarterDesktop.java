package twiplz;

import com.badlogic.gdx.ApplicationListener;
import com.badlogic.gdx.backends.jogl.JoglApplication;

public class GameStarterDesktop
{
	public static void main(String[] argv)
	{
		ApplicationListener app = (ApplicationListener) new GameEngine();
		JoglApplication window = new JoglApplication(app, "twiPLZ", 400, 400,
				false);

		window.getJFrame().setLocation(2100, 550);
	}
}