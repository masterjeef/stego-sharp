# Stego Sharp

**Steganography**
>the practice of concealing messages or information within other nonsecret text or data.

This is a handy .Net library for embedding data into an image, and also reading data from an image. My algorithm uses a technique called the least significant bit (LSB) to read/write bits from the image.

There are a few different strategies to consider when embedding/extracting bits from an image.

### Color channels (bits)

Bits of data can be read/written to any of the following channels

* Red 0-255 (8 bits)
* Green 0-255 (8 bits)
* Blue 0-255 (8 bits)
* Alpha 0-255 (8 bits)
		
SCENARIO : 

Embed the bytes into the Green, Blue, and Red channels. Note that the order of the channels does matter.

CODE : 
	
	var image = new StegoImage("images/hello.png");
	image.Strategy.ColorChannels = new[] { ColorChannel.G, ColorChannel.B, ColorChannel.R };

### Number of bits

Must be > 0 and <= to 8. The is due to the fact that we only have 8 bits to work with in each color channel.

SCENARIO : 

Embed the bits 11 (3 in decimal) into the red color channel for a single pixel. See below.

| BEFORE    |    |     AFTER |
|-----------|:--:|----------:|
| Red (172) | => | Red (175) |
| 10101100  |    |  10101111 |

CODE : 

	var image = new StegoImage("images/world.png");
	image.Strategy.BitsPerChannel = 2;
	

**Considerations**

Increasing the number of bits will increase capacity, but only at the risk of distorting the image. I recommend only embedding up to 2 bits of data in each color channel.

### The Pixels

Embedding the data into the first `x` number of pixels is too easy to read. This library allows the user to decide what pixels to read/write to.

SCENARIO :

Reading/Writing to the even pixels in the image. {0, 2, 4, 6, 8, ...}

CODE : 
	
	var image = new StegoImage("images/goodbye.png");
    image.Strategy.PixelSelection = p => p.Index % 2 == 0;

**Considerations**

Utilizing only a subset of all the pixels will decrease capacity. For example, embedding data into the even pixels will cut the amount of storage in half.

#### More Considerations

* If the image type does not allow transparency, the then entire alpha channel can be used.
* Writing a payload to jpegs is not supported due to lossy compression. Unfortunately the compression corrupts the payload.

### Having Fun with Steganography

And so I decided to stegafy some images, see below :

**Original**            |  **Stegafied**
:-------------------------:|:-------------------------:
![Iguana](https://github.com/masterjeef/stego-sharp/blob/master/StegoSharp/UnitTests/images/iguana.png?raw=true)  |  ![Iguana](https://github.com/masterjeef/stego-sharp/blob/master/StegoSharp/images/iguana-embedded.png?raw=true)

**The hidden payload**

>We exist without skin color, without nationality, without religious bias... and you call us criminals. You build atomic bombs, you wage wars, you murder, cheat, and lie to us and try to make us believe it's for our own good, yet we're the criminals.

#### This got me thinking...

Can we hide a payload within a payload (aka stegaception)? Why yes we can, see below :

![Space](https://github.com/masterjeef/stego-sharp/blob/master/StegoSharp/images/space-embedded.png?raw=true)

**Hidden inside space**

![Astronaut](https://github.com/masterjeef/stego-sharp/blob/master/StegoSharp/images/astronaut-embedded.png?raw=true)

**Hidden inside Mr. DeVito**
 
![Sloth](https://github.com/masterjeef/stego-sharp/blob/master/StegoSharp/images/sloth-embedded.png?raw=true)

**Hidden inside the sloth**
 
>The probability of success is difficult to estimate; but if we never search the chance of success is zero.

#### What would it look like if all bits in each color channel were used?

This experiment produced some cool visualizations of what what the embedded data looks like. See below.

**Default** `x => true`            |  `x => x.Index % 3 == 0;`
:-------------------------:|:-------------------------:
![Pillars](https://github.com/masterjeef/stego-sharp/blob/master/StegoSharp/images/pillar-test-1.png?raw=true)  |  ![Pillars 2](https://github.com/masterjeef/stego-sharp/blob/master/StegoSharp/images/pillar-test-2.png?raw=true)
