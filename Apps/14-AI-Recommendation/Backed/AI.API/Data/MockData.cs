using AI.API.Models;

namespace AI.API.Data
{
    public static class MockData
    {
        private static readonly Random _random = new();

        public static List<UserInteraction> GetInteractions()
        {
            var interactions = new List<UserInteraction>();
            var itemTypes = new[] { "Hotel", "Room", "Restaurant", "Event" };
            var interactionTypes = new[] { "View", "Click", "Book", "Favorite", "Review" };
            
            for (int i = 1; i <= 500; i++)
            {
                interactions.Add(new UserInteraction
                {
                    Id = i,
                    UserId = _random.Next(1, 20),
                    ItemId = _random.Next(1, 50),
                    ItemType = itemTypes[_random.Next(itemTypes.Length)],
                    InteractionType = interactionTypes[_random.Next(interactionTypes.Length)],
                    Rating = _random.Next(0, 10) > 7 ? _random.Next(1, 6) : null,
                    InteractionDate = DateTime.Now.AddDays(-_random.Next(0, 90)),
                    DurationSeconds = _random.Next(5, 300)
                });
            }
            
            return interactions;
        }

        public static List<Recommendation> GetRecommendations()
        {
            var recommendations = new List<Recommendation>();
            var itemTypes = new[] { "Hotel", "Room", "Restaurant", "Event" };
            var algorithms = new[] { "CollaborativeFiltering", "ContentBased", "Popular" };
            
            for (int i = 1; i <= 200; i++)
            {
                recommendations.Add(new Recommendation
                {
                    Id = i,
                    UserId = _random.Next(1, 20),
                    ItemId = _random.Next(1, 50),
                    ItemType = itemTypes[_random.Next(itemTypes.Length)],
                    Score = (decimal)_random.NextDouble(),
                    Algorithm = algorithms[_random.Next(algorithms.Length)],
                    IsClicked = _random.Next(0, 10) > 7,
                    IsBooked = _random.Next(0, 10) > 8,
                    RecommendedAt = DateTime.Now.AddDays(-_random.Next(0, 30)),
                    ClickedAt = _random.Next(0, 10) > 7 ? DateTime.Now.AddDays(-_random.Next(0, 30)) : null,
                    BookedAt = _random.Next(0, 10) > 8 ? DateTime.Now.AddDays(-_random.Next(0, 30)) : null
                });
            }
            
            return recommendations;
        }
    }
}