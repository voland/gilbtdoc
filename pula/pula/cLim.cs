using System.Drawing;
using System;
using System.IO;
using System.Drawing.Imaging;

namespace Pula {
    class cLim {
        public cLim(int x, int y) {
            sizedata = x + ((y - 1) / 32) * x;
            data = new int[sizedata];
            width = x;
            height = y;
        }

        public cLim(Bitmap bb)
            : this(bb.Width, bb.Height) {
            Bitmap b = new Bitmap(bb);
            unsafe {
                BitmapData bData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadOnly, b.PixelFormat);

                for (int y = 0; y < b.Height; y++) {
                    byte* bRow = (byte*)bData.Scan0 + (y * bData.Stride);
                    for (int x = 0; x < b.Width; x++) {
                        setData(x, y, ((bRow[x * 4 + 2] + bRow[x * 4 + 1] + bRow[x * 4 + 0]) > 0));
                    }
                }
                b.UnlockBits(bData);
            }
        }

        public cLim(Image i)
            : this(new Bitmap(i)) {

        }

        public void SaveToFile(String path) {
            using (FileStream fs = new FileStream(path, FileMode.CreateNew)) {
                using (BinaryWriter bw = new BinaryWriter(fs)) {
                    Compile(bw);
                }
            }
        }

        public void Compile(BinaryWriter bw) {
            bw.Write(width);
            bw.Write(height);
            bw.Write(sizedata);
            foreach (int i in data) bw.Write(i);
        }

        public void Compile(Stream s) {
            BinaryWriter bw = new BinaryWriter(s);
            Compile(bw);
            s.Seek(0, SeekOrigin.Begin);
        }

        public void setData(int x, int y, bool c) {
            int temp = x + (y / 32) * width;
            if (c) data[temp] |= (1 << (y % 32));
            else data[temp] &= ~(1 << (y % 32));

        }

        public int getSize() {
            return 3 * sizeof(int) + sizedata * sizeof(int);
        }

        private int width;
        private int height;
        private int[] data;
        private int sizedata;
    }
}
