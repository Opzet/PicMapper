# Mapsui.ReactiveExtensions 

This folder contains a new Layer type for [MapsUi](https://github.com/Mapsui/Mapsui) 
that uses [ReactiveUI](https://www.reactiveui.net/) to keep the layer in sync with
an ObservableCollection or a [DynamicData] (https://www.reactiveui.net/docs/handbook/collections/)
SourceList or SourceCache. 

The layer will be kept in sync with the original collection. As items are added and removed from it,
they'll be updated in the map. A transformation function is required to convert the underlying type
to a MapsUi IFeature (e.g. PointFeature).

## License

Since this code is based off the code for MapsUi, this project is licensed under the [MIT license](License.md). 
THe original code is © The Mapsui authors, and all modifications are © Micah Makaiwi.

## Usage

```csharp
record class MyDataClass(string Name, double X, double Y);
ObservableCollection<MyDataClass> myCollection = new(); 

public void CreateLayer()
{
    var MyLayer = ReactiveObservableLayer.Create(d => new PointFeature(d.X, d.Y), // The function that converts MyDataClass to an IFeature
                                                    myCollection,      // ObservableColelction<T>
                                                                      // or ReadOnlyObservableCollection<T>
                                                                      // or IObservable<IChangeSet<T>> (i.e. SourceList)
                                                                      // or IObservable<IChangeSet<TValue,TKey>> (i.e. SourceCache)
                                                    "CurrentImages"); // Layer Name
    MapView.Map.Layers.Add(MyLayer);
}
```