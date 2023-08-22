﻿using Paprika.Data;
using Paprika.Store;

namespace Paprika.Chain;

/// <summary>
/// Provides a component that combined <see cref="SlottedArray"/> that can be wrapped over a <see cref="Page"/>
/// that comes from <see cref="BufferPool"/>. 
/// </summary>
public readonly struct InBlockMap
{
    private readonly Page _page;

    public InBlockMap(Page page) => _page = page;

    public bool TrySet(in Key key, ReadOnlySpan<byte> data)
    {
        var map = new SlottedArray(_page.Span);
        return map.TrySet(key, data);
    }

    public bool TryGet(scoped in Key key, out ReadOnlySpan<byte> result)
    {
        var map = new SlottedArray(_page.Span);
        return map.TryGet(key, out result);
    }

    public void Apply(IBatch batch)
    {
        var map = new SlottedArray(_page.Span);
        foreach (var item in map.EnumerateAll())
        {
            batch.SetRaw(item.Key, item.RawData);
        }
    }

    public SlottedArray.NibbleEnumerator GetEnumerator()
    {
        var map = new SlottedArray(_page.Span);
        return map.EnumerateAll();
    }
}
