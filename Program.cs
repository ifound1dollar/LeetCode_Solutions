using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            string returnItem;
            string argumentItem = "<div>Some text probably<<div><";

            //method call
            returnItem = CloseDivTags(argumentItem);

            //print value
            Console.WriteLine("RETURN ITEM: " + returnItem);
        }



        //  Mock interview situation, close all <div> tags which are not closed
        static string CloseDivTags(string str)
        {
            //EXAMPLE: "<div>Some text probably<div>", second <div> must be closed
            bool isAwaitingSecond = false;
            StringBuilder sb = new();
            for (int i = 0; i < str.Length; i++)
            {
                //add current char then check if it is valid "<div>" tag
                sb.Append(str[i]);
                if (str[i] == '<')
                {
                    if (i + 4 >= str.Length)
                    {
                        //if 4 more characters is outside bounds, cannot be "<div>"
                        continue;
                    }

                    bool isValidDiv = true;
                    string target = "div>";
                    for (int j = 0; j < 4; j++)
                    {
                        if (str[i + j + 1] != target[j])
                        {
                            //checks next 4 characters to see if there is a mismatch from "div>"
                            isValidDiv = false;
                            break;
                        }
                    }
                    if (!isValidDiv) { continue; }

                    if (!isAwaitingSecond)
                    {
                        //this is first <div> tag found, so now awaiting second to close
                        isAwaitingSecond = true;
                    }
                    else
                    {
                        //else awaiting second to close, so append '/' before continuing to rest of tag
                        sb.Append('/');
                    }
                }
            }

            return sb.ToString();
        }



        //  #15 3Sum, 02/01/2023
        static IList<IList<int>> ThreeSum(int[] nums)
        {
            IList<IList<int>> triplets = new List<IList<int>>();
            int size = nums.Length;
            if (size < 3)
            {
                return triplets;
            }

            Array.Sort(nums, 0, size);
            for (int i = 0; i < size; i++)
            {
                if (nums[i] > 0)
                {
                    //since in ascending order, if greater than 0, there cannot be a solution
                    break;
                }

                //define search value (0 sum is goal) and two pointers for indices
                int searchVal = -nums[i];
                int startIndex = i + 1;
                int endIndex = size - 1;

                //will effectively move pointers together until they meet, searching for a value in O(n) time
                while (startIndex < endIndex)
                {
                    if (nums[startIndex] + nums[endIndex] < searchVal)
                    {
                        //if sum is less than target, should move startIndex up (increases sum)
                        startIndex++;
                    }
                    else if (nums[startIndex] + nums[endIndex] > searchVal)
                    {
                        //else if sum is greater than target, should move endIndex down (decreases sum)
                        endIndex--;
                    }
                    else
                    {
                        //else equal so is valid triplet
                        List<int> triplet = new() { nums[i], nums[startIndex], nums[endIndex] };
                        triplets.Add(triplet);

                        //keep moving startIndex and endIndex until different values than current
                        while (startIndex < endIndex && nums[startIndex] == triplet[1])
                        {
                            startIndex++;
                        }
                        while (startIndex < endIndex && nums[endIndex] == triplet[2])
                        {
                            endIndex--;
                        }
                    }
                }

                //move i until its NEXT value is different (increments at top of loop)
                while (i + 1 < size && nums[i] == nums[i + 1])
                {
                    i++;
                }
            }

            return triplets;
        }



        //  #1626 Best Team With No Conflicts, 01/31/2023
        static int BestTeamWithoutConflicts(int[] scores, int[] ages)
        {
            //sort by order of age, moving scores along with reorders
            for (int i = 0; i < ages.Length - 1; i++)
            {
                int smallestIndex = i;
                for (int j = i + 1; j < ages.Length; j++)
                {
                    if (ages[j] < ages[smallestIndex])
                    {
                        smallestIndex = j;
                    }
                }

                int tempAge = ages[i];
                int tempScore = scores[i];
                ages[i] = ages[smallestIndex];
                scores[i] = scores[smallestIndex];
                ages[smallestIndex] = tempAge;
                scores[smallestIndex] = tempScore;
            }

            //reorder each score within the same exact age (ex. three age 1s)
            for (int i = 0; i < ages.Length - 1; i++)
            {
                int smallestIndex = i;
                for (int j = i + 1; j < ages.Length && ages[i] == ages[j]; j++)
                {
                    if (scores[j] < scores[smallestIndex])
                    {
                        smallestIndex = j;
                    }
                }
                (scores[smallestIndex], scores[i]) = (scores[i], scores[smallestIndex]);
            }
            //NOTE: above simply sorts ascending by age first, then score within age


            //calculates best possible combination at this index (i) and stores in bests[]
            int currentBest = 0;
            int[] bests = new int[ages.Length];
            for (int i = 0; i < ages.Length; i++)
            {
                //set init to score at[i], then iterate through all values smaller than at[i]
                //  checks all values up to at[i], increasing bests[i] if a greater value is found
                bests[i] = scores[i];
                for (int j = 0; j < i; j++)
                {
                    //since is already ordered ascending, can just check if at[j] is less than at[i]
                    if (scores[j] <= scores[i])
                    {
                        //it is possible that score at[i] plus best at[j] is not more than current best
                        //THUS, only add best at[j] and score at[i] if it is greater than bests at[i]
                        //  NOTE: can be interpreted as an inner 'best' check before the main check below
                        if (bests[j] + scores[i] > bests[i])
                        {
                            bests[i] = bests[j] + scores[i];
                        }
                    }
                }

                //if the determined best at[i] is greater that current best, set currentBest
                if (bests[i] > currentBest)
                {
                    currentBest = bests[i];
                }
            }

            return currentBest;
        }



        /// #334 Increasing Triplet Subsequence, 10/11/2022
        static bool IncreasingTriplet(int[] nums)
        {
            //set ints for first and second item to maximum value allowed (problem does not exceed)
            int first = int.MaxValue;
            int second = int.MaxValue;

            //iterate through every item
            for (int i = 0; i < nums.Length; i++)
            {
                //if this item is less than or equal to first, set first to it (probably always set at first iteration)
                //NOTE: any subsequent items smaller than first will decrease first
                if (nums[i] <= first)
                {
                    first = nums[i];
                }
                //else if less than or equal to second, set second to it (will always be greater than first)
                //NOTE: acts as the largest item until a num between first and second is found
                else if (nums[i] <= second)
                {
                    second = nums[i];
                }
                //IF THIS ELSE IS REACHED, first is less than second (logically)
                //  AND [this item] is greater than second (otherwise would have been caught)
                else
                {
                    //because first < second < [this item], a triplet is found, so return true
                    return true;
                }
            }

            //if triplet not found, return false
            return false;




            ///WORKS, BUT TAKES FAR TOO LONG

            ////start by iterating through every num except last two, because 3 elements required
            //for (int i = 0; i < nums.Length - 2; i++)
            //{
            //    //iterate through all characters after i, until last one because 2 more elements required
            //    for (int j = i + 1; j < nums.Length - 1; j++)
            //    {
            //        //if this element is greater than first (i), check all elements after
            //        if (nums[i] < nums[j])
            //        {
            //            //iterate through all characters after j including last one
            //            for (int k = j + 1; k < nums.Length; k++)
            //            {
            //                //if this element is greater than second (j), which was greater than first, triplet found
            //                if (nums[j] < nums[k])
            //                {
            //                    return true;
            //                }
            //            }
            //        }
            //    }
            //}
        }



        /// #3 Longest Substring Without Repeating Characters, 10/11/2022
        static int LongestSubstring(string s)
        {
            int longest = 0;
            int current = 0;
            int currentStartIndex = 0;

            //for each char in string param
            for (int i = 0; i < s.Length; i++)
            {
                //integer to store location of this item found in string (if any)
                int inString = -1;

                //iterate through each item in current (corresponds to substring of param string)
                for (int j = 0; j < current; j++)
                {
                    //if matching char, starting at currentStartIndex and going until end of current (j)
                    if (s[i] == s[currentStartIndex + j])
                    {
                        //note the last position where this item was found (so don't break)
                        inString = j;
                    }
                }

                //if found repeating
                if (inString != -1)
                {
                    //remove everything before the repeating character by subtracting the index
                    //NOTE: index will be 1 less than the length, but current is at a new index which should be added
                    current -= inString;

                    //move currentStartIndex up by index of inString (plus 1 because indices start at 0)
                    currentStartIndex += inString + 1;
                }
                //else not already in string
                else
                {
                    //increment because another valid character was found
                    current++;
                }

                //if current is greater than longest, set longest to current
                if (current > longest)
                {
                    longest = current;
                }
            }

            return longest;
        }



        /// #1559 Detect Cycles in 2D Grid, 08/17/2022
        static readonly int[] X_CHANGE = { 1, -1, 0, 0 };
        static readonly int[] Y_CHANGE = { 0, 0, 1, -1 };
        static bool ContainsCycle(char[][] grid)
        {
            //populate 2d array of bools to determine which node has been visited yet
            bool[][] visited = new bool[grid.Length][];
            for (int i = 0; i < visited.Length; i++)
            {
                visited[i] = new bool[grid[i].Length];
            }

            //iterate through every item in 2d array, using i and j
            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[0].Length; j++)
                {
                    //only call method if the current point has not yet been visited, because any
                    //  point that is potentially a cycle is interconnected to all others of the
                    //  same startChar
                    if (!visited[i][j])
                    {
                        //each recursive return will be either true or false, only setting to true
                        //  if a cycle was found, so only return true if true was returned from method
                        if (ContainsCycleDFS(grid, visited, -1, -1, i, j))
                        {
                            return true;
                        }
                    }
                }
            }

            //will return true above if/when a cycle is found, so if reaches this point, no cycle was found
            return false;
        }
        static bool ContainsCycleDFS(char[][] grid, bool[][] visited,
            int preX, int preY, int curX, int curY)
        {
            //declare bool for whether a cycle was found and set this node to visited
            visited[curX][curY] = true;

            //iterate through all four adjacent nodes, validity checked inside loop
            for (int i = 0; i < 4; i++)
            {
                //use arrays from above to alter X and Y accordingly (x changes in 0, 1 and y changes in 2, 3)
                int newX = curX + X_CHANGE[i];
                int newY = curY + Y_CHANGE[i];

                //check for validity of point, non-negative and not more than bounds of grid
                if ((newX >= 0 && newX < grid.Length) && (newY >= 0 && newY < grid[0].Length))
                {
                    //don't visit previous point (as long as AT LEAST ONE is different, is valid)
                    if (newX != preX || newY != preY)
                    {
                        //only if the char at the new point is the same as the original
                        if (grid[newX][newY] == grid[curX][curY])
                        {
                            //if the new point has been visited after all previous checks, return true
                            //if the recursive call (using curX/Y as preX/Y AND newX/Y as curX/Y) returns
                            //  true, return true further up the chain
                            if (visited[newX][newY] || ContainsCycleDFS(grid, visited, curX, curY, newX, newY))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            //will always return true BEFORE here if a cycle is found, so return false
            return false;
        }



        ///#804 Unique Morse Code Words, 08/17/2022
        static int UniqueMorseRepresentations(string[] words)
        {
            //instantiate string[] of morse code transformations for each character from a - z
            string[] morseValues = new string[]
            {
                //13 per line, first: a - n, second: o - z
                ".-", "-...", "-.-.", "-..", ".", "..-.", "--.", "....", "..", ".---", "-.-", ".-..", "--",
                "-.", "---", ".--.", "--.-", ".-.", "...", "-", "..-", "...-", ".--", "-..-", "-.--", "--.."
            };

            //create LinkedList to hold every unique transformation
            LinkedList<string> result = new();

            //iterate through each word
            foreach (string word in words)
            {
                //declare empty string for transformation
                string transformation = "";

                //iterate through each letter (as char) in word
                foreach (char letter in word.ToCharArray())
                {
                    //append the current morse value to transformation string (accesses morseValues
                    //  at the ASCII value of letter MINUS 97 because the ASCII value for a is 97,
                    //  allowing direct access to the array)
                    transformation += morseValues[letter - 97];
                }

                //after the transformation string is made, check if it is not already in result LinkedList
                if (!result.Contains(transformation))
                {
                    //if not already in result, add to it
                    result.AddLast(transformation);
                }
            }

            //return number of items in result LinkedList
            return result.Count;
        }



        ///#39 Combination Sum, 08/17/2022
        static IList<IList<int>> CombinationSum(int[] candidates, int target)
        {
            ///this will find every possible combination of any size that when summed will equal 'target'

            //instantiate ilist of ilists for entire result and temporary list to be added after recursion
            IList<IList<int>> result = new List<IList<int>>();
            List<int> temp = new();
            //use integer instead of List.Sum() for efficiency
            int tempSum = 0;

            //start of recursive call that assigns all valid values to 'result'
            FindCombinationSum(result, temp, tempSum, start: 0, candidates, target);

            //after recursive operations, return result
            return result;
        }
        static void FindCombinationSum(IList<IList<int>> result, List<int> temp, int tempSum,
            int start, int[] candidates, int target)
        {
            ///this recursive method will add the value at this step, then check if the sum is
            /// too high, correct, or less than the target, and do actions accordingly

            //if higher than target, return now since this value can't be valid for this recursive step
            if (tempSum > target)
            {
                return;
            }
            //else if same as target, then a valid list has been found that sums to target
            else if (tempSum == target)
            {
                //add NEW OBJECT with 'temp' values to the 'result' object, then return
                result.Add(new List<int>(temp));
                return;
            }
            ///IMPLICIT ELSE AFTER RETURN STATEMENTS

            //IF LESS THAN TARGET, iterate through the remainder, starting from 0
            //NOTE: starting from 0 will prevent some later operations that would be completely
            //  unnecessary (starting from highest can try to add a very small number multiple
            //  times, but starting from the smallest may instead just add one larger number once)
            for (int i = start; i < candidates.Length; i++)
            {
                //add this item to 'temp' (will be removed below after recursive call)
                temp.Add(candidates[i]);
                tempSum += candidates[i];

                //after adding item, recursively call this method (using same value for i allows the
                //  same number to be added more than once UNTIL 'temp' becomes higher than target,
                //  which then causes the recursion to return)
                FindCombinationSum(result, temp, tempSum, i, candidates, target);

                //when recursion returns, will move on to the next lowest value and try again AFTER
                //  removing the recently added value to 'temp' (the item at this index will have been
                //  exhausted and can not possibly have any more combinations; was either greater than
                //  or equal to target value)
                temp.RemoveAt(temp.Count - 1);
                tempSum -= candidates[i];
            }
        }



        ///#77 Combinations, 08/17/2022
        static IList<IList<int>> Combine(int n, int k)
        {
            ///this method finds all combinations of length k in a list from 1 to n; [ 1, 2, 3, 4 ] if n = 4
            
            //instantiate ilist of ilists for entire result and temporary list to be added after recursion
            IList<IList<int>> result = new List<IList<int>>();
            List<int> temp = new();

            //start of recursive calls to this method, where 1 is the starting value (1 to n)
            FindCombinationsFromStartIndex(result, temp, 1, n, k);

            //after recursive operations, return object
            return result;
        }
        static void FindCombinationsFromStartIndex(IList<IList<int>> result, List<int> temp, int start, int n, int k)
        {
            ///temp stores current list until complete; k will denote how many remaining characters are needed
            /// for complete temp List; once 0, add the list to 'result' and return
            
            //if k is 0, no more items need to be added to 'temp'
            if (k == 0)
            {
                //add temp as NEW OBJECT, as to not reference the same object each time, then return
                result.Add(new List<int>(temp));
                return;
            }

            //loop through all items AFTER 'start' and UP TO the maximum number of remaining characters
            //  that can be added (if k = 2, then 2 characters remain and thus the last character CANNOT
            //  possibly be the second item in 'temp' List)
            for (int i = start; i <= n - k + 1; i++)
            {
                //add this index, because it is a valid part of 'temp' list (ex. with k of 3, i = 1 can
                //  count as the first item, as could the second as long as i <= n - k + 1)
                temp.Add(i);

                //RECURSIVE CALL, using i + 1 because must start at next item and using k + 1 because
                //  there is 1 less character that must be added to 'temp' (if k = 0, will return when
                //  reaching if statement above, and for loop will continue for as long as necessary)
                FindCombinationsFromStartIndex(result, temp, i + 1, n, k - 1);

                //after recursive call, this specific item (i) must be removed to make room for the next
                temp.RemoveAt(temp.Count - 1);
            }
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
