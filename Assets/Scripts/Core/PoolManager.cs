using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolObject
{
	public string id;
	public GameObject objectPrefab;
	public int startCount = 5;

	public PoolObject(string id, GameObject objectPrefab, int startCount)
	{
		this.id = id;
		this.objectPrefab = objectPrefab;
		this.startCount = startCount;
	}
}

public class PoolManager : MonoBehaviour 
{
	static PoolManager _instance;
	static PoolManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = Instantiate<PoolManager> (Resources.Load<PoolManager> ("Prefabs/PoolManager")) as PoolManager;
				_instance.Init ();
			}
			return _instance;
		}
	}
	 
	public List<PoolObject> poolObjects;

	Dictionary<string, List<GameObject>> pool;

	void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
		}

		Init ();
	}

	bool initialized = false;

	void Init()
	{
		if (initialized)
		{
			return;
		}
		initialized = true;

		pool = new Dictionary<string, List<GameObject>>();

		foreach(PoolObject poolObject in instance.poolObjects)
		{
			InitPoolObject(poolObject);
		}
	}

	public static void InitPoolObject(PoolObject poolObject, bool increaseСapacity = false)
	{
		if (!increaseСapacity && instance.pool.ContainsKey (poolObject.id))
		{
			return;
		}

		if (!instance.poolObjects.Contains (poolObject))
		{
			instance.poolObjects.Add (poolObject);
		}

		List<GameObject> objectList = new List<GameObject> ((int)poolObject.startCount);

		for(int i = 0; i < poolObject.startCount; i++)
		{
			GameObject newGameObject = Instantiate (poolObject.objectPrefab, instance.transform);
			newGameObject.name = poolObject.id;
			newGameObject.SetActive (false);
			objectList.Add (newGameObject);
		}

		instance.pool.Add (poolObject.id, objectList);
	}

	PoolObject GetPoolObject(string id)
	{
		return instance.poolObjects.Find (x => x.id == id);
	}

	public static T Get<T>(string id)
	{
		return Get (id).GetComponent<T> ();
	}

	public static GameObject Get(string id)
	{
		if (!instance.pool.ContainsKey (id))
		{
			Debug.LogError ("Objects pool doesn't contain key: " + id);
			return null;
		}

		if (instance.pool [id].Count > 0)
		{
			GameObject returnObject = instance.pool [id] [0];
			instance.pool [id].RemoveAt (0);
			return returnObject;
		}
		else
		{
			PoolObject poolObject = instance.GetPoolObject (id);
			GameObject newGameObject = Instantiate (poolObject.objectPrefab, instance.transform);
			newGameObject.name = poolObject.id;
			return newGameObject;
		}
	}

	public static void Return(GameObject poolGameObject, string id)
	{
		if (!instance.pool.ContainsKey (id))
		{
			Debug.LogError ("Objects pool doesn't contain key: " + id);
			return;
		}

		poolGameObject.SetActive (false);
		poolGameObject.transform.SetParent (instance.transform);
		instance.pool[id].Add (poolGameObject);
	}
}
