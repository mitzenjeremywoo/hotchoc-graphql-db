using MongoDB.Driver;
using mongodbapp.Model;

namespace app
{
    public class Mutations
    {        
        public async Task<CreateRestaurantPayload> AddRestaurant([Service] IMongoCollection<Restaurant> collection, RestaurantInput input)
        {
            var restaurant = new Restaurant()
            {
                 borough = input.borough,
                 cuisine = input.cuisine,
            };
            await collection.InsertOneAsync(restaurant);
            return new CreateRestaurantPayload(restaurant);
        }
        
        //public bool UpdateRestaurant([Service] IMongoCollection<Restaurant> collection, Restaurant restaurant)
        //{
        //    collection.InsertOne(restaurant);
        //    return true;
        //}

        //public DeleteResult DeleteRestaurant(
        //    [Service] IMongoCollection<Restaurant> collection, ObjectId id)
        //{
        //    return collection.DeleteOne(x => x._id == id);
        //}
    }   
}
