using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AoC2018
{
    class AoC8
    {
        static string[] file;
        class CNode
        {
            public int numSubChild = 0;
            public int numMetaData = 0;
            public int sumMetaData = 0;
            public List<int> MetaData = new List<int>();
            public List<CNode> children = new List<CNode>();
        }

        private static void ReadFile()
        {
            System.IO.StreamReader reader = new StreamReader(@"..\..\Inputs\Input8.txt");
            file = reader.ReadToEnd().Split(' ');
        }

        public static int Function1()
        {
            int index = 0;
            ReadFile();

            return SumMetadata(ref index);
        }

        //Sum up total MetaData values
        private static int SumMetadata(ref int index)
        {
            CNode node = new CNode();
            node.numSubChild = int.Parse(file[index++]);
            node.numMetaData = int.Parse(file[index++]);
            int totalMetaData = 0;

            for (int childIdx = 0; childIdx < node.numSubChild; childIdx++)
                totalMetaData += SumMetadata(ref index);

            for (int i = 0; i < node.numMetaData; i++)
                node.sumMetaData += int.Parse(file[index++]);

            return totalMetaData + node.sumMetaData;
        }

        //Sum up node total. Each node is
        //  a.) Subnode (then MetaData is index so count subnode metavalue
        //  b.) No Subnode (then sum up MetaData)
        //  c.) Subnodes, but index to large = 0
        private static CNode SumMetadata2(ref int index)
        {
            //Header 2 ints (Num SubChidlren / Num MetaData)
            CNode node = new CNode();
            node.numSubChild = int.Parse(file[index++]);
            node.numMetaData = int.Parse(file[index++]);

            for (int childIdx = 0; childIdx < node.numSubChild; childIdx++)
                node.children.Add(SumMetadata2(ref index));

            for (int i = 0; i < node.numMetaData; i++)
            {
                int metadataVal = int.Parse(file[index++]);
                if (node.numSubChild == 0)
                    node.sumMetaData += metadataVal;
                else if (metadataVal <= node.numSubChild)
                    node.sumMetaData += node.children[metadataVal-1].sumMetaData;
            }

            return node;
        }

        public static int Function2()
        {
            int index = 0;
            ReadFile();
            CNode root = SumMetadata2(ref index);

            return root.sumMetaData;
        }
    }
}