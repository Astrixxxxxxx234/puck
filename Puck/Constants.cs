using System;
using UnityEngine;

// Token: 0x02000224 RID: 548
public static class Constants
{
	// Token: 0x04000AD2 RID: 2770
	public const uint APP_ID = 2994020U;

	// Token: 0x04000AD3 RID: 2771
	public const float STEAM_INITIALIZATION_RETRY_DELAY = 5f;

	// Token: 0x04000AD4 RID: 2772
	public const int STEAM_INITIALIZATION_HELP_ATTEMPTS = 3;

	// Token: 0x04000AD5 RID: 2773
	public const float STEAM_WEB_API_TICKET_TIMEOUT = 10f;

	// Token: 0x04000AD6 RID: 2774
	public const float STEAM_WEB_API_TICKET_RETRY_DELAY = 3f;

	// Token: 0x04000AD7 RID: 2775
	public const float DEPLOYMENT_EMPTY_TIMEOUT = 60f;

	// Token: 0x04000AD8 RID: 2776
	public const float DEPLOYMENT_AUTHENTICATION_TIMEOUT = 60f;

	// Token: 0x04000AD9 RID: 2777
	public const string DEFAULT_BACKEND_URL = "ws://localhost:8080";

	// Token: 0x04000ADA RID: 2778
	public const int WEB_SOCKET_CONNECTION_TIMEOUT = 5000;

	// Token: 0x04000ADB RID: 2779
	public const int WEB_SOCKET_RECONNECT_BASE_DELAY = 500;

	// Token: 0x04000ADC RID: 2780
	public const int WEB_SOCKET_RECONNECTION_DELAY_MAX = 30000;

	// Token: 0x04000ADD RID: 2781
	public const int WEB_SOCKET_PING_TIMEOUT = 8000;

	// Token: 0x04000ADE RID: 2782
	public const int WEB_SOCKET_PING_WATCHDOG_INTERVAL = 1000;

	// Token: 0x04000ADF RID: 2783
	public const float SERVER_MOD_READINESS_TIMEOUT = 300f;

	// Token: 0x04000AE0 RID: 2784
	public const float SERVER_MOD_PROGRESS_LOG_INTERVAL = 2f;

	// Token: 0x04000AE1 RID: 2785
	public const string SERVER_BIND_ADDRESS = "0.0.0.0";

	// Token: 0x04000AE2 RID: 2786
	public const string TEAM_BLUE_COLOR = "#3b82f6";

	// Token: 0x04000AE3 RID: 2787
	public const string TEAM_RED_COLOR = "#d13333";

	// Token: 0x04000AE4 RID: 2788
	public const string TEAM_SPECTATOR_COLOR = "#404040";

	// Token: 0x04000AE5 RID: 2789
	public const string PATREON_COLOR = "#f1c40f";

	// Token: 0x04000AE6 RID: 2790
	public const string MODERATOR_COLOR = "#206694";

	// Token: 0x04000AE7 RID: 2791
	public const string ADMIN_COLOR = "#992d22";

	// Token: 0x04000AE8 RID: 2792
	public const string DEVELOPER_COLOR = "#71368a";

	// Token: 0x04000AE9 RID: 2793
	public const string SERVER_COLOR = "#b8b8b8";

	// Token: 0x04000AEA RID: 2794
	public const string VOTE_COLOR = "#e67e22";

	// Token: 0x04000AEB RID: 2795
	public const string GAME_COLOR = "#ffe97f";

	// Token: 0x04000AEC RID: 2796
	public const string ERROR_COLOR = "#e74c3c";

	// Token: 0x04000AED RID: 2797
	public const int CHAT_MESSAGE_MAX_BYTES = 509;

	// Token: 0x04000AEE RID: 2798
	public const DebugMode DEFAULT_SETTINGS_DEBUG = DebugMode.Off;

	// Token: 0x04000AEF RID: 2799
	public const float DEFAULT_SETTINGS_CAMERA_ANGLE = 30f;

	// Token: 0x04000AF0 RID: 2800
	public const PlayerHandedness DEFAULT_SETTINGS_HANDEDNESS = PlayerHandedness.Right;

	// Token: 0x04000AF1 RID: 2801
	public const bool DEFAULT_SETTINGS_SHOW_PUCK_SILHOUETTE = true;

