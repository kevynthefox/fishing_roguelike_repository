using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class effect_catalouge : MonoBehaviour
{
    public static effect_catalouge current;
    public List<GameObject> effects;
    //public List<Sprite> effect_images;

    private void Awake()
    {
        current = this;
    }
}
