using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Sometimes I create Method that are usefull in every project, so I put them here
// I'm maybe going to put it on github one day or the other
public class YovaUtilities
{

    //Method that return a list of GameObject which are the Child of 'obj' and have the tag 'tag'
    public static List<GameObject> FindChildrenWithTag(GameObject obj, string tag)
    {
        List<GameObject> result = new List<GameObject>();

        foreach (Transform child in obj.transform)
        {
            GameObject childObj = child.gameObject;
            if (childObj.CompareTag(tag))
            {
                result.Add(childObj);
            }
            result.AddRange(FindChildrenWithTag(childObj, tag));

        }

        return result;
    }
    //Method that return the Maximum Value of a list or an array of float
    public static float GetMaxValue(IEnumerable list)
    {
        float result = 0;
        foreach (float item in list)
        {
            if(item > result)
            {
                result = item;
            }
        }
        return result;
    }

    //Method that return the Minimum Value of a list or an array of float
    public static float GetMinValue(IEnumerable list)
    {
        float result = 0;
        foreach (float item in list)
        {
            if (item < result)
            {
                result = item;
            }
        }
        return result;
    }

    //Method that return the Sum of a list or an array of float
    public static float GetSum(IEnumerable list)
    {
        float result = 0;
        foreach (float item in list)
        {
            result += item;   
        }
        return result;
    }

    //Method that normalize an array of float
    public static float[] NormalizeArray(float[] array, bool useMax = false)
    {
        float[] normalizedArray = new float[array.Length];
        float maxValue = GetMaxValue(array);
        float sumValue = GetSum(array);
        float value = sumValue;
        if (useMax) { value = maxValue; }

        for (int i = 0; i < array.Length; i++)
        {
            normalizedArray[i] = array[i] / value;
        }
        return normalizedArray;
    }
}
