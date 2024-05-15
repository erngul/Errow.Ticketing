using System.Runtime.Serialization;

namespace Errow.Ticketing.Contracts.Models;

[DataContract]
public class EventPlacement
{
    [DataMember]
    public string? Id { get; set; }
    [DataMember]
    public int Row { get; set; }
    [DataMember]
    public int Column { get; set; }
    [DataMember]
    public bool Available { get; set; }

    public EventPlacement() { }

    public EventPlacement(string id, int row, int column, bool available)
    {
        Id = id;
        Row = row;
        Column = column;
        Available = available;
    }
}