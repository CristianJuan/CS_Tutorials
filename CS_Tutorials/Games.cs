using System;
using System.Collections.Generic;
using static System.Console;

namespace DotNetDesignPatternDemos.SOLID.OCP
{
    public enum GenreType
    {
        FirstPerson, ThirdPerson
    }

    public enum WorldType
    {
        OpenWorld, MapBased, MissionBased
    }

    public class Games
    {
        public string Name;
        public GenreType genre;
        public WorldType worldtype;

        public Games(string name, GenreType genres, WorldType size)
        {
            Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
            genre = genres;
            worldtype = size;
        }
    }

    public class ProductFilter
    {
        // let's suppose we don't want ad-hoc queries on products
        public IEnumerable<Games> FilterByGenre(IEnumerable<Games> games, GenreType genres)
        {
            foreach (var p in games)
                if (p.genre == genres)
                    yield return p;
        }

        public static IEnumerable<Games> FilterByWorldType(IEnumerable<Games> games, WorldType worldtype)
        {
            foreach (var p in games)
                if (p.worldtype == worldtype)
                    yield return p;
        }

        public static IEnumerable<Games> FilterByGenreAndWorldType(IEnumerable<Games> games, WorldType size, GenreType genres)
        {
            foreach (var p in games)
                if (p.worldtype == size && p.genre == genres)
                    yield return p;
        } 
    }


    public interface ISpecification<T>
    {
        bool IsSatisfied(T t);
    }

    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }

    public class GenreSpecification : ISpecification<Games>
    {
        private GenreType genre;

        public GenreSpecification(GenreType genre)
        {
            this.genre = genre;
        }

        public bool IsSatisfied(Games p)
        {
            return p.genre == genre;
        }
    }

    public class SizeSpecification : ISpecification<Games>
    {
        private WorldType size;

        public SizeSpecification(WorldType size)
        {
            this.size = size;
        }

        public bool IsSatisfied(Games p)
        {
            return p.worldtype == size;
        }
    }

    // combinator
    public class AndSpecification<T> : ISpecification<T>
    {
        private ISpecification<T> first, second;

        public AndSpecification(ISpecification<T> first, ISpecification<T> second)
        {
            this.first = first ?? throw new ArgumentNullException(paramName: nameof(first));
            this.second = second ?? throw new ArgumentNullException(paramName: nameof(second));
        }

        public bool IsSatisfied(T p)
        {
            return first.IsSatisfied(p) && second.IsSatisfied(p);
        }
    }

    public class BetterFilter : IFilter<Games>
    {
        public IEnumerable<Games> Filter(IEnumerable<Games> items, ISpecification<Games> spec)
        {
            foreach (var i in items)
                if (spec.IsSatisfied(i))
                    yield return i;
        }
    }

    public class Demo
    {
        static void Main(string[] args)
        {
            var FC5 = new Games("Far Cry 5", GenreType.FirstPerson, WorldType.OpenWorld);
            var PUBG = new Games("PUBG", GenreType.FirstPerson, WorldType.OpenWorld);
            var RainbowSixSiege = new Games("R6", GenreType.FirstPerson, WorldType.MissionBased);
            var ACOrigins = new Games("Assassins Creed Origins", GenreType.ThirdPerson, WorldType.OpenWorld);
            Games[] products = { FC5, PUBG, RainbowSixSiege, ACOrigins };

            WriteLine("All games in this instance:");
            foreach(var g in products)
            {
                WriteLine($" - {g.Name}");
            }
            var pf = new ProductFilter();
            WriteLine("First person games products (Non OCP):");
            foreach (var p in pf.FilterByGenre(products, GenreType.FirstPerson))
                WriteLine($" - {p.Name} is first person");
            WriteLine("Third person games products (Non OCP):");
            foreach (var p in pf.FilterByGenre(products, GenreType.ThirdPerson))
                WriteLine($" - {p.Name} is third person");

            // ^^ BEFORE

            // vv AFTER
            WriteLine("");
            var bf = new BetterFilter();
            WriteLine("First person games (OCP):");
            foreach (var p in bf.Filter(products, new GenreSpecification(GenreType.FirstPerson)))
                WriteLine($" - {p.Name} is first person");
            WriteLine("Third person games (OCP):");
            foreach (var p in bf.Filter(products, new GenreSpecification(GenreType.ThirdPerson)))
                WriteLine($" - {p.Name} is third person");

            WriteLine("Open world games");
            foreach (var p in bf.Filter(products, new SizeSpecification(WorldType.OpenWorld)))
                WriteLine($" - {p.Name} is open world");

            WriteLine("Open world first person shooters");
            foreach (var p in bf.Filter(products,new AndSpecification<Games>(new GenreSpecification(GenreType.FirstPerson), new SizeSpecification(WorldType.OpenWorld)))
            )
            {
                WriteLine($" - {p.Name} is open world first person.");
            }
        }
    }
}
