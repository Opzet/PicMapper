// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Avalonia;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Diagnostics;
using System.Reactive.Concurrency;

namespace MMKiwi.PicMapper.Gui.Avalonia;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        TaskScheduler.UnobservedTaskException += (s, e) => Exceptions.OnError(e.Exception);
        RxApp.DefaultExceptionHandler = Exceptions;
        try
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        catch(Exception ex)
        {
            Exceptions.OnError(ex);
        }
    }

    private static ExceptionHandler Exceptions { get; } = new();


    private class ExceptionHandler : IObserver<Exception>
    {
        public void OnNext(Exception value)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }

            RxApp.MainThreadScheduler.Schedule(() => throw value);
        }

        public void OnError(Exception error)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }

            RxApp.MainThreadScheduler.Schedule(() => throw error);
        }

        public void OnCompleted()
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }

            RxApp.MainThreadScheduler.Schedule(() => throw new NotImplementedException());
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
}