	// Token: 0x04000AF2 RID: 2802
	public const bool DEFAULT_SETTINGS_SHOW_PUCK_OUTLINE = false;

	// Token: 0x04000AF3 RID: 2803
	public const bool DEFAULT_SETTINGS_SHOW_PUCK_ELEVATION = true;

	// Token: 0x04000AF4 RID: 2804
	public const bool DEFAULT_SETTINGS_SHOW_PLAYER_USERNAMES = false;

	// Token: 0x04000AF5 RID: 2805
	public const float DEFAULT_SETTINGS_PLAYER_USERNAMES_FADE_THRESHOLD = 1f;

	// Token: 0x04000AF6 RID: 2806
	public const int DEFAULT_SETTINGS_MAX_MATCHMAKING_RTT = 50;

	// Token: 0x04000AF7 RID: 2807
	public const NetworkBuffering DEFAULT_SETTINGS_NETWORK_BUFFERING = NetworkBuffering.Responsive;

	// Token: 0x04000AF8 RID: 2808
	public const bool DEFAULT_SETTINGS_FILTER_CHAT_PROFANITY = true;

	// Token: 0x04000AF9 RID: 2809
	public const Units DEFAULT_SETTINGS_UNITS = Units.Metric;

	// Token: 0x04000AFA RID: 2810
	public const bool DEFAULT_SETTINGS_SHOW_GAME_USER_INTERFACE = true;

	// Token: 0x04000AFB RID: 2811
	public const bool DEFAULT_SETTINGS_SHOW_TEAM_COLOR_BAR = true;

	// Token: 0x04000AFC RID: 2812
	public const float DEFAULT_SETTINGS_USER_INTERFACE_SCALE = 1f;

	// Token: 0x04000AFD RID: 2813
	public const float DEFAULT_SETTINGS_CHAT_OPACITY = 1f;

	// Token: 0x04000AFE RID: 2814
	public const float DEFAULT_SETTINGS_CHAT_SCALE = 1f;

	// Token: 0x04000AFF RID: 2815
	public const float DEFAULT_SETTINGS_MINIMAP_OPACITY = 1f;

	// Token: 0x04000B00 RID: 2816
	public const float DEFAULT_SETTINGS_MINIMAP_BACKGROUND_OPACITY = 1f;

	// Token: 0x04000B01 RID: 2817
	public const float DEFAULT_SETTINGS_MINIMAP_HORIZONTAL_POSITION = 100f;

	// Token: 0x04000B02 RID: 2818
	public const float DEFAULT_SETTINGS_MINIMAP_VERTICAL_POSITION = 0f;

	// Token: 0x04000B03 RID: 2819
	public const float DEFAULT_SETTINGS_MINIMAP_SCALE = 1f;

	// Token: 0x04000B04 RID: 2820
	public const bool DEFAULT_SETTINGS_SHOW_MINIMAP_STICKS = true;

	// Token: 0x04000B05 RID: 2821
	public const bool DEFAULT_SETTINGS_SHOW_MINIMAP_PUCK_ELEVATION = true;

	// Token: 0x04000B06 RID: 2822
	public const bool DEFAULT_SETTINGS_SHOW_MINIMAP_FALLEN_INDICATOR = true;

	// Token: 0x04000B07 RID: 2823
	public const float MINIMAP_STICK_MAX_HEIGHT = 3f;

	// Token: 0x04000B08 RID: 2824
	public const float MINIMAP_STICK_MIN_SCALE = 0.5f;

	// Token: 0x04000B09 RID: 2825
	public const float MINIMAP_PUCK_MAX_HEIGHT = 8f;

	// Token: 0x04000B0A RID: 2826
	public const float MINIMAP_PUCK_MIN_SCALE = 0.6f;

	// Token: 0x04000B0B RID: 2827
	public const float MINIMAP_FALLEN_OPACITY = 0.4f;

	// Token: 0x04000B0C RID: 2828
	public const float DEFAULT_SETTINGS_GLOBAL_STICK_SENSITIVITY = 0.2f;

	// Token: 0x04000B0D RID: 2829
	public const float DEFAULT_SETTINGS_HORIZONTAL_STICK_SENSITIVITY = 1f;

