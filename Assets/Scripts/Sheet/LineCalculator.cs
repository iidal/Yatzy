using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCalculator
{

    /// <summary>
    /// All lines except upper bonus are calculated here and sent to the lines own script
    /// Upper bonus is not calculated here because this script only uses the results from the dices, upper bonus needs the points from lines 1-6 that this script does not use
    /// </summary>

    static int[] nums = new int[5];
    static string type;

    public static void StartCalculating(SingleLine[] lines, int[] results) {

        results.CopyTo(nums, 0);

        foreach (SingleLine sl in lines) {
            type = sl.lineType;

            //Checking and calculating lines 1-6 
            if (type.Contains("upper"))
            {
                CalculateUpperPart(sl);
            }
            //checking the rest of the lines
            else
            {

                switch (type)
                {
                    case "pair":
                        CalculateAmountOfSameDices(sl, 2);
                        break;
                    case "3X":
                        CalculateAmountOfSameDices(sl, 3);
                        break;
                    case "4X":
                        CalculateAmountOfSameDices(sl, 4);
                        break;
                    case "yatzy":
                        CalculateYatzy(sl);
                        break;
                    case "random":
                        CalculateRandom(sl);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    //lines 1-6
    static void CalculateUpperPart(SingleLine sl) {

        int amount = 0;
        int lineNum = System.Int32.Parse(sl.lineType.Replace("upper", ""));
        foreach (int i in nums)
        {
            if (i == lineNum) {
                amount += i;
            }
        }
        sl.SetDicePoints(amount);
    }

    //Three or four same, pair
    static void CalculateAmountOfSameDices(SingleLine sl, int threeOrFour){
        int amount = 0; //points to be given if three same dices
        Dictionary<int, int> tempDict = new Dictionary<int, int>();
        foreach (int i in nums) {
            if (tempDict.ContainsKey(i))
            {
                tempDict[i] += 1;
            }
            else {
                tempDict.Add(i,1);
            }
            amount += i;
        }

        foreach (KeyValuePair<int, int> kvp in tempDict) {
            if (kvp.Value >= threeOrFour) {
                sl.SetDicePoints(amount);
                return;
            }
        }
        //HOX if there are enough of the same number code beyond this point(return) will not be executed
        sl.SetDicePoints(0);

        //foreach (KeyValuePair<int, int> kvp in tempDict) {
        //    Debug.Log(kvp.Key +" "+ kvp.Value);
        //}
   

    }

    //Random
    static void CalculateRandom(SingleLine sl){
        int amount = 0;
        foreach (int i in nums)
        {
            amount += i;
        }
        sl.SetDicePoints(amount);
        
    }

    //Yatzy
    static void CalculateYatzy(SingleLine sl) {
        int digit = nums[0];
        foreach (int i in nums) {
            if (i != digit) {
                sl.SetOtherPoints(false);
                return;
            }
        }
        sl.SetOtherPoints(true);
    }
}
