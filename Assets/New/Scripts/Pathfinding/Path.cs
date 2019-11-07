using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace kalelovil.utility.pathfinding
{
    [System.Serializable]
    public class Path
    {
        //private TileScript[,] cameFromArray;
        //private double[,] costSoFarArray;
        public IPathfindingNode _startNode;
        public IPathfindingNode _endNode;
        public double _totalCost = -1;
        public int _unitActionCost = -1;

        public List<IPathfindingNode> _pathNodeStack = new List<IPathfindingNode>();
        //    public enum PathDirection { Left, Straight, Right }
        //public List<PathDirection> pathDirectionStack = new List<PathDirection>();
        public List<GameObject> _pathTrailStack = new List<GameObject>();

        public Path(IPathfindingNode startNode, IPathfindingNode endNode,
            Dictionary<IPathfindingNode, IPathfindingNode> cameFromArray,
            Dictionary<IPathfindingNode, double> costSoFarArray)
        {
            _startNode = startNode;
            _endNode = endNode;
            _unitActionCost = _endNode.NumOfActions;

            _totalCost = costSoFarArray[endNode];
            _pathNodeStack = new List<IPathfindingNode>();

            for (IPathfindingNode currentTileScript = endNode;
                currentTileScript != startNode;
                currentTileScript = cameFromArray[currentTileScript])
            {
                _pathNodeStack.Insert(0, currentTileScript);
                //pathDirectionStack.Insert(0, GetDirectionFromTileToTile(currentTileScript, cameFromArray[gridX, gridY]));
            }

            //string pathString = "";
            //foreach (TileScript t in pathTileStack) {
            //    pathString += t.ToString() + ",  ";
            //}
            //Debug.Log("PATH: " + pathString);
        }

        /*
        private PathDirection GetDirectionFromTileToTile(TileScript currentTileScript, TileScript previoustileScript)
        {
            // TO DO: Temp
            int gridXDif = currentTileScript.gridX - previoustileScript.gridX;
            int gridYDif = currentTileScript.gridY - previoustileScript.gridY;

            return PathDirection.Straight;
        }
        */

        internal IPathfindingNode Pop()
        {
            if (_pathNodeStack.Count < 1)
            {
                return null;
            }
            IPathfindingNode node = _pathNodeStack[0];
            _pathNodeStack.RemoveAt(0);
            if (_pathTrailStack.Count > 0)
            {
                UnityEngine.Object.Destroy(_pathTrailStack[0]);
                _pathTrailStack.RemoveAt(0);
            }
            return node;
        }

        internal IPathfindingNode Peek()
        {
            if (_pathNodeStack.Count > 0)
            {
                return _pathNodeStack[0];
            }
            else
            {
                return null;
            }
        }

        internal void DestroyTrail()
        {
            foreach (GameObject obj in _pathTrailStack)
            {
                UnityEngine.Object.Destroy(obj);
            }
            _pathTrailStack.Clear();
        }

        internal bool IsEmpty()
        {
            return (_pathNodeStack == null || _pathNodeStack.Count == 0);
        }

        //internal void Reverse()
        //{
        //    endTileScript = pathTileStack[0];
        //    pathTileStack = new List<TileScript>(pathTileStack.Reverse());
        //    startTileScript = pathTileStack.Peek();
        //}

        // Override '+' Operator
        public static Path operator +(Path p1, Path p2)
        {

            for (int i = 0; i < p2._pathNodeStack.Count; i++)
            {
                p1._pathNodeStack.Add(p2._pathNodeStack[i]);
                p1._endNode = p2._endNode;
                p1._totalCost += p2._totalCost;
            }

            return p1;
        }
    }
}