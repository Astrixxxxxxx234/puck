using System;

// Token: 0x020000FA RID: 250
public class ConnectionData
{
	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x060007AA RID: 1962 RVA: 0x00028676 File Offset: 0x00026876
	// (set) Token: 0x060007AB RID: 1963 RVA: 0x0002867E File Offset: 0x0002687E
	public string SteamId { get; set; }

	// Token: 0x170000CA RID: 202
	// (get) Token: 0x060007AC RID: 1964 RVA: 0x00028687 File Offset: 0x00026887
	// (set) Token: 0x060007AD RID: 1965 RVA: 0x0002868F File Offset: 0x0002688F
	public string Key { get; set; }

	// Token: 0x170000CB RID: 203
	// (get) Token: 0x060007AE RID: 1966 RVA: 0x00028698 File Offset: 0x00026898
	// (set) Token: 0x060007AF RID: 1967 RVA: 0x000286A0 File Offset: 0x000268A0
	public string Password { get; set; }

	// Token: 0x170000CC RID: 204
	// (get) Token: 0x060007B0 RID: 1968 RVA: 0x000286A9 File Offset: 0x000268A9
	// (set) Token: 0x060007B1 RID: 1969 RVA: 0x000286B1 File Offset: 0x000268B1
	public string[] EnabledModIds { get; set; }

	// Token: 0x170000CD RID: 205
	// (get) Token: 0x060007B2 RID: 1970 RVA: 0x000286BA File Offset: 0x000268BA
	// (set) Token: 0x060007B3 RID: 1971 RVA: 0x000286C2 File Offset: 0x000268C2
	public PlayerHandedness Handedness { get; set; }

	// Token: 0x170000CE RID: 206
	// (get) Token: 0x060007B4 RID: 1972 RVA: 0x000286CB File Offset: 0x000268CB
	// (set) Token: 0x060007B5 RID: 1973 RVA: 0x000286D3 File Offset: 0x000268D3
	public int FlagID { get; set; }

	// Token: 0x170000CF RID: 207
	// (get) Token: 0x060007B6 RID: 1974 RVA: 0x000286DC File Offset: 0x000268DC
	// (set) Token: 0x060007B7 RID: 1975 RVA: 0x000286E4 File Offset: 0x000268E4
	public int HeadgearIDBlueAttacker { get; set; }

	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x060007B8 RID: 1976 RVA: 0x000286ED File Offset: 0x000268ED
	// (set) Token: 0x060007B9 RID: 1977 RVA: 0x000286F5 File Offset: 0x000268F5
	public int HeadgearIDRedAttacker { get; set; }

	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x060007BA RID: 1978 RVA: 0x000286FE File Offset: 0x000268FE
	// (set) Token: 0x060007BB RID: 1979 RVA: 0x00028706 File Offset: 0x00026906
	public int HeadgearIDBlueGoalie { get; set; }

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x060007BC RID: 1980 RVA: 0x0002870F File Offset: 0x0002690F
	// (set) Token: 0x060007BD RID: 1981 RVA: 0x00028717 File Offset: 0x00026917
	public int HeadgearIDRedGoalie { get; set; }

	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x060007BE RID: 1982 RVA: 0x00028720 File Offset: 0x00026920
	// (set) Token: 0x060007BF RID: 1983 RVA: 0x00028728 File Offset: 0x00026928
	public int MustacheID { get; set; }

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x060007C0 RID: 1984 RVA: 0x00028731 File Offset: 0x00026931
	// (set) Token: 0x060007C1 RID: 1985 RVA: 0x00028739 File Offset: 0x00026939
	public int BeardID { get; set; }

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x060007C2 RID: 1986 RVA: 0x00028742 File Offset: 0x00026942
	// (set) Token: 0x060007C3 RID: 1987 RVA: 0x0002874A File Offset: 0x0002694A
	public int JerseyIDBlueAttacker { get; set; }

	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x060007C4 RID: 1988 RVA: 0x00028753 File Offset: 0x00026953
	// (set) Token: 0x060007C5 RID: 1989 RVA: 0x0002875B File Offset: 0x0002695B
	public int JerseyIDRedAttacker { get; set; }

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x060007C6 RID: 1990 RVA: 0x00028764 File Offset: 0x00026964
	// (set) Token: 0x060007C7 RID: 1991 RVA: 0x0002876C File Offset: 0x0002696C
	public int JerseyIDBlueGoalie { get; set; }

	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x060007C8 RID: 1992 RVA: 0x00028775 File Offset: 0x00026975
	// (set) Token: 0x060007C9 RID: 1993 RVA: 0x0002877D File Offset: 0x0002697D
	public int JerseyIDRedGoalie { get; set; }

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x060007CA RID: 1994 RVA: 0x00028786 File Offset: 0x00026986
	// (set) Token: 0x060007CB RID: 1995 RVA: 0x0002878E File Offset: 0x0002698E
	public int StickSkinIDBlueAttacker { get; set; }

