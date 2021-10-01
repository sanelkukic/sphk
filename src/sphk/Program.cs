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
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;

namespace sphk
{
    internal class Program
    {
        // Create a Config object called _config
        private static Config _config;
        // Debug mode, by default, is set to false
        private static bool _debug = false;
        // The program's entrypoint
        public static void Main(string[] args)
        {
            // welcome and version information
            Version _version = Assembly.GetExecutingAssembly().GetName().Version;
            string __version = $@"{_version.Major}.{_version.Minor}.{_version.Build}.{_version.Revision}";
            Console.WriteLine("sphk");
            Console.WriteLine($"CLI version {__version}");
            Console.WriteLine("Copyright 2021 Sanel Kukic");
            Console.WriteLine("Licensed under the terms of the MIT License");
            Console.WriteLine("-----------------------");
            // If we did not specify any commandline arguments, show a basic help message and exit with code 1
            if (args.Length == 0)
            {
                Console.WriteLine("Please specify the location of your .json configuration file!\n");
                Console.WriteLine("If you want to generate a file, run this program with the 'generate' command.");
                Console.WriteLine(
                    "For more information and for help using this program, visit https://discord.gg/NSVPhwn9rG");
                Environment.Exit(1);
            }
            
            // If any of our commandline arguments are "-d" or "--debug", then set the debug mode
            // boolean to true
            if (args.Contains("-d") || args.Contains("--debug"))
            {
                _debug = true;
            }
            
            // If our first argument is "--generate" or "-g"...
            if (args[0] == "--generate" || args[0] == "-g")
            {
                // ...And if the total number of arguments we passed is less than 2, then ask the user
                // for the path to save the example JSON to (because it isn't specified)
                if (args.Length < 2)
                {
                    Console.WriteLine("Please specify the full path of where to save the example config file.");
                    Environment.Exit(1);
                }
                // ...But if the total number of arguments is more than 2, then save the string below to
                // the file specified by the user
                string default_config = @"
// Welcome to the default config for sphk!
// Here you can adjust various options to suit your needs!

{
    // This option lets you set the webhook URL to spam. Required.
    ""webhook"": """",
    
    // This option lets you set the message to spam. Required.
    ""content"": """",

    // This option lets you set a custom avatar for your webhook. Paste a link to an avatar. Optional. Defaults to none.
    ""avatar_url"": ""none"",

    // This option lets you set a custom username for your webhook. Optional. Defaults to none.
    ""username"": ""none"",

    // This option lets you set how long, in seconds, sphk will wait in between messages. Required. Defaults to 1 second.
    ""timeout"": ""1"",

    // This option lets you set how many times the webhook will be spammed. Required. If you wish to have the webhook
    // spammed until you hit CTRL+C, set this to -1. Defaults to -1.
    ""times"": ""-1"",

    // This option lets you enable or disable text-to-speech chat messages. Optional. Defaults to false.
    ""use_tts"": ""false""
}
";
                // If the second argument is empty, then it's an invalid file path to write to
                if (args[1] == "" || (args[1] == String.Empty))
                {
                    Console.WriteLine("Invalid path.");
                    // Exit with code 1 (failure)
                    Environment.Exit(1);
                }
                
                // Check if the file specified by the user already exists
                if (File.Exists(args[1]))
                {
                    Console.WriteLine("The example config file already exists!");
                    // Exit with code 1 (failure)
                    Environment.Exit(1);
                }
                else
                {
                    // Try to write the string above to the file specified by the user, and show
                    // a success message if everything goes well!
                    try
                    {
                        // Write Text to File
                        File.WriteAllText(args[1], default_config);
                        Console.WriteLine(
                            $@"An example config file has been created at {args[1]}.");
                        Console.WriteLine(
                            "Open it with your favorite text editor and edit it, then save it and run this program and specify it as the config file");
                        // Exit with code 0 (success)
                        Environment.Exit(0);
                    }
                    // Otherwise show an error message.
                    // If the debug mode variable is set to true, then also show the exception stack trace
                    // and exit with code 1
                    catch (Exception ex)
                    {
                        // Tell the user something went wrong
                        Console.WriteLine("Failed to write example config file! Check your permissions and try again.\n");
                        // If we're in debug mode, print the exception's stack trace
                        if (_debug)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        // And exit with code 1 (failure)
                        Environment.Exit(1);
                    }
                }
            }
            else
            {
                // If the first argument is NOT "-g" or "--generate", then assume
                // that it's a path to the JSON file to load
                
                // If the file specified does not exist, then tell the user about it and exit with
                // code 1
                if (!File.Exists(args[0]))
                {
                    Console.WriteLine("The file you specified does not exist!");
                    Environment.Exit(1);
                }

                // Try to open the file, read the data inside and store it in a string
                // then use Newtonsoft.Json to deserialize the string contents to the Config object
                // created earlier
                try
                {
                    // Read the file
                    using (StreamReader file = File.OpenText(args[0]))
                    {
                        // Deserialize it using JsonSerializer
                        JsonSerializer serializer = new JsonSerializer();
                        // Deserialize to the _config object we made at the very beginning
                        _config = (Config) serializer.Deserialize(file, typeof(Config));
                    }
                }
                // And if we encounter an error while doing all of that
                catch (Exception ex)
                {
                    // Tell the user about it
                    Console.WriteLine(
                        "Failed to deserialize the JSON data, check to make sure your JSON isn't missing any fields and that it is all valid.\n");
                    // And if we're in debug mode, print the exception stack trace
                    if (_debug)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    // And finally, exit with code 1
                    Environment.Exit(1);
                }
                
                // Call the Spam method that we create below
                Spam();
            }
        }
        
