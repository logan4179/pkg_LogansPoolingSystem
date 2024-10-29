using System.Collections.Generic;
using UnityEngine;

//NOTE: older objects are NOT currently deactivated, but rather get cycled to the newest position in the world.
namespace LogansPoolingSystem
{
    public class Pool : MonoBehaviour
    {
        [Header("---------------[[ FOR PREFAB INSTANTIATION ]]-----------------")]
        [SerializeField, Tooltip("Prefab for spawning the pool with")]
        protected GameObject poolPrefab;

        //[Header("---------------[[ REFERENCE ]]-----------------")]
        protected GameObject[] pooledObjects;

        [Header("---------------[[ STATS ]]-----------------")]
        [SerializeField] protected int count_allowedActiveInScene;

        protected int index_lastMadeActive = -1;

        [SerializeField] protected bool rotateRandom = false;
		[SerializeField, Tooltip("Allows you to randomly 'jitter' the spawn position around the spawn point")] protected float randomSpawnOffset = 0f;

		protected virtual void Awake()
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
        /// Main method for spawning/cycling a new pooled instance at an exact locaton and rotation (ignores any ramdoms)
        /// </summary>
        /// <param name="spawnPoint"></param>
        /// <param name="rot_passed"></param>
        /// <returns></returns>
        public virtual GameObject CycleSpawnExact( Vector3 spawnPoint, Quaternion rot_passed )
        {
			int nextPos = LPS_Utils.GetLoopedIndex( pooledObjects.Length, index_lastMadeActive + 1 );
			GameObject go = pooledObjects[nextPos];

			index_lastMadeActive = nextPos;

            go.transform.position = spawnPoint;
            go.transform.rotation = rot_passed;

			go.SetActive( true );

            return go;
        }

		/// <summary>
		/// This overload allows you to rotate the instance to look down vNormal.
		/// </summary>
		/// <param name="spawnPoint"></param>
		/// <param name="vNormal"></param>
		/// <returns></returns>
		public virtual GameObject CycleSpawn( Vector3 spawnPoint, Vector3 vNormal )
		{
			int nextPos = LPS_Utils.GetLoopedIndex( pooledObjects.Length, index_lastMadeActive + 1 );
			GameObject go = pooledObjects[nextPos];

			index_lastMadeActive = nextPos;

			go.transform.position = spawnPoint;

			if ( rotateRandom )
			{
				go.transform.rotation = Quaternion.AngleAxis( Random.Range(0f, 360f), vNormal) * Quaternion.LookRotation(vNormal);
			}
			else
			{
				go.transform.rotation = Quaternion.LookRotation( vNormal );
			}

			go.SetActive( true );

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