using UnityEngine;

public static class ColorExt
{
    #region HTML colors from http://www.w3.org/TR/REC-html40/types.html#h-6.5

    public static Color32 black32 { get { return new Color32(0, 0, 0, 255); } }
    public static Color32 silver32 { get { return new Color32(192, 192, 192, 255); } }
    public static Color32 gray32 { get { return new Color32(128, 128, 128, 255); } }
    public static Color32 white32 { get { return new Color32(255, 255, 255, 255); } }
    public static Color32 maroon32 { get { return new Color32(128, 0, 0, 255); } }
    public static Color32 red32 { get { return new Color32(255, 0, 0, 255); } }
    public static Color32 purple32 { get { return new Color32(128, 0, 128, 255); } }
    public static Color32 fuchsia32 { get { return new Color32(255, 0, 255, 255); } }
    public static Color32 green32 { get { return new Color32(0, 128, 0, 255); } }
    public static Color32 lime32 { get { return new Color32(0, 255, 0, 255); } }
    public static Color32 olive32 { get { return new Color32(128, 128, 0, 255); } }
    public static Color32 yellow32 { get { return new Color32(255, 255, 0, 255); } }
    public static Color32 navy32 { get { return new Color32(0, 0, 128, 255); } }
    public static Color32 blue32 { get { return new Color32(0, 0, 255, 255); } }
    public static Color32 teal32 { get { return new Color32(0, 128, 128, 255); } }
    public static Color32 aqua32 { get { return new Color32(0, 255, 255, 255); } }

    public static Color black { get { return black32; } }
    public static Color silver { get { return silver32; } }
    public static Color gray { get { return gray32; } }
    public static Color white { get { return white32; } }
    public static Color maroon { get { return maroon32; } }
    public static Color red { get { return red32; } }
    public static Color purple { get { return purple32; } }
    public static Color fuchsia { get { return fuchsia32; } }
    public static Color green { get { return green32; } }
    public static Color lime { get { return lime32; } }
    public static Color olive { get { return olive32; } }
    public static Color yellow { get { return yellow32; } }
    public static Color navy { get { return navy32; } }
    public static Color blue { get { return blue32; } }
    public static Color teal { get { return teal32; } }
    public static Color aqua { get { return aqua32; } }

    #endregion Colors

    /// <summary>
    /// Returns inverted color with the same alpha
    /// </summary>
    public static Color Inverted(this Color color)
    {
        var result = Color.white - color;
        result.a = color.a;
        return result;
    }

    /// <summary>
    /// Creates a gradient between two colors
    /// </summary>
    public static Gradient Gradient(Color from, Color to)
    {
        var g = new Gradient();
        g.SetKeys(new[] {new GradientColorKey(from, 0), new GradientColorKey(to, 1)},
            new[] {new GradientAlphaKey(from.a, 0), new GradientAlphaKey(to.a, 1)});
        return g;
    }

    /// <summary>
    /// Returns new color with modified red component
    /// </summary>
    public static Color WithR(this Color color, float r)
    {
        return new Color(r, color.g, color.b, color.a);
    }

    /// <summary>
    /// Returns new color with modified green component
    /// </summary>
    public static Color WithG(this Color color, float g)
    {
        return new Color(color.r, g, color.b, color.a);
    }

    /// <summary>
    /// Returns new color with modified blue component
    /// </summary>
    public static Color WithB(this Color color, float b)
    {
        return new Color(color.r, color.g, b, color.a);
    }

    /// <summary>
    /// Returns new color with modified alpha component
    /// </summary>
    public static Color WithA(this Color color, float a)
    {
        return new Color(color.r, color.g, color.b, a);
    }

	/// <summary>
	/// Representation of color in HSV model
	/// </summary>
	public struct ColorHSV
	{
		/// <summary>
		/// Hue component of the color
		/// </summary>
		public float h;

		/// <summary>
		/// Saturation component of the color
		/// </summary>
		public float s;

		/// <summary>
		/// Value component of the color
		/// </summary>
		public float v;

		/// <summary>
		/// Alpha component of the color
		/// </summary>
		public float a;

		/// <summary>
		/// Constructs a new ColorHSV with given h, s, v, a components
		/// </summary>
		/// <param name="h">Hue component</param>
		/// <param name="s">Saturation component</param>
		/// <param name="v">Value component</param>
		/// <param name="a">Alpha component</param>
		public ColorHSV(float h, float s, float v, float a)
		{
			this.h = h;
			this.s = s;
			this.v = v;
			this.a = a;
		}

		/// <summary>
		/// Constructs a new ColorHSV with given h, s, v components and sets alpha to 1
		/// </summary>
		/// <param name="h">Hue component</param>
		/// <param name="s">Saturation component</param>
		/// <param name="v">Value component</param>
		public ColorHSV(float h, float s, float v)
		{
			this.h = h;
			this.s = s;
			this.v = v;
			a = 1;
		}

		/// <summary>
		/// Constructs a new ColorHSV from a Color
		/// </summary>
		public ColorHSV(Color color)
		{
			float x, max, left, right;
			if (color.r > color.g && color.r > color.b)
			{
				x = 0;
				max = color.r;
				left = color.g;
				right = color.b;
			}
			else if (color.g > color.b)
			{
				x = 2;
				max = color.g;
				left = color.b;
				right = color.r;
			}
			else
			{
				x = 4;
				max = color.b;
				left = color.r;
				right = color.g;
			}

			if (max != 0f)
			{
				float min = right < left ? right : left;
				float chroma = max - min;
				if (chroma != 0f)
				{
					h = x + (left - right)/chroma;
					s = chroma/max;
				}
				else
				{
					h = x + left - right;
					s = 0f;
				}
				h /= 6;
				if (h < 0)
				{
					h++;
				}
			}
			else
			{
				h = 0f;
				s = 0f;
			}
			v = max;
			a = color.a;
		}

		/// <summary>
		/// Converts ColorHSV to a RGB representation
		/// </summary>
		public Color ToColor()
		{
			if (s == 0f)
			{
				return new Color(v, v, v);
			}
			if (v == 0f)
			{
				return Color.black;
			}

			float position = h*6f;
			int sector = Mathf.FloorToInt(position);
			float fractional = position - sector;
			float p = v*(1 - s);
			float q = v*(1 - s*fractional);
			float t = v*(1 - s*(1 - fractional));

			switch (sector)
			{
			case -1:
				return new Color(v, p, q);
			case 0:
				return new Color(v, t, p);
			case 1:
				return new Color(q, v, p);
			case 2:
				return new Color(p, v, t);
			case 3:
				return new Color(p, q, v);
			case 4:
				return new Color(t, p, v);
			case 5:
				return new Color(v, p, q);
			case 6:
				return new Color(v, t, p);
			}
			return Color.black;
		}
	}


}
