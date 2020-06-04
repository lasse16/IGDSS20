using System.Collections;
using System.Collections.Generic;

public class RunTimeSet<T> : IEnumerable<T>
{
    private List<T> set;

    public bool Add(T item)
    {
        if (set.Contains(item))
            return false;

        set.Add(item);
        return true;
    }


    public void Remove(T item)
    {
        set.Remove(item);
    }

    public IEnumerator<T> GetEnumerator() => set.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => set.GetEnumerator();
}