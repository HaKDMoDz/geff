package plz.logic.ui.screens;

import plz.GameEngine;
import plz.engine.GameEngineBase;
import plz.engine.logic.controller.PLZInputMultiplexer;
import plz.engine.logic.ui.components.SensitiveZone;
import plz.engine.logic.ui.screens.ScreenBase;

import com.badlogic.gdx.Application.ApplicationType;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.scenes.scene2d.actions.Delay;
import com.badlogic.gdx.scenes.scene2d.actions.FadeIn;
import com.badlogic.gdx.scenes.scene2d.actions.FadeOut;
import com.badlogic.gdx.scenes.scene2d.actions.Forever;
import com.badlogic.gdx.scenes.scene2d.actions.ScaleTo;
import com.badlogic.gdx.scenes.scene2d.actions.Sequence;
import com.badlogic.gdx.scenes.scene2d.ui.Button;

public class MainMenu extends ScreenBase
{
	public MainMenu(GameEngineBase gameEngine)
	{
		super(gameEngine);
	}

	SensitiveZone btnTwiPLZ;
	SensitiveZone btnGriPLZ;
	
	@Override
	public void InitScreen()
	{
		this.screenName = "MainMenu";
		
		Texture texTwiPLZ = new Texture(Gdx.files.internal("data/twiPLZ_Button.png"));
		Texture texGriPLZ = new Texture(Gdx.files.internal("data/griPLZ_Button.png"));

		btnTwiPLZ = AddSensitiveZone("btnTwiPLZ", texTwiPLZ);
		btnTwiPLZ.releaseListener = btnTwiPLZ_Released;
		btnTwiPLZ.pressListener = btn_Pressed;
		
		btnGriPLZ = AddSensitiveZone("btnGriPLZ", texGriPLZ);
		btnGriPLZ.releaseListener = btnGriPLZ_Released;
		btnGriPLZ.pressListener = btn_Pressed;
		
//		btnTwiPLZ.action(Forever.$(Sequence.$(FadeOut.$(3), FadeIn.$(3))));
		//Stage.getRoot().action(Forever.$(Sequence.$(FadeOut.$(3), FadeIn.$(3))));
	}
	
	@Override
	public void LoadScreen()
	{
		layout.layout();
	}
	
	@Override
	public void render(float delta)
	{
		super.render(delta);
	}
	
	SensitiveZone.PressListener btn_Pressed = new SensitiveZone.PressListener()
	{
		@Override
		public void pressed(SensitiveZone button, float x, float y, int pointer)
		{
			button.action(ScaleTo.$(2f,2f,0.2f));
		}
	};
	
	SensitiveZone.ReleaseListener btnTwiPLZ_Released = new SensitiveZone.ReleaseListener()
	{
		@Override
		public void released(SensitiveZone button, int pointer,
				boolean isOnButton)
		{
			if (isOnButton)
			{
				plz.model.twiplz.Context context =new plz.model.twiplz.Context();
				context.Mini = gameEngine.Context.Mini;
				gameEngine.Context = context;
				
				if(Gdx.app.getType() == ApplicationType.Android)
				{
					((plz.model.twiplz.Context)gameEngine.Context).Mini = false;
					((plz.model.twiplz.Context)gameEngine.Context).selectionOffsetY = 0;
					((plz.model.twiplz.Context)gameEngine.Context).selectionMode = plz.logic.controller.twiplz.SelectionMode.Screentouch;
					((plz.model.twiplz.Context)gameEngine.Context).gameMode = plz.model.twiplz.GameMode.Circular;
				}
				else
				{
					((plz.model.twiplz.Context)gameEngine.Context).selectionMode = plz.logic.controller.twiplz.SelectionMode.Screentouch;
					((plz.model.twiplz.Context)gameEngine.Context).Mini = true;
				}
				
				gameEngine.Render = new plz.logic.render.twiplz.RenderLogic((GameEngine)gameEngine);
				gameEngine.Controller = new plz.logic.controller.twiplz.ControllerLogic((GameEngine)gameEngine);
				gameEngine.GamePlay = new plz.logic.gameplay.twiplz.GamePlayLogic((GameEngine)gameEngine);
				
				//this.CurrentScreen = new GameScreen(this);
				gameEngine.CurrentScreen = new plz.logic.ui.screens.twiplz.GameScreen((GameEngine)gameEngine);
				gameEngine.CurrentScreen.show();
				
				gameEngine.RegisterInput();
			}

			button.action(ScaleTo.$(1f,1f, 0.2f));
		}
	};
	
	SensitiveZone.ReleaseListener btnGriPLZ_Released = new SensitiveZone.ReleaseListener()
	{
		@Override
		public void released(SensitiveZone button, int pointer,
				boolean isOnButton)
		{
			if (isOnButton)
			{
				plz.model.griplz.Context context =new plz.model.griplz.Context();
				context.Mini = gameEngine.Context.Mini;
				gameEngine.Context = context;
				
				if(Gdx.app.getType() == ApplicationType.Android)
				{
					((plz.model.griplz.Context)gameEngine.Context).Mini = false;
					((plz.model.griplz.Context)gameEngine.Context).selectionOffsetY = 0;
				}
				else
				{
					((plz.model.griplz.Context)gameEngine.Context).Mini = true;
				}
				
				gameEngine.Render = new plz.logic.render.griplz.RenderLogic((GameEngine)gameEngine);
				gameEngine.Controller = new plz.logic.controller.griplz.ControllerLogic((GameEngine)gameEngine);
				gameEngine.GamePlay = new plz.logic.gameplay.griplz.GamePlayLogic((GameEngine)gameEngine);
				
				//this.CurrentScreen = new GameScreen(this);
				gameEngine.CurrentScreen = new plz.logic.ui.screens.griplz.GameScreen((GameEngine)gameEngine);
				gameEngine.CurrentScreen.show();
				
				gameEngine.RegisterInput();
			}

			button.action(ScaleTo.$(1f,1f, 0.2f));
		}
	};
}
