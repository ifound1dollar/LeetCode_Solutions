using System;
using System.Collections.Generic;

namespace LeetCode_Template
{
    public class TreeNode
    {
        public int val;
        public TreeNode left;
        public TreeNode right;
        public TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
        {
            this.val = val;
            this.left = left;
            this.right = right;
        }
    }
    class Program
    {
        static void Main()
        {
            ///Main method makes a call to this problem's dedicated method, then prints the value and exits.
            
            //here are this problem's variables
            TreeNode returnItem;
            int[] argumentItem = new int[] { -10, -3, 0, 5, 9 };

            //method call
            returnItem = SortedArrayToBST(argumentItem);

            //print value
            Console.WriteLine("RETURN ITEM: " + returnItem);
        }


        ///#108 Convert Sorted Array to Binary Search Tree, 08/10/2022
        static TreeNode SortedArrayToBST(int[] nums)
        {
            //empty array check
            if (nums.Length == 0)
            {
                return null;
            }

            //creates the head object, CreateNode will handle all left and right TreeNodes in tree
            TreeNode head = CreateNode(nums, 0, nums.Length - 1);

            return head;
        }
        static TreeNode CreateNode(int[] nums, int min, int max)
        {
            //if min is now more than max, every item in array is now part of tree, so return null
            if (min > max)
            {
                return null;
            }

            //set to midpoint of min and max (integer division, so floor operation)
            int mid = (min + max) / 2;

            //create new TreeNode with val of array at mid index
            TreeNode treeNode = new(val: nums[mid])
            {
                ///EXPLANATION
                ///Recursively call CreateNode for left (less than mid) and right (more than
                /// mid) TreeNodes. The array is passed without change; if using the left side
                /// of the array, the minimum value should be passed with this mid value (MINUS
                /// 1 AS TO EXCLUDE ITSELF) as the maximum. OTHERWISE, if using the right side,
                /// the minimum value should be this mid value (AGAIN, EXCLUDING ITSELF).
                ///Because the call is recursive, each new TreeNode only knows the max and min
                /// values from the previously calling method.
                /// 
                ///EXAMPLE: [ -10, -3, 0, 5, 9 ]
                /// The head object calls CreateNode to assign val = 0 (mid item), where min
                /// and max are the actual beginning and end of the array. When CreateNode is
                /// called for the left TreeNode, it only uses indices 0 and 1 (excluding
                /// itself, 2) as the min and max. The right side only uses 3 and 4. Each of
                /// these will perform the same operation (mid always uses floor operation,
                /// remember) until min is greater than max (min becomes greater than max
                /// because mid is always excluded, resulting in a +1 or -1 operation).
                ///The end of this recursive process will ensure that every item in the array
                /// is assigned as a left or right TreeNode; the absolute bottom TreeNodes
                /// will have left and right both be null.

                left = CreateNode(nums, min, mid - 1),
                right = CreateNode(nums, mid + 1, max)
            };

            return treeNode;
        }


        ///#823 Binary Trees With Factors, 08/09/2022
        static int NumFactoredBinaryTrees(int[] arr)
        {
            //sort array and declare longs for number found and modulo value
            Array.Sort(arr);
            long numFound = 0;
            long mod = (long)(Math.Pow(10, 9) + 7);

            //create array corresponding to arr that will store number of factors for each item in arr
            long[] factors = new long[arr.Length];

            //iterate through every item in arr
            for (int i = 0; i < arr.Length; i++)
            {
                //immediately set factor to 1, because each should count itself once right away
                factors[i] = 1;

                //iterate from start but stopping before reaching i (current largest item)
                for (int j = 0; j < i; j++)
                {
                    //IF the larger item (i) is divisible by the smaller item (j)
                    if (arr[i] % arr[j] == 0)
                    {
                        //check if the target value (arr[i]/arr[j]) exists in arr using BinarySearch
                        int index = Array.BinarySearch(arr, arr[i] / arr[j]);

                        //if index is non-negative, it exists and BinarySearch returned a valid index
                        if (index >= 0)
                        {
                            ///EXPLANATION
                            ///Multiply the FACTOR VALUES of the smaller item (j) and this item (index)
                            /// and add to factors[i]. This simulates including subtrees of a parent
                            /// tree.
                            ///EXAMPLE: [ 2, 3, 6, 18] would result in 6 having three potential
                            /// representations of itself: [6], [2,3], and [3,2]. Each different
                            /// representation must be added to the total factors of 18 when it is
                            /// the item at index i. 18's factor would be set to 1 immediately, then
                            /// its factors of [3,6] and [6,3] would multiply the factors of 3(1)
                            /// and 6(3) both times to ensure every case is included (total 7 for 18).
                            ///The addition operation uses the mod operator before assignment to
                            /// prevent int32 overflow problems.
                            ///After the assignment, break from the loop because there can only be
                            /// ONE other item to complete the potential binary tree.

                            factors[i] += (factors[j] * factors[index]) % mod;
                        }
                    }
                }

                //after adding each potential binary tree to factors, add to numFound (and prevent int32 overflow)
                numFound = (numFound + factors[i]) % mod;
            }

            //finally, cast numFound to int and return
            return (int)numFound;
        }


        ///#704, 08/09/2022
        static int BinarySearchTargetIndex(int[] nums, int target)
        {
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


        ///#300, 08/08/2022
        static int LongestIncreasingSubsequence(int[] nums)
        {
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
