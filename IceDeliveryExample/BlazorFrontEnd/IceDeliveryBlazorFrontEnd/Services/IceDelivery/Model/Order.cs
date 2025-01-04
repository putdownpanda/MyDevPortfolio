using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceDeliveryBlazorFrontEnd.Services.IceDelivery.Model;
public class Order
{
    public int Id { get; set; }
    [Required]
    public string CustomerName { get; set; } = string.Empty;
    [Required]
    public string DeliveryAddress { get; set; } = string.Empty;
    [Range(1, 1000)]
    public int QuantityInKg { get; set; }
    public DateTime OrderDate { get; set; }
}
