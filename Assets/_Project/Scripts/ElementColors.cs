using UnityEngine;

// Greybox color-coding so you can read elements at a glance.
public static class ElementColors
{
    public static Color Get(Element e)
    {
        switch (e)
        {
            case Element.Water:    return new Color(0.30f, 0.60f, 1.00f);
            case Element.Fire:     return new Color(1.00f, 0.40f, 0.30f);
            case Element.Grass:    return new Color(0.40f, 0.85f, 0.40f);
            case Element.Ground:   return new Color(0.70f, 0.55f, 0.35f);
            case Element.Electric: return new Color(1.00f, 0.90f, 0.30f);
        }
        return Color.white;
    }
}
