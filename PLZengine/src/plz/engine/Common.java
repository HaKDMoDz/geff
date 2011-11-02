package plz.engine;

import com.badlogic.gdx.math.Vector2;

public class Common
{
	public static float GetAngle(Vector2 vec1, Vector2 vec2)
	{
		float dot = vec1.x * vec2.x + vec1.y * vec2.y;
		float pdot = vec1.x * vec2.y - vec1.y * vec2.x;
		return (float) Math.atan2(pdot, dot);
	}

	private static float ComputeZCoordinate(Vector2 p1, Vector2 p2, Vector2 p3)
	{
		// x1 (y2 - y3) + x2 (y3 - y1) + x3 (y1 - y2)

		return p1.x * (p2.y - p3.y) + p2.x * (p3.y - p1.y) + p3.x * (p1.y - p2.y);
	}

	public static Boolean IsPointInsideTriangle(Vector2[] triangle, Vector2 point)
	{
		float z1 = ComputeZCoordinate(triangle[0], triangle[1], point);
		float z2 = ComputeZCoordinate(triangle[1], triangle[2], point);
		float z3 = ComputeZCoordinate(triangle[2], triangle[0], point);

		return (z1 > 0 && z2 > 0 && z3 > 0) || (z1 < 0 && z2 < 0 && z3 < 0);
	}

	public static int mod(int a, int b)
	{
		int res = a % b;
		if (res < 0 && b > 0)
		{
			res += b;
		}
		return res;
	}

	public static double Gaussian(double x)
	{
		x = (x - 0.5) * 3;

		double sigma = 0.447214;
		double value = Math.exp(-x * x / (2 * sigma * sigma)) / (Math.sqrt(2 * Math.PI) * sigma);
		return value;
	}
	
	public static float Lerp(float a, float b, float percent)
	{
		return a + (b-a) * percent;
	}
}
