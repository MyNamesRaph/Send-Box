using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Placeable Object", menuName = "Game/Placeable Object")]
public class PlaceableObject : ScriptableObject
{
    new public string name;
    public Texture itemTexture;
    public GameObject prefab;
}
