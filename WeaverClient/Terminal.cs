using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;


namespace WeaverClient
{
    public class Terminal : MonoBehaviour
    {
        public TextMeshProUGUI output_display;
        public TMP_InputField input_field;
        public StringBuilder current = new StringBuilder();
        public static BlockingCollection<string> toDisplay = new BlockingCollection<string>();
        public static BlockingCollection<string> toParse = new BlockingCollection<string>();

        void Awake()
        {
            Program.unity_ready = true;
        }

        void Update()
        {
            bool items_left = true;
            string message;

            // If message to display...
            while (items_left)
            { 
                items_left = toDisplay.TryTake(out message, 1);
                if (items_left)
                {
                    UpdateDisplay(message);
                }
            }

            //// If input to get
            //message = input_field.text;
            //if (!String.IsNullOrWhiteSpace(message))
            //{
            //    PushInput(message);
            //}
        }

        public void UpdateDisplay(string message)
        {
            Debug.Log(message); 
            current.Append(message);
            output_display.text = current.ToString();
        }

        public void PushInput(string debug)
        {
            string input = input_field.text;
            toParse.Add(input);
            Debug.Log(input);
            Println(Client.Username + "> " + input);
            input_field.text = ""; // Clear input field
        }

        public static string GetInput(int timeoutMillisecs = 1000) // If no timeout provided, check 1 sec
        {
            String input;
            bool success = toParse.TryTake(out input, timeoutMillisecs);
            if (success)
                return input ?? "";
            else
                throw new TimeoutException("User did not provide input within the time limit.");
        }

        public static void Print(string message)
        {
            toDisplay.TryAdd(message, 1);
        }

        public static void Println(string message) { Print(message + "\n"); }
    }
}
