namespace mongodbapp.Model
{
    public record RestaurantInput(string id, string cuisine, string borough);

    public record CreateRestaurantPayload(Restaurant Restaurant);
}
   
