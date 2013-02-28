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
using ErikEJ.SqlCe;

namespace SurveyApp.Model.Persistance
{
    public class DataStoreInitializer : ForciblyDeleteDatabaseAlways<DataStore>
    {
        protected override void Seed(DataStore context)
        {
            LoadZipCodeLocations(context);
            LoadFoodGroupsAndFoods(context);
            LoadTechProducts(context);
            LoadSurveys(context);

            base.Seed(context);
        }


        /// <summary>
        /// load application supported zipcodes into SQL from the included csv file
        /// </summary>
        /// <param name="context"></param>
        private static void LoadZipCodeLocations(DbContext context)
        {
            var zipCodeCsvPath = Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath, "ZipCodes.csv");

            using (var sr = new StreamReader(zipCodeCsvPath))
            {
                var zipCodeLocations = sr.ReadToEnd()
                    .Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(line =>
                    {
                        var split = line.Split(',');
                        return new ZipCodeLocation
                        {
                            ZipCode = Int32.Parse(split[0]),
                            City = split[1],
                            State = split[2]
                        };
                    }).ToList();

                using (var bulkInsert = new SqlCeBulkCopy(context.Database.Connection.ConnectionString))
                {
                    bulkInsert.DestinationTableName = "ZipCodeLocation";
                    bulkInsert.WriteToServer(zipCodeLocations.AsDataReader());
                }
            }
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

        private static void LoadTechProducts(DataStore context)
        {
            context.TechProducts.Add(new TechProduct { TechProductName = "Desktop" });
            context.TechProducts.Add(new TechProduct { TechProductName = "Laptop" });
            context.TechProducts.Add(new TechProduct { TechProductName = "Console System" });
            context.TechProducts.Add(new TechProduct { TechProductName = "TV" });
            context.TechProducts.Add(new TechProduct { TechProductName = "Tablet" });
            context.TechProducts.Add(new TechProduct { TechProductName = "Smartphone" });
            context.TechProducts.Add(new TechProduct { TechProductName = "E-Reader" });
        }

        private static void LoadSurveys(DataStore context)
        {
            context.Surveys.Add(new Survey
                {
                    FirstName = "Bill",
                    LastName = "Gates",
                    DateOfBirth = new DateTime(1955, 10, 28),
                    FavoriteColor = KnownColor.Green,
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
                    TechProducts = context.TechProducts.Local.ToList(),
                    ZipCode = 98039,
                });

            context.Surveys.Add(new Survey
            {
                FirstName = "Steve",
                LastName = "Jobs",
                DateOfBirth = new DateTime(1955, 2, 24),
                FavoriteColor = KnownColor.Silver,
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
                TechProducts = context.TechProducts.Local.ToList(),
                ZipCode = 94303,
            });

            context.Surveys.Add(new Survey
            {
                FirstName = "Marissa",
                LastName = "Mayer",
                DateOfBirth = new DateTime(1975, 5, 30),
                FavoriteColor = KnownColor.Red,
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
                TechProducts = context.TechProducts.Local.ToList(),
                ZipCode = 94303,
            });

            context.Surveys.Add(new Survey
            {
                FirstName = "Mark",
                LastName = "Zuckerberg",
                DateOfBirth = new DateTime(1984, 5, 14),
                FavoriteColor = KnownColor.Blue,
                FavoriteFoods = new List<Food>
                        {
                            context.Foods.Local.Single(f => f.Name == "Oatmeal"), 
                            context.Foods.Local.Single(f => f.Name == "Cheese")
                        },
                Gender = Gender.Male,
                TechProducts = context.TechProducts.Local.ToList(),
                ZipCode = 94303,
            });

            context.Surveys.Add(new Survey
            {
                FirstName = "Queen Elizabeth",
                LastName = "II",
                DateOfBirth = new DateTime(1926, 4, 21),
                FavoriteColor = KnownColor.Gold,
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
                TechProducts = context.TechProducts.Local.ToList(),
                ZipCode = 98004,
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
