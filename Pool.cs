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
            else
            {
                DeactivateMyObjects();            
            }
        }

        /// <summary>
        /// Main method for spawning/cycling a new pooled instance. NOTE: older objects are NOT currently deactivated, 
		/// but rather get cycled to the newest position in the world.
        /// </summary>
        /// <param name="pos_passed"></param>
        /// <param name="rot_passed"></param>
        /// <returns></returns>
        public GameObject CycleSpawnAtPosition( Vector3 pos_passed, Quaternion rot_passed )
        {
			int nextPos = LPS_Utils.GetLoopedIndex( pooledObjects.Length, index_lastMadeActive + 1 );
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
			if ( pooledObjects == null || pooledObjects.Length <= 0 )
			{
				return null;
			}

			List<GameObject> activeObjects = new List<GameObject>();
			foreach ( GameObject g in pooledObjects )
			{
				if (g.activeSelf)
				{
					activeObjects.Add(g);
				}
			}

			return activeObjects;
		}
	}
}