using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            KA();
        }

        private async void KA()
        {
            Random rnd = new Random();
            int Xmax = 255;
            int Ymax = 255;
            int Tmax = 500;
            int[,,] KA = new int[2, Xmax + 1, Ymax + 1];
            for (int i = 1; i <= Xmax; i++)
            {
                for (int j = 1; j <= Ymax; j++)
                {
                    KA[0, i, j] = 0;
                    KA[1, i, j] = rnd.Next(2);
                }
            }
            int m = 1;
            int k = 0;

            for (int t = 1; t <= Tmax; t++)
            {
                for (int i = 1; i <= Xmax; i++)
                {
                    for (int j = 1; j <= Ymax; j++)
                    {
                        int L = i > 1 ? i - 1 : Xmax;
                        int R = i < Xmax ? i + 1 : 1;
                        int U = j > 1 ? j - 1 : Ymax;
                        int D = j < Ymax ? j + 1 : 1;
                        int S = KA[k, L, U] + KA[k, i, U] + KA[k, R, U] + KA[k, L, j] + KA[k, R, j] + KA[k, L, D] + KA[k, i, D] + KA[k, R, D];
                        switch (S)
                        {
                            case 2:
                                KA[m, i, j] = KA[k, i, j] == 0 ? 1 : 0;
                                break;
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                            case 8:
                                KA[m, i, j] = KA[k, i, j] == 1 ? 0 : 1;
                                break;
                        }
                    }
                }
                m = (m + 1) % 2;
                k = (k + 1) % 2;

                await Task.Delay(100);

                WriteableBitmap Bitmap = new WriteableBitmap(Xmax + 1, Ymax + 1, 96, 96, PixelFormats.Bgr24, null);

                byte[] pixels = new byte[(Xmax + 1) * (Ymax + 1) * 3];
                for (int i = 0; i <= Xmax; i++)
                {
                    for (int j = 0; j <= Ymax; j++)
                    {
                        byte color = (byte)(KA[m, i, j] == 0 ? 255 : 0);
                        int offset = (j * (Xmax + 1) + i) * 3;
                        pixels[offset] = color; 
                        pixels[offset + 1] = color; 
                        pixels[offset + 2] = color; 
                    }
                }

                Bitmap.WritePixels(new Int32Rect(0, 0, Xmax + 1, Ymax + 1), pixels, (Xmax + 1) * 3, 0);
                KAImage.Source = Bitmap;
            }
        }

    }
}
