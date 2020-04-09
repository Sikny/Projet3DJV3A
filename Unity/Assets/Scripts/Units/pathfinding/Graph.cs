using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    public class Graph
    {
        private Tile[,] tiles;
        private int nbRows;
        private int nbCols;

        private Tile beginNode;
        private Tile exitNode;

        private List<Node> nodesList = null;
        private List<Arc> arcsList = null;

        public Graph(Tile[,] tiles, int nbRows, int nbCols, Tile begin, Tile end)
        {
            this.tiles = tiles;
            this.nbCols = nbCols;
            this.nbRows = nbRows;

            beginNode = begin;
            exitNode = end;
            NodesList();
            ArcsList();
            
        }

        public Node BeginningNode()
        {
            return beginNode;
        }

        public Node ExitNode()
        {
            return exitNode;
        }

        public List<Node> NodesList()
        {
            if (nodesList == null)
            {
                nodesList = new List<Node>();
                foreach (var node in tiles)
                {
                    nodesList.Add(node);
                }
            }

            return nodesList;
        }

        public List<Arc> ArcsList()
        {
            if (arcsList == null)
            {
                arcsList = new List<Arc>();
                for (int row = 0; row < nbRows; row++)
                {
                    for (int col = 0; col < nbCols; col++)
                    {
                        if (tiles[row, col].IsValidPath())
                        {
                            if (row - 1 >= 0 && tiles[row - 1, col].IsValidPath())
                            {
                                arcsList.Add(new Arc(tiles[row, col], tiles[row-1, col], tiles[row-1, col].Cost()));
                            }
                            if (col - 1 >= 0 && tiles[row, col-1].IsValidPath())
                            {
                                arcsList.Add(new Arc(tiles[row, col], tiles[row, col-1], tiles[row, col-1].Cost()));
                            }
                            if (row - 1 < 0 && tiles[row + 1, col].IsValidPath())
                            {
                                arcsList.Add(new Arc(tiles[row, col], tiles[row+1, col], tiles[row+1, col].Cost()));
                            }
                            if (col + 1 < 0 && tiles[row, col+1].IsValidPath())
                            {
                                arcsList.Add(new Arc(tiles[row, col], tiles[row, col+1], tiles[row, col+1].Cost()));
                            }
                        }
                    }
                }
            }

            return arcsList;
        }

        public List<Arc> ArcsList(Node currentNode)
        {
            List<Arc> list = new List<Arc>();
            int currentRow = ((Tile) currentNode).Row;
            int currentCol = ((Tile) currentNode).Col;

            if (currentRow - 1 >= 0 && tiles[currentRow - 1, currentCol].IsValidPath())
            {
                list.Add(new Arc(currentNode, tiles[currentRow-1, currentCol], tiles[currentRow-1, currentCol].Cost()));
            }
            if (currentCol - 1 >= 0 && tiles[currentRow, currentCol-1].IsValidPath())
            {
                list.Add(new Arc(currentNode, tiles[currentRow, currentCol-1], tiles[currentRow, currentCol-1].Cost()));
            }
            if (currentRow - 1 < 0 && tiles[currentRow + 1, currentCol].IsValidPath())
            {
                list.Add(new Arc(currentNode, tiles[currentRow+1, currentCol], tiles[currentRow+1, currentCol].Cost()));
            }
            if (currentCol + 1 < 0 && tiles[currentRow, currentCol+1].IsValidPath())
            {
                list.Add(new Arc(currentNode, tiles[currentRow, currentCol+1], tiles[currentRow, currentCol+1].Cost()));
            }
            
            return list;
        }

        public void ComputeEstimatedDistance()
        {
            foreach (var tile in tiles)
            {
                tile.EstimatedDistance = Math.Abs(exitNode.Row - tile.Row) + Math.Abs(exitNode.Col - tile.Col);
            }
        }

        public void Clear()
        {
            nodesList = null;
            arcsList = null;
            for (int row = 0; row < nbRows; row++)
            {
                for (int col = 0; col < nbCols; col++)
                {
                    tiles[row, col].DistanceTraveled = double.PositiveInfinity;
                    tiles[row, col].Previous = null;
                }
            }

            beginNode.DistanceTraveled = beginNode.Cost();
        }
    }
}