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
        //Setting up the dice row for checking it
        results.CopyTo(nums, 0);
       

 
        // checking line
        foreach (SingleLine sl in lines) {
            if(!sl.hasBeenPlayed){
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
                    case "2pairs":
                        CalculateTwoPairs(sl);
                        break;
                    case "3X":
                        CalculateAmountOfSameDices(sl, 3);
                        break;
                    case "4X":
                        CalculateAmountOfSameDices(sl, 4);
                        break;
                    case "house":
                        CalculateFullHouse(sl);
                        break;
                    case "sStraight":
                        CalculateStraight(sl, 4);
                        break;
                    case "lStraight":
                        CalculateStraight(sl, 5);
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

        //if the highest value in tempdict is equal or higher than the checked number, the dice row meets the requirements for giving points
        foreach (KeyValuePair<int, int> kvp in tempDict) {
            if (kvp.Value >= threeOrFour) {
                sl.SetDicePoints(amount);
                return;
            }
        }
        //HOX if there are enough of the same number code beyond this point(return) will not be executed
        sl.SetDicePoints(0);
    }

    //two pairs
    static void CalculateTwoPairs(SingleLine sl) {
        int amount = 0; //points to be given two pairs
        Dictionary<int, int> tempDict = new Dictionary<int, int>();
        foreach (int i in nums)
        {
            if (tempDict.ContainsKey(i))
            {
                tempDict[i] += 1;
            }
            else
            {
                tempDict.Add(i, 1);
            }
            amount += i;
        }
        //if tempdict has more than 3 kvps, two pairs is impossible, give 0 points
        if (tempDict.Count > 3) {
            sl.SetDicePoints(0);
            return;
        }
        //if the highest kvp value is 4 or more, the dice row is for example 11112, which counts as two pairs (2 sets of 1 1)
        foreach (KeyValuePair<int, int> kvp in tempDict)
        {
            if (kvp.Value >= 4)
            {
                sl.SetDicePoints(amount);
                return;
            }
        }
        //finally checking for two pairs, both with different digit (or a full house)
        int amountOfPairs = 0;
        foreach (KeyValuePair<int, int> kvp in tempDict)
        {
            if (kvp.Value ==2 || kvp.Value ==3)
            {
                amountOfPairs++;
            }
        }
        if (amountOfPairs >= 2)
        {
            sl.SetDicePoints(amount);
        }
        //if dice row is for example one pair or three same 
        else {
            sl.SetDicePoints(0);
        }

    }

    //full house
    static void CalculateFullHouse(SingleLine sl) {
        Dictionary<int, int> tempDict = new Dictionary<int, int>();
        foreach (int i in nums)
        {
            if (tempDict.ContainsKey(i))
            {
                tempDict[i] += 1;
            }
            else
            {
                tempDict.Add(i, 1);
            }
        }
        //checking if there is a 3x AND a pair, if yes, full house, else 0 points
        bool threeSame = false;
        bool pair = false;
        foreach (KeyValuePair<int, int> kvp in tempDict)
        {
            if (kvp.Value == 3)
            {
                threeSame = true;
            }
            if (kvp.Value == 2) {
                pair = true;
            }
        }
        if (threeSame && pair)
        {
            sl.SetOtherPoints(true);
        }
        else {
            sl.SetOtherPoints(false);
        }

    }

    //small and large straight
    static void CalculateStraight(SingleLine sl, int numsInRow) {
        //numsinrow= how many dices needs to be in a row for a straight
        //small = 4 digits in row
        //large = 5 digits in row
        List<int> numsList = new List<int>(); //make nums into list so Sort() can be used

        //not adding duplicate values to numslist, they are not needed for checking straight
        foreach (int i in nums) {

            bool exists = numsList.Exists(digit => digit == i);
            if (!exists)
            {
                numsList.Add(i);
            }
        }
        numsList.Sort();

        //there has to be enough different values for a straight to be possible (longer the list more different numbers)
        if (numsList.Count < numsInRow) {
            sl.SetOtherPoints(false);
            return;
        }
        //if enough different values, proceed

        int count = 1; //how many values are in row (cant be zero there is always the first dice)
        for (int i = 1; i<numsList.Count;i++) {
            //the previuous value plus 1 should be same as current value
            if (numsList[i] == (numsList[i -1] + 1))
            {
                count++;
            }
            else {//if not, start from one
                count = 1;
            }
        }
        //if enough numbers in row, give points
        if (count >= numsInRow) {
            sl.SetOtherPoints(true);
        }
        else
        {
            sl.SetOtherPoints(false);
        }

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
                SheetManager.instance.IsItYatzy(false);
                return;
            }
        }
        sl.SetOtherPoints(true);
        SheetManager.instance.IsItYatzy(true);
    }
}
