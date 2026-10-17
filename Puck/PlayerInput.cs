using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x02000050 RID: 80
public class PlayerInput : NetworkBehaviour
{
	// Token: 0x17000025 RID: 37
	// (get) Token: 0x060001DC RID: 476 RVA: 0x0000A6A6 File Offset: 0x000088A6
	[HideInInspector]
	public Vector2 MinimumStickRaycastOriginAngle
	{
		get
		{
			return this.minimumStickRaycastOriginAngle;
		}
	}

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x060001DD RID: 477 RVA: 0x0000A6AE File Offset: 0x000088AE
	[HideInInspector]
	public Vector2 MaximumStickRaycastOriginAngle
	{
		get
		{
			return this.maximumStickRaycastOriginAngle;
		}
	}

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x060001DE RID: 478 RVA: 0x0000A6B6 File Offset: 0x000088B6
	[HideInInspector]
	public Vector2 MinimumLookAngle
	{
		get
		{
			return this.minimumLookAngle;
		}
	}

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x060001DF RID: 479 RVA: 0x0000A6BE File Offset: 0x000088BE
	[HideInInspector]
	public Vector2 MaximumLookAngle
	{
		get
		{
			return this.maximumLookAngle;
		}
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x060001E0 RID: 480 RVA: 0x0000A6C6 File Offset: 0x000088C6
	[HideInInspector]
	public int MinimumBladeAngle
	{
		get
		{
			return this.minimumBladeAngle;
		}
	}

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x060001E1 RID: 481 RVA: 0x0000A6CE File Offset: 0x000088CE
	[HideInInspector]
	public int MaximumBladeAngle
	{
		get
		{
			return this.maximumBladeAngle;
		}
	}

	// Token: 0x1700002B RID: 43
	// (get) Token: 0x060001E2 RID: 482 RVA: 0x0000A6D6 File Offset: 0x000088D6
	// (set) Token: 0x060001E3 RID: 483 RVA: 0x0000A6E3 File Offset: 0x000088E3
	[HideInInspector]
	public float InitialLookAngle
	{
		get
		{
			return this.initialLookAngle.x;
		}
		set
		{
			this.initialLookAngle = new Vector3(value, 0f, 0f);
		}
	}

	// Token: 0x060001E4 RID: 484 RVA: 0x0000A6FB File Offset: 0x000088FB
	private void Awake()
	{
		this.Player = base.GetComponent<Player>();
	}

	// Token: 0x060001E5 RID: 485 RVA: 0x0000A70C File Offset: 0x0000890C
	public override void OnNetworkSpawn()
	{
		if (this.Player.IsReplay.Value)
		{
			this.shouldTickInputs = true;
			return;
		}
		if (!base.IsOwner)
		{
			return;
		}
		InputManager.BladeAngleUpAction.performed += this.OnBladeAngleUpActionPerformed;
		InputManager.BladeAngleDownAction.performed += this.OnBladeAngleDownActionPerformed;
		InputManager.JumpAction.performed += this.OnJumpActionPerformed;
		InputManager.TwistLeftAction.performed += this.OnTwistLeftActionPerformed;
		InputManager.TwistRightAction.performed += this.OnTwistRightActionPerformed;
		InputManager.DashLeftAction.performed += this.OnDashLeftActionPerformed;
		InputManager.DashRightAction.performed += this.OnDashRightActionPerformed;
		this.shouldUpdateInputs = true;
		this.shouldTickInputs = true;
		base.OnNetworkSpawn();
	}

	// Token: 0x060001E6 RID: 486 RVA: 0x0000A7EC File Offset: 0x000089EC
	public override void OnNetworkDespawn()
	{
		if (!base.IsOwner)
		{
			return;
		}
		this.shouldUpdateInputs = false;
		this.shouldTickInputs = false;
		InputManager.BladeAngleUpAction.performed -= this.OnBladeAngleUpActionPerformed;
		InputManager.BladeAngleDownAction.performed -= this.OnBladeAngleDownActionPerformed;
		InputManager.JumpAction.performed -= this.OnJumpActionPerformed;
		InputManager.TwistLeftAction.performed -= this.OnTwistLeftActionPerformed;
		InputManager.TwistRightAction.performed -= this.OnTwistRightActionPerformed;
		InputManager.DashLeftAction.performed -= this.OnDashLeftActionPerformed;
		InputManager.DashRightAction.performed -= this.OnDashRightActionPerformed;
		base.OnNetworkDespawn();
	}

	// Token: 0x060001E7 RID: 487 RVA: 0x0000A8B0 File Offset: 0x00008AB0
	private void Update()
	{
		if (this.shouldUpdateInputs)
		{
			this.UpdateInputs();
		}
		if (this.shouldTickInputs)
		{
			this.tickAccumulator += Time.deltaTime * (float)this.TickRate;
			if (this.tickAccumulator >= 1f)
			{
				while (this.tickAccumulator >= 1f)
				{
					this.tickAccumulator -= 1f;
				}
				this.ClientTick();
			}
		}
	}

	// Token: 0x060001E8 RID: 488 RVA: 0x0000A924 File Offset: 0x00008B24
	private void UpdateInputs()
	{
		if (GlobalStateManager.UIState.IsMouseRequired)
		{
			return;
		}
		this.MoveInput.ClientValue = new Vector2((float)((InputManager.TurnRightAction.IsInProgress() ? 1 : 0) + (InputManager.TurnLeftAction.IsInProgress() ? -1 : 0)), (float)((InputManager.MoveForwardAction.IsInProgress() ? 1 : 0) + (InputManager.MoveBackwardAction.IsInProgress() ? -1 : 0)));
		if (!this.LookInput.ClientValue)
		{
			Vector2 vector = InputManager.StickAction.ReadValue<Vector2>();
			Vector2 b = new Vector2(float.IsNaN(vector.y) ? 0f : (-vector.y * (SettingsManager.GlobalStickSensitivity / 2f) * SettingsManager.VerticalStickSensitivity), float.IsNaN(vector.x) ? 0f : (vector.x * (SettingsManager.GlobalStickSensitivity / 2f) * SettingsManager.HorizontalStickSensitivity));
			this.StickRaycastOriginAngleInput.ClientValue = Utils.Vector2Clamp(this.StickRaycastOriginAngleInput.ClientValue + b, this.minimumStickRaycastOriginAngle, this.maximumStickRaycastOriginAngle);
		}
		this.SlideInput.ClientValue = InputManager.SlideAction.IsInProgress();
		this.SprintInput.ClientValue = InputManager.SprintAction.IsInProgress();
		this.TrackInput.ClientValue = InputManager.TrackAction.IsInProgress();
		this.LookInput.ClientValue = InputManager.LookAction.IsInProgress();
		this.ExtendLeftInput.ClientValue = InputManager.ExtendLeftAction.IsInProgress();
		this.ExtendRightInput.ClientValue = InputManager.ExtendRightAction.IsInProgress();
		this.TalkInput.ClientValue = InputManager.TalkAction.IsInProgress();
		this.StopInput.ClientValue = InputManager.StopAction.IsInProgress();
	}

	// Token: 0x060001E9 RID: 489 RVA: 0x0000AAE8 File Offset: 0x00008CE8
	public void UpdateLookAngle(float deltaTime)
	{
		if (this.TrackInput.ClientValue && !this.LookInput.ClientValue)
		{
			Puck puck = MonoBehaviourSingleton<PuckManager>.Instance.GetPlayerPuck(base.OwnerClientId);
			if (!puck)
			{
				puck = MonoBehaviourSingleton<PuckManager>.Instance.GetPuck(false);
			}
			PlayerCamera playerCamera = this.Player.PlayerCamera;
			PlayerBody playerBody = this.Player.PlayerBody;
			if (puck && playerCamera && playerBody)
			{
				Quaternion rhs = Quaternion.LookRotation(puck.transform.position - playerCamera.transform.position);
				Vector3 vector = Utils.WrapEulerAngles((Quaternion.Inverse(playerBody.transform.rotation) * rhs).eulerAngles);
				vector = Utils.Vector2Clamp(vector, this.minimumLookAngle, this.maximumLookAngle);
				this.LookAngleInput.ClientValue = Vector3.LerpUnclamped(this.LookAngleInput.ClientValue, vector, deltaTime * 10f);
			}
		}
		if (this.LookInput.ClientValue)
		{
			Vector2 vector2 = InputManager.StickAction.ReadValue<Vector2>();
			Vector2 b = new Vector2(float.IsNaN(vector2.y) ? 0f : (-vector2.y * (SettingsManager.LookSensitivity / 2f)), float.IsNaN(vector2.x) ? 0f : (vector2.x * (SettingsManager.LookSensitivity / 2f)));
			this.LookAngleInput.ClientValue = Utils.Vector2Clamp(this.LookAngleInput.ClientValue + b, this.minimumLookAngle, this.maximumLookAngle);
			return;
		}
		if (!this.TrackInput.ClientValue)
		{
			this.LookAngleInput.ClientValue = Vector3.Lerp(this.LookAngleInput.ClientValue, this.initialLookAngle, deltaTime * 10f);
		}
	}

	// Token: 0x060001EA RID: 490 RVA: 0x0000ACEC File Offset: 0x00008EEC
	public void ResetInputs(PlayerHandedness handedness)
	{
		this.MoveInput.ClientValue = Vector2.zero;
		this.LookAngleInput.ClientValue = this.initialLookAngle;
		this.StickRaycastOriginAngleInput.ClientValue = new Vector2(this.initialStickRaycastOriginAngle.x, (handedness == PlayerHandedness.Left) ? (-this.initialStickRaycastOriginAngle.y) : this.initialStickRaycastOriginAngle.y);
		this.BladeAngleInput.ClientValue = 0;
		this.SlideInput.ClientValue = false;
		this.SprintInput.ClientValue = false;
		this.TrackInput.ClientValue = false;
		this.LookInput.ClientValue = false;
		this.ExtendLeftInput.ClientValue = false;
		this.ExtendRightInput.ClientValue = false;
		this.LateralLeftInput.ClientValue = false;
		this.LateralRightInput.ClientValue = false;
		this.StopInput.ClientValue = false;
		this.bladeAngleBuffer = (float)this.BladeAngleInput.ClientValue;
	}

	// Token: 0x060001EB RID: 491 RVA: 0x0000ADE4 File Offset: 0x00008FE4
	private void OnBladeAngleUpActionPerformed(InputAction.CallbackContext context)
	{
		if (GlobalStateManager.UIState.IsMouseRequired)
		{
			return;
		}
		if (!this.Player.Stick)
		{
			return;
		}
		this.bladeAngleBuffer += context.ReadValue<float>();
		this.bladeAngleBuffer = Mathf.Clamp(this.bladeAngleBuffer, (float)this.minimumBladeAngle, (float)this.maximumBladeAngle);
		this.BladeAngleInput.ClientValue = (sbyte)this.bladeAngleBuffer;
	}

	// Token: 0x060001EC RID: 492 RVA: 0x0000AE58 File Offset: 0x00009058
	private void OnBladeAngleDownActionPerformed(InputAction.CallbackContext context)
	{
		if (GlobalStateManager.UIState.IsMouseRequired)
		{
			return;
		}
		if (!this.Player.Stick)
		{
			return;
		}
		this.bladeAngleBuffer -= context.ReadValue<float>();
		this.bladeAngleBuffer = Mathf.Clamp(this.bladeAngleBuffer, (float)this.minimumBladeAngle, (float)this.maximumBladeAngle);
		this.BladeAngleInput.ClientValue = (sbyte)this.bladeAngleBuffer;
	}

	// Token: 0x060001ED RID: 493 RVA: 0x0000AECA File Offset: 0x000090CA
	private void OnJumpActionPerformed(InputAction.CallbackContext context)
	{
		if (GlobalStateManager.UIState.IsMouseRequired)
		{
			return;
		}
		NetworkedInput<byte> jumpInput = this.JumpInput;
		jumpInput.ClientValue += 1;
	}

	// Token: 0x060001EE RID: 494 RVA: 0x0000AEED File Offset: 0x000090ED
	private void OnTwistLeftActionPerformed(InputAction.CallbackContext context)
	{
		if (GlobalStateManager.UIState.IsMouseRequired)
		{
			return;
		}
		NetworkedInput<byte> twistLeftInput = this.TwistLeftInput;
		twistLeftInput.ClientValue += 1;
	}

	// Token: 0x060001EF RID: 495 RVA: 0x0000AF10 File Offset: 0x00009110
	private void OnTwistRightActionPerformed(InputAction.CallbackContext context)
	{
		if (GlobalStateManager.UIState.IsMouseRequired)
		{
			return;
		}
		NetworkedInput<byte> twistRightInput = this.TwistRightInput;
		twistRightInput.ClientValue += 1;
	}

	// Token: 0x060001F0 RID: 496 RVA: 0x0000AF33 File Offset: 0x00009133
	private void OnDashLeftActionPerformed(InputAction.CallbackContext context)
	{
		if (GlobalStateManager.UIState.IsMouseRequired)
		{
			return;
		}
		NetworkedInput<byte> dashLeftInput = this.DashLeftInput;
		dashLeftInput.ClientValue += 1;
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x0000AF56 File Offset: 0x00009156
	private void OnDashRightActionPerformed(InputAction.CallbackContext context)
	{
		if (GlobalStateManager.UIState.IsMouseRequired)
		{
			return;
		}
		NetworkedInput<byte> dashRightInput = this.DashRightInput;
		dashRightInput.ClientValue += 1;
	}

	// Token: 0x060001F2 RID: 498 RVA: 0x0000AF7C File Offset: 0x0000917C
	private void ClientTick()
	{
		if (this.MoveInput.HasChanged)
		{
			this.Client_MoveInputRpc(NetworkingUtils.CompressFloatToShort(this.MoveInput.ClientValue.x, -1f, 1f), NetworkingUtils.CompressFloatToShort(this.MoveInput.ClientValue.y, -1f, 1f), default(RpcParams));
			this.MoveInput.ClientTick();
		}
		if (this.StickRaycastOriginAngleInput.HasChanged)
		{
			this.Client_RaycastOriginAngleInputRpc(NetworkingUtils.CompressFloatToShort(this.StickRaycastOriginAngleInput.ClientValue.x, this.minimumStickRaycastOriginAngle.x, this.maximumStickRaycastOriginAngle.x), NetworkingUtils.CompressFloatToShort(this.StickRaycastOriginAngleInput.ClientValue.y, this.minimumStickRaycastOriginAngle.y, this.maximumStickRaycastOriginAngle.y), default(RpcParams));
			this.StickRaycastOriginAngleInput.ClientTick();
		}
		if (this.LookAngleInput.HasChanged)
		{
			this.Client_LookAngleInputRpc(NetworkingUtils.CompressFloatToShort(this.LookAngleInput.ClientValue.x, this.minimumLookAngle.x, this.maximumLookAngle.x), NetworkingUtils.CompressFloatToShort(this.LookAngleInput.ClientValue.y, this.minimumLookAngle.y, this.maximumLookAngle.y), default(RpcParams));
			this.LookAngleInput.ClientTick();
		}
		if (this.BladeAngleInput.HasChanged)
		{
			this.Client_BladeAngleInputRpc(this.BladeAngleInput.ClientValue, default(RpcParams));
			this.BladeAngleInput.ClientTick();
		}
		if (this.SlideInput.HasChanged)
		{
			this.Client_SlideInputRpc(this.SlideInput.ClientValue, default(RpcParams));
			this.SlideInput.ClientTick();
		}
		if (this.SprintInput.HasChanged)
		{
			this.Client_SprintInputRpc(this.SprintInput.ClientValue, default(RpcParams));
			this.SprintInput.ClientTick();
		}
		if (this.TrackInput.HasChanged)
		{
			this.Client_TrackInputRpc(this.TrackInput.ClientValue, default(RpcParams));
			this.TrackInput.ClientTick();
		}
		if (this.LookInput.HasChanged)
		{
			this.Client_LookInputRpc(this.LookInput.ClientValue, default(RpcParams));
			this.LookInput.ClientTick();
		}
		if (this.JumpInput.HasChanged)
		{
			this.Client_JumpInputRpc(default(RpcParams));
			this.JumpInput.ClientTick();
		}
		if (this.StopInput.HasChanged)
		{
			this.Client_StopInputRpc(this.StopInput.ClientValue, default(RpcParams));
			this.StopInput.ClientTick();
		}
		if (this.TwistLeftInput.HasChanged)
		{
			this.Client_TwistLeftInputRpc(default(RpcParams));
			this.TwistLeftInput.ClientTick();
		}
		if (this.TwistRightInput.HasChanged)
		{
			this.Client_TwistRightInputRpc(default(RpcParams));
			this.TwistRightInput.ClientTick();
		}
		if (this.DashLeftInput.HasChanged)
		{
			this.Client_DashLeftInputRpc(default(RpcParams));
			this.DashLeftInput.ClientTick();
		}
		if (this.DashRightInput.HasChanged)
		{
			this.Client_DashRightInputRpc(default(RpcParams));
			this.DashRightInput.ClientTick();
		}
		if (this.ExtendLeftInput.HasChanged)
		{
			this.Client_ExtendLeftInputRpc(this.ExtendLeftInput.ClientValue, default(RpcParams));
			this.ExtendLeftInput.ClientTick();
		}
		if (this.ExtendRightInput.HasChanged)
		{
			this.Client_ExtendRightInputRpc(this.ExtendRightInput.ClientValue, default(RpcParams));
			this.ExtendRightInput.ClientTick();
		}
		if (this.LateralLeftInput.HasChanged)
		{
			this.Client_LateralLeftInputRpc(this.LateralLeftInput.ClientValue, default(RpcParams));
			this.LateralLeftInput.ClientTick();
		}
		if (this.LateralRightInput.HasChanged)
		{
			this.Client_LateralRightInputRpc(this.LateralRightInput.ClientValue, default(RpcParams));
			this.LateralRightInput.ClientTick();
		}
		if (this.TalkInput.HasChanged)
		{
			this.Client_TalkInputRpc(this.TalkInput.ClientValue, default(RpcParams));
			this.TalkInput.ClientTick();
		}
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x0000B3D4 File Offset: 0x000095D4
	[Rpc(SendTo.Server, DeferLocal = true)]
	public void Client_MoveInputRpc(short x, short y, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 2880114289U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true
			};
			FastBufferWriter writer = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			BytePacker.WriteValueBitPacked(writer, x);
			BytePacker.WriteValueBitPacked(writer, y);
			base.__endSendRpc(ref writer, 2880114289U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		if (rpcParams.Receive.SenderClientId != base.OwnerClientId && rpcParams.Receive.SenderClientId != 0UL)
		{
			return;
		}
		this.Server_MoveInputRpc(x, y, base.RpcTarget.Everyone);
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x0000B4F4 File Offset: 0x000096F4
	[Rpc(SendTo.SpecifiedInParams, InvokePermission = RpcInvokePermission.Server, DeferLocal = true)]
	public void Server_MoveInputRpc(short x, short y, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 2371540155U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				InvokePermission = RpcInvokePermission.Server,
				DeferLocal = true
			};
			FastBufferWriter writer = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
			BytePacker.WriteValueBitPacked(writer, x);
			BytePacker.WriteValueBitPacked(writer, y);
			base.__endSendRpc(ref writer, 2371540155U, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		Vector2 vector = new Vector2(NetworkingUtils.DecompressShortToFloat(x, -1f, 1f), NetworkingUtils.DecompressShortToFloat(y, -1f, 1f));
		vector = Utils.Vector2Clamp(vector, -Vector2.one, Vector2.one);
		this.MoveInput.ServerValue = vector;
	}

	// Token: 0x060001F5 RID: 501 RVA: 0x0000B634 File Offset: 0x00009834
	[Rpc(SendTo.Server, DeferLocal = true, Delivery = RpcDelivery.Unreliable)]
	public void Client_RaycastOriginAngleInputRpc(short x, short y, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 4145643342U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true,
				Delivery = RpcDelivery.Unreliable
			};
			FastBufferWriter writer = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Unreliable);
			BytePacker.WriteValueBitPacked(writer, x);
			BytePacker.WriteValueBitPacked(writer, y);
			base.__endSendRpc(ref writer, 4145643342U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Unreliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		if (rpcParams.Receive.SenderClientId != base.OwnerClientId && rpcParams.Receive.SenderClientId != 0UL)
		{
			return;
		}
		this.Server_RaycastOriginAngleInputRpc(x, y, base.RpcTarget.Everyone);
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x0000B760 File Offset: 0x00009960
	[Rpc(SendTo.SpecifiedInParams, InvokePermission = RpcInvokePermission.Server, DeferLocal = true, Delivery = RpcDelivery.Unreliable)]
	public void Server_RaycastOriginAngleInputRpc(short x, short y, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 3031376689U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				InvokePermission = RpcInvokePermission.Server,
				DeferLocal = true,
				Delivery = RpcDelivery.Unreliable
			};
			FastBufferWriter writer = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Unreliable);
			BytePacker.WriteValueBitPacked(writer, x);
			BytePacker.WriteValueBitPacked(writer, y);
			base.__endSendRpc(ref writer, 3031376689U, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Unreliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		Vector2 vector = new Vector2(NetworkingUtils.DecompressShortToFloat(x, this.minimumStickRaycastOriginAngle.x, this.maximumStickRaycastOriginAngle.x), NetworkingUtils.DecompressShortToFloat(y, this.minimumStickRaycastOriginAngle.y, this.maximumStickRaycastOriginAngle.y));
		vector = Utils.Vector2Clamp(vector, this.minimumStickRaycastOriginAngle, this.maximumStickRaycastOriginAngle);
		this.StickRaycastOriginAngleInput.ServerValue = vector;
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x0000B8C4 File Offset: 0x00009AC4
	[Rpc(SendTo.Server, DeferLocal = true, Delivery = RpcDelivery.Unreliable)]
	public void Client_LookAngleInputRpc(short x, short y, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 2301322626U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true,
				Delivery = RpcDelivery.Unreliable
			};
			FastBufferWriter writer = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Unreliable);
			BytePacker.WriteValueBitPacked(writer, x);
			BytePacker.WriteValueBitPacked(writer, y);
			base.__endSendRpc(ref writer, 2301322626U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Unreliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		if (rpcParams.Receive.SenderClientId != base.OwnerClientId && rpcParams.Receive.SenderClientId != 0UL)
		{
			return;
		}
		this.Server_LookAngleInputRpc(x, y, base.RpcTarget.Everyone);
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x0000B9F0 File Offset: 0x00009BF0
	[Rpc(SendTo.SpecifiedInParams, InvokePermission = RpcInvokePermission.Server, DeferLocal = true, Delivery = RpcDelivery.Unreliable)]
	public void Server_LookAngleInputRpc(short x, short y, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 1047632353U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				InvokePermission = RpcInvokePermission.Server,
				DeferLocal = true,
				Delivery = RpcDelivery.Unreliable
			};
			FastBufferWriter writer = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Unreliable);
			BytePacker.WriteValueBitPacked(writer, x);
			BytePacker.WriteValueBitPacked(writer, y);
			base.__endSendRpc(ref writer, 1047632353U, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Unreliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		Vector2 vector = new Vector2(NetworkingUtils.DecompressShortToFloat(x, this.minimumLookAngle.x, this.maximumLookAngle.x), NetworkingUtils.DecompressShortToFloat(y, this.minimumLookAngle.y, this.maximumLookAngle.y));
		vector = Utils.Vector2Clamp(vector, this.minimumLookAngle, this.maximumLookAngle);
		this.LookAngleInput.ServerValue = vector;
	}

	// Token: 0x060001F9 RID: 505 RVA: 0x0000BB54 File Offset: 0x00009D54
	[Rpc(SendTo.Server, DeferLocal = true)]
	public void Client_BladeAngleInputRpc(sbyte value, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 4018011136U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			fastBufferWriter.WriteValueSafe<sbyte>(value, default(FastBufferWriter.ForPrimitives));
			base.__endSendRpc(ref fastBufferWriter, 4018011136U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		if (rpcParams.Receive.SenderClientId != base.OwnerClientId && rpcParams.Receive.SenderClientId != 0UL)
		{
			return;
		}
		this.Server_BladeAngleInputRpc(value, base.RpcTarget.Everyone);
	}

	// Token: 0x060001FA RID: 506 RVA: 0x0000BC74 File Offset: 0x00009E74
	[Rpc(SendTo.SpecifiedInParams, InvokePermission = RpcInvokePermission.Server, DeferLocal = true)]
	public void Server_BladeAngleInputRpc(sbyte value, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 817646686U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				InvokePermission = RpcInvokePermission.Server,
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
			fastBufferWriter.WriteValueSafe<sbyte>(value, default(FastBufferWriter.ForPrimitives));
			base.__endSendRpc(ref fastBufferWriter, 817646686U, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		this.BladeAngleInput.ServerValue = (sbyte)Mathf.Clamp((int)value, this.minimumBladeAngle, this.maximumBladeAngle);
	}

	// Token: 0x060001FB RID: 507 RVA: 0x0000BD88 File Offset: 0x00009F88
	[Rpc(SendTo.Server, DeferLocal = true)]
	public void Client_SlideInputRpc(bool value, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 3775351339U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			fastBufferWriter.WriteValueSafe<bool>(value, default(FastBufferWriter.ForPrimitives));
			base.__endSendRpc(ref fastBufferWriter, 3775351339U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		if (rpcParams.Receive.SenderClientId != base.OwnerClientId && rpcParams.Receive.SenderClientId != 0UL)
		{
			return;
		}
		this.Server_SlideInputRpc(value, base.RpcTarget.Everyone);
	}

	// Token: 0x060001FC RID: 508 RVA: 0x0000BEA8 File Offset: 0x0000A0A8
	[Rpc(SendTo.SpecifiedInParams, InvokePermission = RpcInvokePermission.Server, DeferLocal = true)]
	public void Server_SlideInputRpc(bool value, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 4107840079U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				InvokePermission = RpcInvokePermission.Server,
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
			fastBufferWriter.WriteValueSafe<bool>(value, default(FastBufferWriter.ForPrimitives));
			base.__endSendRpc(ref fastBufferWriter, 4107840079U, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		this.SlideInput.ServerValue = value;
	}

	// Token: 0x060001FD RID: 509 RVA: 0x0000BFA8 File Offset: 0x0000A1A8
	[Rpc(SendTo.Server, DeferLocal = true)]
	public void Client_SprintInputRpc(bool value, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 3297803930U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			fastBufferWriter.WriteValueSafe<bool>(value, default(FastBufferWriter.ForPrimitives));
			base.__endSendRpc(ref fastBufferWriter, 3297803930U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		if (rpcParams.Receive.SenderClientId != base.OwnerClientId && rpcParams.Receive.SenderClientId != 0UL)
		{
			return;
		}
		this.Server_SprintInputRpc(value, base.RpcTarget.Everyone);
	}

	// Token: 0x060001FE RID: 510 RVA: 0x0000C0C8 File Offset: 0x0000A2C8
	[Rpc(SendTo.SpecifiedInParams, InvokePermission = RpcInvokePermission.Server, DeferLocal = true)]
	public void Server_SprintInputRpc(bool value, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 778340344U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				InvokePermission = RpcInvokePermission.Server,
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
			fastBufferWriter.WriteValueSafe<bool>(value, default(FastBufferWriter.ForPrimitives));
			base.__endSendRpc(ref fastBufferWriter, 778340344U, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		this.SprintInput.ServerValue = value;
	}

	// Token: 0x060001FF RID: 511 RVA: 0x0000C1C8 File Offset: 0x0000A3C8
	[Rpc(SendTo.Server, DeferLocal = true)]
	public void Client_TrackInputRpc(bool value, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 3231418942U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			fastBufferWriter.WriteValueSafe<bool>(value, default(FastBufferWriter.ForPrimitives));
			base.__endSendRpc(ref fastBufferWriter, 3231418942U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		if (rpcParams.Receive.SenderClientId != base.OwnerClientId && rpcParams.Receive.SenderClientId != 0UL)
		{
			return;
		}
		this.Server_TrackInputRpc(value, base.RpcTarget.Everyone);
	}

	// Token: 0x06000200 RID: 512 RVA: 0x0000C2E8 File Offset: 0x0000A4E8
	[Rpc(SendTo.SpecifiedInParams, InvokePermission = RpcInvokePermission.Server, DeferLocal = true)]
	public void Server_TrackInputRpc(bool value, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 2722698928U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				InvokePermission = RpcInvokePermission.Server,
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
			fastBufferWriter.WriteValueSafe<bool>(value, default(FastBufferWriter.ForPrimitives));
			base.__endSendRpc(ref fastBufferWriter, 2722698928U, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		this.TrackInput.ServerValue = value;
	}

	// Token: 0x06000201 RID: 513 RVA: 0x0000C3E8 File Offset: 0x0000A5E8
	[Rpc(SendTo.Server, DeferLocal = true)]
	public void Client_LookInputRpc(bool value, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 894150284U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			fastBufferWriter.WriteValueSafe<bool>(value, default(FastBufferWriter.ForPrimitives));
			base.__endSendRpc(ref fastBufferWriter, 894150284U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		if (rpcParams.Receive.SenderClientId != base.OwnerClientId && rpcParams.Receive.SenderClientId != 0UL)
		{
			return;
		}
		this.Server_LookInputRpc(value, base.RpcTarget.Everyone);
	}

	// Token: 0x06000202 RID: 514 RVA: 0x0000C508 File Offset: 0x0000A708
	[Rpc(SendTo.SpecifiedInParams, InvokePermission = RpcInvokePermission.Server, DeferLocal = true)]
	public void Server_LookInputRpc(bool value, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 3779091983U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				InvokePermission = RpcInvokePermission.Server,
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
			fastBufferWriter.WriteValueSafe<bool>(value, default(FastBufferWriter.ForPrimitives));
			base.__endSendRpc(ref fastBufferWriter, 3779091983U, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		this.LookInput.ServerValue = value;
	}

	// Token: 0x06000203 RID: 515 RVA: 0x0000C608 File Offset: 0x0000A808
	[Rpc(SendTo.Server, DeferLocal = true)]
	public void Client_JumpInputRpc(RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 566720222U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			base.__endSendRpc(ref fastBufferWriter, 566720222U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		if (rpcParams.Receive.SenderClientId != base.OwnerClientId && rpcParams.Receive.SenderClientId != 0UL)
		{
			return;
		}
		if (!this.JumpInput.ShouldChange)
		{
			return;
		}
		EventManager.TriggerEvent("Event_Server_OnPlayerJumpInput", new Dictionary<string, object>
		{
			{
				"player",
				this.Player
			}
		});
		this.JumpInput.ServerTick();
	}

	// Token: 0x06000204 RID: 516 RVA: 0x0000C72C File Offset: 0x0000A92C
	[Rpc(SendTo.Server, DeferLocal = true)]
	public void Client_StopInputRpc(bool value, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 212770831U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			fastBufferWriter.WriteValueSafe<bool>(value, default(FastBufferWriter.ForPrimitives));
			base.__endSendRpc(ref fastBufferWriter, 212770831U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		if (rpcParams.Receive.SenderClientId != base.OwnerClientId && rpcParams.Receive.SenderClientId != 0UL)
		{
			return;
		}
		this.StopInput.ServerValue = value;
	}

	// Token: 0x06000205 RID: 517 RVA: 0x0000C840 File Offset: 0x0000AA40
	[Rpc(SendTo.Server, DeferLocal = true)]
	public void Client_DashLeftInputRpc(RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 1929006103U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			base.__endSendRpc(ref fastBufferWriter, 1929006103U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		if (rpcParams.Receive.SenderClientId != base.OwnerClientId && rpcParams.Receive.SenderClientId != 0UL)
		{
			return;
		}
		if (!this.DashLeftInput.ShouldChange)
		{
			return;
		}
		EventManager.TriggerEvent("Event_Server_OnPlayerDashLeftInput", new Dictionary<string, object>
		{
			{
				"player",
				this.Player
			}
		});
		this.DashLeftInput.ServerTick();
	}

	// Token: 0x06000206 RID: 518 RVA: 0x0000C964 File Offset: 0x0000AB64
	[Rpc(SendTo.Server, DeferLocal = true)]
	public void Client_DashRightInputRpc(RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 3135613427U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			base.__endSendRpc(ref fastBufferWriter, 3135613427U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		if (rpcParams.Receive.SenderClientId != base.OwnerClientId && rpcParams.Receive.SenderClientId != 0UL)
		{
			return;
		}
		if (!this.DashRightInput.ShouldChange)
		{
			return;
		}
		EventManager.TriggerEvent("Event_Server_OnPlayerDashRightInput", new Dictionary<string, object>
		{
			{
				"player",
				this.Player
			}
		});
		this.DashRightInput.ServerTick();
	}

	// Token: 0x06000207 RID: 519 RVA: 0x0000CA88 File Offset: 0x0000AC88
	[Rpc(SendTo.Server, DeferLocal = true)]
	public void Client_TwistLeftInputRpc(RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 4104804754U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			base.__endSendRpc(ref fastBufferWriter, 4104804754U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		if (rpcParams.Receive.SenderClientId != base.OwnerClientId && rpcParams.Receive.SenderClientId != 0UL)
		{
			return;
		}
		if (!this.TwistLeftInput.ShouldChange)
		{
			return;
		}
		EventManager.TriggerEvent("Event_Server_OnPlayerTwistLeftInput", new Dictionary<string, object>
		{
			{
				"player",
				this.Player
			}
		});
		this.TwistLeftInput.ServerTick();
	}

	// Token: 0x06000208 RID: 520 RVA: 0x0000CBAC File Offset: 0x0000ADAC
	[Rpc(SendTo.Server, DeferLocal = true)]
	public void Client_TwistRightInputRpc(RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 2735818857U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			base.__endSendRpc(ref fastBufferWriter, 2735818857U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		if (rpcParams.Receive.SenderClientId != base.OwnerClientId && rpcParams.Receive.SenderClientId != 0UL)
		{
			return;
		}
		if (!this.TwistRightInput.ShouldChange)
		{
			return;
		}
		EventManager.TriggerEvent("Event_Server_OnPlayerTwistRightInput", new Dictionary<string, object>
		{
			{
				"player",
				this.Player
			}
		});
		this.TwistRightInput.ServerTick();
	}

	// Token: 0x06000209 RID: 521 RVA: 0x0000CCD0 File Offset: 0x0000AED0
	[Rpc(SendTo.Server, DeferLocal = true)]
	public void Client_ExtendLeftInputRpc(bool value, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 537498773U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			fastBufferWriter.WriteValueSafe<bool>(value, default(FastBufferWriter.ForPrimitives));
			base.__endSendRpc(ref fastBufferWriter, 537498773U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		if (rpcParams.Receive.SenderClientId != base.OwnerClientId && rpcParams.Receive.SenderClientId != 0UL)
		{
			return;
		}
		this.Server_ExtendLeftInputRpc(value, base.RpcTarget.Everyone);
	}

	// Token: 0x0600020A RID: 522 RVA: 0x0000CDF0 File Offset: 0x0000AFF0
	[Rpc(SendTo.SpecifiedInParams, InvokePermission = RpcInvokePermission.Server, DeferLocal = true)]
	public void Server_ExtendLeftInputRpc(bool value, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 3288109408U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				InvokePermission = RpcInvokePermission.Server,
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
			fastBufferWriter.WriteValueSafe<bool>(value, default(FastBufferWriter.ForPrimitives));
			base.__endSendRpc(ref fastBufferWriter, 3288109408U, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		this.ExtendLeftInput.ServerValue = value;
	}

	// Token: 0x0600020B RID: 523 RVA: 0x0000CEF0 File Offset: 0x0000B0F0
	[Rpc(SendTo.Server, DeferLocal = true)]
	public void Client_ExtendRightInputRpc(bool value, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 4044541524U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			fastBufferWriter.WriteValueSafe<bool>(value, default(FastBufferWriter.ForPrimitives));
			base.__endSendRpc(ref fastBufferWriter, 4044541524U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		if (rpcParams.Receive.SenderClientId != base.OwnerClientId && rpcParams.Receive.SenderClientId != 0UL)
		{
			return;
		}
		this.Server_ExtendRightInputRpc(value, base.RpcTarget.Everyone);
	}

	// Token: 0x0600020C RID: 524 RVA: 0x0000D010 File Offset: 0x0000B210
	[Rpc(SendTo.SpecifiedInParams, InvokePermission = RpcInvokePermission.Server, DeferLocal = true)]
	public void Server_ExtendRightInputRpc(bool value, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 152722375U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				InvokePermission = RpcInvokePermission.Server,
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
			fastBufferWriter.WriteValueSafe<bool>(value, default(FastBufferWriter.ForPrimitives));
			base.__endSendRpc(ref fastBufferWriter, 152722375U, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		this.ExtendRightInput.ServerValue = value;
	}

	// Token: 0x0600020D RID: 525 RVA: 0x0000D110 File Offset: 0x0000B310
	[Rpc(SendTo.Server, DeferLocal = true)]
	public void Client_LateralLeftInputRpc(bool value, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 867760499U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			fastBufferWriter.WriteValueSafe<bool>(value, default(FastBufferWriter.ForPrimitives));
			base.__endSendRpc(ref fastBufferWriter, 867760499U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		if (rpcParams.Receive.SenderClientId != base.OwnerClientId && rpcParams.Receive.SenderClientId != 0UL)
		{
			return;
		}
		this.Server_LateralLeftInputRpc(value, base.RpcTarget.Everyone);
	}

	// Token: 0x0600020E RID: 526 RVA: 0x0000D230 File Offset: 0x0000B430
	[Rpc(SendTo.SpecifiedInParams, InvokePermission = RpcInvokePermission.Server, DeferLocal = true)]
	public void Server_LateralLeftInputRpc(bool value, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 1623507309U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				InvokePermission = RpcInvokePermission.Server,
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
			fastBufferWriter.WriteValueSafe<bool>(value, default(FastBufferWriter.ForPrimitives));
			base.__endSendRpc(ref fastBufferWriter, 1623507309U, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		this.LateralLeftInput.ServerValue = value;
	}

	// Token: 0x0600020F RID: 527 RVA: 0x0000D330 File Offset: 0x0000B530
	[Rpc(SendTo.Server, DeferLocal = true)]
	public void Client_LateralRightInputRpc(bool value, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 2139362476U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			fastBufferWriter.WriteValueSafe<bool>(value, default(FastBufferWriter.ForPrimitives));
			base.__endSendRpc(ref fastBufferWriter, 2139362476U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		if (rpcParams.Receive.SenderClientId != base.OwnerClientId && rpcParams.Receive.SenderClientId != 0UL)
		{
			return;
		}
		this.Server_LateralRightInputRpc(value, base.RpcTarget.Everyone);
	}

	// Token: 0x06000210 RID: 528 RVA: 0x0000D450 File Offset: 0x0000B650
	[Rpc(SendTo.SpecifiedInParams, InvokePermission = RpcInvokePermission.Server, DeferLocal = true)]
	public void Server_LateralRightInputRpc(bool value, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 3221925618U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				InvokePermission = RpcInvokePermission.Server,
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
			fastBufferWriter.WriteValueSafe<bool>(value, default(FastBufferWriter.ForPrimitives));
			base.__endSendRpc(ref fastBufferWriter, 3221925618U, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		this.LateralRightInput.ServerValue = value;
	}

	// Token: 0x06000211 RID: 529 RVA: 0x0000D550 File Offset: 0x0000B750
	[Rpc(SendTo.Server, DeferLocal = true)]
	public void Client_TalkInputRpc(bool value, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 1563095812U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			fastBufferWriter.WriteValueSafe<bool>(value, default(FastBufferWriter.ForPrimitives));
			base.__endSendRpc(ref fastBufferWriter, 1563095812U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		if (rpcParams.Receive.SenderClientId != base.OwnerClientId && rpcParams.Receive.SenderClientId != 0UL)
		{
			return;
		}
		this.Server_TalkInputRpc(value, base.RpcTarget.Everyone);
	}

	// Token: 0x06000212 RID: 530 RVA: 0x0000D670 File Offset: 0x0000B870
	[Rpc(SendTo.SpecifiedInParams, InvokePermission = RpcInvokePermission.Server, DeferLocal = true)]
	public void Server_TalkInputRpc(bool value, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 3713736028U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				InvokePermission = RpcInvokePermission.Server,
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
			fastBufferWriter.WriteValueSafe<bool>(value, default(FastBufferWriter.ForPrimitives));
			base.__endSendRpc(ref fastBufferWriter, 3713736028U, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		this.TalkInput.ServerValue = value;
		EventManager.TriggerEvent("Event_Everyone_OnPlayerTalkInput", new Dictionary<string, object>
		{
			{
				"player",
				this.Player
			},
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000213 RID: 531 RVA: 0x0000D7A0 File Offset: 0x0000B9A0
	public void Server_ForceSynchronizeClientId(ulong clientId)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		this.Server_MoveInputRpc(NetworkingUtils.CompressFloatToShort(this.MoveInput.ServerValue.x, -1f, 1f), NetworkingUtils.CompressFloatToShort(this.MoveInput.ServerValue.y, -1f, 1f), base.RpcTarget.Single(clientId, RpcTargetUse.Persistent));
		this.Server_RaycastOriginAngleInputRpc(NetworkingUtils.CompressFloatToShort(this.StickRaycastOriginAngleInput.ServerValue.x, this.minimumStickRaycastOriginAngle.x, this.maximumStickRaycastOriginAngle.x), NetworkingUtils.CompressFloatToShort(this.StickRaycastOriginAngleInput.ServerValue.y, this.minimumStickRaycastOriginAngle.y, this.maximumStickRaycastOriginAngle.y), base.RpcTarget.Single(clientId, RpcTargetUse.Persistent));
		this.Server_LookAngleInputRpc(NetworkingUtils.CompressFloatToShort(this.LookAngleInput.ServerValue.x, this.minimumLookAngle.x, this.maximumLookAngle.x), NetworkingUtils.CompressFloatToShort(this.LookAngleInput.ServerValue.y, this.minimumLookAngle.y, this.maximumLookAngle.y), base.RpcTarget.Single(clientId, RpcTargetUse.Persistent));
		this.Server_BladeAngleInputRpc(this.BladeAngleInput.ServerValue, base.RpcTarget.Single(clientId, RpcTargetUse.Persistent));
		this.Server_SlideInputRpc(this.SlideInput.ServerValue, base.RpcTarget.Single(clientId, RpcTargetUse.Persistent));
		this.Server_SprintInputRpc(this.SprintInput.ServerValue, base.RpcTarget.Single(clientId, RpcTargetUse.Persistent));
		this.Server_TrackInputRpc(this.TrackInput.ServerValue, base.RpcTarget.Single(clientId, RpcTargetUse.Persistent));
		this.Server_LookInputRpc(this.LookInput.ServerValue, base.RpcTarget.Single(clientId, RpcTargetUse.Persistent));
		this.Server_ExtendLeftInputRpc(this.ExtendLeftInput.ServerValue, base.RpcTarget.Single(clientId, RpcTargetUse.Persistent));
		this.Server_ExtendRightInputRpc(this.ExtendRightInput.ServerValue, base.RpcTarget.Single(clientId, RpcTargetUse.Persistent));
		this.Server_LateralLeftInputRpc(this.LateralLeftInput.ServerValue, base.RpcTarget.Single(clientId, RpcTargetUse.Persistent));
		this.Server_LateralRightInputRpc(this.LateralRightInput.ServerValue, base.RpcTarget.Single(clientId, RpcTargetUse.Persistent));
		this.Server_TalkInputRpc(this.TalkInput.ServerValue, base.RpcTarget.Single(clientId, RpcTargetUse.Persistent));
	}

	// Token: 0x06000215 RID: 533 RVA: 0x0000DCF0 File Offset: 0x0000BEF0
	protected override void __initializeVariables()
	{
		base.__initializeVariables();
	}

	// Token: 0x06000216 RID: 534 RVA: 0x0000DD08 File Offset: 0x0000BF08
	protected override void __initializeRpcs()
	{
		base.__registerRpc(2880114289U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_2880114289), "Client_MoveInputRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(2371540155U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_2371540155), "Server_MoveInputRpc", RpcInvokePermission.Server);
		base.__registerRpc(4145643342U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_4145643342), "Client_RaycastOriginAngleInputRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(3031376689U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_3031376689), "Server_RaycastOriginAngleInputRpc", RpcInvokePermission.Server);
		base.__registerRpc(2301322626U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_2301322626), "Client_LookAngleInputRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(1047632353U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_1047632353), "Server_LookAngleInputRpc", RpcInvokePermission.Server);
		base.__registerRpc(4018011136U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_4018011136), "Client_BladeAngleInputRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(817646686U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_817646686), "Server_BladeAngleInputRpc", RpcInvokePermission.Server);
		base.__registerRpc(3775351339U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_3775351339), "Client_SlideInputRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(4107840079U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_4107840079), "Server_SlideInputRpc", RpcInvokePermission.Server);
		base.__registerRpc(3297803930U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_3297803930), "Client_SprintInputRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(778340344U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_778340344), "Server_SprintInputRpc", RpcInvokePermission.Server);
		base.__registerRpc(3231418942U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_3231418942), "Client_TrackInputRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(2722698928U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_2722698928), "Server_TrackInputRpc", RpcInvokePermission.Server);
		base.__registerRpc(894150284U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_894150284), "Client_LookInputRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(3779091983U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_3779091983), "Server_LookInputRpc", RpcInvokePermission.Server);
		base.__registerRpc(566720222U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_566720222), "Client_JumpInputRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(212770831U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_212770831), "Client_StopInputRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(1929006103U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_1929006103), "Client_DashLeftInputRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(3135613427U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_3135613427), "Client_DashRightInputRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(4104804754U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_4104804754), "Client_TwistLeftInputRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(2735818857U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_2735818857), "Client_TwistRightInputRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(537498773U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_537498773), "Client_ExtendLeftInputRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(3288109408U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_3288109408), "Server_ExtendLeftInputRpc", RpcInvokePermission.Server);
		base.__registerRpc(4044541524U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_4044541524), "Client_ExtendRightInputRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(152722375U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_152722375), "Server_ExtendRightInputRpc", RpcInvokePermission.Server);
		base.__registerRpc(867760499U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_867760499), "Client_LateralLeftInputRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(1623507309U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_1623507309), "Server_LateralLeftInputRpc", RpcInvokePermission.Server);
		base.__registerRpc(2139362476U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_2139362476), "Client_LateralRightInputRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(3221925618U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_3221925618), "Server_LateralRightInputRpc", RpcInvokePermission.Server);
		base.__registerRpc(1563095812U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_1563095812), "Client_TalkInputRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(3713736028U, new NetworkBehaviour.RpcReceiveHandler(global::PlayerInput.__rpc_handler_3713736028), "Server_TalkInputRpc", RpcInvokePermission.Server);
		base.__initializeRpcs();
	}

	// Token: 0x06000217 RID: 535 RVA: 0x0000E140 File Offset: 0x0000C340
	private static void __rpc_handler_2880114289(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		short x;
		ByteUnpacker.ReadValueBitPacked(reader, out x);
		short y;
		ByteUnpacker.ReadValueBitPacked(reader, out y);
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Client_MoveInputRpc(x, y, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000218 RID: 536 RVA: 0x0000E1C4 File Offset: 0x0000C3C4
	private static void __rpc_handler_2371540155(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		short x;
		ByteUnpacker.ReadValueBitPacked(reader, out x);
		short y;
		ByteUnpacker.ReadValueBitPacked(reader, out y);
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Server_MoveInputRpc(x, y, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000219 RID: 537 RVA: 0x0000E248 File Offset: 0x0000C448
	private static void __rpc_handler_4145643342(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		short x;
		ByteUnpacker.ReadValueBitPacked(reader, out x);
		short y;
		ByteUnpacker.ReadValueBitPacked(reader, out y);
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Client_RaycastOriginAngleInputRpc(x, y, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x0600021A RID: 538 RVA: 0x0000E2CC File Offset: 0x0000C4CC
	private static void __rpc_handler_3031376689(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		short x;
		ByteUnpacker.ReadValueBitPacked(reader, out x);
		short y;
		ByteUnpacker.ReadValueBitPacked(reader, out y);
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Server_RaycastOriginAngleInputRpc(x, y, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x0600021B RID: 539 RVA: 0x0000E350 File Offset: 0x0000C550
	private static void __rpc_handler_2301322626(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		short x;
		ByteUnpacker.ReadValueBitPacked(reader, out x);
		short y;
		ByteUnpacker.ReadValueBitPacked(reader, out y);
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Client_LookAngleInputRpc(x, y, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x0600021C RID: 540 RVA: 0x0000E3D4 File Offset: 0x0000C5D4
	private static void __rpc_handler_1047632353(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		short x;
		ByteUnpacker.ReadValueBitPacked(reader, out x);
		short y;
		ByteUnpacker.ReadValueBitPacked(reader, out y);
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Server_LookAngleInputRpc(x, y, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x0600021D RID: 541 RVA: 0x0000E458 File Offset: 0x0000C658
	private static void __rpc_handler_4018011136(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		sbyte value;
		reader.ReadValueSafe<sbyte>(out value, default(FastBufferWriter.ForPrimitives));
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Client_BladeAngleInputRpc(value, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x0600021E RID: 542 RVA: 0x0000E4D8 File Offset: 0x0000C6D8
	private static void __rpc_handler_817646686(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		sbyte value;
		reader.ReadValueSafe<sbyte>(out value, default(FastBufferWriter.ForPrimitives));
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Server_BladeAngleInputRpc(value, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x0600021F RID: 543 RVA: 0x0000E558 File Offset: 0x0000C758
	private static void __rpc_handler_3775351339(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		bool value;
		reader.ReadValueSafe<bool>(out value, default(FastBufferWriter.ForPrimitives));
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Client_SlideInputRpc(value, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000220 RID: 544 RVA: 0x0000E5D8 File Offset: 0x0000C7D8
	private static void __rpc_handler_4107840079(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		bool value;
		reader.ReadValueSafe<bool>(out value, default(FastBufferWriter.ForPrimitives));
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Server_SlideInputRpc(value, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000221 RID: 545 RVA: 0x0000E658 File Offset: 0x0000C858
	private static void __rpc_handler_3297803930(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		bool value;
		reader.ReadValueSafe<bool>(out value, default(FastBufferWriter.ForPrimitives));
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Client_SprintInputRpc(value, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000222 RID: 546 RVA: 0x0000E6D8 File Offset: 0x0000C8D8
	private static void __rpc_handler_778340344(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		bool value;
		reader.ReadValueSafe<bool>(out value, default(FastBufferWriter.ForPrimitives));
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Server_SprintInputRpc(value, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000223 RID: 547 RVA: 0x0000E758 File Offset: 0x0000C958
	private static void __rpc_handler_3231418942(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		bool value;
		reader.ReadValueSafe<bool>(out value, default(FastBufferWriter.ForPrimitives));
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Client_TrackInputRpc(value, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000224 RID: 548 RVA: 0x0000E7D8 File Offset: 0x0000C9D8
	private static void __rpc_handler_2722698928(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		bool value;
		reader.ReadValueSafe<bool>(out value, default(FastBufferWriter.ForPrimitives));
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Server_TrackInputRpc(value, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000225 RID: 549 RVA: 0x0000E858 File Offset: 0x0000CA58
	private static void __rpc_handler_894150284(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		bool value;
		reader.ReadValueSafe<bool>(out value, default(FastBufferWriter.ForPrimitives));
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Client_LookInputRpc(value, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000226 RID: 550 RVA: 0x0000E8D8 File Offset: 0x0000CAD8
	private static void __rpc_handler_3779091983(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		bool value;
		reader.ReadValueSafe<bool>(out value, default(FastBufferWriter.ForPrimitives));
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Server_LookInputRpc(value, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000227 RID: 551 RVA: 0x0000E958 File Offset: 0x0000CB58
	private static void __rpc_handler_566720222(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Client_JumpInputRpc(ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000228 RID: 552 RVA: 0x0000E9B8 File Offset: 0x0000CBB8
	private static void __rpc_handler_212770831(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		bool value;
		reader.ReadValueSafe<bool>(out value, default(FastBufferWriter.ForPrimitives));
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Client_StopInputRpc(value, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000229 RID: 553 RVA: 0x0000EA38 File Offset: 0x0000CC38
	private static void __rpc_handler_1929006103(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Client_DashLeftInputRpc(ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x0600022A RID: 554 RVA: 0x0000EA98 File Offset: 0x0000CC98
	private static void __rpc_handler_3135613427(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Client_DashRightInputRpc(ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x0600022B RID: 555 RVA: 0x0000EAF8 File Offset: 0x0000CCF8
	private static void __rpc_handler_4104804754(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Client_TwistLeftInputRpc(ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x0600022C RID: 556 RVA: 0x0000EB58 File Offset: 0x0000CD58
	private static void __rpc_handler_2735818857(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Client_TwistRightInputRpc(ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x0600022D RID: 557 RVA: 0x0000EBB8 File Offset: 0x0000CDB8
	private static void __rpc_handler_537498773(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		bool value;
		reader.ReadValueSafe<bool>(out value, default(FastBufferWriter.ForPrimitives));
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Client_ExtendLeftInputRpc(value, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x0600022E RID: 558 RVA: 0x0000EC38 File Offset: 0x0000CE38
	private static void __rpc_handler_3288109408(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		bool value;
		reader.ReadValueSafe<bool>(out value, default(FastBufferWriter.ForPrimitives));
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Server_ExtendLeftInputRpc(value, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x0600022F RID: 559 RVA: 0x0000ECB8 File Offset: 0x0000CEB8
	private static void __rpc_handler_4044541524(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		bool value;
		reader.ReadValueSafe<bool>(out value, default(FastBufferWriter.ForPrimitives));
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Client_ExtendRightInputRpc(value, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000230 RID: 560 RVA: 0x0000ED38 File Offset: 0x0000CF38
	private static void __rpc_handler_152722375(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		bool value;
		reader.ReadValueSafe<bool>(out value, default(FastBufferWriter.ForPrimitives));
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Server_ExtendRightInputRpc(value, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000231 RID: 561 RVA: 0x0000EDB8 File Offset: 0x0000CFB8
	private static void __rpc_handler_867760499(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		bool value;
		reader.ReadValueSafe<bool>(out value, default(FastBufferWriter.ForPrimitives));
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Client_LateralLeftInputRpc(value, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000232 RID: 562 RVA: 0x0000EE38 File Offset: 0x0000D038
	private static void __rpc_handler_1623507309(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		bool value;
		reader.ReadValueSafe<bool>(out value, default(FastBufferWriter.ForPrimitives));
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Server_LateralLeftInputRpc(value, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000233 RID: 563 RVA: 0x0000EEB8 File Offset: 0x0000D0B8
	private static void __rpc_handler_2139362476(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		bool value;
		reader.ReadValueSafe<bool>(out value, default(FastBufferWriter.ForPrimitives));
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Client_LateralRightInputRpc(value, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000234 RID: 564 RVA: 0x0000EF38 File Offset: 0x0000D138
	private static void __rpc_handler_3221925618(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		bool value;
		reader.ReadValueSafe<bool>(out value, default(FastBufferWriter.ForPrimitives));
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Server_LateralRightInputRpc(value, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000235 RID: 565 RVA: 0x0000EFB8 File Offset: 0x0000D1B8
	private static void __rpc_handler_1563095812(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		bool value;
		reader.ReadValueSafe<bool>(out value, default(FastBufferWriter.ForPrimitives));
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Client_TalkInputRpc(value, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000236 RID: 566 RVA: 0x0000F038 File Offset: 0x0000D238
	private static void __rpc_handler_3713736028(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		bool value;
		reader.ReadValueSafe<bool>(out value, default(FastBufferWriter.ForPrimitives));
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((global::PlayerInput)target).Server_TalkInputRpc(value, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000237 RID: 567 RVA: 0x0000F0B6 File Offset: 0x0000D2B6
	protected internal override string __getTypeName()
	{
		return "PlayerInput";
	}

	// Token: 0x04000195 RID: 405
	[Header("Settings")]
	[SerializeField]
	private Vector3 initialLookAngle = new Vector3(30f, 0f, 0f);

	// Token: 0x04000196 RID: 406
	[Space(20f)]
	[SerializeField]
	private Vector2 initialStickRaycastOriginAngle = new Vector2(40f, 80f);

	// Token: 0x04000197 RID: 407
	[SerializeField]
	private Vector2 minimumStickRaycastOriginAngle = new Vector2(-25f, -92.5f);

	// Token: 0x04000198 RID: 408
	[SerializeField]
	private Vector2 maximumStickRaycastOriginAngle = new Vector2(80f, 92.5f);

	// Token: 0x04000199 RID: 409
	[Space(20f)]
	[SerializeField]
	private Vector2 minimumLookAngle = new Vector2(-25f, -135f);

	// Token: 0x0400019A RID: 410
	[SerializeField]
	private Vector2 maximumLookAngle = new Vector2(75f, 135f);

	// Token: 0x0400019B RID: 411
	[Space(20f)]
	[SerializeField]
	private int minimumBladeAngle = -4;

	// Token: 0x0400019C RID: 412
	[SerializeField]
	private int maximumBladeAngle = 4;

	// Token: 0x0400019D RID: 413
	public NetworkedInput<Vector2> MoveInput = new NetworkedInput<Vector2>(default(Vector2), null, null);

	// Token: 0x0400019E RID: 414
	public NetworkedInput<Vector2> StickRaycastOriginAngleInput = new NetworkedInput<Vector2>(default(Vector2), (Vector2 lastSentValue, Vector2 clientValue) => Vector2.Distance(lastSentValue, clientValue) > 0.1f, null);

	// Token: 0x0400019F RID: 415
	public NetworkedInput<Vector2> LookAngleInput = new NetworkedInput<Vector2>(default(Vector2), (Vector2 lastSentValue, Vector2 clientValue) => Vector2.Distance(lastSentValue, clientValue) > 0.1f, null);

	// Token: 0x040001A0 RID: 416
	public NetworkedInput<sbyte> BladeAngleInput = new NetworkedInput<sbyte>(0, null, null);

	// Token: 0x040001A1 RID: 417
	public NetworkedInput<bool> SlideInput = new NetworkedInput<bool>(false, null, null);

	// Token: 0x040001A2 RID: 418
	public NetworkedInput<bool> SprintInput = new NetworkedInput<bool>(false, null, null);

	// Token: 0x040001A3 RID: 419
	public NetworkedInput<bool> TrackInput = new NetworkedInput<bool>(false, null, null);

	// Token: 0x040001A4 RID: 420
	public NetworkedInput<bool> LookInput = new NetworkedInput<bool>(false, null, null);

	// Token: 0x040001A5 RID: 421
	public NetworkedInput<byte> JumpInput = new NetworkedInput<byte>(0, null, (byte lastReceivedValue, double lastReceivedTime, byte serverValue) => Time.timeAsDouble - lastReceivedTime > 0.5);

	// Token: 0x040001A6 RID: 422
	public NetworkedInput<bool> StopInput = new NetworkedInput<bool>(false, null, null);

	// Token: 0x040001A7 RID: 423
	public NetworkedInput<byte> TwistLeftInput = new NetworkedInput<byte>(0, null, (byte lastReceivedValue, double lastReceivedTime, byte serverValue) => Time.timeAsDouble - lastReceivedTime > 0.5);

	// Token: 0x040001A8 RID: 424
	public NetworkedInput<byte> TwistRightInput = new NetworkedInput<byte>(0, null, (byte lastReceivedValue, double lastReceivedTime, byte serverValue) => Time.timeAsDouble - lastReceivedTime > 0.5);

	// Token: 0x040001A9 RID: 425
	public NetworkedInput<byte> DashLeftInput = new NetworkedInput<byte>(0, null, (byte lastReceivedValue, double lastReceivedTime, byte serverValue) => Time.timeAsDouble - lastReceivedTime > 0.25);

	// Token: 0x040001AA RID: 426
	public NetworkedInput<byte> DashRightInput = new NetworkedInput<byte>(0, null, (byte lastReceivedValue, double lastReceivedTime, byte serverValue) => Time.timeAsDouble - lastReceivedTime > 0.25);

	// Token: 0x040001AB RID: 427
	public NetworkedInput<bool> ExtendLeftInput = new NetworkedInput<bool>(false, null, null);

	// Token: 0x040001AC RID: 428
	public NetworkedInput<bool> ExtendRightInput = new NetworkedInput<bool>(false, null, null);

	// Token: 0x040001AD RID: 429
	public NetworkedInput<bool> LateralLeftInput = new NetworkedInput<bool>(false, null, null);

	// Token: 0x040001AE RID: 430
	public NetworkedInput<bool> LateralRightInput = new NetworkedInput<bool>(false, null, null);

	// Token: 0x040001AF RID: 431
	public NetworkedInput<bool> TalkInput = new NetworkedInput<bool>(false, null, null);

	// Token: 0x040001B0 RID: 432
	[HideInInspector]
	public Player Player;

	// Token: 0x040001B1 RID: 433
	[HideInInspector]
	public int TickRate = 200;

	// Token: 0x040001B2 RID: 434
	private float bladeAngleBuffer;

	// Token: 0x040001B3 RID: 435
	private bool shouldUpdateInputs;

	// Token: 0x040001B4 RID: 436
	private bool shouldTickInputs;

	// Token: 0x040001B5 RID: 437
	private float tickAccumulator;
}
