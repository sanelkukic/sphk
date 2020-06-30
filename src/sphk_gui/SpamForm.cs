// Copyright 2020 Sanel Kukic (3reetop)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is furnished to do so, subject to the
// following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
// BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace sphk_gui
{
    public partial class SpamForm : Form
    {
        // Get the debug boolean directly from Form1
        private bool is_debug = new Form1().is_debug;
        // Define cli_process as a new Process object we'll use later
        private Process cli_process;
        public SpamForm()
        {
            InitializeComponent();
        }
        

        private void SpamForm_Load(object sender, EventArgs e)
        {
            // As soon as the form is loaded, open the OpenFileDialog
            if (openConfigDialog.ShowDialog() == DialogResult.OK)
            {
                // Store the full path to the currently running executable as a string my_exe_dir
                string my_exe_dir = new FileInfo(Assembly.GetEntryAssembly().Location).Directory.ToString();
                // Get the full path to the file selected in the OpenFileDialog
                string file_path = Path.GetFullPath(openConfigDialog.FileName);
                // Create a new string for commandline arguments to pass to sphk.exe
                string cli_run = file_path;
                // If we're in Debug Mode, append the -d flag to the commandline arguments.
                if (is_debug)
                {
                    cli_run += " -d";
                }
                // If the file selected in the OpenFileDialog does not exist, show a warning to the user.
                if (!File.Exists(file_path))
                {
                    DialogResult res = MessageBox.Show("That file does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    switch (res)
                    {
                        default:
                            // Close the currently open Form when the user acknowledges the warning
                            Close();
                            break;
                    }
                }
                // Start a new process
                cli_process = new Process();
                // Specify the full file path to the executable to run
                cli_process.StartInfo.FileName = my_exe_dir+"/sphk.exe";
                // Specify commandline arguments
                cli_process.StartInfo.Arguments = cli_run;
                // Redirect standard output to our application
                cli_process.StartInfo.RedirectStandardOutput = true;
                // Do not create a window for the program
                cli_process.StartInfo.CreateNoWindow = true;
                // Disable shell execute
                cli_process.StartInfo.UseShellExecute = false;
                // Enable event handlers
                cli_process.EnableRaisingEvents = true;

                // Handle the process exiting in the process_Exited method
                cli_process.Exited += process_Exited;
                // Handle standard output data received in the process_Data_Received method
                cli_process.OutputDataReceived += process_Data_Received;
                // Start the process
                cli_process.Start();
                // Begin reading from standard output
                cli_process.BeginOutputReadLine();
            }
            else
            {
                // This warning will show if the user cancels the OpenFileDialog by clicking the Cancel button
                DialogResult res = MessageBox.Show("An error was encountered while trying to open the dialog",
                    "Oops", MessageBoxButtons.OK, MessageBoxIcon.Question);
                switch (res)
                {
                    default:
                        // Close the currently open Form when the user acknowledges the warning
                        Close();
                        break;
                }
            }
        }

        private void process_Exited(object sender, EventArgs e)
        {
            // Create a new StringBuilder to store our exiting string before
            // appending it to the RichTextBox
            StringBuilder data_output = new StringBuilder();
            // If the RichTextBox requires invoking, then invoke it. Needed because
            // you cannot modify or access controls from any thread other than the UI thread
            // that they're on.
            if (commandlineOutputBox.InvokeRequired)
            {
                commandlineOutputBox.BeginInvoke(new EventHandler(process_Exited),
                    new[] {sender, e});
            }
            else
            {
                // If it does not require invoking, then append the string "Finished!" to
                // our StringBuilder, and then append the string in the StringBuilder to
                // our RichTextBox
                data_output.Append("\nFinished");
                commandlineOutputBox.AppendText(data_output.ToString());
            }
        }

        private void process_Data_Received(object sender, DataReceivedEventArgs e)
        {
            // Create a StringBuilder to store our standard output data before appending it
            // to the RichTextBox
            StringBuilder data_output = new StringBuilder();
            // If the RichTextBox requires invoking, then invoke it. Needed because
            // you cannot modify or access controls from any thread other than the UI thread
            // that they're on.
            if (commandlineOutputBox.InvokeRequired)
            {
                commandlineOutputBox.BeginInvoke(new DataReceivedEventHandler(process_Data_Received),
                    new[] {sender, e});
            }
            else
            {
                // Append the standard output data to our StringBuilder, and then append the contents
                // in our string builder to our RichTextBox
                data_output.Append(e.Data);
                commandlineOutputBox.AppendText(data_output.ToString()+"\n");
            }
        }

        private void closeWindowButton_Click(object sender, EventArgs e)
        {
            // When I click the close button, append the text below to the RichTextBox
            // using the same StringBuilder method as we used above
            // then kill the Process, and close the Form.
            //
            // We likely don't need the StringBuilder at all and we might be able to just
            // call AppendText on the RichTextBox control directly, but I guess it's better to
            // be safe than sorry
            StringBuilder builder = new StringBuilder();
            builder.Append("\nExiting...");
            commandlineOutputBox.AppendText(builder.ToString());
            cli_process.Kill();
            this.Close();
        }

        private void SpamForm_FormClosing(object sender, EventArgs e)
        {
            cli_process.Kill();
        }
    }
}