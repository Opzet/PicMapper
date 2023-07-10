namespace MMKiwi.PicMapper.Models.Services;

public interface IFileLoader
{
    IAsyncEnumerable<IBitmapProvider> LoadImageAsync();
}