	// Token: 0x04000B0E RID: 2830
	public const float DEFAULT_SETTINGS_VERTICAL_STICK_SENSITIVITY = 1f;

	// Token: 0x04000B0F RID: 2831
	public const float DEFAULT_SETTINGS_LOOK_SENSITIVITY = 0.2f;

	// Token: 0x04000B10 RID: 2832
	public const float DEFAULT_SETTINGS_GLOBAL_VOLUME = 0.5f;

	// Token: 0x04000B11 RID: 2833
	public const float DEFAULT_SETTINGS_AMBIENT_VOLUME = 1f;

	// Token: 0x04000B12 RID: 2834
	public const float DEFAULT_SETTINGS_GAME_VOLUME = 1f;

	// Token: 0x04000B13 RID: 2835
	public const float DEFAULT_SETTINGS_VOICE_VOLUME = 1f;

	// Token: 0x04000B14 RID: 2836
	public const float DEFAULT_SETTINGS_UI_VOLUME = 0.5f;

	// Token: 0x04000B15 RID: 2837
	public const FullScreenMode DEFAULT_SETTINGS_FULL_SCREEN_MODE = FullScreenMode.FullScreenWindow;

	// Token: 0x04000B16 RID: 2838
	public const int DEFAULT_SETTINGS_DISPLAY_INDEX = 0;

	// Token: 0x04000B17 RID: 2839
	public const int DEFAULT_SETTINGS_RESOLUTION_INDEX = -1;

	// Token: 0x04000B18 RID: 2840
	public const bool DEFAULT_SETTINGS_VSYNC = false;

	// Token: 0x04000B19 RID: 2841
	public const int DEFAULT_SETTINGS_FPS_LIMIT = 240;

	// Token: 0x04000B1A RID: 2842
	public const float DEFAULT_SETTINGS_FOV = 90f;

	// Token: 0x04000B1B RID: 2843
	public const ApplicationQuality DEFAULT_SETTINGS_QUALITY = ApplicationQuality.High;

	// Token: 0x04000B1C RID: 2844
	public const global::ShadowQuality DEFAULT_SETTINGS_SHADOW_QUALITY = global::ShadowQuality.High;

	// Token: 0x04000B1D RID: 2845
	public const bool DEFAULT_SETTINGS_MOTION_BLUR = true;

	// Token: 0x04000B1E RID: 2846
	public const int SHADOW_CASCADE_COUNT = 4;

	// Token: 0x04000B1F RID: 2847
	public static readonly Vector3 SHADOW_CASCADE_RANGES = new Vector3(4.5f, 9f, 16.5f);

	// Token: 0x04000B20 RID: 2848
	public const PlayerTeam DEFAULT_TEAM = PlayerTeam.Blue;

	// Token: 0x04000B21 RID: 2849
	public const PlayerRole DEFAULT_ROLE = PlayerRole.Attacker;

	// Token: 0x04000B22 RID: 2850
	public const bool DEFAULT_APPLY_FOR_BOTH_TEAMS = false;

	// Token: 0x04000B23 RID: 2851
	public const int NO_ITEM_ID = -1;

	// Token: 0x04000B24 RID: 2852
	public const int DEFAULT_FLAG_ID = -1;

	// Token: 0x04000B25 RID: 2853
	public const int DEFAULT_HEADGEAR_ID_BLUE_ATTACKER = 513;

	// Token: 0x04000B26 RID: 2854
	public const int DEFAULT_HEADGEAR_ID_RED_ATTACKER = 513;

	// Token: 0x04000B27 RID: 2855
	public const int DEFAULT_HEADGEAR_ID_BLUE_GOALIE = 527;

	// Token: 0x04000B28 RID: 2856
	public const int DEFAULT_HEADGEAR_ID_RED_GOALIE = 527;

	// Token: 0x04000B29 RID: 2857
	public const int DEFAULT_MUSTACHE_ID = -1;

	// Token: 0x04000B2A RID: 2858
	public const int DEFAULT_BEARD_ID = -1;

	// Token: 0x04000B2B RID: 2859
	public const int DEFAULT_JERSEY_ID_BLUE_ATTACKER = 2048;

	// Token: 0x04000B2C RID: 2860
	public const int DEFAULT_JERSEY_ID_RED_ATTACKER = 2048;

