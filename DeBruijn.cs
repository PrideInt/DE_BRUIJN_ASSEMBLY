using System;

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
        static Graph GenerateGraph(String sequence, int k) {
            return null;
        }
        // Function to see if there are overlaps between the k-mers
        static bool Overlap() {
            return false;
        }
        // Function to find the Eulerian path
        static Path EulerianPath() {
            return null;
        }
        // Function to find the Hamiltonian path
        static Path HamiltonianPath() {
            return null;
        }
    }

    class Graph {
        public Node? head, tail;
        public int size;
        public int v, e;

        public Node[] vertices;
        public Edge[] edges;

        public Graph() {
            head = tail = null;
            size = 0;
            v = e = 0;
        }

        public Graph(int n) : this() {
            vertices = new Node[n];
            edges = new Edge[n];
        }

        public void AddNode(String data) {
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
    }
    class Node {
        public Node? next, prev;
        public String data;

        public Node() {
            next = prev = null;
        }
        public Node(String data) : this() {
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

        public class Weight {
            public String weight;
            public Weight() {
                weight = "";
            }
            public Weight(String w) {
                weight = w;
            }
            public String GetWeight() {
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
    }
}