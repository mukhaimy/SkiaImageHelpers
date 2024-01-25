using System.IO;
using SkiaSharp;
using static System.Net.Mime.MediaTypeNames;


namespace SkiaImageHelpers
{
    public static class ImageOperations
    {
        public static byte[] ResizeToWidth(byte[] imageData, int newWidth)
        {
            using (var inputStream = new SKManagedStream(new MemoryStream(imageData)))
            {
                using (var originalBitmap = SKBitmap.Decode(inputStream))
                {
                    double aspectRatio = (double)originalBitmap.Width / originalBitmap.Height;
                    // Calculate new dimensions while maintaining aspect ratio
                    int newHeight = (int)(newWidth / aspectRatio);

                    // Resize the image
                    using (var resizedBitmap = originalBitmap.Resize(new SKImageInfo(newWidth, newHeight), SKFilterQuality.High))
                    {
                        // Save the resized image to a new MemoryStream
                        using (var outputMemoryStream = new MemoryStream())
                        {
                            using (var outputStream = new SKManagedWStream(outputMemoryStream))
                            {
                                resizedBitmap.Encode(outputStream, SKEncodedImageFormat.Jpeg, 90); // Change format and quality as needed
                            }

                            // Now, outputMemoryStream contains the resized image data
                            byte[] resizedImageData = outputMemoryStream.ToArray();

                            return resizedImageData;
                        }
                    }
                }
            }
        }

        public static byte[] ResizeToHeight(byte[] imageData, int newHeight)
        {
            using (var inputStream = new SKManagedStream(new MemoryStream(imageData)))
            {
                using (var originalBitmap = SKBitmap.Decode(inputStream))
                {
                    double aspectRatio = (double)originalBitmap.Width / originalBitmap.Height;
                    // Calculate new dimensions while maintaining aspect ratio
                    int newWidth = (int)(newHeight * aspectRatio);

                    // Resize the image
                    using (var resizedBitmap = originalBitmap.Resize(new SKImageInfo(newWidth, newHeight), SKFilterQuality.High))
                    {
                        // Save the resized image to a new MemoryStream
                        using (var outputMemoryStream = new MemoryStream())
                        {
                            using (var outputStream = new SKManagedWStream(outputMemoryStream))
                            {
                                resizedBitmap.Encode(outputStream, SKEncodedImageFormat.Jpeg, 90); // Change format and quality as needed
                            }

                            // Now, outputMemoryStream contains the resized image data
                            byte[] resizedImageData = outputMemoryStream.ToArray();

                            return resizedImageData;
                        }
                    }
                }
            }
        }

        public static byte[] Zoom(byte[] imageData, int newWidth, int newHeight)
        {
            int maxWidth = newWidth + 2; // Tambah 2 untuk jaga2 kalau error
            int maxHeight = newHeight + 2; 

            using (var inputStream = new SKManagedStream(new MemoryStream(imageData)))
            {
                using (var originalBitmap = SKBitmap.Decode(inputStream))
                {

                    // Calculate the new dimensions while maintaining the aspect ratio
                    float scaleX = (float)maxWidth / (float)originalBitmap.Width;
                    float scaleY = (float)maxHeight / (float)originalBitmap.Height;
                    float scaleR = Math.Max(scaleX, scaleY);

                    
                    int newTWidth = (int)(originalBitmap.Width * scaleR);   // new temporary width
                    int newTHeight = (int)(originalBitmap.Height * scaleR);

                    // Resize the image
                    using (var resizedBitmap = originalBitmap.Resize(new SKImageInfo(newTWidth, newTHeight), SKFilterQuality.High))
                    {
                        int cropX = (newTWidth - maxWidth) / 2;
                        int cropY = (newTHeight - maxHeight) / 2;
                        if (cropX > 0) --cropX;
                        if (cropY > 0) --cropY;

                        // Specify the cropping rectangle (left, top, width, height)
                        SKRectI cropRect = new SKRectI(cropX, cropY, maxWidth - 2, maxHeight - 2); // Adjust these values as needed

                        // Create a new bitmap with the cropped region
                        using (var croppedBitmap = new SKBitmap(newWidth, newHeight))
                        {
                            using (var canvas = new SKCanvas(croppedBitmap))
                            {
                                canvas.DrawBitmap(resizedBitmap, cropRect, new SKRectI(0, 0, croppedBitmap.Width, croppedBitmap.Height));
                            }

                            // Save the cropped image to a new MemoryStream
                            using (var outputMemoryStream = new MemoryStream())
                            {
                                using (var outputStream = new SKManagedWStream(outputMemoryStream))
                                {
                                    croppedBitmap.Encode(outputStream, SKEncodedImageFormat.Jpeg, 90); // Change format and quality as needed
                                }

                                // Now, outputMemoryStream contains the cropped image data
                                byte[] croppedImageData = outputMemoryStream.ToArray();

                                // You can use the croppedImageData as needed (e.g., save it to a file, send it over the network, etc.)
                                return croppedImageData;
                            }
                        }

                    }
                }
            }
        }
    }
}
