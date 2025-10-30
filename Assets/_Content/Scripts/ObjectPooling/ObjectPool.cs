using System;
using System.Collections.Generic;
using UnityEngine; // можно убрать, если не пулить юнити-объекты

public sealed class ObjectPool<T> where T : class
{
    private readonly Stack<T> _stack;
    private readonly Func<T> _createFunc;
    private readonly Action<T> _onGet;
    private readonly Action<T> _onRelease;
    private readonly Action<T> _onDestroy;
    private readonly int _maxSize;

    public int CountInactive => _stack.Count;
    public int MaxSize => _maxSize;

    public ObjectPool(
        Func<T> createFunc,
        Action<T> onGet = null,
        Action<T> onRelease = null,
        Action<T> onDestroy = null,
        int initialCapacity = 0,
        int maxSize = int.MaxValue)
    {
        if (createFunc == null) throw new ArgumentNullException(nameof(createFunc));
        if (maxSize <= 0) throw new ArgumentOutOfRangeException(nameof(maxSize), "maxSize must be > 0");

        _createFunc = createFunc;
        _onGet = onGet;
        _onRelease = onRelease;
        _onDestroy = onDestroy;
        _maxSize = maxSize;

        _stack = new Stack<T>(Math.Max(0, initialCapacity));
        if (initialCapacity > 0)
            Prewarm(initialCapacity);
    }

    ~ObjectPool()
    {
        Debug.Log("destruct");
        Clear();
    }

    public void Prewarm(int count)
    {
        for (int i = 0; i < count; i++)
            _stack.Push(_createFunc());
    }

    public T Get()
    {
        T item = _stack.Count > 0 ? _stack.Pop() : _createFunc();
        _onGet?.Invoke(item);
        return item;
    }

    public void Release(T item)
    {
        if (item == null) return;

        _onRelease?.Invoke(item);

        if (_stack.Count < _maxSize)
        {
            _stack.Push(item);
        }
        else
        {
            _onDestroy?.Invoke(item);
        }
    }

    public void Clear()
    {
        if (_onDestroy != null)
            foreach (var item in _stack)
                _onDestroy(item);

        _stack.Clear();
    }
}