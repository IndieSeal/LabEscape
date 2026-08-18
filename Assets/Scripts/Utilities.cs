using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static T GetRandom<T>(this List<T> list) => list[GetRandomIndex(list)];
    public static int GetRandomIndex<T>(this List<T> list) => Random.Range(0, list.Count);
}