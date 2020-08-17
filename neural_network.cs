using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;

namespace Snake____final_version
{
    [Serializable]
    class neuron
    {
        /*
        // Examination of the back propagation function by teaching the neural network to do xor.
        // One output would be XOR of the two inputs, the second would be NOT XOR, the third AND the the fourth OR.
        static void Main(string[] args)
        {

            neuralnetwork asd = new neuralnetwork(2, 4, 2, 4);
            double[] output = new double[2];



            // the input values
            double[] x1 = { 0, 0 };
            double[] x2 = { 0, 1 };
            double[] x3 = { 1, 0 };
            double[] x4 = { 1, 1 };


            int epoch = 0;

        Retry:
            epoch++;
            for (int i = 0; i < 4; i++)
            {
                // 1) forward propagation (calculates output)

                double[] results = new double[4];       // desired results

                if (i == 0)
                {
                    output = asd.run(x1);
                    results[0] = (int)x1[0] ^ (int)x1[1];
                    results[1] = 1 - ((int)x1[0] ^ (int)x1[1]);
                    results[2] = (int)x1[0] & (int)x1[1];
                    results[3] = (int)x1[0] | (int)x1[1];
                    asd.bp(results);
                    asd.activateAdjustments();
                }
                if (i == 1)
                {
                    output = asd.run(x2);
                    results[0] = (int)x2[0] ^ (int)x2[1];
                    results[1] = 1 - ((int)x2[0] ^ (int)x2[1]);
                    results[2] = (int)x2[0] & (int)x2[1];
                    results[3] = (int)x2[0] | (int)x2[1];
                    asd.bp(results);
                    asd.activateAdjustments();


                }
                if (i == 2)
                {
                    output = asd.run(x3);
                    results[0] = (int)x3[0] ^ (int)x3[1];
                    results[1] = 1 - ((int)x3[0] ^ (int)x3[1]);
                    results[2] = (int)x3[0] & (int)x3[1];
                    results[3] = (int)x3[0] | (int)x3[1];
                    asd.bp(results);
                    asd.activateAdjustments();


                }
                if (i == 3)
                {
                    output = asd.run(x4);
                    //Console.WriteLine("{0} xor {1} = {2} ------- {3}", x4[0], x4[1], output[0], output[1]);
                    results[0] = (int)x4[0] ^ (int)x4[1];
                    results[1] = 1 - ((int)x4[0] ^ (int)x4[1]);
                    results[2] = (int)x4[0] & (int)x4[1];
                    results[3] = (int)x4[0] | (int)x4[1];
                    asd.bp(results);
                    asd.activateAdjustments();

                }

            }

            if (epoch < 50000)
                goto Retry;

            for (int i = 0; i < 4; i++)
            {
                // 1) forward propagation (calculates output)
                double[] qwe = new double[1];
                if (i == 0)
                {
                    output = asd.run(x1);
                    Console.WriteLine("{0} xor {1} = {2} ------- {0} not xor {1} = {3} ------- {0} and {1} = {4} ------- {0} or {1} = {5}", x1[0], x1[1], output[0], output[1], output[2], output[3]);
                }
                if (i == 1)
                {
                    output = asd.run(x2);
                    Console.WriteLine("{0} xor {1} = {2} ------- {0} not xor {1} = {3} ------- {0} and {1} = {4} ------- {0} or {1} = {5}", x2[0], x2[1], output[0], output[1], output[2], output[3]);
                }
                if (i == 2)
                {
                    output = asd.run(x3);
                    Console.WriteLine("{0} xor {1} = {2} ------- {0} not xor {1} = {3} ------- {0} and {1} = {4} ------- {0} or {1} = {5}", x3[0], x3[1], output[0], output[1], output[2], output[3]);

                }
                if (i == 3)
                {
                    output = asd.run(x4);
                    Console.WriteLine("{0} xor {1} = {2} ------- {0} not xor {1} = {3} ------- {0} and {1} = {3} ------- {0} or {1} = {3}", x4[0], x4[1], output[0], output[1], output[2], output[3]);
                }

            }
            Console.ReadLine();
        }*/

        static Random rnd = new Random();

