using System;

namespace CallingConventions
{
    public static class Program
    {
        public static void Main()
        {
            Console.WriteLine("CALL BY VALUE");
            Console.WriteLine();

            var sampleValueType = new SampleValueType { IntValue = 10, StringValue = "This" };
            var sampleReferenceType = new SampleReferenceType { IntValue = 100, StringValue = "Also This" };

            Console.WriteLine("BEFORE: SampleValueType     {{ IntValue = {0}, StringValue = {1} }}", sampleValueType.IntValue, sampleValueType.StringValue);
            Console.WriteLine("BEFORE: SampleReferenceType {{ IntValue = {0}, StringValue = {1} }}", sampleReferenceType.IntValue, sampleReferenceType.StringValue);

            Console.WriteLine();

            WorkOnValueTypeByValue(sampleValueType);
            WorkOnReferenceTypeByValue(sampleReferenceType);

            Console.WriteLine("AFTER : SampleValueType     {{ IntValue = {0}, StringValue = {1} }}", sampleValueType.IntValue, sampleValueType.StringValue);
            Console.WriteLine("AFTER : SampleReferenceType {{ IntValue = {0}, StringValue = {1} }}", sampleReferenceType.IntValue, sampleReferenceType.StringValue);

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("CALL BY REFERENCE (REQUIRES INIT BEFORE CALL)");
            Console.WriteLine();

            var sampleValueType2 = new SampleValueType { IntValue = 10, StringValue = "This" };
            var sampleReferenceType2 = new SampleReferenceType { IntValue = 100, StringValue = "Also This" };

            Console.WriteLine("BEFORE: SampleValueType     {{ IntValue = {0}, StringValue = {1} }}", sampleValueType2.IntValue, sampleValueType2.StringValue);
            Console.WriteLine("BEFORE: SampleReferenceType {{ IntValue = {0}, StringValue = {1} }}", sampleReferenceType2.IntValue, sampleReferenceType2.StringValue);

            Console.WriteLine();

            WorkOnValueTypeByReferenceRequiresInitBeforeCall(ref sampleValueType2);
            WorkOnReferenceTypeByReferenceRequiresInitBeforeCall(ref sampleReferenceType2);

            Console.WriteLine("AFTER : SampleValueType     {{ IntValue = {0}, StringValue = {1} }}", sampleValueType2.IntValue, sampleValueType2.StringValue);
            Console.WriteLine("AFTER : SampleReferenceType {{ IntValue = {0}, StringValue = {1} }}", sampleReferenceType2.IntValue, sampleReferenceType2.StringValue);

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("CALL BY REFERENCE (DOES NOT REQUIRE INIT BEFORE CALL)");
            Console.WriteLine();

            SampleValueType sampleValueType3;
            SampleReferenceType sampleReferenceType3;

            // Console.WriteLine("BEFORE: SampleValueType     {{ IntValue = {0}, StringValue = {1} }}", sampleValueType3.IntValue, sampleValueType3.StringValue);
            // Console.WriteLine("BEFORE: SampleReferenceType {{ IntValue = {0}, StringValue = {1} }}", sampleReferenceType3.IntValue, sampleReferenceType3.StringValue);

            // Console.WriteLine();

            WorkOnValueTypeByReferenceDoesNotRequireInitBeforeCall(out sampleValueType3);
            WorkOnReferenceTypeByReferenceDoesNotRequireInitBeforeCall(out sampleReferenceType3);

            Console.WriteLine("AFTER : SampleValueType     {{ IntValue = {0}, StringValue = {1} }}", sampleValueType3.IntValue, sampleValueType3.StringValue);
            Console.WriteLine("AFTER : SampleReferenceType {{ IntValue = {0}, StringValue = {1} }}", sampleReferenceType3.IntValue, sampleReferenceType3.StringValue);

            Console.WriteLine();
            Console.WriteLine();

            Console.Write("Press any key ... ");
            Console.ReadKey(true);
            Console.WriteLine();
        }

        private static void WorkOnValueTypeByValue(SampleValueType sampleValueType)
            => sampleValueType = new SampleValueType { IntValue = 20, StringValue = "That" };

        private static void WorkOnReferenceTypeByValue(SampleReferenceType sampleReferenceType)
            => sampleReferenceType = new SampleReferenceType { IntValue = 200, StringValue = "Also That" };

        private static void WorkOnValueTypeByReferenceRequiresInitBeforeCall(ref SampleValueType sampleValueType)
            => sampleValueType = new SampleValueType { IntValue = 30, StringValue = "Or That" };

        private static void WorkOnReferenceTypeByReferenceRequiresInitBeforeCall(ref SampleReferenceType sampleReferenceType)
            => sampleReferenceType = new SampleReferenceType { IntValue = 300, StringValue = "Or Also That" };

        private static void WorkOnValueTypeByReferenceDoesNotRequireInitBeforeCall(out SampleValueType sampleValueType)
            => sampleValueType = new SampleValueType { IntValue = 40, StringValue = "And That" };

        private static void WorkOnReferenceTypeByReferenceDoesNotRequireInitBeforeCall(out SampleReferenceType sampleReferenceType)
            => sampleReferenceType = new SampleReferenceType { IntValue = 400, StringValue = "And Also That" };
    }

    public struct SampleValueType
    {
        public int IntValue;
        public string StringValue;
    }

    public class SampleReferenceType
    {
        public int IntValue;
        public string StringValue;
    }
}
