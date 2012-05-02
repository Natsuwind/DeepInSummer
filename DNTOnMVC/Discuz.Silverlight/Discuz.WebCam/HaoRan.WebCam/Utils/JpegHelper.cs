using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System;
using System.Windows.Media.Imaging;

using FluxJpeg.Core;
using FluxJpeg.Core.Encoder;

namespace HaoRan.WebCam
{
    public class JpegHelper
    {
        public static void EncodeJpeg(WriteableBitmap bmp, Stream dstStream)
        {
            // Init buffer in FluxJpeg format
            int w = bmp.PixelWidth;
            int h = bmp.PixelHeight;
            int[] p = bmp.Pixels;
            byte[][,] pixelsForJpeg = new byte[3][,]; // RGB colors
            pixelsForJpeg[0] = new byte[h, w];
            pixelsForJpeg[1] = new byte[h, w];
            pixelsForJpeg[2] = new byte[h, w];

            // Copy WriteableBitmap data into buffer for FluxJpeg
            int i = 0;
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    int color = p[i++];
                    // Swap x and y coordinates to cheaply rotate the image 90° clockwise
                    pixelsForJpeg[0][y, x] = (byte)(color >> 16); // R
                    pixelsForJpeg[1][y, x] = (byte)(color >> 8);  // G
                    pixelsForJpeg[2][y, x] = (byte)(color);       // B
                }
            }

            using (MemoryStream memStream = new MemoryStream())
            {
                //Encode Image as JPEG
                var jpegImage = new FluxJpeg.Core.Image(new ColorModel { colorspace = ColorSpace.RGB }, pixelsForJpeg);
                var encoder = new JpegEncoder(jpegImage, 95, memStream);
                encoder.Encode();

                // Seek to begin of stream and write the encoded bytes to the FileSteram
                memStream.Seek(0, SeekOrigin.Begin);
                // Use the new .Net 4 CopyTo method :)
                memStream.CopyTo(dstStream);
            }
        }

        public static WriteableBitmap DecodeJpeg(Stream srcStream)
        {
            // Decode JPEG
            var decoder = new FluxJpeg.Core.Decoder.JpegDecoder(srcStream);
            var jpegDecoded = decoder.Decode();
            var img = jpegDecoded.Image;
            img.ChangeColorSpace(ColorSpace.RGB);

            // Init Buffer
            int w = img.Width;
            int h = img.Height;
            var result = new WriteableBitmap(w, h);
            int[] p = result.Pixels;
            byte[][,] pixelsFromJpeg = img.Raster;

            // Copy FluxJpeg buffer into WriteableBitmap
            int i = 0;
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    p[i++] = (0xFF << 24) // A
                                | (pixelsFromJpeg[0][x, y] << 16) // R
                                | (pixelsFromJpeg[1][x, y] << 8)  // G
                                | pixelsFromJpeg[2][x, y];       // B
                }
            }

            return result;
        }

    }
}
