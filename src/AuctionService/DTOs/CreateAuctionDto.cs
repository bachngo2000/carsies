using System.ComponentModel.DataAnnotations;

namespace AuctionService.DTOs;

// To create an auctionDto, we'll need the properties that we require from the user when they attempt to create an auction
public class CreateAuctionDto
{   [Required]
    public string Make {get; set;}
    [Required]
    public string Model {get; set;}
    [Required]
    public int Year {get; set;}
    [Required]
    public string Color {get; set;}
    [Required]
    public int Mileage {get; set;}
    [Required]
    public string ImageUrl {get; set;}
    [Required]
    public int ReservePrice {get; set;}
    [Required]
    public DateTime AuctionEnd {get; set;}

}