        public static void Spam()
        {
            // Create 2 empty integers, one that will hold the user-configurable length of the
            // request timeout, and one that will hold the number of times to spam the webhook (also
            // user-configurable)
            int timeout_length; // Contains the request timeout
            int times_to_spam; // Contains the times to spam the webhook
            
            // Check if the value for the times to spam is a valid integer, and if so
            // convert it to an integer and store it in times_to_spam.
            //
            // int.TryParse returns a boolean that we can check later
            bool is_number = int.TryParse(_config.times, out times_to_spam);
            // And do the same for the timeout length
            bool is_timeout_number = int.TryParse(_config.timeout, out timeout_length);
            
            // If the number of times to spam is not a valid integer
            if (!is_number)
            {
                // Tell the user about it
                Console.WriteLine("Invalid setting for \"times\" in the config file. It must be a number");
                // And exit with code 1 (failure)
                Environment.Exit(1);
            }

            // And if the request timeout is not a valid integer, tell the user
            if (!is_timeout_number)
            {
                Console.WriteLine("Invalid setting for \"timeout\" in the config file. It must be a number.");
                // And exit with code 1 (failure)
                Environment.Exit(1);
            }

            // The timeout in the config file is stored in seconds, so let's convert it
            // to milliseconds to make things easier
            //
            // The formula to convert X seconds into milliseconds is X * 1000
            timeout_length = timeout_length * 1000;

            // If the user wants to spam this webhook forever (which is what -1 means)
            string output = "";
            if (times_to_spam == -1)
            {
                // Create an integer holding the number of the request we're currently on
                int request_number = 1;
                // Start an infinite loop
                while (true)
                {
                    // Call SendRequest(), and get the status code returned from it and store it in
                    // the "status" integer
                    int status = SendRequest();
                    
                    // If the value of the status integer is 0, meaning the request was successful
                    if (status == 0)
                    {
                        // Print a success message to standard output
                        output = $@"Successfully sent request {request_number}";
                        if (_debug)
                        {
                            output += " (204 No Content response)";
                        }
                        Console.WriteLine(output);
                    }
                    // Otherwise, if the value of the status integer is 1, meaning we reached Discord's
                    // ratelimit
                    else if (status == 1)
                    {
                        output = $@"We've hit the ratelimit on request number {request_number}";
                        if (_debug)
                        {
                            output += " (429 Too Many Requests response)";
                        }
                        // Print a ratelimit warning to standard output
                        Console.WriteLine(output);
                    }
                    // Otherwise, if the value of the status integer is 2, meaning the request failed
                    else if (status == 2)
                    {
                        output = $@"An error was encountered when trying to send request {request_number}";
                        if (_debug)
                        {
                            output += " (400 Bad Request response)";
                        }
                        // Print a failure message to standard output
                        Console.WriteLine(output);
                    }
                    // Usually, you shouldn't be able to reach this block of code below
                    // But if you somehow do, might as well put a nice little easter egg in there, eh?
                    // ;)
                    else
                    {
                        Console.WriteLine(
                            "Well, this is unexpected behavior. If you can see this message, report it to 3reetop. Oops.");
                    }
                    
                    // Increment the counter from before
                    request_number++;
                    // Pause all execution on the current thread for however long the timeout
                    // is configured for
                    //
                    // I know that Thread.Sleep is generally a bad idea since it's blocking but it's 
                    // perfectly fine in this situation
                    Thread.Sleep(timeout_length);
                }
            }
            else
            {
                // Otherwise, if we have a set, finite number of times to spam the webhook
                // create a for loop to iterate that many times
                for (var i = 1; i <= times_to_spam; i++)
                {
                    // Call SendRequest() and store it's return value in the status integer
                    int status = SendRequest();
                    // If the return code from SendRequest() was 0, meaning the request
                    // was successful
                    if (status == 0)
                    {
                        output = $@"Successfully sent request {i} out of {times_to_spam}";
                        if (_debug)
                        {
                            output += " (204 No Content response)";
                        }
                        // Then print a success message to standard output
                        Console.WriteLine(output);
                    }
                    // Otherwise, if the return code from SendRequest() was 1, meaning
                    // that we hit Discord's rate limit
                    else if (status == 1)
                    {
                        output = $@"We've hit the ratelimit on request number {i} out of {times_to_spam}";
                        if (_debug)
                        {
                            output += " (429 Too Many Requests response)";
                        }
                        // Then print a ratelimit warning to standard output
                        Console.WriteLine(output);
                    }
                    // Otherwise, if the return code from SendRequest() was 2, meaning the
                    // request was a failure
                    else if (status == 2)
                    {
                        output = $@"An error was encountered when trying to send request {i} out of {times_to_spam}";
                        if (_debug)
                        {
                            output += " (400 Bad Request response)";
                        }
                        // Then print a failure message to standard output
                        Console.WriteLine(
                            output);
                    }
                    // Usually, you shouldn't be able to reach this block of code below
                    // But, if you somehow do, might as well put a nice little easter egg in there, eh?
                    // ;)
                    else
                    {
                        Console.WriteLine(
                            $@"Well, this is unexpected behavior. If you can see this message, report it to 3reetop. Oops.");
                    }

                    // I know that Thread.Sleep is blocking, see my explanation on line 273
                    Thread.Sleep(timeout_length);
                }

                // Once we finish spamming, print a completion message to standard output
                Console.WriteLine("Spamming complete.");
                // And exit the process with a code of 0 (success)
                Environment.Exit(0);
            }
        }

