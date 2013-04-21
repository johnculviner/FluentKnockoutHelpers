using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SurveyApp.Model.Models;
using SurveyApp.Model.Services;

namespace SurveyApp.Web.ApiControllers
{
    public class FoodGroupController : ApiController
    {
        private readonly IFoodGroupService _foodGroupService;

        public FoodGroupController(IFoodGroupService foodGroupService)
        {
            _foodGroupService = foodGroupService;
        }

        public IEnumerable<FoodGroup> Get()
        {
            return _foodGroupService.GetAllFoodGroups();
        } 
    }
}
