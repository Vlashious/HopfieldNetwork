// Written by Uładzimir Śniežka 821701
// 2020-12-10

using System;
using HopfieldNetwork;

var input = Reader.ReadInput("1.txt", "2.txt", "3.txt", "4.txt", "5.txt");
Network network = new(input.Join());
input.Randomize();
input.ToImage("noise");
network.Process(input[0]);
Console.ReadKey();