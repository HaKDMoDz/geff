package meeplz;

import com.badlogic.gdx.backends.jogl.JoglApplication;

public class GameEngineDesktop
{
	public static void main(String[] argv)
	{
		new JoglApplication(new GameEngine(), "Meeplz", 320, 240, false);
	}
}