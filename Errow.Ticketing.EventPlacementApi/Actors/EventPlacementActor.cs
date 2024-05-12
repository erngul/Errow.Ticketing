using System.Text.Json;
using Dapr.Actors.Runtime;
using Errow.Ticketing.EventPlacementApi.Interfaces;
using Errow.Ticketing.EventPlacementApi.Models;

namespace Errow.Ticketing.EventPlacementApi.Actors;

public class EventPlacementActor(ActorHost host) : Actor(host), IEventPlacementActor
{
    private EventPlacement _ep = new EventPlacement();

    public async Task<EventPlacement> ReserveAsync(string seatId)
    {
        _ep = await this.StateManager.GetStateAsync<EventPlacement>("ep");

        if (_ep is null)
        {
            throw new ArgumentException("Seat not found");
        }

        if (!_ep.Available)
        {
            throw new ArgumentException("Seat is not available");
        }

        _ep.Available = false;
        await this.StateManager.SetStateAsync("ep", _ep);

        return _ep;
    }

    public async Task CreateAsync(EventPlacement eventPlacement)
    {
        var test = Id.GetId();
        await this.StateManager.SetStateAsync("ep", eventPlacement);
    }
    
    public async Task<EventPlacement> GetStateAsync()
    {
        return await StateManager.GetStateAsync<EventPlacement>("ep");
    }

    // class TimerParams
    // {
    //     public int IntParam { get; set; }
    //     public string StringParam { get; set; }
    // }

    // // protected override Task OnActivateAsync()
    // // {
    // //     return RegisterTimer();
    // // }
    //
    // /// <inheritdoc/>
    // public Task RegisterTimer()
    // {
    //     var timerParams = new TimerParams
    //     {
    //         IntParam = 100,
    //         StringParam = "timer test",
    //     };
    //
    //     var serializedTimerParams = JsonSerializer.SerializeToUtf8Bytes(timerParams);
    //     return this.RegisterTimerAsync("TestTimer", nameof(this.TimerCallback), serializedTimerParams,
    //         TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3));
    // }
    //
    // /// <summary>
    // /// This method is called when the timer is triggered based on its registration.
    // /// It updates the PropertyA value.
    // /// </summary>
    // /// <param name="data">Timer input data.</param>
    // /// <returns>A task that represents the asynchronous operation.</returns>
    // public Task TimerCallback(byte[] data)
    // {
    //     return Task.CompletedTask;
    //     // var state = await this.StateManager.GetStateAsync<MyData>(StateName);
    //     // state.PropertyA = $"Timer triggered at '{DateTime.Now:yyyyy-MM-ddTHH:mm:s}'";
    //     // await this.StateManager.SetStateAsync<MyData>(StateName, state, ttl: TimeSpan.FromMinutes(5));
    //     // var timerParams = JsonSerializer.Deserialize<TimerParams>(data);
    //     // Console.WriteLine("Timer parameter1: " + timerParams.IntParam);
    //     // Console.WriteLine("Timer parameter2: " + timerParams.StringParam);
    // }
}