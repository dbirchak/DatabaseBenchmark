﻿using DatabaseBenchmark.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DatabaseBenchmark.Commands.Test
{
    /// <summary>
    /// The main command that handles the execution of the tests.
    /// </summary>
    public class TestExecutionCommand : TestCommand
    {
        public TestExecutionCommand(MainForm form, Database[] databases, ITest[] tests)
            : base(form, databases, tests)
        {
        }

        public override void Start()
        {
            // Prepare GUI.
            Form.Cancellation = new CancellationTokenSource();

            // Start the benchmark.
            Form.MainTask = Task.Factory.StartNew(DoBenchmark, Form.Cancellation.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public override void Stop()
        {
            if (Form.MainTask == null)
                return;

            Form.Cancellation.Cancel();
        }

        private void DoBenchmark()
        {
            var benchmark = Form.History[0];

            try
            {
                benchmark.ExecuteTests(Form.Cancellation.Token, Tests);
            }
            catch (Exception exc)
            {
                Logger.Error("Test execution error...", exc);
            }
        }
    }
}