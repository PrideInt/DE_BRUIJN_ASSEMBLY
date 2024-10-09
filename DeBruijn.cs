using System;
using System.Text;

namespace DeBruijn {
    class DeBruijn {
        static void Main(string[] args) {
            // Test run

            /*
            Graph graph = new(5);
            graph.AddNode(1);
            graph.AddNode(2);
            graph.AddNode(3);
            graph.AddNode(4);
            graph.AddEdge(graph.vertices[0], graph.vertices[1]);

            graph.vertices[0].Display();
            graph.vertices[1].Display();
            graph.edges[0].Display();
            */

            Console.WriteLine("Enter Sequence k: ");
            string sequence = Console.ReadLine();
            int k = 3;

            Console.WriteLine();

            // k-mers
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("k-mers: ");
            Console.ResetColor();

            string[] kmers = IsolateKMers(sequence, k);

            foreach (string kmer in kmers) {
                Console.WriteLine(kmer);
            }
            Console.ResetColor();
            Console.WriteLine();

            // (k-1)-mers
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("(k-1)-mers: ");
            Console.ResetColor();

            string[] k1mers = IsolateKMers(sequence, k - 1);

            // Console.WriteLine(k1mers.Length);

            foreach (string kmer in k1mers) {
                Console.WriteLine(kmer);
            }
            Console.WriteLine();

            // De Bruijn graph

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("De Bruijn graph: ");
            Console.ResetColor();

            Graph graph = GenerateGraph(sequence, k);

            // Display vertices
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nVertices: ");
            Console.ResetColor();

            for (int i = 0; i < graph.v; i++) {
                graph.vertices[i].Display();
            }

            // Display edges
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nEdges: ");
            Console.ResetColor();

            for (int i = 0; i < graph.e; i++) {
                graph.edges[i].Display();
            }

            // Display adjacency list
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("\nAdjacency list: ");
            Console.ResetColor();

            graph.Display();
        }

        static string[] IsolateKMers(string k, int length) {
            string[] kmers = new string[k.Length];
            for (int i = 0; i < kmers.Length; i++) {
                var builder = new System.Text.StringBuilder();

                if (i + length > k.Length) {
                    builder.Append(k.Substring(i, k.Length - i));
                    builder.Append(k.Substring(0, (i + length) - k.Length));
                } else {
                    builder.Append(k.Substring(i, length));
                }
                kmers[i] = builder.ToString();
            }
            return RemoveDuplicates(kmers);
        }

        static string[] RemoveDuplicates(string[] kmers) {
            // Allocate a new array to store the unique kmers. Need to know the size
            // of the new array.

            // O(2 * n^2)
            int size = 0;

            for (int i = 0; i < kmers.Length; i++) {
                bool isUnique = true;
                for (int j = i; j < kmers.Length; j++) {
                    if (i != j && kmers[i] == kmers[j]) {
                        isUnique = false;
                        break;
                    }
                }
                if (isUnique) {
                    size++;
                }
            }
            // Console.WriteLine(size);

            string[] uniqueKmers = new string[size];

            for (int i = 0, j = 0; i < kmers.Length; i++) {
                bool isUnique = true;
                for (int k = 0; k < uniqueKmers.Length; k++) {
                    if (i != k && kmers[i] == uniqueKmers[k]) {
                        isUnique = false;
                        break;
                    }
                }
                if (isUnique) {
                    uniqueKmers[j++] = kmers[i];
                }
            }
            return uniqueKmers;
        }

        // TODO: Implement the following methods

        // Generate the de Bruijn graph
        static Graph GenerateGraph(string sequence, int k) {
            string[] kmers = IsolateKMers(sequence, k);
            string[] k1mers = IsolateKMers(sequence, k - 1);

            Dictionary<string, List<string>> table = new(kmers.Length);

            for (int i = 0; i < k1mers.Length; i++) {
                string k1mer = k1mers[i];

                for (int j = 0; j < kmers.Length; j++) {
                    if (Overlap(k1mer, kmers[j])) {
                        string overlap = GetOverlap(k1mer, kmers[j]);

                        if (table.ContainsKey(k1mer)) {
                            table[k1mer].Add(overlap);
                        } else {
                            table.Add(k1mer, [overlap]);
                        }
                    }
                }
            }
            Graph graph = new(k1mers.Length);

            // Add vertices
            for (int i = 0; i < k1mers.Length; i++) {
                graph.AddNode(k1mers[i]);
            }
            // Add edges
            // TODO: This is O(n^3), however, building a De Bruijn graph should take O(n) time.
            for (int i = 0; i < k1mers.Length; i++) {
                List<string> values = table[k1mers[i]];

                foreach (string value in values) {
                    for (int j = 0; j < graph.vertices.Length; j++) {
                        if (graph.vertices[j].data == value) {
                            string kmerWeight = graph.vertices[i].data.Substring(0, graph.vertices[i].data.Length - 1) + "" + graph.vertices[j].data;

                            graph.AddEdge(graph.vertices[i], graph.vertices[j], new Edge.Weight(kmerWeight));
                        }
                    }
                }
            }
            return graph;
        }
        // Function to see if there are overlaps between the k-mers
        static bool Overlap(string k1mer, string kmer) {
            if (kmer.Substring(0, k1mer.Length) == k1mer) {
                return true;
            }
            return false;
        }
        static string GetOverlap(string k1mer, string kmer) {
            if (Overlap(k1mer, kmer)) {
                return kmer.Substring(kmer.Length - k1mer.Length, k1mer.Length);
            }
            return "";
        }
        // Function to find the Eulerian path
        static Path EulerianPath(Graph graph) {
            return Path.Fleurys(graph);
        }
        // Function to find the Hamiltonian path
        static Path HamiltonianPath(Graph graph) {
            return Path.Backtracking(graph);
        }
    }

