using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;
using System.Linq;

namespace kalelovil.utility.pathfinding
{
    public static class Pathfinder
    {
        static float diagonalCost = 1.4142f;

        internal static GameObject pathTrailStraightPrefab = null;
        internal static GameObject pathTrailLeftPrefab = null;
        internal static GameObject pathTrailRightPrefab = null;

        internal static Dictionary<IPathfindingNode, IPathfindingNode> cameFromArray = new Dictionary<IPathfindingNode, IPathfindingNode>();
        internal static Dictionary<IPathfindingNode, double> costSoFarArray = new Dictionary<IPathfindingNode, double>();
        internal static PriorityQueue<IPathfindingNode> frontier = new PriorityQueue<IPathfindingNode>();
        internal static IPathfindingNode currentNode = null;

        internal static float stepCost = 0f;
        internal static double newCost = 0.0;
        internal static double currentLowestCost = 0.0;
        internal static double priority = 0.0;

        internal static Path path = null;

        public enum PathfindingType
        {
            Ground,
            Air
        }

        public static Path GetPathOfTypeForUnit(IPathfindingNode startNode, IPathfindingNode endNode,
            PathfindingType pathfindingType, NodeUnit unit)
        {
            IPathfindingNode[] neighbourNodes = null;
            IPathfindingNode neighbourNode;

            cameFromArray.Clear();
            costSoFarArray.Clear();
            frontier.Clear();
            currentNode = null;

            frontier.Enqueue(startNode, 0);
            cameFromArray[startNode] = null;
            costSoFarArray[startNode] = 0.0;

            while (frontier.NumItems > 0)
            {
                currentNode = frontier.Dequeue();

                //float startTime = Time.realtimeSinceStartup;

                if (currentNode == endNode)
                {
                    //float endTime = Time.realtimeSinceStartup;
                    //float totalTime = endTime - startTime;

                    path = new Path(startNode, currentNode, cameFromArray, costSoFarArray);
                    return path;
                }

                neighbourNodes = currentNode.GetConnectedNodes().ToArray();
                for (int i = 0; i < neighbourNodes.Length; i++)
                {
                    neighbourNode = neighbourNodes[i];
                    if (neighbourNode != null && neighbourNode != startNode &&
                        (!neighbourNode.BlocksUnit(unit) || pathfindingType == PathfindingType.Air))
                    {
                        stepCost = currentNode.GetConnectionToNode(neighbourNode)._movementPointCost;
                        bool hasFoundCost = costSoFarArray.TryGetValue(neighbourNode, out currentLowestCost);
                        var costSoFar = costSoFarArray[currentNode];
                        newCost = costSoFar + stepCost;

                        if (currentLowestCost <= 0 || newCost < currentLowestCost)
                        {
                            costSoFarArray[neighbourNode] = newCost;
                            //if (pathfindingType == PathfindingType.Tile)
                            //{
                            //priority = 1.0 / (newCost + heuristic(endPathfindingNode, neighbourPathfindingNode));
                            //} else
                            //{
                            priority = 1.0 / newCost;
                            //}
                            frontier.Enqueue(neighbourNode, priority);
                            cameFromArray[neighbourNode] = currentNode;
                        }
                    }
                }
            }

            return null;
        }

        public static Dictionary<IPathfindingNode, double> GetMovementCostArray(IPathfindingNode startNode, PathfindingType pathfindingType, int maxCost)
        {
            IPathfindingNode[] neighbourNodes = null;

            IPathfindingNode neighbourNode;
            cameFromArray.Clear();
            costSoFarArray.Clear();
            frontier.Clear();
            currentNode = null;

            frontier.Enqueue(startNode, 0);
            cameFromArray[startNode] = null;
            costSoFarArray[startNode] = 0.0;

            while (frontier.NumItems > 0)
            {
                currentNode = frontier.Dequeue();

                //float startTime = Time.realtimeSinceStartup;

                neighbourNodes = currentNode.GetConnectedNodes().ToArray();
                for (int i = 0; i < neighbourNodes.Length; i++)
                {
                    neighbourNode = neighbourNodes[i];
                    if (neighbourNode != null && neighbourNode != startNode &&
                        (neighbourNode.IsEmpty || pathfindingType == PathfindingType.Air))
                    {
                        //stepCost = GetTransportTimeToNextTileInDirection(currentPathfindingNode, i, isDirect);
                        stepCost = 1;
                        costSoFarArray.TryGetValue(neighbourNode, out currentLowestCost);
                        newCost = currentLowestCost + stepCost;

                        if ((currentLowestCost <= 0 || newCost < currentLowestCost) && newCost <= maxCost)
                        {
                            costSoFarArray[neighbourNode] = newCost;
                            //if (pathfindingType == PathfindingType.Tile)
                            //{
                            //priority = 1.0 / (newCost + heuristic(endPathfindingNode, neighbourPathfindingNode));
                            //} else
                            //{
                            priority = 1.0 / newCost;
                            //}
                            frontier.Enqueue(neighbourNode, priority);
                            cameFromArray[neighbourNode] = currentNode;
                        }
                    }
                }
            }

            return costSoFarArray;
        }

