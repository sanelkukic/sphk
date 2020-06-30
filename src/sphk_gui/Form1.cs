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
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;

namespace sphk_gui
{
    public partial class Form1 : Form
    {
        // Create a publicly accessible boolean that determines if we're in debug mode
        public bool is_debug;
        public Form1()
        {
            InitializeComponent();
            // In the Form's constructor, immediately set the value of the debug mode boolean
            // This works because we already called InitializeComponent() only 1 line above
            if (debugModeCheck.Checked)
            {
                is_debug = true;
            }
            else
            {
                is_debug = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // As soon as the form loads, immediately check if we have the sphk.exe commandline executable
            // in the same directory as the GUI.
            if (!File.Exists("./sphk.exe"))
            {
                // If we don't then show this warning to the user
                DialogResult res = MessageBox.Show(
                    "You are missing the sphk.exe file. Please download it and place it in the same directory as sphk_gui.exe", "Missing file",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                switch (res)
                {
                    // When the user acknowledges the warning, close the Form.
                    // This should completely exit the application and is more graceful than
                    // just calling Environment.Exit
                    //
                    // Environment.Exit also doesn't return anything so breaking after it doesn't
                    // make sense
                    default:
                        Close();
                        break;
                }
            }
            // Otherwise if we do have the sphk.exe executable file, run everything below
            else
            {
                // This code will get the version of the program and show it in a label
                // in the GUI and on the title bar, nothing special
                //
                // Get the version
                Version _version = Assembly.GetExecutingAssembly().GetName().Version;
                // Format the version string
                string version_text =
                    $@"Version {_version.Major.ToString()}.{_version.Minor.ToString()}.{_version.Build.ToString()}.{_version.Revision.ToString()}";
                // Set the GUI label contents to the newly formatted string we now have
                versionLabel.Text = version_text;
                // Append the newly formatted version string to the titlebar text
                Text += " (" + version_text + ")";
            }
        }

        private void generateConfigFileButton_Click(object sender, EventArgs e)
        {
            // When the user clicks the button to "generate template config file", show a SaveFileDialog
            if (saveTemplateFile.ShowDialog() == DialogResult.OK)
            {
                // A string containing the full path to the currently running executable (the GUI's exe)
                string my_exe_dir = new FileInfo(Assembly.GetEntryAssembly().Location).Directory.ToString();
                // A string containing the full path to the file the user selected in the SaveFileDialog
                string file_path = Path.GetFullPath(saveTemplateFile.FileName);
                // A string containing commandline arguments to pass to sphk.exe later
                string cli_run = "--generate " + file_path;
                // If we are in debug mode, then append the -d flag to the commandline arguments string
                if (is_debug)
                {
                    cli_run += " -d";
                }

                // Create a new Process for our sphk.exe
                Process cli_process = new Process();
                // Specify the full path to sphk.exe, which should be in the same directory
                // as our GUI's executable
                cli_process.StartInfo.FileName = my_exe_dir+"/sphk.exe";
                // Specify the commandline arguments to pass to sphk.exe
                cli_process.StartInfo.Arguments = cli_run;
                // Redirect standard output from the commandline to our application
                cli_process.StartInfo.RedirectStandardOutput = true;
                // Do not create a window for the commandline process
                cli_process.StartInfo.CreateNoWindow = true;
                // Disable shell execute
                cli_process.StartInfo.UseShellExecute = false;
                // Start the process
                cli_process.Start();
                // An empty string to store the commandline output, in case we get any errors.
                // (sphk.exe outputs errors to stdout instead of stderr to make things easier)
                string cli_output = "";
                // Until we reach the end of the output stream for the Process
                while (!cli_process.StandardOutput.EndOfStream)
                {
                    // Read the next line and append it to the cli_output string created above
                    cli_output += cli_process.StandardOutput.ReadLine();
                }
                // Wait for the process to exit
                // Yes, this is blocking but it doesn't matter since it SHOULD only block
                // for a few milliseconds, which is practically unnoticeable.
                cli_process.WaitForExit();
                // Get the exit code of the process as an integer
                int exit_code = cli_process.ExitCode;
                // If the exit code is not 0 (successful)
                if (exit_code != 0)
                {
                    // Show a warning saying that an error happened
                    MessageBox.Show("An error happened while trying to generate the template file:\n" + cli_output,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Otherwise, show a success message!
                    MessageBox.Show("The template file was successfully generated and saved to " + file_path, "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                // If the user cancels the SaveFileDialog by clicking the Cancel button, show a warning
                DialogResult res = MessageBox.Show("An error was encountered when trying to open the dialog.",
                    "Oops", MessageBoxButtons.OK, MessageBoxIcon.Question);
                switch (res)
                {
                    // And then once the user acknowledges the warning, close the Form
                    default:
                        Close();
                        break;
                }
            }
        }

        // The function below gets called anytime the Debug Mode checkbox in the GUI gets
        // updated (anytime you check it or uncheck it)
        private void debugModeCheck_CheckedChanged(object sender, EventArgs e)
        {
            // If the debug mode checkbox is checked, set the is_debug boolean to true
            if (debugModeCheck.Checked)
            {
                is_debug = true;
            }
            // If the debug mode checkbox is not checked, set the is_debug boolean to false
            else
            {
                is_debug = false;
            }
        }

        // The function below gets called when you press the "Start Spamming" button
        // in the GUI
        private void startSpamButton_Click_1(object sender, EventArgs e)
        {
            // Create a new instance of the SpamForm and display it
            // The SpamForm handles everything from here in it's Load event handler
            // so this is all we need to do here.
            SpamForm form = new SpamForm();
            form.ShowDialog();
        }
    }
}