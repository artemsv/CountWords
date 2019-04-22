# CountWords
A simple utility allows you calculate words matching in input files based on some tricky algorithm

# Requirements
.NET Core 2.2

# How to Run
You may run it from the command line by starting run.but. It compiles the sources and runs the application. After executing the application you will see some output like this:


![Some tets results](https://image.prntscr.com/image/MXhKviixQZKvg-qWNNlKPQ.png)

In case there are some issues you may open the solution in Visual Studio 2017 (tested in VS 2017 15.9.3)

# Overview

There are three main algorithms:

- Simplest (download all files at first and then parse them in the same thread)
- Streaming (download file using streaming and buffering and parse it in oieces)
- Streaming with threads (download file using streaming and buffering and parse it in separate threads)

In our impression, the latter method is the fastest. Corresponding test results can be found in TestResults folder (output_Simples_XX.txt, output_Streaming_XX.txt and output_Fastest_XX.txt respectively)

Due to the uncertain nature of the network, these tests provide a fairly wide variation in results.

# Benchmarking

Also there are several types of an internal algorithm for a direct matching calculation. There are some benchmarks (please, see TestResults\Benchmarks_XX.png) were used in order to choose the optimal algorithm. Unfortuntely we have failed to setup benchmark environment in command line with arguments so for now the 

The choosen one is DictionaryBased2 (the best by speed and memory consuption) but it seems that there are also many opportunities for optimization...
 
