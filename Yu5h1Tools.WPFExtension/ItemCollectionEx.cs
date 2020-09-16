using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;
using System;

namespace Yu5h1Tools.WPFExtension
{
    public static class ItemCollectionEx
    {
        public static ItemCollection<T> GetItems<T>(this ItemsControl itemsControl) where T : ItemsControl
        => new ItemCollection<T>(itemsControl);
    }
    public class ItemCollection<T> : IList<T> where T : ItemsControl
    {
        public ItemCollection Items;

        public ItemCollection(ItemsControl itemsControl) => Items = itemsControl.Items;

        public int Count => Items.Count;
        public bool IsReadOnly { get; }

        public T this[int index] {
            get => Items[index] as T;
            set => Items[index] = value;
        }
        public void Add(T item) => Items.Add(item);
        public void Remove(T item) => Items.Remove(item);
        public void RemoveAt(int index) => Items.RemoveAt(index);
        public void Clear() => Items.Clear();
        public int IndexOf(T item) => Items.IndexOf(item);
        public void Insert(int index, T item) => Items.Insert(index, item);
        public bool Contains(T item) => Items.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => Items.CopyTo(array, arrayIndex);

        bool ICollection<T>.Remove(T item) {
            bool result = Items.Contains(item);
            Items.Remove(item);
            return result;
        }
       List<T> ToList(ItemCollection itemsControl)
        {
            List<T> results = new List<T>();
            for (int i = 0; i < itemsControl.Count; i++)
                results.Add((T)itemsControl[i]);
            return results;
        }
        public IEnumerator<T> GetEnumerator() => ToList(Items).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ToList(Items).GetEnumerator();
    }
}
