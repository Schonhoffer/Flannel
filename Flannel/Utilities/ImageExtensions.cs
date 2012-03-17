using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Flannel.Utilities
{
	public static class ImageExtensions
	{
		public static bool FixOrientation(this Image image)
		{
			// 0x0112 is the EXIF byte address for the orientation tag
			if (!image.PropertyIdList.Contains(0x0112))
			{
				return false;
			}

			// get the first byte from the orientation tag and convert it to an integer
			byte orientationNumber = image.GetPropertyItem(0x0112).Value[0];

			switch (orientationNumber)
			{
					// up is pointing to the right
				case 8:
					image.RotateFlip(RotateFlipType.Rotate270FlipNone);
					return true;
					// up is pointing to the bottom (image is upside-down)
				case 3:
					image.RotateFlip(RotateFlipType.Rotate180FlipNone);
					return true;
					// up is pointing to the left
				case 6:
					image.RotateFlip(RotateFlipType.Rotate90FlipNone);
					return true;
					// up is pointing up (correct orientation)
				case 1:
					break;
			}

			return false;
		}

		public static Stream JpegToPng(this Stream stream)
		{
			var image = Image.FromStream(stream);

			var ms = new MemoryStream();

			image.Save(ms, ImageFormat.Png);

			return ms;
		}


		public static Stream FixOrientation(this Stream stream)
		{
			var image = Image.FromStream(stream);
			if (image.FixOrientation())
			{
				var ms = new MemoryStream();

				image.Save(ms, ImageFormat.Jpeg);

				return ms;
			}

			return stream;
		}
	}
}