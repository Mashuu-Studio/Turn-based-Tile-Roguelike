using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteManager
{
    public static Dictionary<string, Sprite> Sprites { get { return sprites; } }
    private static Dictionary<string, Sprite> sprites;
    public static void Init()
    {
        Sprite[] arr = Resources.LoadAll<Sprite>("Sprites");

        sprites = new Dictionary<string, Sprite>();
        foreach(var sprite in arr)
        {
            sprites.Add(sprite.name.ToUpper(), sprite);
        }
    }

    public static Sprite GetSprite(string name)
    {
        name = name.ToUpper();
        if (sprites.ContainsKey(name)) return sprites[name];
        return null;
    }
}
