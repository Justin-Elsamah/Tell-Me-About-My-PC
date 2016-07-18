using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Speech.Synthesis;


namespace Tell_Me_About_My_PC
{
    class Program
    {
        //this creates an instants of the SpeechSynthesizer function
        private static SpeechSynthesizer synth = new SpeechSynthesizer();
        //Main function
        
        static void Main(string[] args)
        {
            //Creating an instance of a random number
            Random randNum = new Random();
            //List of messages that will be selected at ranfom when CPU is hammered
            List<String> cpuMessages = new List<string>();
            cpuMessages.Add("WARNING, HOLY CRAP YOUR CPU IS ABOUT TO CATCH FIRE");
            cpuMessages.Add("WARNING, OH MY GOD YOU SHOULD NOT RUN YOUR CPU THAT HARD");
            cpuMessages.Add("WARNING, RED ALERT, RED ALERT, RED ALERT");
            cpuMessages.Add("WARNING, IM SCARED");
            cpuMessages.Add("WARNING, IT IS TIME TO STOP WHATEVER YOUR DOING");

            //Thi uses the instance to cause the computer to speak in the default voice
            synth.Speak("Welcome to tell me about my PC version one point oh!");

            #region My Performance Coutners
            //Creating new instance of PerformanceCounter that pulls CPU Load in percentage
            PerformanceCounter perfCpuCounter = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            perfCpuCounter.NextValue();

            //Creating new instance of PerformanceCounter that pulls Available Memory in Megabytes
            PerformanceCounter perfMemCounter = new PerformanceCounter("Memory", "Available MBytes");
            perfMemCounter.NextValue();

            //Creating new instance of PerformanceCounter that pulls System Up Time in seconds
            PerformanceCounter perfUptimeCounter = new PerformanceCounter("System", "System Up Time");
            perfUptimeCounter.NextValue();

            #endregion

            TimeSpan uptimeSpan = TimeSpan.FromSeconds(perfUptimeCounter.NextValue());
            String systemUptimeMessage = String.Format("The current system up time is {0}, days {1}, hours {2}, minutes {3}, seconds", (int)uptimeSpan.TotalDays, (int)uptimeSpan.Hours, (int)uptimeSpan.Minutes, (int)uptimeSpan.Seconds);
            Justin_Speak(systemUptimeMessage,VoiceGender.Male,3);
            //Initializing 
            int speechSpeed = 1;
            //Start of permanent loop 
            while (true)
            {
              
                //Setting an int equal to the next cpu value
                int currentCpuPercentage = (int)perfCpuCounter.NextValue();

                //Setting an int equal to the next current memory value
                int currentAvailableMemory = (int)perfMemCounter.NextValue();

                //outputs the CPU load 
                Console.WriteLine("Cpu Load: {0}%:", currentCpuPercentage);

                //Outputs the Available Memory 
                Console.WriteLine("Available Memory: {0}MB:", currentAvailableMemory);
                //only speaks when the CPU usage is above 80 percent
                #region Logic
                if (currentCpuPercentage > 80)
                {
                    if (currentCpuPercentage == 100)
                    {
                        //This was made to make sure the speech speed does not exceed 6 times normal
                        if (speechSpeed<=5)
                        {
                            speechSpeed++;
                        }
                        //Saves a string cpuLoadVocalMessage to be used in the text to speech
                        string cpuLoadVocalMessage = cpuMessages[randNum.Next(5)];
                        //Actually speaks the current CPU usage using Justin_Speak function
                        Justin_Speak(cpuLoadVocalMessage,VoiceGender.Female, speechSpeed);

                        OpenRickRoll("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
                        //Actually speaks the current CPU usage using Justin_Speak function
                        Justin_Speak(cpuLoadVocalMessage, VoiceGender.Female, speechSpeed);

                    }
                    else
                    {
                       
                        //Saves a string cpuLoadVocalMessage to be used in the text to speech
                        string cpuLoadVocalMessage = string.Format("the current CPU load is {0} percent", currentCpuPercentage);
                        //Actually speaks the current CPU usage using Justin_Speak function
                        Justin_Speak(cpuLoadVocalMessage,VoiceGender.Male,5);

                    }
                }
                //Only speaks when the Available Memory is below one gigabyte
                if (currentAvailableMemory < 1024)
                {
                    //Change voice back to male
                 
                    //Saves a string memAvailableVocalMessage to be used in the text to speech
                    string memAvailableVocalMessage = string.Format("the currently have {0} megabytes of memory available", currentAvailableMemory);
                    //Actually speaks the current Available Memory using Justin_Speak fuction
                    Justin_Speak(memAvailableVocalMessage, VoiceGender.Male,10);
                }
                #endregion Logic
                //Causes console to sleep for one second
                Thread.Sleep(1000);
            }
        }
        //A function that takes in a message and a selected voice
        public static void Justin_Speak(String message, VoiceGender voiceGender)
        {
            //Change voice back to male
            synth.SelectVoiceByHints(voiceGender);
            //Actually speaks the current CPU Load
            synth.Speak(message);
        }
        //A function that takes in a message and a selected voice as well as a selected speed
        public static void Justin_Speak(String message, VoiceGender voiceGender,int rate)
        {
            //Change voice back to male
            synth.SelectVoiceByHints(voiceGender);
            //Actually speaks the current CPU Load
            synth.Speak(message);
            //speach rate
            synth.Rate = rate;
        }
        //Open a website
        public static void OpenRickRoll(String URL)
        {
            Process process = new Process();
            process.StartInfo.FileName = "chrome.exe";
            process.StartInfo.Arguments = URL;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            process.Start();
        }
    }
}
