package plz.engine;

import com.badlogic.gdx.math.Vector2;

public class Common 
{
	public static float GetAngle(Vector2 vec1, Vector2 vec2)
    {
        float dot = vec1.x * vec2.x + vec1.y * vec2.y;
        float pdot = vec1.x * vec2.y - vec1.y * vec2.x;
        return (float)Math.atan2(pdot, dot);
    }
}
