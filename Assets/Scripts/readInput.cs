using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class readInput : MonoBehaviour
{
    public List<string> input = new List<string>();

    public void ReadStringInput(string s)
    {
        input.Add(s);
    }
}
