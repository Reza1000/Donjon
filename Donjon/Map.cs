﻿using Donjon.Entities;
using System.Collections.Generic;
using System.Linq;


namespace Donjon
{
    internal class Map
    {
        public int Width { get; }
        public int Height { get; }
        private Cell[,] cells;

        public List<Cell> Cells
        {
            get {
                List<Cell> list = new List<Cell>();
                foreach (var cell in cells)
                {
                    list.Add(cell);
                }
                return list;
            }
        }

        public List<Creature> Creatures
        {
            get {
                var populatedCells = Cells.Where(c => c.Creature != null);
                var creatures = populatedCells.Select(c => c.Creature);
                var listOfCreatures = creatures.ToList();
                return listOfCreatures;
            }
        }

        public List<Monster> Monsters => Creatures
            .Where(c => c is Monster)
            .Select(c => c as Monster)
            .ToList();

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            cells = new Cell[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    cells[x, y] = new Cell(new Position(x, y));
                }
            }
        }

        public Cell Cell(Position position)
        {
            return Cell(position.X, position.Y);
        }

        public Cell Cell(int x, int y)
        {
            // todo: validate x and y
            if (x < 0 || x >= Width || y < 0 || y >= Height) return null;
            return cells[x, y];
        }

        public bool Place(Creature creature, Cell destination)
        {
            if (destination == null) return false;
            if (destination.Creature != null) return false;
            creature.Position = destination.Position;
            destination.Creature = creature;
            return true;
        }

        public bool Move(Cell from, Cell destination)
        {
            var creature = from?.Creature;
            if (creature == null) return false;
            if (destination == null) return false;

            var opponent = destination.Creature;
            if (opponent != null) return creature.Attack(opponent);

            creature.Position = destination.Position;
            from.Creature = null;
            destination.Creature = creature;
            return true;
        }
    }
}