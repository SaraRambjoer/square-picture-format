using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Diagnostics;
namespace encoder
{

}

public class SquarePictureFormatter
{
    /// <summary>
    /// Creates a spf file
    /// </summary>
    /// <param name="imagePath">Path to original image</param>
    /// <param name="savePath">Where to save the .spf</param>
    /// <param name="difTol">Measure of how tolerant the squares should be of different colors</param>
    public static void createSpf(String imagePath, String savePath, int difTol) //Problem all colours r same
    {
        Bitmap sourceImage = new Bitmap($" { imagePath} ");
        Stream stream = new FileStream(savePath, FileMode.Create, FileAccess.Write);
        BinaryWriter bw = new BinaryWriter(stream);
        bw.Write(sourceImage.Width);
        bw.Write(sourceImage.Height);
        byte[] binaryBoolArray = new byte[sourceImage.Width * sourceImage.Height]; //Array describing what pixels have been assigned a color
        for (int i0 = 0; i0 < sourceImage.Width; i0++)
        {
            for (int i1 = 0; i1 < sourceImage.Height; i1++)
            {
                Color pixel = sourceImage.GetPixel(i0, i1);
                int[] rgbcColorArea = new int[] { 0, 0, 0, 0 }; //red green blue count
                if (binaryBoolArray[i1 * sourceImage.Width + i0] == 0) //If the pixel hasn't been assigned a color
                {
                    int dif = 0;
                    int size = 1;
                    List<int> sizeList = new List<int>();
                    bool leftImage1 = false;
                    bool leftImage2 = false;
                    while (true)
                    {
                        //Sets the colors in an area
                        int[] rgbcColorAreaNew = new int[] { 0, 0, 0, 0 };
                        for (int i2 = 0; i2 < size; i2++)
                        {
                            try //Expands the edges with one starting from the origin pixel
                            {
                                Color pixColor = sourceImage.GetPixel(i0 + i2, i1 + size);
                                rgbcColorAreaNew[0] += pixColor.R;
                                rgbcColorAreaNew[1] += pixColor.G;
                                rgbcColorAreaNew[2] += pixColor.B;
                                rgbcColorAreaNew[3] += 1;
                                dif += (Math.Abs(pixColor.R - pixel.R) + Math.Abs(pixColor.G - pixel.G) + Math.Abs(pixColor.B - pixel.B)) ^ 2;
                            }
                            catch
                            {
                                leftImage1 = true;
                                goto outsideImage1;
                            }
                        }
                    outsideImage1:;
                        for (int i2 = 0; i2 < size - 1; i2++) //-1 because the edge has been covered by previous for loop. 
                        {
                            try
                            {
                                Color pixColor = sourceImage.GetPixel(i0 + size, i1 + i2);
                                rgbcColorAreaNew[0] += pixColor.R;
                                rgbcColorAreaNew[1] += pixColor.G;
                                rgbcColorAreaNew[2] += pixColor.B;
                                rgbcColorAreaNew[3] += 1;
                                dif += (Math.Abs(pixColor.R - pixel.R) + Math.Abs(pixColor.G - pixel.G) + Math.Abs(pixColor.B - pixel.B)) ^ 2;
                            }
                            catch
                            {
                                leftImage2 = true;
                                goto outsideImage2;
                            }
                        }
                    outsideImage2:;
                        if (dif < difTol && !(leftImage1 && leftImage2)) //Finds right size by using binary search
                        {
                            size *= 2;
                            rgbcColorArea = rgbcColorAreaNew;
                        }
                        else if (dif < difTol && !(leftImage1 && leftImage2) && !sizeList.Contains(size))
                        {
                            size = (int)Math.Floor((float)3 * size / 4);
                            rgbcColorArea = rgbcColorAreaNew;
                        }
                        else
                        {
                            if (rgbcColorArea[3] != 0)
                            {
                                bw.Write(i0);
                                bw.Write(i1);
                                int red = Math.Min(rgbcColorArea[0] / rgbcColorArea[3], 255);
                                int green = Math.Min(rgbcColorArea[1] / rgbcColorArea[3], 255);
                                int blue = Math.Min(rgbcColorArea[2] / rgbcColorArea[3], 255);
                                bw.Write(red);
                                bw.Write(green);
                                bw.Write(blue);
                                bw.Write((size - 1));
                            }
                            for (int i2 = 0; i2 < size - 1; i2++)
                            {
                                for (int i3 = 0; i3 < size - 1; i3++)
                                {
                                    try
                                    {
                                        binaryBoolArray[(i1 + i3) * sourceImage.Width + i0 + i2] = 1;
                                    }
                                    catch 
                                    {
                                        goto outsideLoop2 ; //If pixel is outside the image the rest of the pixels for this i2 and greater will be too
                                    }
                                }
                            }
                        outsideLoop2:;
                            break;
                        }
                    }
                }
            }
        }
        sourceImage.Dispose();
        stream.Close();
        bw.Dispose();

    }

    /// <summary>
    /// Makes a image (png/jpg) from a .spf
    /// </summary>
    /// <param name="savePath">Path to save the image</param>
    /// <param name="spfFilePath">Path to the .spf</param>
    public static void createPictureFromSpf(string savePath, string spfFilePath)
    {
        Stream stream = new FileStream(spfFilePath, FileMode.Open, FileAccess.Read);
        BinaryReader br = new BinaryReader(stream);
        Bitmap destinationImage = new Bitmap(br.ReadInt32(), br.ReadInt32());
        try
        {
            //Save format: int32: width int32:height repeating: int.32: x-cordinate int.32: y-cord int32: rgb:9 chars, int32 size and so on, alpha is always 255 so no need to save. Idea for improvement: One could save the size of the squares just once and instead have a header with a index to each grouping - an Int32 size object can fit all rgbvalues
            while (true)
            {
                int[] dotPoint = new int[] { br.ReadInt32(), br.ReadInt32() };
                Color avgColor = Color.FromArgb(255, Math.Min(br.ReadInt32(), 255),
                    Math.Min(br.ReadInt32(), 255), Math.Min(br.ReadInt32(), 255));
                int size = br.ReadInt32();
                for (int i1 = 0; i1 < size; i1++) //Gets data about colors in a diamond of radius pixels away from dotPoint
                {
                    for (int i2 = 0; i2 < size; i2++)
                    {
                        try
                        {
                            destinationImage.SetPixel(dotPoint[0] + i1, dotPoint[1] + i2, avgColor);
                        }
                        catch
                        {
                            goto outsideLoop;
                        }
                    }
                }
            outsideLoop:;
            }
        }
        catch
        {
            //Do nothing. This happens at the end of the file 
        }
        Bitmap save = new Bitmap(destinationImage); //Workaround for destinationImage being 'locked' causing an error if one attempts to save it.
        save.Save(savePath);
        destinationImage.Dispose();
        save.Dispose();
        br.Dispose();
        stream.Close();
    }

}
