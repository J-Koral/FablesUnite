using UnityEngine;

public class ElementIcons : MonoBehaviour
{
    public Sprite water, fire, grass, ground, electric;

    public Sprite Get(Element e)
    {
        switch (e)
        {
            case Element.Water:    return water;
            case Element.Fire:     return fire;
            case Element.Grass:    return grass;
            case Element.Ground:   return ground;
            case Element.Electric: return electric;
        }
        return null;
    }
}
