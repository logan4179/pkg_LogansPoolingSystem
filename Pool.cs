using System.Collections.Generic;
using UnityEngine;

namespace LogansPoolingSystem
{
    public class Pool : MonoBehaviour
    {
        [Header("---------------[[ FOR PREFAB INSTANTIATION ]]-----------------")]
        [SerializeField, Tooltip("Prefab for spawning the pool with")]
        private GameObject poolPrefab;

        //[Header("---------------[[ REFERENCE ]]-----------------")]
        private GameObject[] pooledObjects;

        [Header("---------------[[ STATS ]]-----------------")]
        [SerializeField] private int count_allowedActiveInScene;
        private int index_lastMadeActive = -1;

        private void Awake()
        {
            if ( poolPrefab != null && count_allowedActiveInScene > 0 )
            {
                pooledObjects = new GameObject[count_allowedActiveInScene];
                for ( int i = 0; i < count_allowedActiveInScene; i++ )
                {
                    GameObject g = Instantiate( poolPrefab, transform );
                    pooledObjects[i] = g;
                    g.SetActive( false );
                }
            }
            else if ( pooledObjects != null && pooledObjects.Length > 0 )
            {
                foreach ( GameObject g in pooledObjects )
                {
                    g.SetActive( false );
                }
            }
        }

        /// <summary>
        /// Main method for spawning/cycling a new pooled instance.
        /// </summary>
        /// <param name="pos_passed"></param>
        /// <param name="rot_passed"></param>
        /// <returns></returns>
        public GameObject CycleSpawnAtPosition( Vector3 pos_passed, Quaternion rot_passed )
        {
			int nextPos = GetLoopedIndex( pooledObjects.Length, index_lastMadeActive + 1 );
			GameObject go = pooledObjects[nextPos];

			index_lastMadeActive = nextPos;

            go.transform.position = pos_passed;
            go.transform.rotation = rot_passed;
			go.SetActive(true);

            return go;
        }

        /// <summary>
        /// Sets all pooled objects to inactive, disappearing them from the scene. Also resets pool spawn order.
        /// </summary>
        public void DeactivateMyObjects()
        {
			if ( pooledObjects != null && pooledObjects.Length > 0 )
			{
				foreach ( GameObject g in pooledObjects )
				{
					g.SetActive(false);
				}
			}

            index_lastMadeActive = -1;
		}

		/// <summary>
		/// Returns a list of the pooled objects that are currentlly active in the scene.
		/// </summary>
		/// <returns></returns>
		public List<GameObject> GetCurrentlyActiveInScene()
		{
			if (pooledObjects == null || pooledObjects.Length <= 0)
			{
				return null;
			}

			List<GameObject> activeObjects = new List<GameObject>();
			foreach (GameObject g in pooledObjects)
			{
				if (g.activeSelf)
				{
					activeObjects.Add(g);
				}
			}

			return activeObjects;
		}

		/// <summary>
		/// Returns a 'looped' index, meaning if the index goes above the passed list count, or below 0, it will loop the index while
		/// staying within the list bounds.
		/// </summary>
		/// <param name="listCount_passed">count property of the list you're currently intending to cycle through</param>
		/// <param name="index_passed">Your intended index. If the index goes above the count of the list that you pass, or below 0,
		/// it will use this index as a staring point to determine the cycled index.</param>
		/// <returns></returns>
		private int GetLoopedIndex(int listCount_passed, int index_passed)
		{
			//TODO: Check for if the passed index is multiple times larger (or smaller, IE: negatives) than the passed list's count...

			if ( index_passed >= listCount_passed )
			{
				return index_passed - listCount_passed;
			}
			else if ( index_passed < 0 )
			{
				return listCount_passed - Mathf.Abs( index_passed );
			}
			else
			{
				return index_passed;
			}
		}
	}
}