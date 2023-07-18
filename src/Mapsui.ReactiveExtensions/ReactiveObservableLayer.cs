using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using DynamicData;
using System.Reactive.Linq;
using ReactiveUI;
using DynamicData.Binding;
using Mapsui.Styles;
using Mapsui.Extensions;

namespace Mapsui.Layers;

public static class ReactiveObservableLayer
{

    public static ReactiveObservableLayer<T> Create<T>(Func<T, IFeature?> getFeature, ReadOnlyObservableCollection<T> baseCollection, string? name = null) where T : class
    {
        return new(getFeature, baseCollection.ToObservableChangeSet().Transform(getFeature).WhereItemNotNull());
    }

    public static ReactiveObservableLayer<T> Create<T>(Func<T, IFeature?> getFeature, ObservableCollection<T> baseCollection, string? name = null) where T : class
    {
        return new(getFeature, baseCollection.ToObservableChangeSet().Transform(getFeature).WhereItemNotNull());
    }

    public static ReactiveObservableLayer<T> Create<T>(Func<T, IFeature?> getFeature, IObservable<IChangeSet<T>> baseCollection, string? name = null) where T : class
    {
        return new(getFeature, baseCollection.Transform(getFeature).WhereItemNotNull());
    }

    public static ReactiveObservableLayer<T> Create<T, TKey>(Func<T, IFeature?> getFeature, IObservable<IChangeSet<T, TKey>> baseCollection, string? name = null)
        where T : class
        where TKey : notnull
    {
        ReadOnlyObservableCollection<IFeature> x;
        baseCollection.Transform(getFeature).WhereItemNotNull().ObserveOn(RxApp.MainThreadScheduler).Bind(out x).Subscribe();
        var res = new ReactiveObservableLayer<T>(getFeature, x, name);
        res.Subscribe(baseCollection);
        return res;
    }
}

public class ReactiveObservableLayer<T> : BaseLayer
    where T : class
{
    private readonly ReadOnlyObservableCollection<IFeature> _observableCollection;

    internal ReactiveObservableLayer(Func<T, IFeature?> getFeature, IObservable<IChangeSet<IFeature>> baseCollection, string? name = null)
    : base(name ?? "ReactiveObservableMemoryLayer")
    {
        baseCollection.ObserveOn(RxApp.MainThreadScheduler).Bind(out _observableCollection).Subscribe((_) =>
        {
            DataHasChanged();
        });
    }

    internal ReactiveObservableLayer(Func<T, IFeature?> getFeature, ReadOnlyObservableCollection<IFeature> baseCollection, string? name = null)
        : base(name ?? "ReactiveObservableMemoryLayer")
    {
        _observableCollection = baseCollection;

    }

    internal void Subscribe<TVal, TKey>(IObservable<IChangeSet<TVal, TKey>> observable) where TKey : notnull
    {
        observable.Subscribe((_) =>
        {
            DataHasChanged();
        });
    }

    IEnumerable<IFeature> Features => _observableCollection;
    public override MRect? Extent => Features.GetExtent();

    public override IEnumerable<IFeature> GetFeatures(MRect? rect, double resolution)
    {
        if (rect == null)
        {
            return new List<IFeature>();
        }

        MRect biggerRect = rect!.Grow(SymbolStyle.DefaultWidth * 2.0 * resolution, SymbolStyle.DefaultHeight * 2.0 * resolution);
        return Features.Where((IFeature f) => f.Extent?.Intersects(biggerRect) ?? false);
    }

}