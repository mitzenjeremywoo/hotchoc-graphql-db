using HotChocolate.Data;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace app
{
    public class Query
    {
        [UsePaging]
        [UseProjection]
        [UseSorting]
        [UseFiltering]
        public IExecutable<Restaurant> GetPersons([Service] IMongoCollection<Restaurant> collection)
        {
            return collection.AsExecutable();
        }

        [UseFirstOrDefault]
        public IExecutable<Restaurant> GetAllRestaurant(
            [Service] IMongoCollection<Restaurant> collection)
        {
            return collection.AsExecutable();
        }
    }   
}

[Node(
    IdField = nameof(cuisine),
    NodeResolverType = typeof(PersonNodeResolver),
    NodeResolver = nameof(PersonNodeResolver.ResolveAsync))]
public class Restaurant
{
    [BsonId]
    public MongoDB.Bson.ObjectId _id { get; set; }   
    public string borough { get; set; }
    public string cuisine { get; set; }
}

public class PersonNodeResolver
{
    public Task<Restaurant> ResolveAsync(
        [Service] IMongoCollection<Restaurant> collection,
        string cuisine)
    {
        return collection.Find(x => x.cuisine == cuisine).FirstOrDefaultAsync();
    }
}