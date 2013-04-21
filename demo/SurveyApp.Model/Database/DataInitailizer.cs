using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using Raven.Client;
using Raven.Database.Linq.PrivateExtensions;
using SurveyApp.Model.Models;

namespace SurveyApp.Model.Database
{
    public static class DataInitializer
    {
        public static void PopulateData(IDocumentStore initializedStore)
        {
            using (var session = initializedStore.OpenSession())
            {
                LoadFoodGroupsAndFoods(session);
                session.SaveChanges();
            }

            using (var session = initializedStore.OpenSession())
            {
                LoadSurveys(session);   //Load survey uses some randomness from above
                session.SaveChanges();
            }
        }

        private static void LoadFoodGroupsAndFoods(IDocumentSession session)
        {
            session.Store(new FoodGroup
            {
                Name = "Fruits",
                Foods = new List<Food>
                {
                    new Food{Name = "Apples", HealthyScore = 9},
                    new Food{Name = "Bananas", HealthyScore = 8},
                    new Food{Name = "Grapes", HealthyScore = 8},
                    new Food{Name = "Oranges", HealthyScore = 9},
                    new Food{Name = "Pears", HealthyScore = 8},				
                }
            });


            session.Store(new FoodGroup
            {
                Name = "Vegetable",
                Foods = new List<Food>
                {
                    new Food{Name = "Broccoli", HealthyScore = 10},
                    new Food{Name = "Spinach", HealthyScore = 9},
                    new Food{Name = "Corn", HealthyScore = 7},
                    new Food{Name = "Carrots", HealthyScore = 8},
                    new Food{Name = "Tomatoes", HealthyScore = 9},				
                }
            });


            session.Store(new FoodGroup
            {
                Name = "Grains",
                Foods = new List<Food>
                {
                    new Food{Name = "Pasta", HealthyScore = 4},
                    new Food{Name = "Rice", HealthyScore = 5},
                    new Food{Name = "White Bread", HealthyScore = 4},
                    new Food{Name = "Oatmeal", HealthyScore = 8},
                    new Food{Name = "Tortillas", HealthyScore = 3},				
                }
            });


            session.Store(new FoodGroup
            {
                Name = "Protein Foods",
                Foods = new List<Food>
                {
                    new Food{Name = "Bacon", HealthyScore = 6},		
                    new Food{Name = "Beef", HealthyScore = 5},
                    new Food{Name = "Chicken", HealthyScore = 6},
                    new Food{Name = "Beans", HealthyScore = 8},
                    new Food{Name = "Tofu", HealthyScore = 8},
                }
            });


            session.Store(new FoodGroup
            {
                Name = "Dairy",
                Foods = new List<Food>
                {
                    new Food{Name = "Milk", HealthyScore = 5},
                    new Food{Name = "Cheese", HealthyScore = 2},
                    new Food{Name = "Butter", HealthyScore = 1},
                    new Food{Name = "Ice Cream", HealthyScore = 1},
                    new Food{Name = "Yogurt", HealthyScore = 6},				
                }
            });
        }

