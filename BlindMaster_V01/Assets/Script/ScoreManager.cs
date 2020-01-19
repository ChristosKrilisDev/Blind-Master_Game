using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager
{

    public static int finalScore;
    private static int nStars;

    public static int CalculateScore(int score , int errors)
    {
        finalScore = score - errors;


        if (finalScore >= 100)
        {
            //3 Star
            nStars = 3;
        }
        else if (finalScore >= 50)
        {
            //2 stars
            nStars = 2;
        }
        else if (finalScore >= 25)
        {
            //1 stars
            nStars = 1;
        }
        else if (finalScore < 0)
        {
            //Zero stars
            finalScore = 0;
            nStars = 1;
        }

        return nStars;
        
    }

}
