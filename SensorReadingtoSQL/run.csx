#r "Newtonsoft.Json"

using System;
using System.Configuration;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

public static void Run(string myEventHubMessage, TraceWriter log)
{
    log.Info($"C# Event Hub trigger function processed a message: {myEventHubMessage}");

    // Put the message object into class
    SensorReading myR = new SensorReading();
    myR = JsonConvert.DeserializeObject<SensorReading>(myEventHubMessage);

    using (var db = new SensorReadingContext())
    {
        db.SensorReadings.Add(myR);
        db.SaveChanges();
    }

}

[Table("SensorReading")]
public class SensorReading
{
    public int Id { get; set; }
    public string DeviceId { get; set; }
    public string SensorName { get; set; }
    public string SensorType { get; set; }
    public string SensorValue { get; set; }
    public DateTime? OutputTime { get; set; }
}

public class SensorReadingContext : DbContext
{
    public SensorReadingContext()
        : base("name=SensorReadingContext")
    {
    }

    public virtual DbSet<SensorReading> SensorReadings { get; set; }
}