using System;

public class Report
{
    public string idClient { get; set; }
    public int idConfig { get; set; }
    public bool success { get; set; }
    public string message { get; set; }
    public DateTime date { get; set; }
    public int size { get; set; }
}