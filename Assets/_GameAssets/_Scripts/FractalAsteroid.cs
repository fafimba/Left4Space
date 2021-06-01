//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//    class FractalAsteroid
//    {

//        public int maxDepth;

//        private int depth;



//       FractalManager fractalManager;
//        int[] _position;

//        public FractalAsteroid()
//        { }

//        public FractalAsteroid(int[] position)
//        {
//            maxDepth = 10;
//            _position = position;
//            fractalManager.positions = new List<int[]>();
//           fractalManager.positions.Add(position);
//            Start();
//        }

//        public void Start()
//        {

//            if (depth < maxDepth)
//            {
//              //new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this);

//                for (int x = _position[0] - 1; x < _position[0] + 1; x++) 
//                {
//                    for (int y = _position[1] - 1; y < _position[1] + 1; y++) 
//                    {
//                        for (int z = _position[2] - 1; z < _position[2] + 1; z++) 
//                        {
//                            int[] positionToCreate = new int[]{x,y,z};
//                            if (!fractalManager.positions.Contains(positionToCreate))
//                            {
//                                Random rnd1 = new Random();
//                              //if ( rnd1.Next(0,10) > 1)
//	                          //     {
//                                        fractalManager.positions.Add(positionToCreate);
//                                        new FractalAsteroid().Initialize(this, positionToCreate,ref fractalManager);
//	                          //      }
//                            }
//                        }
//                    }
//                }


//            }
//        }

//        private void Initialize(FractalAsteroid node, int[] position, ref FractalManager manager)
//        {

//            fractalManager = manager;
//            _position = position;
//            maxDepth = node.maxDepth;
//            depth = node.depth + 1;
//            //Console.WriteLine("New node with depth " + depth);
//            Console.WriteLine("New node with depth " + depth + " and position X:" + position[0] + " Y:" + position[1] + " Z:" + position[2]);
//            Console.ReadKey();
//            Start();
//        }

//    }

