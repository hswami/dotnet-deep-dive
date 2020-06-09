using System;

namespace ReferenceValueTypes
{
    public static class Program
    {
        public static void Main()
        {
            var sampleValueType = new SampleValueType { IntValue = 10, StringValue = "This" };
            var sampleReferenceType = new SampleReferenceType { IntValue = 100, StringValue = "Also This" };

            Console.WriteLine("BEFORE: SampleValueType     {{ IntValue = {0}, StringValue = {1} }}", sampleValueType.IntValue, sampleValueType.StringValue);
            Console.WriteLine("BEFORE: SampleReferenceType {{ IntValue = {0}, StringValue = {1} }}", sampleReferenceType.IntValue, sampleReferenceType.StringValue);

            Console.WriteLine();

            WorkOnValueType(sampleValueType);
            WorkOnReferenceType(sampleReferenceType);

            Console.WriteLine("AFTER : SampleValueType     {{ IntValue = {0}, StringValue = {1} }}", sampleValueType.IntValue, sampleValueType.StringValue);
            Console.WriteLine("AFTER : SampleReferenceType {{ IntValue = {0}, StringValue = {1} }}", sampleReferenceType.IntValue, sampleReferenceType.StringValue);

            Console.WriteLine();
            Console.WriteLine();

            Console.Write("Press any key ... ");
            Console.ReadKey(true);
            Console.WriteLine();
        }

        private static void WorkOnValueType(SampleValueType sampleValueType)
        {
            sampleValueType.IntValue = 20;
            sampleValueType.StringValue = "That";
        }

        private static void WorkOnReferenceType(SampleReferenceType sampleReferenceType)
        {
            sampleReferenceType.IntValue = 200;
            sampleReferenceType.StringValue = "Also That";
        }
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
