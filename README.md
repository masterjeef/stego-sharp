# Stego Sharp

**Steganography**
>the practice of concealing messages or information within other nonsecret text or data.

This is a handy .Net library for embedding data into an image, and also reading data from an image. My algorithm uses a technique called the least significant bit (LSB) to read/write bits from the image.

There are a few different strategies to consider when embedding/extracting bits from an image.

#### Color channels (bits)

Bits of data can be read/written to any of the following channels

* Red 0-255 (8 bits)
* Green 0-255 (8 bits)
* Blue 0-255 (8 bits)
* Alpha 0-255 (8 bits)
		
EX : 

Embed the bytes into the Green, Blue, and Red channels. Note that the order of the channels does matter.

CODE : 
	
	var image = new StegoImage("images/hello.png");
	image.Strategy.ColorChannels = new[] { ColorChannel.G, ColorChannel.B, ColorChannel.R };

#### Number of bits

Must be > 0 and <= to 8. The is due to the fact that we only have 8 bits to work with on each color channel.

EX : 

We want to embed the bits 11 (3 in decimal) into the red color channel for a single pixel. See below.

BEFORE				AFTER
Red (172) 	=> 		Red (175)
10101100			10101111

CODE : 

	var image = new StegoImage("images/world.png");
	image.Strategy.BitsPerChannel = 2;
	
##### Considerations

Increasing the number of bits will increase capacity, but only at the risk of distorting the image. I recommend only embedding up to 2 bits of data in each color channel.

#### The pixels that we want to read/write data into, or the pixels in question.
Embedding the data into the first `x` number of pixels is too easy to read. This library allows the user to decide what pixels to read/write to.

EX :

Reading/Writing to the even pixels in the image. {0, 2, 4, 6, 8, ...}

CODE : 
	
	var image = new StegoImage("images/goodbye.png");
    image.Strategy.PixelSelection = p => p.Index % 2 == 0;

##### Considerations

Utilizing only a subset of all the pixels will decrease capacity. For example, embedding data into the even pixels will cut the amount of storage in half.

#### Other Considerations

* If the image type does not allow transparency, the then entire alpha channel can be used.
* Writing data to jpegs is not supported due to lossy compression. Unfortunately the compression changes the pixels thus corrupted our hidden data.

### Having Fun with Steganography

coming soon...
