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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace sphk_gui
{
    public partial class SpamForm : Form
    {
        // boolean to store debug mode flag, defaults to false
        public bool is_debug = false;
        
        // Define cli_process as a new Process object we'll use later
        private Process cli_process;
        public SpamForm(bool _is_debug)
        {
            InitializeComponent();
            // if a debug mode flag set to true is passed in the constructor, then set the
            // debug mode boolean is_debug in this form to true
            //
            // the way i managed to get Form1 and SpamForm to communicate with each other is to
            // have an event handler in Form1, and to create a new instance of SpamForm in the scope of
            // the entire Form1 class, and then updating the is_debug property of the SpamForm instance
            // anytime the event handler fires
            //
            // sounds hacky but it works
            if (is_debug)
            {
                is_debug = _is_debug;
            }
        }
        

        private void SpamForm_Load(object sender, EventArgs e)
        {
            // clear all text from the commandLineOutputBox control
            commandlineOutputBox.Clear();
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
                DialogResult res = MessageBox.Show("Please select a file!",
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
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("\nExiting...");
                commandlineOutputBox.AppendText(builder.ToString());
                // if the sphk.exe process is not null (to avoid NullReferenceExceptions)
                if (cli_process != null)
                {
                    // AND the process has not already exited on its own
                    if (!cli_process.HasExited)
                    {
                        // forcefully kill it
                        cli_process.Kill();
                    }

                    // and then close the currently open window
                    Close();
                }
            }
            catch (InvalidOperationException ex)
            {
                // this exception is thrown when the process has already exited, which fixes
                // issue #1
                // we should just ignore the exception and close the form, since sphk.exe has
                // already been killed or has exited on its own
                //
                // but if we're in debug mode, show an exception stack trace to the user
                if (is_debug)
                {
                    MessageBox.Show(ex.ToString(), "Error killing sphk.exe process", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

                // and then close the form/window
                Close();
            }
            catch (Win32Exception ex)
            {
                // this exception handler will take care of any situation where
                // the process could not be terminated for whatever reason
                //
                // in this situation, we'll show a nice and user-friendly error message
                // (optionally with the exception stack trace appended to it if debug mode is on)
                // and exit the program forcefully
                //
                if (is_debug)
                {
                    // show an error to the user containing the exception stacktrace
                    MessageBox.Show(ex.ToString(), "Error killing sphk.exe process", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

                // otherwise just close the current window gracefully
                Close();
            }
            catch (Exception ex)
            {
                // this exception handler will take care of any other exception that the above 2 handlers
                // cannot
                //
                // it really just does the exact same thing as the ones above.
                //
                // a string to store our error message in
                string error_msg = "Failed to terminate sphk.exe process.";
                if (is_debug)
                {
                    // append the exception stacktrace to the error message if we're in debug mode
                    error_msg += "\n" + ex.ToString();
                }

                // show the message box to the user
                DialogResult res = MessageBox.Show(error_msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                switch (res)
                {
                    // once the user acknowledges the message box, forcefully exit the entire application
                    default:
                        Environment.Exit(1);
                        break;
                }
            }
        }

        private void SpamForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // clear the contents of the output log textbox
            commandlineOutputBox.Clear();
            try
            {
                // if the sphk.exe process is not null
                if (cli_process != null)
                {
                    // and if it hasn't already exited on its own
                    if (!cli_process.HasExited)
                    {
                        // then kill it
                        cli_process.Kill();
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                // this exception handler fixes issue #1
                //
                // if we're in debug mode
                if (is_debug)
                {
                    // show an error to the user containing the exception stacktrace
                    MessageBox.Show(ex.ToString(), "Error killing sphk.exe process", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

                // otherwise just close the current window gracefully
                Close();
            }
            catch (Win32Exception ex)
            {
                // if we're in debug mode
                if (is_debug)
                {
                    // show an error to the user containing the exception stacktrace
                    MessageBox.Show(ex.ToString(), "Error killing sphk.exe process", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

                // otherwise just close the current window gracefully
                Close();
            }
            catch (NullReferenceException ex)
            {
                // fixes an issue where cancelling the OpenFileDialog results in a
                // NullReferenceException when the form closes
                // 
                // this exception was being thrown because I was not checking if cli_process was null
                // and when you don't select a config file, it is null
                //
                // and trying to access the HasExited property or trying to call .Kill on a null object
                // obviously throws a NullReferenceException
                if (is_debug)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // close the form window
                Close();
            }
            catch (Exception ex)
            {
                // this exception handler will take care of any other exception that the above 2 handlers
                // cannot
                //
                // it really just does the exact same thing as the ones above.
                //
                // a string to store our error message in
                string error_msg = "Failed to terminate sphk.exe process.";
                if (is_debug)
                {
                    // append the exception stacktrace to the error message if we're in debug mode
                    error_msg += "\n" + ex.ToString();
                }

                // show the message box to the user
                DialogResult res = MessageBox.Show(error_msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                switch (res)
                {
                    // once the user acknowledges the message box, forcefully exit the entire application
                    default:
                        Environment.Exit(1);
                        break;
                }
            }
        }
    }
}