	// Token: 0x170000DA RID: 218
	// (get) Token: 0x060007CC RID: 1996 RVA: 0x00028797 File Offset: 0x00026997
	// (set) Token: 0x060007CD RID: 1997 RVA: 0x0002879F File Offset: 0x0002699F
	public int StickSkinIDRedAttacker { get; set; }

	// Token: 0x170000DB RID: 219
	// (get) Token: 0x060007CE RID: 1998 RVA: 0x000287A8 File Offset: 0x000269A8
	// (set) Token: 0x060007CF RID: 1999 RVA: 0x000287B0 File Offset: 0x000269B0
	public int StickSkinIDBlueGoalie { get; set; }

	// Token: 0x170000DC RID: 220
	// (get) Token: 0x060007D0 RID: 2000 RVA: 0x000287B9 File Offset: 0x000269B9
	// (set) Token: 0x060007D1 RID: 2001 RVA: 0x000287C1 File Offset: 0x000269C1
	public int StickSkinIDRedGoalie { get; set; }

	// Token: 0x170000DD RID: 221
	// (get) Token: 0x060007D2 RID: 2002 RVA: 0x000287CA File Offset: 0x000269CA
	// (set) Token: 0x060007D3 RID: 2003 RVA: 0x000287D2 File Offset: 0x000269D2
	public int StickShaftTapeIDBlueAttacker { get; set; }

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x060007D4 RID: 2004 RVA: 0x000287DB File Offset: 0x000269DB
	// (set) Token: 0x060007D5 RID: 2005 RVA: 0x000287E3 File Offset: 0x000269E3
	public int StickShaftTapeIDRedAttacker { get; set; }

	// Token: 0x170000DF RID: 223
	// (get) Token: 0x060007D6 RID: 2006 RVA: 0x000287EC File Offset: 0x000269EC
	// (set) Token: 0x060007D7 RID: 2007 RVA: 0x000287F4 File Offset: 0x000269F4
	public int StickShaftTapeIDBlueGoalie { get; set; }

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x060007D8 RID: 2008 RVA: 0x000287FD File Offset: 0x000269FD
	// (set) Token: 0x060007D9 RID: 2009 RVA: 0x00028805 File Offset: 0x00026A05
	public int StickShaftTapeIDRedGoalie { get; set; }

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x060007DA RID: 2010 RVA: 0x0002880E File Offset: 0x00026A0E
	// (set) Token: 0x060007DB RID: 2011 RVA: 0x00028816 File Offset: 0x00026A16
	public int StickBladeTapeIDBlueAttacker { get; set; }

	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x060007DC RID: 2012 RVA: 0x0002881F File Offset: 0x00026A1F
	// (set) Token: 0x060007DD RID: 2013 RVA: 0x00028827 File Offset: 0x00026A27
	public int StickBladeTapeIDRedAttacker { get; set; }

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x060007DE RID: 2014 RVA: 0x00028830 File Offset: 0x00026A30
	// (set) Token: 0x060007DF RID: 2015 RVA: 0x00028838 File Offset: 0x00026A38
	public int StickBladeTapeIDBlueGoalie { get; set; }

	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x060007E0 RID: 2016 RVA: 0x00028841 File Offset: 0x00026A41
	// (set) Token: 0x060007E1 RID: 2017 RVA: 0x00028849 File Offset: 0x00026A49
	public int StickBladeTapeIDRedGoalie { get; set; }
}
