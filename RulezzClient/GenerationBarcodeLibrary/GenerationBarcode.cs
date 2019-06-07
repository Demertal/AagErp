using System;
using System.Drawing;
using System.Text;
using Zen.Barcode;

namespace GenerationBarcodeLibrary
{
    public class GenerationBarcode
    {
        private static readonly string[] NumberL = { "0001101", "0011001", "0010011", "0111101", "0100011", "0110001", "0101111", "0111011", "0110111", "0001011" };
        private static readonly string[] NumberG = { "0100111", "0110011", "0011011", "0100001", "0011101", "0111001", "0000101", "0010001", "0001001", "0010111" };
        private static readonly string[] NumberR = { "1110010", "1100110", "1101100", "1000010", "1011100", "1001110", "1010000", "1000100", "1001000", "1110100" };

        public static string GenerateBarcode()
        {
            StringBuilder barcode = new StringBuilder(13);
            Random rand = new Random((int)DateTime.Now.Ticks);
            while (barcode.Length != 12)
            {
                barcode.Append(rand.Next(0, 9));
            }

            int sumEven = 0, sumNoEven = 0;
            for (int i = 0; i < 12; i++)
            {
                if((i + 1) % 2 == 0) sumEven += int.Parse(barcode[i].ToString());
                else sumNoEven += int.Parse(barcode[i].ToString());
            }

            int control = sumEven * 3 + sumNoEven;
            int largeControl = control;
            while (largeControl % 10 != 0)
            {
                largeControl++;
            }

            barcode.Append(largeControl - control);
            return barcode.ToString();
        }

        public static Image GetImageBarcode(string barcode)
        {
            BarcodeSymbology s = BarcodeSymbology.CodeEan13;
            BarcodeDraw drawObject = BarcodeDrawFactory.GetSymbology(s);
            var metrics = drawObject.GetDefaultMetrics(60);
            metrics.Scale = 2;
            string temp = barcode.Remove(barcode.Length - 1, 1);
            var img = drawObject.Draw(temp, metrics);
            var resultImage = new Bitmap(img.Width, img.Height + 20); // 20 is bottom padding, adjust to your text

            using (var graphics = Graphics.FromImage(resultImage))
            using (var font = new Font("Consolas", 10))
            using (var brush = new SolidBrush(Color.Black))
            using (var format = new StringFormat
            {
                Alignment = StringAlignment.Center, // Also, horizontally centered text, as in your example of the expected output
                LineAlignment = StringAlignment.Far
            })
            {
                graphics.Clear(Color.White);
                graphics.DrawImage(img, 0, 0);
                graphics.DrawString(barcode, font, brush, resultImage.Width / 2, resultImage.Height, format);
            }
            //Image img = code.Draw(barcode, 20);
            //float scale = 120F/25.4F;
            //float widthImg = 37.29F;
            //float heightImg = 25.93F;
            //float heightLine = 22.85F * scale;
            //float widthLine = 0.33F* scale;
            //string encrypteBarcode = GrtEncryptBarcode(GetType(int.Parse(barcode[0].ToString())), barcode);
            ////Bitmap img = new Bitmap((int)(widthImg * scale), (int)(heightImg * scale));
            //Graphics drawing = Graphics.FromImage(img);
            //GraphicsState gs = drawing.Save();
            //drawing.PageUnit = GraphicsUnit.Pixel;
            //drawing.PageScale = 1;
            //RectangleF rect = new RectangleF(0, 0, widthImg * scale, heightImg * scale);
            //drawing.FillRectangle(new SolidBrush(Color.White), rect);
            //float curPosX = 11 * widthLine;
            //foreach (var enBar in encrypteBarcode)
            //{
            //    if (enBar == '1')
            //    {
            //        rect = new RectangleF(curPosX, 0, widthLine, heightLine);
            //        drawing.FillRectangle(new SolidBrush(Color.Black), rect);
            //    }
            //    else if (enBar == '2')
            //    {
            //        rect = new RectangleF(curPosX, 0, widthLine, heightLine + 5 * widthLine);
            //        drawing.FillRectangle(new SolidBrush(Color.Black), rect);
            //    }
            //    curPosX += widthLine;
            //}

            //curPosX = 11 * widthLine - 3.63F* scale;
            //for (int i = 0; i < barcode.Length; i++)
            //{
            //    drawing.DrawString(barcode[i].ToString(), new Font("Arial", 2.75F * scale), new SolidBrush(Color.Black), curPosX,
            //        heightLine + 0.5F * widthLine);
            //    if (i == 0)
            //    {
            //        curPosX = 14 * widthLine;
            //    }
            //    if(i != 0)
            //    {
            //        curPosX += drawing.MeasureString(barcode[i].ToString(),
            //            new Font("Arial", 2.75F * scale)).Width;
            //    }
            //}

            //drawing.Restore(gs);


            //Bitmap res = new Bitmap((int)Math.Round(widthImg), (int)Math.Round(heightImg));
            //Graphics rdrawing = Graphics.FromImage(res);
            //rdrawing.DrawImage(img, 0, 0, widthImg*scale, heightImg*scale);
            //drawing.Dispose();
            //img.Dispose();
            //rdrawing.Dispose();
            return resultImage;
        }

        private static string GetType(int number)
        {
            switch (number)
            {
                case 0: return "LLLLLLRRRRRR";
                case 1: return "LLGLGGRRRRRR";
                case 2: return "LLGGLGRRRRRR";
                case 3: return "LLGGGLRRRRRR";
                case 4: return "LGLLGGRRRRRR";
                case 5: return "LGGLLGRRRRRR";
                case 6: return "LGGGLLRRRRRR";
                case 7: return "LGLGLGRRRRRR";
                case 8: return "LGLGGLRRRRRR";
                case 9: return "LGGLGLRRRRRR";
                default: return "";
            }
        }

        private static string GrtEncryptBarcode(string type, string barcode)
        {
            string result = "2020";
            for (int i = 1; i < barcode.Length; i++)
            {
                if (i == 7)
                {
                    result += "02020";
                }

                switch (type[i-1])
                {
                    case 'L':
                        result += NumberL[int.Parse(barcode[i].ToString())];
                        break;
                    case 'G':
                        result += NumberG[int.Parse(barcode[i].ToString())];
                        break;
                    case 'R':
                        result += NumberR[int.Parse(barcode[i].ToString())];
                        break;
                }
            }
            result += "0202";
            return result;
        }
    }
}
