package plz.engine.logic.controller;

import java.util.ArrayList;

import com.badlogic.gdx.InputProcessor;

public class PLZInputMultiplexer implements InputProcessor {
	private ArrayList<InputProcessor> processors = new ArrayList<InputProcessor>(4);

	public PLZInputMultiplexer () {
	}

	public PLZInputMultiplexer (InputProcessor... processors) {
		for (int i = 0; i < processors.length; i++)
			this.processors.add(processors[i]);
	}

	public void addProcessor (InputProcessor processor) {
		processors.add(processor);
	}

	public void removeProcessor (InputProcessor processor) {
		processors.remove(processor);
	}

	public void clear () {
		processors.clear();
	}

	public boolean keyDown (int keycode) {
		for (int i = 0, n = processors.size(); i < n; i++)
			if (processors.get(i).keyDown(keycode)) return true;
		return false;
	}

	public boolean keyUp (int keycode) {
		for (int i = 0, n = processors.size(); i < n; i++)
			if (processors.get(i).keyUp(keycode)) return true;
		return false;
	}

	public boolean keyTyped (char character) {
		for (int i = 0, n = processors.size(); i < n; i++)
			processors.get(i).keyTyped(character);
		return false;
	}

	public boolean touchDown (int x, int y, int pointer, int button) {
		for (int i = 0, n = processors.size(); i < n; i++)
			processors.get(i).touchDown(x, y, pointer, button);
		return false;
	}

	public boolean touchUp (int x, int y, int pointer, int button) {
		for (int i = 0, n = processors.size(); i < n; i++)
			processors.get(i).touchUp(x, y, pointer, button);
		return false;
	}

	public boolean touchDragged (int x, int y, int pointer) {
		for (int i = 0, n = processors.size(); i < n; i++)
			processors.get(i).touchDragged(x, y, pointer);
		return false;
	}

	@Override
	public boolean touchMoved (int x, int y) {
		for (int i = 0, n = processors.size(); i < n; i++)
			processors.get(i).touchMoved(x, y);
		return false;
	}

	@Override
	public boolean scrolled (int amount) {
		for (int i = 0, n = processors.size(); i < n; i++)
			processors.get(i).scrolled(amount);
		return false;
	}
}
