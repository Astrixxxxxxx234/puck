using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200022F RID: 559
public class NetworkBehaviourSingleton<T> : NetworkBehaviour where T : NetworkBehaviour
{
	// Token: 0x17000176 RID: 374
	// (get) Token: 0x060011A6 RID: 4518 RVA: 0x0005316C File Offset: 0x0005136C
	public static T Instance
	{
		get
		{
			return NetworkBehaviourSingleton<T>.instance;
		}
	}

	// Token: 0x060011A7 RID: 4519 RVA: 0x00053174 File Offset: 0x00051374
	public virtual void Awake()
	{
		if (NetworkBehaviourSingleton<T>.instance != null && NetworkBehaviourSingleton<T>.instance != this)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		if (NetworkBehaviourSingleton<T>.instance == null)
		{
			NetworkBehaviourSingleton<T>.instance = (this as T);
			Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	// Token: 0x060011A8 RID: 4520 RVA: 0x000530D6 File Offset: 0x000512D6
	public void AllowSceneDestruction()
	{
		UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(base.gameObject, UnityEngine.SceneManagement.SceneManager.GetActiveScene());
	}

	// Token: 0x060011AA RID: 4522 RVA: 0x000531E0 File Offset: 0x000513E0
	protected override void __initializeVariables()
	{
		base.__initializeVariables();
	}

	// Token: 0x060011AB RID: 4523 RVA: 0x000021FA File Offset: 0x000003FA
	protected override void __initializeRpcs()
	{
		base.__initializeRpcs();
	}

	// Token: 0x060011AC RID: 4524 RVA: 0x000531F6 File Offset: 0x000513F6
	protected internal override string __getTypeName()
	{
		return "NetworkBehaviourSingleton`1";
	}

	// Token: 0x04000B86 RID: 2950
	private static T instance;
}
