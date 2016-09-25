using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWedding.Models
{
    public class Guest
    {
        public int Id {get;set;}
        public string Code {get;set;}
        public string Name {get;set;}
        public bool IsAttending {get;set;}
        public EMealType MealType {get;set;}
        public string Comments {get;set;}
        
        public bool HasResponded {get;set;}
    }

    public enum EMealType {
        Fish,
        Meat,
        Vegetarian
    }
}
