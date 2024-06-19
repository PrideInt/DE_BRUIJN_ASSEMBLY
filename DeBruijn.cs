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

            Console.WriteLine("Enter k: ");
            string sequence = Console.ReadLine();
            int k = 3;

            // k-mers
            string[] kmers = RemoveDuplicates(IsolateKMers(sequence, k));

            /*
            foreach (string kmer in kmers) {
                Console.WriteLine(kmer);
            }
            */

            // (k-1)-mers
            string[] k1mers = RemoveDuplicates(IsolateKMers(sequence, k - 1));

            /*
            Console.WriteLine(k1mers.Length);

            foreach (string kmer in k1mers) {
                Console.WriteLine(kmer);
            }
            */

            Console.ReadLine();
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
            return kmers;
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
            Console.WriteLine(size);

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

        static Graph GenerateGraph() {
            return null;
        }
    }

    class Graph {
        public Node head, tail;
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

        public void AddNode(int data) {
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
        public void AddEdge(Node from, Node to) {
            Edge edge = new(from, to);
            from.AddNext(to);
            edges[e++] = edge;
        }
    }
    class Node {
        public Node next, prev;
        public int data;

        public Node() {
            next = prev = null;
        }
        public Node(int data) : this() {
            this.data = data;
        }
        public void AddNext(Node n) {
            next = n;
            n.prev = this;
        }
        public void Display() {
            Console.WriteLine(data);
        }
    }
    class Edge {
        public Node from;
        public Node to;

        public Edge() {
            from = to = null;
        }
        public Edge(Node from, Node to) {
            this.from = from;
            this.to = to;
        }
        public void Display() {
            Console.WriteLine(from.data + " -> " + to.data);
        }
    }
}