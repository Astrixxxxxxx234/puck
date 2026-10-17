using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace __GEN
{
	// Token: 0x020002B1 RID: 689
	internal class NetworkVariableSerializationHelper
	{
		// Token: 0x06001480 RID: 5248 RVA: 0x00059750 File Offset: 0x00057950
		[RuntimeInitializeOnLoadMethod]
		internal static void InitializeSerialization()
		{
			NetworkVariableSerializationTypedInitializers.InitializeSerializer_UnmanagedINetworkSerializable<NetworkObjectCollision>();
			NetworkVariableSerializationTypedInitializers.InitializeEqualityChecker_UnmanagedIEquatable<NetworkObjectCollision>();
			NetworkVariableSerializationTypedInitializers.InitializeSerializer_UnmanagedINetworkSerializable<NetworkObjectReference>();
			NetworkVariableSerializationTypedInitializers.InitializeEqualityChecker_UnmanagedIEquatable<NetworkObjectReference>();
			NetworkVariableSerializationTypedInitializers.InitializeSerializer_UnmanagedByMemcpy<byte>();
			NetworkVariableSerializationTypedInitializers.InitializeEqualityChecker_UnmanagedIEquatable<byte>();
			NetworkVariableSerializationTypedInitializers.InitializeSerializer_UnmanagedByMemcpy<bool>();
			NetworkVariableSerializationTypedInitializers.InitializeEqualityChecker_UnmanagedIEquatable<bool>();
			NetworkVariableSerializationTypedInitializers.InitializeSerializer_UnmanagedINetworkSerializable<PlayerGameState>();
			NetworkVariableSerializationTypedInitializers.InitializeEqualityChecker_UnmanagedIEquatable<PlayerGameState>();
			NetworkVariableSerializationTypedInitializers.InitializeSerializer_UnmanagedINetworkSerializable<PlayerCustomizationState>();
			NetworkVariableSerializationTypedInitializers.InitializeEqualityChecker_UnmanagedIEquatable<PlayerCustomizationState>();
			NetworkVariableSerializationTypedInitializers.InitializeSerializer_UnmanagedByMemcpy<PlayerHandedness>();
			NetworkVariableSerializationTypedInitializers.InitializeEqualityChecker_UnmanagedValueEquals<PlayerHandedness>();
			NetworkVariableSerializationTypedInitializers.InitializeSerializer_FixedString<FixedString32Bytes>();
			NetworkVariableSerializationTypedInitializers.InitializeEqualityChecker_UnmanagedIEquatable<FixedString32Bytes>();
			NetworkVariableSerializationTypedInitializers.InitializeSerializer_UnmanagedINetworkSerializable<GameState>();
			NetworkVariableSerializationTypedInitializers.InitializeEqualityChecker_UnmanagedIEquatable<GameState>();
			NetworkVariableSerializationTypedInitializers.InitializeSerializer_UnmanagedINetworkSerializable<GameModeClientConfig>();
			NetworkVariableSerializationTypedInitializers.InitializeEqualityChecker_UnmanagedIEquatable<GameModeClientConfig>();
			NetworkVariableSerializationTypedInitializers.InitializeSerializer_UnmanagedINetworkSerializable<Server>();
			NetworkVariableSerializationTypedInitializers.InitializeEqualityChecker_UnmanagedIEquatable<Server>();
		}
	}
}
