package meeplz.android;

import meeplz.GameEngine;
import android.os.Bundle;

import com.badlogic.gdx.ApplicationListener;
import com.badlogic.gdx.backends.android.AndroidApplication;

public class MeeplzAndroid extends AndroidApplication {
	@Override public void onCreate (Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		ApplicationListener app = new GameEngine();
		initialize(app, false);			
	}
}
