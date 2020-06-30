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

namespace sphk
{
    public class Config
    {
        // Define a string to hold the URL of the webhook to spam
        public string webhook { get; set; }
        // Define a string to hold the message we will be spamming
        public string content { get; set; }
        // Define a string to hold the webhook's custom avatar URL
        public string avatar_url { get; set; }
        // Define a string to hold the webhook's custom username
        public string username { get; set; }
        // Define a string to hold the time we should wait, in seconds, in between POST requests
        public string timeout { get; set; }
        // Define a string containing how many times we should spam this webhook
        // If this string is set to "-1", then we should continue spamming the webhook
        // until the user manually stops by hitting CTRL+C
        public string times { get; set; }
        // Define a string that contains whether or not the message will be text-to-speech enabled.
        public string use_tts { get; set; }
    }
}