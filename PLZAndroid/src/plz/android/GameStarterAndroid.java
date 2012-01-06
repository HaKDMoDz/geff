package plz.android;

import plz.GameEngine;
import plz.logic.controller.twiplz.SelectionMode;
import plz.model.twiplz.Context;
import plz.model.twiplz.GameMode;
import android.content.pm.ActivityInfo;
import android.os.Bundle;

import com.badlogic.gdx.ApplicationListener;
import com.badlogic.gdx.backends.android.AndroidApplication;
import com.badlogic.gdx.backends.android.AndroidApplicationConfiguration;
import com.badlogic.gdx.backends.android.surfaceview.FillResolutionStrategy;

public class GameStarterAndroid extends AndroidApplication {
	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE);

		AndroidApplicationConfiguration config = new AndroidApplicationConfiguration();
		config.useCompass = false;
		config.useAccelerometer = false;
		config.useWakelock = false;
		config.resolutionStrategy = new FillResolutionStrategy();
		config.useGL20 = false;
		
		GameEngine app = new GameEngine(null);

		initialize(app, config);
		

	}
}
