﻿using System.Configuration;

namespace TouchMouseMate.Configuration
{
    public class TouchMouseMateSection : ConfigurationSection
    {
        [ConfigurationProperty("minClickTimeout", IsRequired = true)]
        [IntegerValidator]
        public int MinClickTimeout => (int)base["minClickTimeout"];

        [ConfigurationProperty("maxClickTimeout", IsRequired = true)]
        [IntegerValidator]
        public int MaxClickTimeout => (int)base["maxClickTimeout"];

	    [ConfigurationProperty("middleClick", IsRequired = true)]
        public bool MiddleClick
        {
            get { return ((bool)base["middleClick"]); }
            set { base["middleClick"] = value; }
        }

        [ConfigurationProperty("touchOverClick", IsRequired = true)]
        public bool TouchOverClick
        {
            get { return (bool)base["touchOverClick"]; }
            set { base["touchOverClick"] = value; }
        }

        [ConfigurationProperty("leftHandMode", IsRequired = true)]
        public bool LeftHandMode
        {
            get { return (bool)base["leftHandMode"]; }
            set { base["leftHandMode"] = value; }
        }

        [ConfigurationProperty("moveDetect", IsRequired = true)]
        public bool MoveDetect
        {
            get { return (bool) base["moveDetect"]; }
            set { base["moveDetect"] = value; }
        }

        [ConfigurationProperty("moveThreshold", IsRequired = true)]
        public double MoveThreshold
        {
            get { return (double) base["moveThreshold"]; }
            set { base["moveThreshold"] = value; }
        }
    }
}