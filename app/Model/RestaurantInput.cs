namespace mongodbapp.Model
{
    public record RestaurantInput(string cuisine, string borough);

    public record CreateRestaurantPayload(Restaurant Restaurant);
}
   
