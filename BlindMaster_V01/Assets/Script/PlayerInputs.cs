using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    private static Dictionary<KeyCode , string> BasicKeysDir = new Dictionary<KeyCode , string>()
    {
        {KeyCode.A , "A"},
        {KeyCode.S , "S"},
        {KeyCode.D , "D"},
        {KeyCode.F , "F"},
        {KeyCode.G , "G" },
        {KeyCode.H,"H" },
        {KeyCode.J, "J" },
        {KeyCode.K,"K" },
        {KeyCode.L,"L" },
        {KeyCode.Semicolon,";" },

        //Top raw 
         {KeyCode.Q , "Q"},
        {KeyCode.W , "W"},
        {KeyCode.E , "E"},
        {KeyCode.R , "R"},
        {KeyCode.T , "T" },
        {KeyCode.Y,"Y" },
        {KeyCode.U, "U" },
        {KeyCode.I,"I" },
        {KeyCode.O,"O" },
        {KeyCode.P,"P" },

        //Bottom raw
        {KeyCode.Z , "Z"},
        {KeyCode.X , "X"},
        {KeyCode.C , "C"},
        {KeyCode.V , "V"},
        {KeyCode.B , "B" },
        {KeyCode.N,  "N" },
        {KeyCode.M, "M" },
        {KeyCode.Comma,"," },
        {KeyCode.Period,"." },
        {KeyCode.Slash,"/" },

    };


    public static int GetDictionaryCount()
    {
        return BasicKeysDir.Count;
    }

    public static KeyCode GetKeyCode( int index )
    {
        return BasicKeysDir.ElementAt(index).Key;
    }
    public static string GetStringCode( int index )
    {
        return BasicKeysDir.ElementAt(index).Value;
    }


}


