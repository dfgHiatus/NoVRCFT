using System;
using VRCFaceTracking;

namespace WCFace.Parser {
    public static class WCFTParser
    {
        public static string[] SplitMessage(string message)
        {
            /*
             * Messages are formatted something like this
             * true/false,[Quaternion],[Vector3],[double; double],[double; double],[double; double],[double; double],double
             * Here's what the parameters are:
             * 0 - Is the face tracking?
             * 1 - Rotation of Head (could be passed to a varibale, but not sure it could be used)
             * 2 - Position of Head (could be passed to a varibale, but not sure it could be used)
             * 3 - Blink (minimum blink - maximum blink)
             * 4 - Mouth (mouth open, mouth wide)
             * 5 - Brow Up (Brow down Left, Brow down Right)
             * 6 - Gaze Data
             * 7 - EyebrowSteepness (Sad/Angry)
            */
            return message.Split(',');
            // yes it's that easy
        }

        public static bool IsMessageValid(string[] splitMessage) => splitMessage.Length == 8;
        public static bool IsMessageValid(string Message) => SplitMessage(Message).Length == 8;

        /*
             * So NeosWCFT has it's own parsing method, and it's not just strings
             * It's parsed in a "Neos-Friendly" variable but fear not! This can be reversed with a hecc ton of splits and conditions!
             * A double2 would look something like this: [1.1;1.1]
             * A Quaternion would look something like this: [1.1;1.1;1.1;1.1]
             * This can be reversed easily,
             * First, split the first [ away, then split the second ] away
             * Now what we have is: 1.1;1.1;1.1;1.1
             * Now we split away the ";"
             * We're left with: 1.1,1.1,1.1,1.1 (commas being each iteration of the array)
            */
        // Note that the type should not be Quaternion or Position, but double, int, bool, string, etc.
        // It should be able to be a TypeCode

        public static T GetValueFromWebsocketArray<T>(string input, int index = 0)
        {
            T valueToReturn = default(T);
            bool didContain = false;
            string[] phase3 = null;
            if (input.Contains("[") && input.Contains("]") && input.Contains(";"))
            {
                didContain = true;
                // Removes the [
                string[] phase1 = input.Split('[');
                // Removes the ]
                string[] phase2 = phase1[1].Split(']');
                // Removes the ;
                phase3 = phase2[0].Split(';');
                // Iterate through all the indexes and cast them to the input type
            }
            string val = string.Empty;
            if (didContain)
                val = phase3[index];
            else
                val = input;
            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Boolean:
                    try { valueToReturn = (T)Convert.ChangeType(val, typeof(T)); } catch (Exception e) { Logger.Error($"Failed to cast value: {val} with exception of: {e}"); }
                    break;
                case TypeCode.Double:
                    try { valueToReturn = (T)Convert.ChangeType(val, typeof(T)); } catch (Exception e) { Logger.Error($"Failed to cast value: {val} with exception of: {e}"); }
                    break;
                default:
                    if (typeof(T) == typeof(float))
                    {
                        try { valueToReturn = (T)Convert.ChangeType(val, typeof(T)); } catch (Exception e) { Logger.Error($"Failed to cast value: {val} with exception of: {e}"); }
                        break;
                    }
                    break;
            }

            return valueToReturn;
        }
    }
}