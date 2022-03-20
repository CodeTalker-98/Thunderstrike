using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave.asset", menuName = "Scriptable Objects/New Wave")]
public class Waves : ScriptableObject
{
    public List<GameObject> sequence;
}