        // The method below handles the networking aspect of the webhook spammer
        // and takes care of sending the request to Discord's API and reading the response
        private static int SendRequest()
        {
            // Create a new PostBody object called _body
            PostBody _body = new PostBody();
            // Set the avatar URL, username, and content of the new PostBody object to the
            // avatar, username, and content in our Config object
            _body.avatar_url = _config.avatar_url;
            _body.username = _config.username;
            _body.content = _config.content;

            // If our Config has it's use_tts property set to the string "false"
            // then set the use_tts boolean property of our PostBody to false
            // and vice versa
            if (_config.use_tts == "false")
            {
                // Set the use_tts boolean property of our PostBody to false
                _body.use_tts = false;
            }
            else
            {
                // Set the use_tts boolean property of our PostBody to true
                _body.use_tts = true;
            }

            // Serialize our PostBody object into a JSON string using Newtonsoft.Json
            string json = JsonConvert.SerializeObject(_body, Formatting.Indented);
            // An integer to store the value that this method will return
            int return_value = 0;
            // Try to send an HTTP POST request to Discord's API
            try
            {
                // Create a new HttpWebRequest to send to the webhook in the "webhook" property
                // of the Config object
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(_config.webhook);
                // Set the Content-Type header of our request to "application/json", meaning the request
                // contains a JSON-formatted body
                request.ContentType = "application/json";
                // Set the method of our HTTP request to POST, meaning we're sending data
                request.Method = "POST";
                // Write the contents of our JSON string to the request's stream using
                // a new StreamWriter instance
                using (var stream_writer = new StreamWriter(request.GetRequestStream()))
                {
                    // Write the JSON string to the stream
                    stream_writer.Write(json);
                }

                // Get the response for our request and cast it to an HttpWebResponse object
                var httpresp = (HttpWebResponse) request.GetResponse();
                // Discord returns a 204 No Content response as opposed to a 200 OK response
                // for all webhook POST requests that were successful.
                //
                // If the status code of our response is NOT 204, then we encountered an error
                if (httpresp.StatusCode != HttpStatusCode.NoContent)
                {
                    // Set the return_value to 2
                    return_value = 2;
                }
                // Otherwise, if the status code of our response is 429, that means we hit
                // Discord's rate limit
                //
                // For whatever reason, the HttpStatusCode class does not contain a property for
                // the 429 Too Many Requests status. So a solution is to cast the integer 429 to
                // an HttpStatusCode class
                else if (httpresp.StatusCode == (HttpStatusCode) 429)
                {
                    // Set the return_value to 1
                    return_value = 1;
                }
                // Otherwise, if the status code of our response is 204 No Content
                // then the request succeeded.
                else if (httpresp.StatusCode == HttpStatusCode.NoContent)
                {
                    // Set the return_value to 0, meaning success
                    return_value = 0;
                }
            }
            // However, if we encounter an application-level exception during all of this
            catch (Exception ex)
            {
                // Then print an error message to standard output
                Console.WriteLine("An exception was encountered when trying to send the request to Discord's API. Try checking your internet connection and try again\n");
                // And if we have debug mode enabled, print the exception's stack trace
                // to standard output
                if (_debug)
                {
                    Console.WriteLine(ex.ToString());
                }

                // And lastly, exit with a code of 1 (meaning failure)
                Environment.Exit(1);
            }

            // Now, to conclude all of the code for this program, we have just one line
            // This line returns the integer variable return_value, which contains
            // the return value for this entire SendRequest() method
            return return_value;
        }
    }
}