
using Errow.Ticketing.Contracts.Models;

namespace Errow.Ticketing.EventPlacementApi.Interfaces;

public interface IEventPlacementService
{
    Task<List<EventPlacement>> GetAvailableEventPlacementAsync();
    Task InitializeEventPlacementsAsync();
}