using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200022D RID: 557
public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	// Token: 0x17000175 RID: 373
	// (get) Token: 0x060011A0 RID: 4512 RVA: 0x00053064 File Offset: 0x00051264
	public static T Instance
	{
		get
		{
			return MonoBehaviourSingleton<T>.instance;
		}
	}

	// Token: 0x060011A1 RID: 4513 RVA: 0x0005306C File Offset: 0x0005126C
	public virtual void Awake()
	{
		if (MonoBehaviourSingleton<T>.instance != null && MonoBehaviourSingleton<T>.instance != this)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		if (MonoBehaviourSingleton<T>.instance == null)
		{
			MonoBehaviourSingleton<T>.instance = (this as T);
			Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	// Token: 0x060011A2 RID: 4514 RVA: 0x000530D6 File Offset: 0x000512D6
	public void AllowSceneDestruction()
	{
		UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(base.gameObject, UnityEngine.SceneManagement.SceneManager.GetActiveScene());
	}

	// Token: 0x04000B82 RID: 2946
	private static T instance;
}