	// Token: 0x04000B2D RID: 2861
	public const int DEFAULT_JERSEY_ID_BLUE_GOALIE = 2048;

	// Token: 0x04000B2E RID: 2862
	public const int DEFAULT_JERSEY_ID_RED_GOALIE = 2048;

	// Token: 0x04000B2F RID: 2863
	public const int DEFAULT_STICK_SKIN_ID_BLUE_ATTACKER = 2621;

	// Token: 0x04000B30 RID: 2864
	public const int DEFAULT_STICK_SKIN_ID_RED_ATTACKER = 2621;

	// Token: 0x04000B31 RID: 2865
	public const int DEFAULT_STICK_SKIN_ID_BLUE_GOALIE = 2621;

	// Token: 0x04000B32 RID: 2866
	public const int DEFAULT_STICK_SKIN_ID_RED_GOALIE = 2621;

	// Token: 0x04000B33 RID: 2867
	public const int DEFAULT_STICK_SHAFT_TAPE_ID_BLUE_ATTACKER = -1;

	// Token: 0x04000B34 RID: 2868
	public const int DEFAULT_STICK_SHAFT_TAPE_ID_RED_ATTACKER = -1;

	// Token: 0x04000B35 RID: 2869
	public const int DEFAULT_STICK_SHAFT_TAPE_ID_BLUE_GOALIE = -1;

	// Token: 0x04000B36 RID: 2870
	public const int DEFAULT_STICK_SHAFT_TAPE_ID_RED_GOALIE = -1;

	// Token: 0x04000B37 RID: 2871
	public const int DEFAULT_STICK_BLADE_TAPE_ID_BLUE_ATTACKER = -1;

	// Token: 0x04000B38 RID: 2872
	public const int DEFAULT_STICK_BLADE_TAPE_ID_RED_ATTACKER = -1;

	// Token: 0x04000B39 RID: 2873
	public const int DEFAULT_STICK_BLADE_TAPE_ID_BLUE_GOALIE = -1;

	// Token: 0x04000B3A RID: 2874
	public const int DEFAULT_STICK_BLADE_TAPE_ID_RED_GOALIE = -1;

	// Token: 0x04000B3B RID: 2875
	public const ushort DEFAULT_SERVER_PORT = 30609;

	// Token: 0x04000B3C RID: 2876
	public const string DEFAULT_SERVER_NAME = "MY PUCK SERVER";

	// Token: 0x04000B3D RID: 2877
	public const ushort DEFAULT_SERVER_MAX_PLAYERS = 12;

	// Token: 0x04000B3E RID: 2878
	public const string DEFAULT_SERVER_PASSWORD = null;

	// Token: 0x04000B3F RID: 2879
	public const ushort DEFAULT_SERVER_TICK_RATE = 200;

	// Token: 0x04000B40 RID: 2880
	public const bool DEFAULT_SERVER_IS_PUBLIC = true;

	// Token: 0x04000B41 RID: 2881
	public const bool DEFAULT_SERVER_USE_VOIP = false;

	// Token: 0x04000B42 RID: 2882
	public const bool DEFAULT_SERVER_USE_WHITELIST = false;

	// Token: 0x04000B43 RID: 2883
	public static readonly ModConfig[] DEFAULT_SERVER_MODS = new ModConfig[0];

	// Token: 0x04000B44 RID: 2884
	public const string DEFAULT_SERVER_GAME_MODE = "public";

	// Token: 0x04000B45 RID: 2885
	public const string DEFAULT_SERVER_LEVEL = "default";

	// Token: 0x04000B46 RID: 2886
	public const float KICK_TIMEOUT = 60f;

	// Token: 0x04000B47 RID: 2887
	public const float VOICE_TRANSMIT_LEVEL_GAIN = 14f;

	// Token: 0x04000B48 RID: 2888
	public const float VOICE_LEVEL_SMOOTHING_RATE = 15f;

	// Token: 0x04000B49 RID: 2889
	public const float VOICE_LEVEL_HOLD_SECONDS = 0.1f;

	// Token: 0x04000B4A RID: 2890
	public static readonly string[] CHAT_BLACKLIST = new string[]
	{
		"卐",
		"卍",
		"☭",
		"⛧"
	};

