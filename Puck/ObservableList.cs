using System;
using System.Collections.Generic;
using System.ComponentModel;

// Token: 0x02000234 RID: 564
internal class ObservableList<T> : List<T> where T : INotifyPropertyChanged
{
	// Token: 0x14000007 RID: 7
	// (add) Token: 0x060011C4 RID: 4548 RVA: 0x00053458 File Offset: 0x00051658
	// (remove) Token: 0x060011C5 RID: 4549 RVA: 0x00053490 File Offset: 0x00051690
	public event ObservableList<T>.OnAdd onAdd;

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x060011C6 RID: 4550 RVA: 0x000534C8 File Offset: 0x000516C8
	// (remove) Token: 0x060011C7 RID: 4551 RVA: 0x00053500 File Offset: 0x00051700
	public event ObservableList<T>.OnRemove onRemove;

	// Token: 0x14000009 RID: 9
	// (add) Token: 0x060011C8 RID: 4552 RVA: 0x00053538 File Offset: 0x00051738
	// (remove) Token: 0x060011C9 RID: 4553 RVA: 0x00053570 File Offset: 0x00051770
	public event ObservableList<T>.OnClear onClear;

	// Token: 0x1400000A RID: 10
	// (add) Token: 0x060011CA RID: 4554 RVA: 0x000535A8 File Offset: 0x000517A8
	// (remove) Token: 0x060011CB RID: 4555 RVA: 0x000535E0 File Offset: 0x000517E0
	public event ObservableList<T>.OnModify onModify;

	// Token: 0x060011CC RID: 4556 RVA: 0x00053618 File Offset: 0x00051818
	public new void Add(T item)
	{
		base.Add(item);
		ObservableList<T>.OnAdd onAdd = this.onAdd;
		if (onAdd != null)
		{
			onAdd(item);
		}
		item.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
		{
			ObservableList<T>.OnModify onModify = this.onModify;
			if (onModify == null)
			{
				return;
			}
			onModify(item, e);
		};
	}

	// Token: 0x060011CD RID: 4557 RVA: 0x00053679 File Offset: 0x00051879
	public new void Remove(T item)
	{
		base.Remove(item);
		ObservableList<T>.OnRemove onRemove = this.onRemove;
		if (onRemove == null)
		{
			return;
		}
		onRemove(item);
	}

	// Token: 0x060011CE RID: 4558 RVA: 0x00053694 File Offset: 0x00051894
	public new void Clear()
	{
		base.Clear();
		ObservableList<T>.OnClear onClear = this.onClear;
		if (onClear == null)
		{
			return;
		}
		onClear();
	}

	// Token: 0x02000235 RID: 565
	// (Invoke) Token: 0x060011D1 RID: 4561
	public delegate void OnAdd(T item);

	// Token: 0x02000236 RID: 566
	// (Invoke) Token: 0x060011D5 RID: 4565
	public delegate void OnRemove(T item);

	// Token: 0x02000237 RID: 567
	// (Invoke) Token: 0x060011D9 RID: 4569
	public delegate void OnClear();

	// Token: 0x02000238 RID: 568
	// (Invoke) Token: 0x060011DD RID: 4573
	public delegate void OnModify(T item, PropertyChangedEventArgs e);
}
