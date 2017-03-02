using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace JS_Prog
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static bool NextPermutation<T>(IList<T> a) where T : IComparable<T>
        {
            if (a.Count < 2) return false;
            var k = a.Count - 2;

            while (k >= 0 && a[k].CompareTo(a[k + 1]) >= 0) k--;
            if (k < 0) return false;

            var l = a.Count - 1;
            while (l > k && a[l].CompareTo(a[k]) <= 0) l--;

            var tmp = a[k];
            a[k] = a[l];
            a[l] = tmp;

            var i = k + 1;
            var j = a.Count - 1;
            while (i < j)
            {
                tmp = a[i];
                a[i] = a[j];
                a[j] = tmp;
                i++;
                j--;
            }

            return true;
        }

        private static List<List<T>> Premutations<T>(IList<T> a) where T : IComparable<T>
        {
            var l = new List<List<T>>();
            do
            {
                l.Add(a.ToList());
            } while (NextPermutation(a));
            return l;
        }

        public static List<List<T>> GetAllCombos<T>(List<T> list)
        {
            var comboCount = (int) Math.Pow(2, list.Count) - 1;
            var result = new List<List<T>>();
            for (var i = 1; i < comboCount + 1; i++)
            {
                // make each combo here
                result.Add(new List<T>());
                for (var j = 0; j < list.Count; j++)
                    if ((i >> j) % 2 != 0)
                        result.Last().Add(list[j]);
            }
            return result;
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            Graph g = new Graph("../../../Data/le-1.txt");
            g.Matrix("../../../Data/msm.txt");
            var a = g.Dijkstra(0)[1];
            var b = g.Path(a, 5);
            var clients_raw = Enumerable.Range(0, 9).ToArray();
            var clients = AllCombinations.Combinations(Enumerable.Range(0, 9).ToArray()).SelectMany(x => Premutations(x)).ToList().OrderBy(x => x[0]);
            List<int> distances = new List<int>();
            foreach (var combination in clients)
            {
                
            }
            var s = clients.Aggregate("", (current, x) => current + (string.Join(", ", x) + "\t"));
            MessageBox.Show(s);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }


    internal class AllCombinations
    {
        public static List<List<int>> Combinations(int[] array, int startingIndex = 0, int combinationLenght = 2)
        {
            var combinations = new List<List<int>>();
            if (combinationLenght == 2)
            {
                var combinationsListIndex = 0;
                for (var arrayIndex = startingIndex; arrayIndex < array.Length; arrayIndex++)
                for (var i = arrayIndex + 1; i < array.Length; i++)
                {
                    //add new List in the list to hold the new combination
                    combinations.Add(new List<int>());

                    //add the starting index element from “array”
                    combinations[combinationsListIndex].Add(array[arrayIndex]);
                    while (combinations[combinationsListIndex].Count < combinationLenght)
                        combinations[combinationsListIndex].Add(array[i]);
                    combinationsListIndex++;
                }

                return combinations;
            }

            var combinationsofMore = new List<List<int>>();
            for (var i = startingIndex; i < array.Length - combinationLenght + 1; i++)
            {
                //generate combinations of lenght-1(if lenght > 2 we enter into recursion)
                combinations = Combinations(array, i + 1, combinationLenght - 1);

                //add the starting index Elemetn in the begginnig of each newly generated list
                for (var index = 0; index < combinations.Count; index++)
                    combinations[index].Insert(0, array[i]);

                for (var y = 0; y < combinations.Count; y++)
                    combinationsofMore.Add(combinations[y]);
            }

            return combinationsofMore;
        }
    }
}