        /*
        private static float GetTransportTimeToNextTileInDirection(PathfindingNode currentPathfindingNode, int directionIndex, bool isDirect)
        {
            float speed = GoodTransporterScript.Base_Speed;
            InfrastructureScript infra = currentPathfindingNode.roadToNeighbours[directionIndex];
            if (infra != null && !isDirect)
            {
                if (transporter != null)
                {
                    speed = Math.Min(GoodTransporterScript.Max_Speed, infra.typeScript.definition.levelDefinitionList[infra.level].movementSpeed);
                } else
                {
                    speed = infra.typeScript.definition.levelDefinitionList[infra.level].movementSpeed;
                }
            }
            float timeCost = 1.0f / speed;
            return timeCost;
        }
        */

        /*
        private static bool IsSuitableEndTile(PathfindingNode currentPathfindingNode, PathfindingType pathfindingType, PathfindingNode endPathfindingNode)
        {
            switch (pathfindingType)
            {
                case PathfindingType.Tile:
                {
                    if (currentPathfindingNode == endPathfindingNode)
                    {
                        return true;
                    }
                    return false;
                }
                case PathfindingType.CivilianStructure:
                {
                    if (currentPathfindingNode.currentLocation is CivilianStructureScript)
                    {
                        if (goodType == null)
                        {
                            return true;
                        } else
                        {
                            foreach (PopScript pop in ((CivilianStructureScript)currentPathfindingNode.currentLocation).popNameScriptMap.Values)
                            {
                                //if (pop.IsValidGoodInput(goodType))
                                //{
                                //    return true;
                                //}
                            }
                        }
                        return false;
                    }
                    return false;
                }
                case PathfindingType.Capital:
                {
                    if (currentPathfindingNode.currentLocation is CapitalScript)
                    {
                        return true;
                    }
                    return false;
                }
                case PathfindingType.Factory:
                {
                    //float startTime = Time.realtimeSinceStartup;
                    if (currentPathfindingNode.currentLocation is FactoryScript)
                    {
                        if (((FactoryScript)currentPathfindingNode.currentLocation).IsValidGoodInput(goodType))
                        {
                            //float endTime = Time.realtimeSinceStartup;
                            //float totalTime = endTime - startTime;
                            return true;
                        }
                    }
                    return false;
                }
                default:
                {
                    return false;
                }
            }
        }*/

        internal static Vector3 heading;

        private static double Heuristic(IPathfindingNode endNode, IPathfindingNode nextNode)
        {
            //heading = endPathfindingNode.transform.position - nextPathfindingNode.transform.position;
            heading = endNode.PositionForHeuristic - nextNode.PositionForHeuristic;
            return heading.magnitude;
        }





        internal static void CreateTrailForPath(Path path)
        {
            //GameObject pathTrailPrefab = null;
            if (pathTrailStraightPrefab == null)
            {
                string prefabPath = "Prefabs/Pathfinding/PathStraightPrefab";
                pathTrailStraightPrefab = Resources.Load<GameObject>(prefabPath);
                prefabPath = "Prefabs/Pathfinding/PathLeftPrefab";
                pathTrailLeftPrefab = Resources.Load<GameObject>(prefabPath);
                prefabPath = "Prefabs/Pathfinding/PathRightPrefab";
                pathTrailRightPrefab = Resources.Load<GameObject>(prefabPath);
            }
            //Vector2 relativetransform.position = new Vector2();
            Vector2 dir = new Vector2();
            float angle = 0;
            float previousAngle = 0;
            for (int i = 0; i < path._pathNodeStack.Count; i++)
            {
                // Get Angle
                if (i >= 1)
                {
                    dir = path._pathNodeStack[i - 1].PositionForHeuristic - path._pathNodeStack[i].PositionForHeuristic;
                }
                else
                {
                    dir = path._startNode.PositionForHeuristic - path._pathNodeStack[i].PositionForHeuristic;
                }

                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                if (angle < previousAngle - 1)
                {
                    path._pathTrailStack.Add(GameObject.Instantiate(pathTrailLeftPrefab, path._pathNodeStack[i].PositionForHeuristic, Quaternion.AngleAxis(angle, Vector3.forward)));
                }
                else if (angle > previousAngle + 1)
                {
                    path._pathTrailStack.Add(GameObject.Instantiate(pathTrailRightPrefab, path._pathNodeStack[i].PositionForHeuristic, Quaternion.AngleAxis(angle, Vector3.forward)));
                }
                else
                {
                    path._pathTrailStack.Add(GameObject.Instantiate(pathTrailStraightPrefab, path._pathNodeStack[i].PositionForHeuristic, Quaternion.AngleAxis(angle, Vector3.forward)));
                }
                //path.pathTrailStack[0].transform.position += path.pathTrailStack[0].transform.forward * 100f;
                previousAngle = angle;
            }
        }
    }
}