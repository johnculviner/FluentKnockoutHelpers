using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SurveyApp.Model.Helpers;
using SurveyApp.Model.Models;

namespace SurveyApp.Model.Persistance
{
    public class DataStoreInitializer : ForciblyDeleteDatabaseAlways<DataStore>
    {
        protected override void Seed(DataStore context)
        {
            LoadFoodGroupsAndFoods(context);
            LoadSurveys(context);

            base.Seed(context);
        }

        private static void LoadFoodGroupsAndFoods(DataStore context)
        {
            context.FoodGroups.Add(new FoodGroup
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
			
			context.FoodGroups.Add(new FoodGroup
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
			
			context.FoodGroups.Add(new FoodGroup
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
			
			context.FoodGroups.Add(new FoodGroup
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
			
			context.FoodGroups.Add(new FoodGroup
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

        private static void LoadSurveys(DataStore context)
        {
            context.Surveys.Add(new Survey
                {
                    FirstName = "Bill",
                    LastName = "Gates",
                    LikesBooleans = true,
                    DateOfBirth = new DateTime(1955, 10, 28),
                    FavoriteWebsite = "http://www.apple.com",
                    FavoriteColorId = KnownColor.Green,
                    FavoriteFoods = new List<Food>
                        {
                            context.Foods.Local.Single(f => f.Name == "Broccoli"), 
                            context.Foods.Local.Single(f => f.Name == "Cheese")
                        },
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
                                    SerialNumber = "A",
                                    Mhz = 4000,
                                    GigsOfRam = 32,
                                    HasSsd = true,
                                    NumberOfMonitors = 3,
                                },
                            new PointAndShoot
                                {
                                    SerialNumber = "B",
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

            context.Surveys.Add(new Survey
            {
                FirstName = "Steve",
                LastName = "Jobs",
                LikesBooleans = true,
                DateOfBirth = new DateTime(1955, 2, 24),
                DateOfDeath = new DateTime(2011, 10, 5),
                FavoriteWebsite = "http://www.google.com",
                FavoriteColorId = KnownColor.Silver,
                FavoriteFoods = new List<Food>
                        {
                            context.Foods.Local.Single(f => f.Name == "Rice"), 
                            context.Foods.Local.Single(f => f.Name == "Tofu")
                        },
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
                                    SerialNumber = "C",
                                    Mhz = 2500,
                                    GigsOfRam = 16,
                                    HasSsd = true,
                                    ScreenSize = 13,
                                },
                            new Slr
                                {
                                    SerialNumber = "D",
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

            context.Surveys.Add(new Survey
            {
                FirstName = "Marissa",
                LastName = "Mayer",
                LikesBooleans = false,
                FavoriteWebsite = "http://www.yahoo.com",
                DateOfBirth = new DateTime(1975, 5, 30),
                FavoriteColorId = KnownColor.Red,
                FavoriteFoods = new List<Food>
                        {
                            context.Foods.Local.Single(f => f.Name == "Yogurt"), 
                            context.Foods.Local.Single(f => f.Name == "Carrots")
                        },
                Children = new List<Relation>
                        {
                            new Relation{Name = "Macallister"},
                        },
                Gender = Gender.Female,
                TechProducts = new List<TechProduct>
                        {
                            new Laptop
                                {
                                    SerialNumber = "E",
                                    Mhz = 2600,
                                    GigsOfRam = 8,
                                    HasSsd = false,
                                    ScreenSize = 15,
                                },
                            new PointAndShoot
                                {
                                    SerialNumber = "F",
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

            context.Surveys.Add(new Survey
            {
                FirstName = "Mark",
                LastName = "Zuckerberg",
                LikesBooleans = true,
                DateOfBirth = new DateTime(1984, 5, 14),
                FavoriteWebsite = "http://www.linkedin.com",
                FavoriteColorId = KnownColor.Blue,
                FavoriteFoods = new List<Food>
                        {
                            context.Foods.Local.Single(f => f.Name == "Oatmeal"), 
                            context.Foods.Local.Single(f => f.Name == "Cheese")
                        },
                Gender = Gender.Male,
                TechProducts = new List<TechProduct>
                        {
                            new Laptop
                                {
                                    SerialNumber = "G",
                                    Mhz = 2200,
                                    GigsOfRam = 8,
                                    HasSsd = false,
                                    ScreenSize = 15,
                                },
                            new Desktop
                                {
                                    SerialNumber = "H",
                                    Mhz = 3200,
                                    GigsOfRam = 32,
                                    HasSsd = true,
                                    NumberOfMonitors = 3,
                                },
                            new PointAndShoot
                                {
                                    SerialNumber = "I",
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

            context.Surveys.Add(new Survey
            {
                FirstName = "Queen Elizabeth",
                LastName = "II",
                LikesBooleans = false,
                FavoriteWebsite = "http://www.bbc.co.uk/",
                DateOfBirth = new DateTime(1926, 4, 21),
                FavoriteColorId = KnownColor.Gold,
                FavoriteFoods = new List<Food>
                        {
                            context.Foods.Local.Single(f => f.Name == "Ice Cream"), 
                            context.Foods.Local.Single(f => f.Name == "Cheese")
                        },
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
                                    SerialNumber = "J",
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
    }

    public class ForciblyDeleteDatabaseAlways<TContext> : IDatabaseInitializer<TContext> where TContext : DbContext
    {
        public void InitializeDatabase(TContext context)
        {
            if (context.Database.Exists())
                context.Database.Delete();

            context.Database.Create();
            this.Seed(context);
            context.SaveChanges();
        }

        protected virtual void Seed(TContext context)
        {
        }
    }
}
