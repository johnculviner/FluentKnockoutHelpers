using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SurveyApp.Model.Models
{
    public abstract class TechProduct
    {
        public int TechProductId { get; set; }
    }

    #region Computer
    public abstract class Computer : TechProduct
    {
        [Range(1, 10000, ErrorMessage = ">10000 Mhz?! I doubt it. (Error not future proof!)")]
        public int Mhz { get; set; }
        [Required]
        public int GigsOfRam { get; set; }
        public bool HasSsd { get; set; }
    }

    public class Desktop : Computer
    {
        [Required]
        public int NumberOfMonitors { get; set; }
    }

    public class Laptop : Computer
    {
        [Required]
        public double ScreenSize { get; set; }
    }
    #endregion


    #region Digital Camera
    public abstract class DigitalCamera : TechProduct
    {
        [Required]
        public double MegaPixels { get; set; }
    }

    public class PointAndShoot : DigitalCamera
    {
        [Required]
        public double XZoom { get; set; }
    }

    public class Slr : DigitalCamera
    {
        [Required]
        public int NumberOfLenses { get; set; }
    }
    #endregion

}
