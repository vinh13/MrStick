using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionUtil
{

    public static void ResetTransform(this MonoBehaviour monoBehaviour)
    {

        monoBehaviour.transform.position = Vector3.zero;
        monoBehaviour.transform.localScale = Vector3.one;
        monoBehaviour.transform.rotation = Quaternion.identity;
    }

    public static T PickARandom<T>(this List<T> list)
    {
        if(list.Count == 0)
        {
            throw new System.Exception("The list get random need count > 0");
        }
        var index = Random.Range(0, list.Count);

        return list[index];
    }

    public static T PickARandom<T>(this T[] array)
    {
        if (array.Length == 0)
        {
            throw new System.Exception("The array get random need count > 0");
        }
        var index = Random.Range(0, array.Length);

        return array[index];
    }
}
