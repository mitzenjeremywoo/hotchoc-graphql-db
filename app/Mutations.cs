using MongoDB.Bson;
using MongoDB.Driver;
using mongodbapp.Model;

namespace app
{
    public class Mutations
    {
        //mutation { 
        // addRestaurant(input: {
        //    borough: "test", 
        // cuisine: "Bakery2" }) {
        //      restaurant {
        //         id
        //    }
        //  }
        //}

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

        //mutation {
        //   updateRestaurant(input: {
        //    id: "6636a133f2b3a33b5885b387", borough: "new", cuisine: "american"
        //   }) {
        //         restaurant {
        //               borough
        //        }
        //   }
        //}
        public async Task<CreateRestaurantPayload> UpdateRestaurant([Service] IMongoCollection<Restaurant> collection, RestaurantInput input)
        {          
            var restaurant = new Restaurant()
            {
                _id = new ObjectId(input.id),
                borough = input.borough,
                cuisine = input.cuisine,
            };

            var update = Builders<Restaurant>.Update
                .Set("cuisine", "American")
                .Set("borough", input.borough);
         
            await collection.UpdateOneAsync(x => x._id == restaurant._id, update);
            return new CreateRestaurantPayload(restaurant);
        }

        //public DeleteResult DeleteRestaurant(
        //    [Service] IMongoCollection<Restaurant> collection, ObjectId id)
        //{
        //    return collection.DeleteOne(x => x._id == id);
        //}
    }
}
