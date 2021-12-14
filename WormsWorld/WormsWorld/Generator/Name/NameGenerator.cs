using System;

namespace WormsWorld.Generator.Name
{
    public class NameGenerator : INameGenerator
    { 
        public string GenerateName()
        {
            string[] name =
            {
                "Elizabeth", "Victoria", "Eadmund", "William", "Stephen",
                "Matilda", "Richard", "Charles", "Harold", "Edward",
                "Philip", "George", "Henry", "Louis", "James",
                "John", "Jane", "Mary", "Mary", "Anne"
            };
            string[] nickname =
            {
                "the Magnificent", "the Peaceable", "the Confessor", "the Conqueror",
                "the Lionheart", "the Glorious", "the Bastard", "Longshanks",
                "Curtmantle", "the Martyr", "Beauclerc", "Godwinson",
                "Forkbeard", "the Great", "the Elder", "Ironside",
                "Lackland", "Harefoot", "All-Fair", "Rufus"
            };
            var random = new Random();
            return (string) name.GetValue(random.Next(name.Length)) + " " +
                   (string) nickname.GetValue(random.Next(nickname.Length));
        }
    }
}