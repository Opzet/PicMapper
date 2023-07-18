// Licensed under the MIT license. See LICENSE.md file in this folder for details

using System;
using DynamicData;

namespace Mapsui.Layers;

internal static class Helpers
{
    public static IObservable<IChangeSet<T>> WhereItemNotNull<T>(this IObservable<IChangeSet<T?>> collection)
    {
        return collection.Filter(feature => feature is not null)!;
    }

    public static IObservable<IChangeSet<T,TKey>> WhereItemNotNull<T, TKey>(this IObservable<IChangeSet<T?, TKey>> collection)
        where TKey : notnull
    {
        return collection.Filter(feature => feature is not null)!;
    }
}
