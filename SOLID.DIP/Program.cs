using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace SOLID.DIP
{
    public enum Relationship
    {
        Parent,
        Child,
        Sibling
    }

    public class Person
    {
        public string Name;
        //public DateTime DateOfBirth { get; set; }
    }

    #region DIP Solution
    public interface IRelationShipBrowser
    {
        IEnumerable<Person> FindAllChildrenOf(string name);
    }
    #endregion

    // low-level
    public class Relationships : IRelationShipBrowser
    {
        public List<(Person, Relationship, Person)> relations = new List<(Person, Relationship, Person)>();

        public void AddParentAndChild(Person parent, Person child)
        {
            relations.Add((parent, Relationship.Parent, child));
            relations.Add((child, Relationship.Child, parent));
        }

        #region DIP Solution
        public IEnumerable<Person> FindAllChildrenOf(string name)
        {
           return relations.Where(
             x => x.Item1.Name == name &&
                  x.Item2 == Relationship.Parent
           ).Select(r => r.Item3);
        }
        #endregion
    }

    public class Research
    {
        #region DIP Violation
        public Research(Relationships relationships)
        {
            var relations = relationships.relations;
            foreach (var r in relations.Where(
               x => x.Item1.Name == "Leandro" &&
               x.Item2 == Relationship.Parent))
            {
                WriteLine($"Leandro has a child called {r.Item3.Name}");
            }
        }
        #endregion

        #region DIP Solution
        public Research(IRelationShipBrowser browser)
        {
            foreach (var p in browser.FindAllChildrenOf("Leandro"))
                WriteLine($"Leandro has a child called {p.Name}");
        }
        #endregion
        static void Main(string[] args)
        {
            var parent = new Person { Name = "Leandro" };
            var child1 = new Person { Name = "João" };
            var child2 = new Person { Name = "Antônio" };

            var relationships = new Relationships();
            relationships.AddParentAndChild(parent, child1);
            relationships.AddParentAndChild(parent, child2);

            new Research(relationships);
        }
    }
}
