using UnityEngine;

namespace AtlasLib.Style
{
    public class Style
    {
        public readonly string Id;
        public readonly string Name;
        public readonly Color Colour = StyleColours.White;
        public readonly float FreshnessDecayMultiplier = 1;

        public string FullString
        {
            get
            {
                if (Colour != StyleColours.White)
                {
                    return $"<color={ColorUtility.ToHtmlStringRGB(Colour)}>{Name}</color>";
                }

                return Name;
            }
        }
        
        public Style(string id, string name, float freshnessDecayMultiplier = 1)
        {
            Id = id;
            Name = name;
            FreshnessDecayMultiplier = freshnessDecayMultiplier;
        }

        public Style(string id, string name, Color colour)
        {
            Id = id;
            Name = name;
            Colour = colour;
        }
        
        public Style(string id, string name, float freshnessDecayMultiplier, Color colour)
        {
            Id = id;
            Name = name;
            Colour = colour;
            FreshnessDecayMultiplier = freshnessDecayMultiplier;
        }
    }
}