        public double[] Weights;    // the weights values are between -1 -> 1. The output from this neuron to neuron in the next layer is multiplied by its mached weight.
        public double bias;         // the bias is added to the value of this neuron every time the neuron gets its value.
        public double value = 0;    // the value the neuron holds. 
        public neuron[] neuronsOut; // array of the neurons which this neuron is conected directly to.
        public double[] errorArr;   // the errors for each neuron in the output level of the network
                                    // error = desired result - actual result.
        public double[] influence;  // influence of this neuron on every output neuron in the final output layer.
        public double[,] influence2;// influence of every output neuron on every neuron in the final output layer.
        double[] changeSum;         // sum of the changes that the back prop function gives.
        double divideBy;            // the num of times the back prop function was called before adjustments were activated.
        int FinaleoutputLength;     // the output length of the last layer (output layer) in the neural network which this neuron belongs to.
        int outputLength;           // the num of neurons in the next layer.



        // constructor function. gets the number of output neurons of this neuron and the number of neurons in last layer (output layer).
        public neuron(int outputLength, int FinaleoutputLength)
        {
            this.outputLength = outputLength;
            neuronsOut = new neuron[outputLength];
            errorArr = new double[FinaleoutputLength];
            influence = new double[FinaleoutputLength];
            influence2 = new double[outputLength, FinaleoutputLength];
            this.FinaleoutputLength = FinaleoutputLength;
            Weights = new double[outputLength];
            changeSum = new double[outputLength + 1];
            divideBy = 0;
            for (int i = 0; i < outputLength; i++)  // create randomized weights
            {
                Weights[i] = 2 * rnd.NextDouble() - 1;
            }
            bias = 2 * rnd.NextDouble() - 1;
        }

        // fills a neuron slot in the 'neuronsOut' array with a given neuron. The slot position is also given.
        public void neuronsOutFill(neuron x, int place)
        {
            neuronsOut[place] = x;
        }

        // relu activation function
        public void Activation_function()
        {
            if (value > 999)
            {
                value = 999;
                return;
            }

            if (value > 0)
            {
                return;
            }
            else
            {
                double a = 0.05;
                value = a * value;
            }
            if (value < -999)
            {
                value = -999;
            }
        }

        // sigmoid activation function
        //public void Activation_function() { value = 1.0 / (1.0 + Math.Exp(-value)); }

        // relu derivative
        public double derivative() 
        {
            if (value > 0)
            {
                return (1);
            }
            else
            {
                double a = 0.05;
                return (a);
            }
        }


        // sigmoid derivative
        //public double derivative() { return value * (1 - value); }

        // Changes the bias and all the weights randomly. The variable 'a' determines the degree of change.
        // good for evolutionary purposes, creates mutations.
        public void Change(double a)
        {
            for (int i = 0; i < Weights.Length; i++)
            {
                Weights[i] *= 1 + ((2 * rnd.NextDouble() - 1) / a);
                Weights[i] += (2 * rnd.NextDouble() - 1) / (a * 3);
            }
            bias *= 1 + ((2 * rnd.NextDouble() - 1) / a);
            bias += (2 * rnd.NextDouble() - 1) / (a * 3);
        }

        // called by the back prop that's in the neuralnetwork class. Adjust the Bias changes that need to be done by the back prop function but does not activate them yet.
        public void adjustBias()
        {
            double sum = 0;
            for (int j = 0; j < FinaleoutputLength; j++)
            {
                sum += errorArr[j] * influence[j] * derivative();
            }
            changeSum[outputLength] += sum / FinaleoutputLength;
            divideBy++;
        }

        // called by the back prop that's in the neuralnetwork class. Adjust the Bias changes that need to be done by the back prop function but does not activate them yet.
        public void adjustWeights()
        {
            double sum;
            for (int z = 0; z < outputLength; z++)
            {
                sum = 0;
                for (int j = 0; j < FinaleoutputLength; j++)
                {

                    sum += errorArr[j] * influence2[z, j] * value;
                }
                changeSum[z] += sum / FinaleoutputLength;
            }
        }

        // activates the adjustments saved by both 'adjustWeights()' and 'adjustBias()'.
        public void activateAdjustments()
        {
            double w = 0.01;    // learning rate
            for (int z = 0; z < outputLength; z++)
            {
                Weights[z] += w * changeSum[z] / divideBy;
            }
            bias += w * changeSum[outputLength] / divideBy;

            changeSum = new double[outputLength + 1];
            divideBy = 0;
        }

        // resets the variables: 'errorArr', 'influence', 'influence2'.
        public void initialization()
        {
            errorArr = new double[FinaleoutputLength];
            influence = new double[FinaleoutputLength];
            influence2 = new double[outputLength, FinaleoutputLength];
        }
    }

