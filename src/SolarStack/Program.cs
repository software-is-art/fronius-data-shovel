﻿using Amazon.CDK;
using Env = System.Environment;

namespace SolarStack
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new SolarStack(app, new StackProps {
                Env = new Environment {
                    Account = "AccountIdHere",
                    Region = "us-east-1"
				}
            });
            app.Synth();
        }
    }
}
