using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace kalelovil.utility.pathfinding
{
    public class PriorityQueue<T>
    {
        // The items and priorities.
        List<T> Values = new List<T>();
        List<double> Priorities = new List<double>();

        // Return the number of items in the queue.
        public int NumItems
        {
            get
            {
                return Values.Count;
            }
        }

        public T Front()
        {
            // Find the hightest priority.
            int best_index = 0;
            double best_priority = Priorities[0];
            for (int i = 1; i < Priorities.Count; i++)
            {
                if (best_priority < Priorities[i])
                {
                    best_priority = Priorities[i];
                    best_index = i;
                }
            }

            T item = Values[best_index];

            return item;
        }

        // Add an item to the queue.
        public void Enqueue(T new_value, double new_priority)
        {
            Values.Add(new_value);
            Priorities.Add(new_priority);
        }

        // Remove the item with the largest priority from the queue.
        public T Dequeue()
        {
            // Find the hightest priority.
            int best_index = 0;
            double best_priority = Priorities[0];
            for (int i = 1; i < Priorities.Count; i++)
            {
                if (best_priority < Priorities[i])
                {
                    best_priority = Priorities[i];
                    best_index = i;
                }
            }

            T item = Values[best_index];

            // Remove the item from the lists.
            Values.RemoveAt(best_index);
            Priorities.RemoveAt(best_index);

            return item;
        }

        internal void Clear()
        {
            Values.Clear();
            Priorities.Clear();
        }
    }
}