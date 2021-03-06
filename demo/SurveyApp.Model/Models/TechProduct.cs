﻿using System.ComponentModel.DataAnnotations;

namespace SurveyApp.Model.Models
{
    public abstract class TechProduct
    {
    }

    #region Computer
    public abstract class Computer : TechProduct
    {
        [Range(0, 10000)]
        public int Mhz { get; set; }

        [Display(Name = "GB RAM")]
        public int GigsOfRam { get; set; }
        public bool HasSsd { get; set; }
    }

    public class Desktop : Computer
    {
        [Display(Name = "# of Monitors")]
        public int NumberOfMonitors { get; set; }
    }

    public class Laptop : Computer
    {
        [Display(Name = "Screen Size")]
        public double ScreenSize { get; set; }
    }
    #endregion


    #region Digital Camera
    public abstract class DigitalCamera : TechProduct
    {
        [Display(Name = "MegaPixels")]
        public double MegaPixels { get; set; }
    }

    public class PointAndShoot : DigitalCamera
    {
        [Display(Name = "X Zoom")]
        public double XZoom { get; set; }
    }

    public class Slr : DigitalCamera
    {
        [Display(Name = "# of Lenses")]
        public int NumberOfLenses { get; set; }
    }
    #endregion

}