    //*****************************************************************************************************************************************************
    [Serializable]
    public class neuralnetwork
    {
        static Random rnd = new Random();
        public double score = 0;            // score of the network. usefull for evolutionary learning.
        neuron[] entryLevel;                // array of neurons that is the entry level layer (network input).
        neuron[] outputLevel;               // array of neurons that is the last layer of the network (network output).
        public int length;                  // num of neurons in every hidden layer.
        public int numOfneuronsRows;        // num of hidden layers (must be at least one).
        neuron[,] neurons;                  // two dimensional array of neurons. Those are the hidden layers.
        public int outputLength;            // last layer length (network output lenght).

        // constructor function.
        public neuralnetwork(int numOfneuronsRows1, int length1, int entryLevelLenght, int outputLength)
        {
            numOfneuronsRows = numOfneuronsRows1;
            length = length1;
            this.outputLength = outputLength;
            entryLevel = new neuron[entryLevelLenght];
            outputLevel = new neuron[outputLength];
            neurons = new neuron[numOfneuronsRows, length];
            build();
        }

        // build the neural network. Fills the arrays with neurons and connects them.
        void build()
        {
            for (int i = 0; i < entryLevel.Length; i++)
            {
                entryLevel[i] = new neuron(length, outputLength);
            }
            for (int i = 0; i < numOfneuronsRows; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (i != numOfneuronsRows - 1)
                    {
                        neurons[i, j] = new neuron(length, outputLength);
                    }
                    else
                    {
                        neurons[i, j] = new neuron(outputLength, outputLength);
                    }
                }
            }
            for (int i = 0; i < entryLevel.Length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    entryLevel[i].neuronsOutFill(neurons[0, j], j);
                }
            }
            for (int i = 0; i < numOfneuronsRows; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (i != numOfneuronsRows - 1)
                    {
                        for (int z = 0; z < length; z++)
                        {
                            neurons[i, j].neuronsOutFill(neurons[i + 1, z], z);
                        }
                    }
                }
            }
            for (int i = 0; i < outputLevel.Length; i++)
            {
                outputLevel[i] = new neuron(outputLength, outputLength);
                for (int j = 0; j < length; j++)
                {
                    neurons[numOfneuronsRows - 1, j].neuronsOutFill(outputLevel[i], i);
                }
            }
        }

        // running the input through the network.
        void forward()
        {
            // input layer to first hidden layer
            for (int i = 0; i < length; i++)
            {
                neurons[0, i].value = 0;
                for (int z = 0; z < entryLevel.Length; z++)
                {
                    neurons[0, i].value += entryLevel[z].value * entryLevel[z].Weights[i];
                }
                neurons[0, i].value += neurons[0, i].bias;
                neurons[0, i].Activation_function();
            }

            // every hidden layer to next hidden layer
            for (int i = 1; i < numOfneuronsRows; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    neurons[i, j].value = 0;
                    for (int z = 0; z < length; z++)
                    {
                        neurons[i, j].value += neurons[i - 1, z].value * neurons[i - 1, z].Weights[j];
                    }
                    neurons[i, j].value += neurons[i, j].bias;
                    neurons[i, j].Activation_function();
                }
            }

            // last hidden layer to output layer
            for (int i = 0; i < outputLevel.Length; i++)
            {
                outputLevel[i].value = 0;
                for (int z = 0; z < length; z++)
                {
                    outputLevel[i].value += neurons[numOfneuronsRows - 1, z].value * neurons[numOfneuronsRows - 1, z].Weights[i];
                }
                outputLevel[i].value += outputLevel[i].bias;
                outputLevel[i].Activation_function();
            }
        }

        // public function which gets the input, runs it through the network (with forward()) and returns the last layer (output layer) values in an arrey.
        public double[] run(double[] input)
        {
            // fill the entry Level.
            for (int i = 0; i < entryLevel.Length; i++)
            {
                entryLevel[i].value = input[i];
            }

            // running the input through the network.
            forward();

            double[] output = new double[outputLevel.Length];

            // fill the output array which will be return.
            for (int i = 0; i < outputLevel.Length; i++)
            {
                output[i] = outputLevel[i].value;
            }

            return (output);
        }

        // public function which calls 'run()' and then returns the position in the returned array which holds the largest value.
        public int Result(double[] x)
        {
            run(x);

            double temp = 0;
            int biggest = 0;
            for (int i = 0; i < outputLevel.Length; i++)
            {
                if (outputLevel[i].value > temp)
                {
                    biggest = i;
                    temp = outputLevel[i].value;
                }
            }
            return (biggest);
        }
        public void print()
        {
            for (int i = 0; i < outputLevel.Length; i++)
            {
                Console.Write(outputLevel[i].value + "  ");
            }
        }

        // public function which calls 'run()' and then returns the position in the returned array which holds the largest value and also is true is the 'authorizedPositions' bool array.
        public int Result(double[] x, bool[] authorizedPositions)
        {
            run(x);
            double temp = 0;
            int biggest = 0;
            for (int i = 0; i < outputLevel.Length; i++)
            {
                if (outputLevel[i].value > temp && authorizedPositions[i])
                {
                    biggest = i;
                    temp = outputLevel[i].value;
                }
            }
            return (biggest);
        }


        // changes all the bias and weights in the neural network by a small number. the variable 'a' determine the degree of change.
        // good for evolutionary purposes. creates mutations in the network.
        public void Change(double a)
        {
            for (int i = 0; i < entryLevel.Length; i++)
            {
                entryLevel[i].Change(a);
            }
            for (int i = 0; i < numOfneuronsRows; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    neurons[i, j].Change(a);
                }
            }
            for (int i = 0; i < outputLevel.Length; i++)
            {
                outputLevel[i].Change(a);
            }
        }


        // saving the network to the computer memory.
        public void Save(string x)
        {
            BinaryFormatter bf = new BinaryFormatter();
            int y = 0;
            FileStream file;
            if (File.Exists(x + length + "_" + (numOfneuronsRows) + "_" + y))
            {
                while (File.Exists(x + length + "_" + (numOfneuronsRows) + "_" + y))
                {
                    y++;
                }
                file = File.Create(x + length + "_" + (numOfneuronsRows) + "_" + y);
            }
            else
            {
                file = File.Create(x + length + "_" + (numOfneuronsRows) + "_" + 0);
            }
            //  "c:\\4inRow\\_neuralnetwork.dat"
            neuralnetwork data = new neuralnetwork(numOfneuronsRows, length, entryLevel.Length, outputLength);

            for (int i = 0; i < entryLevel.Length; i++)
            {
                for (int z = 0; z < entryLevel[i].Weights.Length; z++)
                {
                    data.entryLevel[i].Weights[z] = entryLevel[i].Weights[z];
                }
                data.entryLevel[i].bias = entryLevel[i].bias;

            }
            for (int i = 0; i < numOfneuronsRows; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    for (int z = 0; z < neurons[i, j].Weights.Length; z++)
                    {
                        data.neurons[i, j].Weights[z] = neurons[i, j].Weights[z];
                    }

                    data.neurons[i, j].bias = neurons[i, j].bias;
                }
            }
            for (int i = 0; i < outputLevel.Length; i++)
            {
                for (int z = 0; z < outputLevel[i].Weights.Length; z++)
                {
                    data.outputLevel[i].Weights[z] = outputLevel[i].Weights[z];
                }
                data.outputLevel[i].bias = outputLevel[i].bias;
            }

            data.score = score;

            bf.Serialize(file, data);
            file.Close();
        }

        // loading the network from the computer memory.
        public void Load(neuralnetwork data)
        {
            for (int i = 0; i < entryLevel.Length; i++)
            {
                for (int z = 0; z < entryLevel[i].Weights.Length; z++)
                {
                    entryLevel[i].Weights[z] = data.entryLevel[i].Weights[z];
                }
                entryLevel[i].bias = data.entryLevel[i].bias;

            }
            for (int i = 0; i < numOfneuronsRows; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    for (int z = 0; z < neurons[i, j].Weights.Length; z++)
                    {
                        neurons[i, j].Weights[z] = data.neurons[i, j].Weights[z];
                    }

                    neurons[i, j].bias = data.neurons[i, j].bias;
                }
            }
            for (int i = 0; i < outputLevel.Length; i++)
            {
                for (int z = 0; z < outputLevel[i].Weights.Length; z++)
                {
                    outputLevel[i].Weights[z] = data.outputLevel[i].Weights[z];
                }
                outputLevel[i].bias = data.outputLevel[i].bias;
            }
        }

        // loading a network from a given network (duplicating the given network into this network).
        public void Load(int fileNum, string x)
        {

            if (File.Exists(x + length + "_" + (numOfneuronsRows) + "_" + fileNum))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(x + length + "_" + (numOfneuronsRows) + "_" + fileNum, FileMode.Open);
                neuralnetwork data = (neuralnetwork)bf.Deserialize(file);
                file.Close();
                for (int i = 0; i < entryLevel.Length; i++)
                {
                    for (int z = 0; z < entryLevel[i].Weights.Length; z++)
                    {
                        entryLevel[i].Weights[z] = data.entryLevel[i].Weights[z];
                    }
                    entryLevel[i].bias = data.entryLevel[i].bias;

                }
                for (int i = 0; i < numOfneuronsRows; i++)
                {
                    for (int j = 0; j < length; j++)
                    {
                        for (int z = 0; z < neurons[i, j].Weights.Length; z++)
                        {
                            neurons[i, j].Weights[z] = data.neurons[i, j].Weights[z];
                        }
                        neurons[i, j].bias = data.neurons[i, j].bias;
                    }
                }
                for (int i = 0; i < outputLevel.Length; i++)
                {
                    for (int z = 0; z < outputLevel[i].Weights.Length; z++)
                    {
                        outputLevel[i].Weights[z] = data.outputLevel[i].Weights[z];
                    }
                    outputLevel[i].bias = data.outputLevel[i].bias;
                }
            }
        }


        //*********************************************************************************************************************************************************************************

        //back propogation
        public void bp(double[] desiredResults)
        {
            // adjust the bias and weights of every neuron in the last layer in the network (output layer).
            for (int i = 0; i < outputLevel.Length; i++)
            {
                for (int j = 0; j < outputLevel.Length; j++)
                {
                    outputLevel[i].influence2[j, i] = 1;
                    outputLevel[i].errorArr[j] = (desiredResults[j] - outputLevel[j].value);
                }
                outputLevel[i].influence[i] = 1;
                outputLevel[i].adjustBias();
                outputLevel[i].adjustWeights();
            }

            // adjust the bias and weights of every neuron in the hidden layers. Each adjustment is based on information from later layers which are being dealt first, as it called, back propagation...
            for (int i = numOfneuronsRows - 1; i >= 0; i--)
            {
                for (int j = 0; j < length; j++)
                {
                    for (int z = 0; z < neurons[i, j].neuronsOut.Length; z++)
                    {
                        for (int m = 0; m < outputLevel.Length; m++)
                        {
                            if (z == 0)
                            {
                                neurons[i, j].errorArr[m] = neurons[i, j].neuronsOut[z].errorArr[m];
                            }
                            neurons[i, j].influence2[z, m] = neurons[i, j].neuronsOut[z].influence[m] * neurons[i, j].neuronsOut[z].derivative();
                        }
                    }
                    neurons[i, j].adjustWeights();
                    for (int z = 0; z < neurons[i, j].neuronsOut.Length; z++)
                    {
                        for (int m = 0; m < outputLevel.Length; m++)
                        {
                            neurons[i, j].influence[m] += neurons[i, j].neuronsOut[z].influence[m] * neurons[i, j].neuronsOut[z].derivative() * neurons[i, j].Weights[z];
                        }
                    }
                    neurons[i, j].adjustBias();
                }
            }

            // adjust the bias and weights of every neuron in the neurons of the first layer (input layet). Each adjustment is based on information from the first hidden layer.
            for (int i = 0; i < entryLevel.Length; i++)
            {
                for (int z = 0; z < length; z++)
                {
                    for (int m = 0; m < outputLevel.Length; m++)
                    {
                        if (z == 0)
                        {
                            entryLevel[i].errorArr[m] = entryLevel[i].neuronsOut[z].errorArr[m];
                        }
                        entryLevel[i].influence2[z, m] += entryLevel[i].neuronsOut[z].influence[m] * entryLevel[i].neuronsOut[z].derivative();
                    }
                }
                entryLevel[i].adjustWeights();
                for (int z = 0; z < length; z++)
                {
                    for (int m = 0; m < outputLevel.Length; m++)
                    {
                        entryLevel[i].influence[m] += entryLevel[i].neuronsOut[z].influence[m] * entryLevel[i].neuronsOut[z].derivative() * entryLevel[i].Weights[z];
                    }
                }
                entryLevel[i].adjustBias();
            }
            initialization();
        }


        // initializing 'errorArr', 'influence' and 'influence2' in every single neuron in the network.
        private void initialization()
        {
            for (int i = 0; i < entryLevel.Length; i++)
            {
                entryLevel[i].initialization();
            }
            for (int i = 0; i < numOfneuronsRows; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    neurons[i, j].initialization();
                }
            }

            for (int i = 0; i < outputLevel.Length; i++)
            {
                outputLevel[i].initialization();
            }
        }

        // activate the adjustments made by the back prop function for every single neuron in the network.
        public void activateAdjustments()
        {
            for (int i = 0; i < entryLevel.Length; i++)
            {
                entryLevel[i].activateAdjustments();
            }
            for (int i = 0; i < numOfneuronsRows; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    neurons[i, j].activateAdjustments();
                }
            }
            for (int i = 0; i < outputLevel.Length; i++)
            {
                outputLevel[i].activateAdjustments();
            }
        }
    }
}