        private static void LoadSurveys(IDocumentSession session)
        {
            var allFoods = session.Query<FoodGroup>().ToList().SelectMany(x => x.Foods).ToList();

            session.Store(new Survey
                {
                    FirstName = "Bill",
                    LastName = "Gates",
                    PersonIdNumber = 1234,
                    LikesBooleans = true,
                    DateOfBirth = new DateTime(1955, 10, 28),
                    FavoriteWebsite = "http://www.apple.com",
                    FavoriteColorId = KnownColor.Green,
                    FavoriteFoodId = GetRandomFood(allFoods),
                    Children = new List<Relation>
                        {
                            new Relation{Name = "Jennifer"},
                            new Relation{Name = "Rory"},
                            new Relation{Name = "Phoebe"}
                        },
                    Gender = Gender.Male,
                    TechProducts = new List<TechProduct>
                        {
                            new Desktop
                                {
                                    Mhz = 4000,
                                    GigsOfRam = 32,
                                    HasSsd = true,
                                    NumberOfMonitors = 3,
                                },
                            new PointAndShoot
                                {
                                    MegaPixels = 20,
                                    XZoom = 30
                                }
                        },
                    HomeLocation = new Location
                        {
                            FormattedLocation = "Medina, WA 98039, USA",
                            Latitude = 47.6258071,
                            Longitude = -122.2421963
                        },
                });


            session.Store(new Survey
            {
                FirstName = "Steve",
                LastName = "Jobs",
                PersonIdNumber = 2345,
                LikesBooleans = true,
                DateOfBirth = new DateTime(1955, 2, 24),
                DateOfDeath = new DateTime(2011, 10, 5),
                FavoriteWebsite = "http://www.google.com",
                FavoriteColorId = KnownColor.Silver,
                FavoriteFoodId = GetRandomFood(allFoods),
                Children = new List<Relation>
                        {
                            new Relation{Name = "Lisa"},
                            new Relation{Name = "Reed"},
                            new Relation{Name = "Erin"},
                            new Relation{Name = "Eve"}
                        },
                Gender = Gender.Male,
                TechProducts = new List<TechProduct>
                        {
                            new Laptop
                                {
                                    Mhz = 2500,
                                    GigsOfRam = 16,
                                    HasSsd = true,
                                    ScreenSize = 13,
                                },
                            new Slr
                                {
                                    MegaPixels = 18.2,
                                    NumberOfLenses = 10
                                }
                        },
                HomeLocation = new Location
                {
                    FormattedLocation = "San Francisco, CA, USA",
                    Latitude = 37.7749295,
                    Longitude = -122.4194155
                }
            });


            session.Store(new Survey
            {
                FirstName = "Marissa",
                LastName = "Mayer",
                PersonIdNumber = 3456,
                LikesBooleans = false,
                FavoriteWebsite = "http://www.yahoo.com",
                DateOfBirth = new DateTime(1975, 5, 30),
                FavoriteColorId = KnownColor.Red,
                FavoriteFoodId = GetRandomFood(allFoods),
                Children = new List<Relation>
                        {
                            new Relation{Name = "Macallister"},
                        },
                Gender = Gender.Female,
                TechProducts = new List<TechProduct>
                        {
                            new Laptop
                                {
                                    Mhz = 2600,
                                    GigsOfRam = 8,
                                    HasSsd = false,
                                    ScreenSize = 15,
                                },
                            new PointAndShoot
                                {
                                    MegaPixels = 18.2,
                                    XZoom = 22
                                }
                        },
                HomeLocation = new Location
                {
                    FormattedLocation = "Sunnyvale, CA 94089, USA",
                    Latitude = 37.4298335,
                    Longitude = -122.004179
                }
            });


            session.Store(new Survey
            {
                FirstName = "Mark",
                LastName = "Zuckerberg",
                PersonIdNumber = 4567,
                LikesBooleans = true,
                DateOfBirth = new DateTime(1984, 5, 14),
                FavoriteWebsite = "http://www.linkedin.com",
                FavoriteColorId = KnownColor.Blue,
                FavoriteFoodId = GetRandomFood(allFoods),
                Gender = Gender.Male,
                TechProducts = new List<TechProduct>
                        {
                            new Laptop
                                {
                                    Mhz = 2200,
                                    GigsOfRam = 8,
                                    HasSsd = false,
                                    ScreenSize = 15,
                                },
                            new Desktop
                                {
                                    Mhz = 3200,
                                    GigsOfRam = 32,
                                    HasSsd = true,
                                    NumberOfMonitors = 3,
                                },
                            new PointAndShoot
                                {
                                    MegaPixels = 18.2,
                                    XZoom = 22
                                }
                        },
                HomeLocation = new Location
                {
                    FormattedLocation = "Palo Alto, CA 94301, USA",
                    Latitude = 37.4457966,
                    Longitude = -122.1575745
                }
            });


            session.Store(new Survey
            {
                FirstName = "Queen Elizabeth",
                LastName = "II",
                PersonIdNumber = 5678,
                LikesBooleans = false,
                FavoriteWebsite = "http://www.bbc.co.uk/",
                DateOfBirth = new DateTime(1926, 4, 21),
                FavoriteColorId = KnownColor.Gold,
                FavoriteFoodId = GetRandomFood(allFoods),
                Children = new List<Relation>
                        {
                            new Relation{Name = "Princess Anne", Children = new List<Relation>
                                {
                                    new Relation{Name = "Peter Phillips"},
                                    new Relation{Name = "Zara Phillips"}
                                }},
                            new Relation{Name = "Prince Charles", Children = new List<Relation>
                                {
                                    new Relation{Name = "Prince William"},
                                    new Relation{Name = "Prince Henry"}
                                }},
                        },
                Gender = Gender.Female,
                TechProducts = new List<TechProduct>
                        {
                            new Desktop()
                                {
                                    Mhz = 1800,
                                    GigsOfRam = 4,
                                    HasSsd = false,
                                    NumberOfMonitors = 1,
                                }
                        },
                HomeLocation = new Location
                {
                    FormattedLocation = "Buckingham Palace, London, Greater London SW1A 1AA, UK",
                    Latitude = 51.501364,
                    Longitude = -0.14189
                }
            });
        }

        public static Guid GetRandomFood(List<Food> foods)
        {
            var rnd = new Random();
            return foods[rnd.Next(0, foods.Count - 1)].Id;
        }
    }
}