    class Graph {
        public Node? head, tail;
        public int size;
        public int v, e;

        public Node[] vertices;
        public Edge[] edges;

        public Dictionary<Node, List<Edge>> adjList = [];

        public Graph() {
            head = tail = null;
            size = 0;
            v = e = 0;
        }

        public Graph(int n) : this() {
            vertices = new Node[n];
            edges = new Edge[n * 2];
        }

        public void AddNode(string data) {
            Node n = new(data) {
                data = data
            };
            if (head == null) {
                head = tail = n;
            } else {
                tail.AddNext(n);
                tail = n;
            }
            vertices[v++] = n;
            size++;
        }
        public void AddEdge(Node from, Node to, Edge.Weight w) {
            Edge edge = new(from, to, w);
            from.AddNext(to);
            edges[e++] = edge;
        }
        public void AddEdge(Node from, Node to) {
            AddEdge(from, to, null);
        }
        public Dictionary<Node, List<Edge>> GetAdjList() {
            for (int i = 0; i < vertices.Length; i++) {
                List<Edge> neighbors = new();

                for (int j = 0; j < edges.Length; j++) {
                    if (edges[j] != null && edges[j].From() != null && (edges[j].From() == vertices[i])) {
                        neighbors.Add(edges[j]);
                    }
                }
                adjList.Add(vertices[i], neighbors);
            }
            return adjList;
        }
        public void Display() {
            Dictionary<Node, List<Edge>> table = GetAdjList();

            foreach (KeyValuePair<Node, List<Edge>> pair in table) {
                Console.Write(pair.Key.data + " -> ");
                foreach (Edge edge in pair.Value) {
                    Console.Write(edge.To().data + " ");
                }
                Console.WriteLine();
            }
        }
    }
    class Node {
        public Node? next, prev;
        public string data;

        public Node() {
            next = prev = null;
        }
        public Node(string data) : this() {
            this.data = data;
        }
        public void AddNext(Node n) {
            next = n;
            n.prev = this;
        }
        public int Value() {
            try {
                return int.Parse(data);
            } catch (FormatException) {
                return -1;
            }
        }
        public void Display() {
            Console.WriteLine(data);
        }
    }
    class Edge {
        public Node? from;
        public Node? to;
        public Weight? weight;

        public Edge() {
            from = to = null;
        }
        public Edge(Node from, Node to) {
            this.from = from;
            this.to = to;
            this.weight = null;
        }
        public Edge(Node from, Node to, Weight w) {
            this.from = from;
            this.to = to;
            this.weight = w;
        }
        public void Display() {
            if (weight != null) {
                Console.WriteLine(from.data + " -> " + to.data + " : " + weight.GetWeight());
            } else {
                Console.WriteLine(from.data + " -> " + to.data);
            }
        }
        public Node From() {
            return from;
        }
        public Node To() {
            return to;
        }

        public class Weight {
            public string weight;
            public Weight() {
                weight = "";
            }
            public Weight(string w) {
                weight = w;
            }
            public string GetWeight() {
                return weight;
            }
            public int Value() {
                try {
                    return int.Parse(weight);
                } catch (FormatException) {
                    return -1;
                }
            }
        }
    }
    class Path {
        public Node[] path;
        public int size;

        public Path() {
            path = new Node[100];
            size = 0;
        }
        public Path(int n) {
            path = new Node[n];
            size = 0;
        }
        public void AddNode(Node n) {
            path[size++] = n;
        }
        public void Display() {
            var builder = new System.Text.StringBuilder();
            for (int i = 0; i < size; i++) {
                if (i == size - 1) {
                    builder.Append(path[i].data);
                } else {
                    builder.Append(path[i].data + " -> ");
                }
            }
            Console.WriteLine(builder.ToString());
        }

        /*
        Let's use Fleury's algortihm to find the Eulerian path.
        */
        public static Path Fleurys(Dictionary<Node, List<Edge>> list) {
            return null;
        }
        public static Path Fleurys(Graph graph) {
            Dictionary<Node, List<Edge>> list = graph.GetAdjList();
            return null;
        }
        /*
        Let's use the Backtracking algorithm to find the Hamiltonian path.
        */
        public static Path Backtracking(Dictionary<Node, List<Edge>> list) {
            return null;
        }
        public static Path Backtracking(Graph graph) {
            Dictionary<Node, List<Edge>> list = graph.GetAdjList();
            return null;
        }
    }
    class Pair<T, V>(T key, V value) {
        public T key = key;
        public V value = value;

        static Pair<T, V> Of(T key, V value) {
            return new Pair<T, V>(key, value);
        }
    }
}