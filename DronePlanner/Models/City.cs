using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DronePlanner.Models;

internal class City
{
    public int Id { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public double Temp { get; set; }
    public double Humidity { get; set; }
    public double Wind { get; set; }
    public int SafeToFly { get; set; }


}
