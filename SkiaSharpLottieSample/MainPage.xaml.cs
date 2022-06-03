using Microsoft.Maui.Dispatching;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace SkiaSharpLottieSample;

public partial class MainPage : ContentPage
{
    SkiaSharp.Skottie.Animation animation;
    double time;

    public MainPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Lottie file from https://lottiefiles.com/107653-trophy
        var json = await FileSystem.OpenAppPackageFileAsync("107653-trophy.json");

        // load Lottie animation
        SkiaSharp.Skottie.Animation.TryCreate(json, out animation);

        // Read important values
        var duration = animation.Duration;
        var interval = 1.0 / 60.0;

        // Start a timer to tick
        Dispatcher.StartTimer(TimeSpan.FromSeconds(interval), () =>
        {
            // Move to the next frame
            animation.SeekFrameTime(time);

            // Increase the time for the next frame
            time = (time + interval) % duration;

            // Invalidate the canvas
            skiaView.InvalidateSurface();

            return true;
        });
    }

    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;

        // Clear the canvas
        canvas.Clear(SKColors.Transparent);

        // Render the animation
        animation?.Render(canvas, e.Info.Rect);
    }
}
