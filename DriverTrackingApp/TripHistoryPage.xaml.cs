using DriverTrackingApp.Services;
using Microsoft.EntityFrameworkCore;

namespace DriverTrackingApp;

public partial class TripHistoryPage : ContentPage
{
    private readonly AppDbContext _db;
    private readonly GetTrips _getTrips;

    public TripHistoryPage(AppDbContext db, GetTrips getTrips)
    {
        InitializeComponent();
        _db = db;
        _getTrips = getTrips;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadTripsAsync();
    }

    private async Task LoadTripsAsync()
    {
        var trips = await _db.TripData
            .OrderByDescending(t => t.StartTime)
            .ToListAsync();

        // Display in a CollectionView or ListView
        TripCollectionView.ItemsSource = trips;
    }
}