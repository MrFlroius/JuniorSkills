using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace JS_Prog
{
    internal class Graph
    {
        public List<int[]> Edges;
        public int[,] Mat;
        private bool IsChanged = true;
        private List<int> _path = new List<int>();

        public Graph(string path)
        {
            StreamReader r = new StreamReader(path);
            Edges = Regex.Split(r.ReadToEnd(), "\r\n").ToList().Select(s => s.Split().Select( i => int.Parse(i) - 1).ToArray()).ToList();
            this.Matrix();
        }

        public void AddEdge(int point1, int point2, int weight)
        {
            Edges.Add(new int[] { point1, point2, weight });
            Edges.Add(new int[] { point2, point1, weight });
            IsChanged = true;
        }

        public void AddDirectEdge(int from, int to, int weight)
        {
            Edges.Add(new int[] { from, to, weight });
            IsChanged = true;
        }

        public void Matrix()
        {
            if (IsChanged)
            {
                int size = Edges.OrderBy(s => s.Take(2).Max()).ToList()[Edges.Count - 1].Take(2).Max() + 1;
                Mat = new int[size, size];
                foreach (var edge in Edges) Mat[edge[0], edge[1]] = edge[2];
            }
        }

        internal void Matrix(string path)
        {
            this.Matrix();
            StreamWriter w = new StreamWriter(path);
            w.Write(this.ToString());
            w.Close();
        }

        public override string ToString()
        {
            int rowLength = Mat.GetLength(0);
            int colLength = Mat.GetLength(1);
            string s = "";
            int tabulate = Mat.Cast<int>().Max().ToString().Length;

            for (int i = 0; i < rowLength; i++){
                for (int j = 0; j < colLength; j++)
                    s += string.Format(string.Format("{{0, {0}}}", tabulate + 1), Mat[i, j]);
                s += (Environment.NewLine + Environment.NewLine);
            }
            return s;
        }

        public int MinDistance(int[] dist, bool[] queue)
        {
            int min = int.MaxValue;
            int min_idx = -1;
            for(int i = 0; i < dist.Length; i++)
            {
                if((dist[i] < min) && (!queue[i])) 
                {
                    min = dist[i];
                    min_idx = i;
                }
            }
            return min_idx;
        }

        private int _Path(IReadOnlyList<int> parent, int j)
        {
            _path.Add(j);
            if (parent[j] == -1)
            {
                _path.Add(j);
                return j;
            }
            return _Path(parent, parent[j]);;
            //path.Add(j);
        }

        public List<int> Path(int[] parent, int j)
        {
            _Path(parent, j);
            _path = _path.Take(count: _path.Count - 1).Reverse().ToList();
            return _path;
        }

        public int[][] Dijkstra(int start)
        { 
            int n = Mat.GetLength(0);
            int[] dist = Enumerable.Repeat<int>(int.MaxValue, n).ToArray();
            dist[start] = 0;
            int[] parent = Enumerable.Repeat<int>(-1, n).ToArray();
            bool[] queue = new bool[n];

            while (queue.Contains(false))
            {
                int u = MinDistance(dist, queue);
                queue[u] = true;
                for (int i = 0; i < n; i++)
                {
                    if ((Mat[u,i] != 0) && (!queue[i]))
                        if (dist[u] + Mat[u, i] < dist[i])
                        {
                            dist[i] = dist[u] + Mat[u, i];
                            parent[i] = u;
                        }
                }
            }
            int [][] ret = new int[2][];
            ret[0] = dist;
            ret[1] = parent;
            return ret;
        }

        public List<int[][]> DistanceMatrix()
        {
            List<int[][]> ret = new List<int[][]>();
            for (int i = 0; i < Mat.GetLength(0); i++)
            {
                ret.Add(Dijkstra(i));
            }
            return ret;
        }
    }
}
