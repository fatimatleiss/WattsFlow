using System.ComponentModel.DataAnnotations;
using WattsFlowProject.Models;

public class TrafficByMonthViewModel
{
    public string CustomerName { get; set; }
    public string CustomerAddress { get; set; }
    public int PreviousTraffic { get; set; }
    public DateTime PreviousTrafficDate { get; set; }
    public int CurrentTraffic { get; set; }
    public DateTime CurrentTrafficDate { get; set; }
    public decimal KwPrice { get; set; }
    public decimal LiraRate { get; set; }
    public decimal Consumption { get; set; }
    public decimal PriceUSD { get; set; }
    public decimal PriceLBP { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public string Month { get; set; }
    public int Year { get; set; }



}