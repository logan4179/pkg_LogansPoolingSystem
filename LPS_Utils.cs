using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogansPoolingSystem
{
    public static class LPS_Utils
    {
		/// <summary>
		/// Returns a 'looped' index, meaning if the index goes above the passed list count, or below 0, it will loop the index while
		/// staying within the list bounds.
		/// </summary>
		/// <param name="listCount_passed">count property of the list you're currently intending to cycle through</param>
		/// <param name="index_passed">Your intended index. If the index goes above the count of the list that you pass, or below 0,
		/// it will use this index as a staring point to determine the cycled index.</param>
		/// <returns></returns>
		public static int GetLoopedIndex(int listCount_passed, int index_passed)
		{
			//TODO: Check for if the passed index is multiple times larger (or smaller, IE: negatives) than the passed list's count...

			if (index_passed >= listCount_passed)
			{
				return index_passed - listCount_passed;
			}
			else if (index_passed < 0)
			{
				return listCount_passed - Mathf.Abs(index_passed);
			}
			else
			{
				return index_passed;
			}
		}
	}
}
