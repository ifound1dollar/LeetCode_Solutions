using System;
using System.Collections.Generic;

namespace LeetCode_Template
{
    class Program
    {
        static void Main()
        {
            ///Main method makes a call to this problem's dedicated method, then prints the value and exits.
            
            //here are this problem's variables
            int returnItem;
            int[] argumentItem = new int[] { 2, 3, 6, 18 };

            //method call
            returnItem = NumFactoredBinaryTrees(argumentItem);

            //print value
            Console.WriteLine("RETURN ITEM: " + returnItem);
        }

        static int NumFactoredBinaryTrees(int[] arr)
        {
            ///#823, 08/09/2022 (look-up)

            //sort array and declare longs for number found and modulo value
            Array.Sort(arr);
            long numFound = 0;
            long mod = (long)(Math.Pow(10, 9) + 7);

            //declare dictionary to store number of factors of each int in array, then iterate
            Dictionary<int, long> factors = new();
            for (int i = 0; i < arr.Length; i++)
            {
                //always includes itself right away
                factors[arr[i]] = 1;

                //iterate from beginning up to index i
                for (int j = 0; j < i; j++)
                {
                    //if the item at index i is divisible by the item at index j
                    if (arr[i] % arr[j] == 0)
                    {
                        //Set value at index i dictionary key to its existing value (originally 1) PLUS
                        //  the value at index j TIMES the value that MAY EXIST when i and j's values
                        //  are divided (if not, defaults to 0 and only adds 1 to factors[arr[i]]).
                        //This operation should simulate adding all subtrees to a parent tree.
                        //Modulo operation is used in case of int32 overflow.
                        factors[arr[i]] = (factors[arr[i]]
                            + (factors[arr[j]] * factors.GetValueOrDefault(arr[i] / arr[j]))) % mod;
                    }
                }

                //after each i iteration, add value at dictionary key to numFound (and prevent int32 overflow)
                numFound = (numFound + factors[arr[i]]) % mod;
            }

            //finally, cast numFound to int and return
            return (int)numFound;
        }

        static int BinarySearchTargetIndex(int[] nums, int target)
        {
            ///#704, 08/09/2022

            //variables for indices of minimum, maximum
            int min = 0;
            int max = nums.Length - 1;

            //iterate until max > min (include equality in case last possible iteration is target)
            while (min <= max)
            {
                //set mid index to half (or just below half for odd size, ex. 4.5 -> 4)
                int mid = (min + max)/2;

                //if mid is the correct index for target, return it
                if (nums[mid] == target)
                {
                    return mid;
                }
                //else if mid is less than target, set min to mid + 1 (exclude this mid item)
                else if (nums[mid] < target)
                {
                    min = mid + 1;
                }
                //else mid is greater than target, so set max to mid - 1 (again, exclude this mid item)
                else
                {
                    max = mid - 1;
                }
            }

            //only reaches here if target is not in array, which then should return -1
            return -1;
        }
        static int LongestIncreasingSubsequence(int[] nums)
        {
            ///#300, 08/08/2022
            
            //create List and add first item of array, then iterate through array
            List<int> subList = new() { nums[0] };
            for (int i = 1; i < nums.Length; i++)
            {
                //if greater than item at last index of List, add to List
                if (nums[i] > subList[^1])
                {
                    subList.Add(nums[i]);
                }
                //else if smaller than item at FIRST index of List, replace first item
                else if (nums[i] <= subList[0])
                {
                    subList[0] = nums[i];
                }
                //else somewhere in between first and last items, so insert right after closest smaller item
                else
                {
                    //iterate through each item in subList
                    for (int j = subList.Count - 1; j > 0; j--)
                    {
                        //if smaller than this index but larger than previous, replace this and break
                        if (nums[i] > subList[j - 1])
                        {
                            subList[j] = nums[i];
                            break;
                        }
                    }
                }
            }
            
            return subList.Count;
        }
    }
}
