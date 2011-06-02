package meeplz;

import com.badlogic.gdx.ApplicationListener;
import com.badlogic.gdx.backends.jogl.JoglApplication;

public class GameStarterDesktop
{
	public static void main(String[] argv)
	{
		ApplicationListener app = (ApplicationListener) new GameEngine();
		new JoglApplication(app, "meePLZ", 320, 240, false);
	}
}