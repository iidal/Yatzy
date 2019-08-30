using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCalculator
{
    static int[] nums = new int[5];
    static string type;

    public static void StartCalculating(SingleLine[] lines, int[] results) {

        results.CopyTo(nums,0);

        foreach (SingleLine sl in lines) {
            type = sl.lineType;

            //upper part
            if (type.Contains("upper")) {
                CalculateUpperPart(sl);
            }

            switch (type) {
                case "test":
                    Debug.Log("test");
                    break;
                defult:
                    Debug.Log("default");
                    break;
            }
        }
    }
   static void CalculateUpperPart(SingleLine sl) {

       int amount = 0;
       int lineNum = System.Int32.Parse(sl.lineType.Replace("upper", ""));
       foreach (int i in nums)
       {
            if (i == lineNum) {
                amount += i;
            }
       }
        sl.SetPoint(amount);
    }
}
