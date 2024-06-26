﻿using HotChocolate.Data;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace app
{
    public class Query
    {
         //       query { 
         //         restaurantByCuisine(cuisine: "Bakery")
         //         {
         //           nodes
         //           {
         //             borough
         //           }
         //         }
         //       }

        [UsePaging]
        [UseProjection]
        [UseSorting]
        [UseFiltering]
        public IExecutable<Restaurant> GetRestaurantByCuisine([Service] IMongoCollection<Restaurant> collection, string cuisine)
        {
            return collection.Find(x => x.cuisine == cuisine).AsExecutable();
        }

        public IExecutable<Restaurant> GetAllRestaurant(
            [Service] IMongoCollection<Restaurant> collection)
        {

            return collection.AsExecutable();
        }
    }   
}

[Node(
    IdField = nameof(cuisine),
    NodeResolverType = typeof(RestaurantNodeResolver),
    NodeResolver = nameof(RestaurantNodeResolver.ResolveAsync))]
    public class Restaurant
    {
        [BsonId]
        public MongoDB.Bson.ObjectId _id { get; set; }
    
        public string borough { get; set; }
        public string cuisine { get; set; }
    }

    public class RestaurantNodeResolver
    {
        public Task<Restaurant> ResolveAsync(
            [Service] IMongoCollection<Restaurant> collection,
            string cuisine)
        {
            return collection.Find(x => x.cuisine == cuisine).FirstOrDefaultAsync();
        }
    }