	// Token: 0x04000B4B RID: 2891
	public static readonly string[] CHAT_WHITELIST = new string[]
	{
		"❤️",
		"\ud83d\ude2d",
		"\ud83d\udd25",
		"\ud83d\udcaf"
	};

	// Token: 0x04000B4C RID: 2892
	public const float INPUT_DEADZONE = 0.05f;

	// Token: 0x04000B4D RID: 2893
	public const float SPRINT_STAMINA_THRESHOLD = 0.25f;

	// Token: 0x04000B4E RID: 2894
	public const float MATCH_PLAYER_JOIN_TIMEOUT = 60f;

	// Token: 0x04000B4F RID: 2895
	public const float MATCH_PLAYER_ABANDONMENT_TIMEOUT = 120f;

	// Token: 0x04000B50 RID: 2896
	public const float GOAL_SLOW_MOTION_SCALE = 0.35f;

	// Token: 0x04000B51 RID: 2897
	public const float GOAL_SLOW_MOTION_RAMP_IN_SECONDS = 0.08f;

	// Token: 0x04000B52 RID: 2898
	public const float GOAL_SLOW_MOTION_HOLD_SECONDS = 0.28f;

	// Token: 0x04000B53 RID: 2899
	public const float GOAL_SLOW_MOTION_RAMP_OUT_SECONDS = 0.42f;

	// Token: 0x04000B54 RID: 2900
	public const float SLOW_MOTION_AUDIO_LOWPASS_MIN_HZ = 900f;

	// Token: 0x04000B55 RID: 2901
	public const float SLOW_MOTION_AUDIO_LOWPASS_MAX_HZ = 22000f;

	// Token: 0x04000B56 RID: 2902
	public const float SLOW_MOTION_AUDIO_PITCH_FLOOR = 0.95f;

	// Token: 0x04000B57 RID: 2903
	public const float GOAL_SLOW_MOTION_CAMERA_FOV_PUNCH = 0f;

	// Token: 0x04000B58 RID: 2904
	public const float REPLAY_TOUCH_SLOW_MOTION_CAMERA_FOV_PUNCH = 12f;

	// Token: 0x04000B59 RID: 2905
	public const float REPLAY_GOAL_SLOW_MOTION_CAMERA_FOV_PUNCH = 12f;

	// Token: 0x04000B5A RID: 2906
	public const float REPLAY_TOUCH_SLOW_MOTION_SCALE = 0.3f;

	// Token: 0x04000B5B RID: 2907
	public const float REPLAY_TOUCH_SLOW_MOTION_LEAD_SECONDS = 0.6f;

	// Token: 0x04000B5C RID: 2908
	public const float REPLAY_TOUCH_SLOW_MOTION_RAMP_IN_SECONDS = 0.5f;

	// Token: 0x04000B5D RID: 2909
	public const float REPLAY_TOUCH_SLOW_MOTION_HOLD_SECONDS = 0.45f;

	// Token: 0x04000B5E RID: 2910
	public const float REPLAY_TOUCH_SLOW_MOTION_RAMP_OUT_SECONDS = 0.5f;

	// Token: 0x04000B5F RID: 2911
	public const float REPLAY_GOAL_SLOW_MOTION_SCALE = 0.3f;

	// Token: 0x04000B60 RID: 2912
	public const float REPLAY_GOAL_SLOW_MOTION_LEAD_SECONDS = 0.4f;

	// Token: 0x04000B61 RID: 2913
	public const float REPLAY_GOAL_SLOW_MOTION_RAMP_IN_SECONDS = 0.4f;

	// Token: 0x04000B62 RID: 2914
	public const float REPLAY_GOAL_SLOW_MOTION_HOLD_SECONDS = 0.4f;

	// Token: 0x04000B63 RID: 2915
	public const float REPLAY_GOAL_SLOW_MOTION_RAMP_OUT_SECONDS = 0.5f;

	// Token: 0x04000B64 RID: 2916
	public const float REPLAY_OWN_GOAL_TOUCH_THRESHOLD_SECONDS = 3f;

	// Token: 0x04000B65 RID: 2917
	public const float REPLAY_CELLY_CAM_DELAY_SECONDS = 0.75f;

	// Token: 0x04000B66 RID: 2918
	public const float GOAL_FLASH_PEAK_OPACITY = 0.4f;
}
