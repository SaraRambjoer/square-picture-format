# Square Picture Format
A lossy picture format that raster images can be generated from. It works by saving a pixel coordinate, a color, and the size of the sides of a rectangle. 

Save format: 
Header: Int32 Image Width, Int32 Image Height
Content: Int32 x-coordinate, Int32 y-coordinate, Int32 red value, Int32 blue value, Int32 green value, Int32 side size - repeating

The format represents all images as having 255 alpha
# Screenshots
The sensitivity is the threshold for when a new rectangle should be created. It is the sum of squares of the difference between the rgb values of every pixel from the original pixel. 

Original images:
![](https://i.imgur.com/zx8Ixoa.jpg)
![](https://images.pexels.com/photos/2096697/pexels-photo-2096697.jpeg)
https://www.pexels.com/photo/cityscape-2096697/

1000:
![](https://i.imgur.com/m4wHf3n.png)
![](https://i.imgur.com/ScOEzfA.png)


2500:
![](https://i.imgur.com/aomkff1.png)
![](https://i.imgur.com/cSKH6Ar.png)


4000:
![](https://i.imgur.com/bpyrZYP.png)
![](https://i.imgur.com/UOonmf9.png)


5000:
![](https://i.imgur.com/njpS0rd.png)
![](https://i.imgur.com/99keQIb.png)


6000:
![](https://i.imgur.com/j0mDrSP.png)
![](https://i.imgur.com/PtTBX29.png)


7500:
![](https://i.imgur.com/NkzrNJa.png)
![](https://i.imgur.com/lUmx7kJ.png)


10000:
![](https://i.imgur.com/9zlnbPf.png)
![](https://i.imgur.com/AjFl8Vd.png)



# Performance
The following table details how much space is saved in comparison to a bmp formatting of the corresponding image. 
| Sensitivity | image 1 spf  | image 1 bmp | image 2 spf  | image 2 bmp |
| ------------- | ------------- | ------------- | ------------- | ------------- |
| 1000 |  149 KB |  136 KB | 4318 KB  | 3243 KB  |
| 2500  | 68 KB |  96 KB | 1294 KB  | 2306 KB  |
| 4000  |  50 KB | 79 KB  | 705 KB  | 1892 KB  |
| 5000  |  45 KB | 68 KB  | 538 KB  | 1702 KB  |
| 6000  |  41 KB | 57 KB  | 430 KB  |  1529 KB |
| 7500  |  39 KB | 52 KB  | 332 KB  | 1370 KB  |
| 10000  |  37 KB | 48 KB  | 258 KB  | 1288 KB  |

# How to use
The project is a C# class file. 

# Credits
One of the photos is a stock photo, credits to Brayden Law at Pexels: https://www.pexels.com/photo/cityscape-2096697/

# License
MIT © Jon Oddvar Rambjør

