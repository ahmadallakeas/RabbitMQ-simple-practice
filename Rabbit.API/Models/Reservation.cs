using System;
using System.Collections.Generic;

namespace Rabbit.API.Models;

public partial class Reservation
{
    public int ReservationId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Date { get; set; }
}
