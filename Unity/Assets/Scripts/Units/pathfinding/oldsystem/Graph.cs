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

        private List<Node> nodesList = null;
        private List<Arc> arcsList = null;
        
        //Opti buffer
        private Arc[] bufferArc = new Arc[4];
        private int bufferArcIndex = 0;
        
        public Graph(Tile[,] tiles, int nbRows, int nbCols, Tile begin, Tile end)
        {
            this.tiles = tiles;
            this.nbCols = nbCols;
            this.nbRows = nbRows;

            BeginningNode = begin;
            ExitNode = end;
            NodesList();
            ArcsList();
            
        }

        public Tile BeginningNode { get; set; }

        public Tile ExitNode { get; set; }

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
                            if (row + 1 < nbRows && tiles[row + 1, col].IsValidPath())
                            {
                                arcsList.Add(new Arc(tiles[row, col], tiles[row+1, col], tiles[row+1, col].Cost()));
                            }
                            if (col + 1 < nbCols && tiles[row, col+1].IsValidPath())
                            {
                                arcsList.Add(new Arc(tiles[row, col], tiles[row, col+1], tiles[row, col+1].Cost()));
                            }
                        }
                    }
                }
            }

            return arcsList;
        }

        public Arc[] ArcsList(Node currentNode)
        {
            int currentRow = ((Tile) currentNode).Row;
            int currentCol = ((Tile) currentNode).Col;

            bufferArc[0] = null;
            bufferArc[1] = null;
            bufferArc[2] = null;
            bufferArc[3] = null;
            bufferArcIndex = 0;
            
            if (currentRow - 1 >= 0 && tiles[currentRow - 1, currentCol].IsValidPath())
            {
                //list.Add(new Arc(currentNode, tiles[currentRow-1, currentCol], tiles[currentRow-1, currentCol].Cost()));
                bufferArc[bufferArcIndex++] = new Arc(currentNode, tiles[currentRow - 1, currentCol],
                    tiles[currentRow - 1, currentCol].Cost());
            }
            if (currentCol - 1 >= 0 && tiles[currentRow, currentCol-1].IsValidPath())
            {
                //list.Add(new Arc(currentNode, tiles[currentRow, currentCol-1], tiles[currentRow, currentCol-1].Cost()));
                bufferArc[bufferArcIndex++] = new Arc(currentNode, tiles[currentRow, currentCol - 1],
                    tiles[currentRow, currentCol - 1].Cost());
            }
            if (currentRow + 1 < nbRows && tiles[currentRow + 1, currentCol].IsValidPath())
            {
                //list.Add(new Arc(currentNode, tiles[currentRow+1, currentCol], tiles[currentRow+1, currentCol].Cost()));
                bufferArc[bufferArcIndex++] = new Arc(currentNode, tiles[currentRow + 1, currentCol],
                    tiles[currentRow + 1, currentCol].Cost());
            }
            if (currentCol + 1 < nbCols && tiles[currentRow, currentCol+1].IsValidPath())
            {
                //list.Add(new Arc(currentNode, tiles[currentRow, currentCol+1], tiles[currentRow, currentCol+1].Cost()));
                bufferArc[bufferArcIndex++] = new Arc(currentNode, tiles[currentRow, currentCol + 1],
                    tiles[currentRow, currentCol + 1].Cost());
            }
            
            return bufferArc;
        }

        public void ComputeEstimatedDistance()
        {
            foreach (var tile in tiles)
            {
                //tile.EstimatedDistance = Math.Abs(exitNode.Row - tile.Row) + Math.Abs(exitNode.Col - tile.Col);
                tile.EstimatedDistance = Vector2.Distance(new Vector2(ExitNode.Row, ExitNode.Col),
                    new Vector2(tile.Row, tile.Col));
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

            BeginningNode.DistanceTraveled = BeginningNode.Cost();
        }

        public Stack<Tile> ReconstructPath()
        {
            if (!double.IsInfinity(ExitNode.DistanceTraveled))
            {
                Stack<Tile> stacksItineraire = new Stack<Tile>();

                Tile currentNode = ExitNode;
                Tile prevNode = (Tile) ExitNode.Previous;


                while (prevNode != null)
                {

                    stacksItineraire.Push(currentNode);
                    currentNode = prevNode;
                    prevNode = (Tile) prevNode.Previous;
                }

                stacksItineraire.Push(currentNode);
                return stacksItineraire;
            }

            return new Stack<Tile>();

        }
    }
}