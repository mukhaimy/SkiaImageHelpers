namespace ConsoleTester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            D1();
            Console.WriteLine("--- SELESAI");
        }

        static void D1()
        {
            byte[] imageData = File.ReadAllBytes(@"E:\Development2024\03\SkiaImageHelpers\images\p1.jpg");

            //byte[] b1 = SkiaImageHelpers.ImageOperations.ResizeToWidth(imageData, 400);
            //File.WriteAllBytes(@"E:\Development2024\03\SkiaImageHelpers\images\p2.jpg", b1);

            //byte[] b2 = SkiaImageHelpers.ImageOperations.ResizeToHeight(imageData, 400);
            //File.WriteAllBytes(@"E:\Development2024\03\SkiaImageHelpers\images\p3.jpg", b2);

            byte[] b4 = SkiaImageHelpers.ImageOperations.Zoom(imageData, 400, 300);
            File.WriteAllBytes(@"E:\Development2024\03\SkiaImageHelpers\images\p4.jpg", b4);
        }
    }
}
