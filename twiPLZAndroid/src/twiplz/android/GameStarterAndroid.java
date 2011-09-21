package twiplz.android;

import twiplz.Context;
import twiplz.GameEngine;
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

		Context.Mini = false;
		Context.selectionOffsetY = 512;
		
		AndroidApplicationConfiguration config = new AndroidApplicationConfiguration();
		config.useCompass = false;
		config.useAccelerometer = false;
		config.useWakelock = false;
		config.resolutionStrategy = new FillResolutionStrategy();

		ApplicationListener app = new GameEngine();

		initialize(app, config);
	}
}
