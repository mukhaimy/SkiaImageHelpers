using System.IO;
using SkiaSharp;
using static System.Net.Mime.MediaTypeNames;


namespace SkiaImageHelpers
{
    public static class ImageOperations
    {
        public static readonly int QualityNumber = 95;

        public static byte[] ResizeToWidth(byte[] imageData, int newWidth, bool isPng = false)
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
                                if(isPng)
                                {
                                    resizedBitmap.Encode(outputStream, SKEncodedImageFormat.Png, QualityNumber);
                                }
                                else
                                {
                                    resizedBitmap.Encode(outputStream, SKEncodedImageFormat.Jpeg, QualityNumber);
                                }
                                
                            }

                            // Now, outputMemoryStream contains the resized image data
                            byte[] resizedImageData = outputMemoryStream.ToArray();

                            return resizedImageData;
                        }
                    }
                }
            }
        }

        public static byte[] ResizeToHeight(byte[] imageData, int newHeight, bool isPng = false)
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
                                if (isPng)
                                {
                                    resizedBitmap.Encode(outputStream, SKEncodedImageFormat.Png, QualityNumber);
                                }
                                else
                                {
                                    resizedBitmap.Encode(outputStream, SKEncodedImageFormat.Jpeg, QualityNumber);
                                }
                            }

                            // Now, outputMemoryStream contains the resized image data
                            byte[] resizedImageData = outputMemoryStream.ToArray();

                            return resizedImageData;
                        }
                    }
                }
            }
        }

        private static void CalculateAspectResizedDimensions(int originalWidth, int originalHeight, int maxWidth, int maxHeight, out int newWidth, out int newHeight)
        {
            double aspectRatio = (double)originalWidth / originalHeight;

            if (originalWidth > maxWidth)
            {
                newWidth = maxWidth;
                newHeight = (int)(newWidth / aspectRatio);
            }
            else if (originalHeight > maxHeight)
            {
                newHeight = maxHeight;
                newWidth = (int)(newHeight * aspectRatio);
            }
            else
            {
                // No resizing needed
                newWidth = originalWidth;
                newHeight = originalHeight;
            }
        }

        public static byte[] Zoom(byte[] imageData, int targetWidth, int targetHeight, bool isPng = false)
        {
            using (var inputStream = new SKManagedStream(new MemoryStream(imageData)))
            {
                using (var originalBitmap = SKBitmap.Decode(inputStream))
                {
                    // Specify the maximum dimensions for the resized image
                    int maxWidth = targetWidth; // Replace with your desired maximum width
                    int maxHeight = targetHeight; // Replace with your desired maximum height

                    // Calculate new dimensions while maintaining aspect ratio
                    int newWidth, newHeight;
                    CalculateAspectResizedDimensions(originalBitmap.Width, originalBitmap.Height, maxWidth, maxHeight, out newWidth, out newHeight);

                    // Resize the image
                    using (var resizedBitmap = originalBitmap.Resize(new SKImageInfo(newWidth, newHeight), SKFilterQuality.High))
                    {
                        // Calculate the crop rectangle for the center
                        int cropX = (resizedBitmap.Width - maxWidth) / 2;
                        int cropY = (resizedBitmap.Height - maxHeight) / 2;
                        SKRectI cropRect = new SKRectI(cropX, cropY, cropX + maxWidth, cropY + maxHeight);

                        // Create a new bitmap with the cropped region
                        using (var croppedBitmap = new SKBitmap(maxWidth, maxHeight))
                        {
                            using (var canvas = new SKCanvas(croppedBitmap))
                            {
                                canvas.DrawBitmap(resizedBitmap, cropRect, new SKRectI(0, 0, croppedBitmap.Width, croppedBitmap.Height));
                            }

                            // Save the resized and cropped image to a new MemoryStream
                            using (var outputMemoryStream = new MemoryStream())
                            {
                                using (var outputStream = new SKManagedWStream(outputMemoryStream))
                                {
                                    if (isPng)
                                    {
                                        croppedBitmap.Encode(outputStream, SKEncodedImageFormat.Png, QualityNumber);
                                    }
                                    else
                                    {
                                        croppedBitmap.Encode(outputStream, SKEncodedImageFormat.Jpeg, QualityNumber);
                                    }                                    
                                }

                                // Now, outputMemoryStream contains the resized and cropped image data
                                byte[] resizedAndCroppedImageData = outputMemoryStream.ToArray();

                                return resizedAndCroppedImageData;
                            }
                        }
                    }
                }
            } // EOF - using (var inputStream = new SKManagedStream(new MemoryStream(imageData)))
        }

        public static int ImageWidth(byte[] imageData)
        {
            try
            {
                using (var inputStream = new SKManagedStream(new MemoryStream(imageData)))
                {
                    using (var originalBitmap = SKBitmap.Decode(inputStream))
                    {
                        return originalBitmap.Width;                        
                    }
                }
            }
            catch (Exception)
            {
                return -1;
            }            
        }

        public static int ImageHeight(byte[] imageData)
        {
            try
            {
                using (var inputStream = new SKManagedStream(new MemoryStream(imageData)))
                {
                    using (var originalBitmap = SKBitmap.Decode(inputStream))
                    {
                        return originalBitmap.Height;
                    }
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }

    }
}
